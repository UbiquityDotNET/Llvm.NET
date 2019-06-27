// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;
using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.JIT;
using Llvm.NET.Transforms;
using Llvm.NET.Values;

using ConstantExpression = Kaleidoscope.Grammar.AST.ConstantExpression;

namespace Kaleidoscope.Chapter71
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    public sealed class CodeGenerator
        : AstVisitorBase<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        #region Initialization
        public CodeGenerator( DynamicRuntimeState globalState, bool disableOptimization = false )
            : base( null )
        {
            if( globalState.LanguageLevel > LanguageLevel.MutableVariables )
            {
                throw new ArgumentException( "Language features not supported by this generator", nameof( globalState ) );
            }

            RuntimeState = globalState;
            Context = new Context( );
            DisableOptimizations = disableOptimization;
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
        }
        #endregion

        #region Dispose
        public void Dispose( )
        {
            JIT.Dispose( );
            Module?.Dispose( );
            FunctionPassManager?.Dispose( );
            Context.Dispose( );
        }
        #endregion

        #region Generate
        public Value Generate( IAstNode ast, Action<CodeGeneratorException> codeGenerationErroHandler )
        {
            try
            {
                // Prototypes, including extern are ignored as AST generation
                // adds them to the RuntimeState so that already has the declarations
                if( !( ast is FunctionDefinition definition ) )
                {
                    return null;
                }

                // Anonymous functions are called immediately then removed from the JIT
                // so no point in setting them up as a lazy compilation item.
                if( definition.IsAnonymous )
                {
                    InitializeModuleAndPassManager( );
                    var function = ( IrFunction )definition.Accept( this );

                    // eagerly compile modules for anonymous functions as calling the function is the guaranteed next step
                    ulong jitHandle = JIT.AddEagerlyCompiledModule( Module );
                    var nativeFunc = JIT.GetFunctionDelegate<KaleidoscopeJIT.CallbackHandler0>( definition.Name );
                    var retVal = Context.CreateConstant( nativeFunc( ) );
                    JIT.RemoveModule( jitHandle );
                    return retVal;
                }

                // Unknown if any future input will call the function so don't even generate IR
                // until it is needed. JIT triggers the callback to generate the IR module so the JIT
                // can then generate native code only when required.
                FunctionDefinition implDefinition = CloneAndRenameFunction( definition );

                // register the generator as a stub with the original source name
                JIT.AddLazyFunctionGenerator( definition.Name, ( ) =>
                {
                    InitializeModuleAndPassManager( );
                    var function = ( IrFunction )implDefinition.Accept( this );
                    return (implDefinition.Name, function.ParentModule);
                } );
                return null;
            }
            catch( CodeGeneratorException ex ) when( codeGenerationErroHandler != null )
            {
                codeGenerationErroHandler( ex );
                return null;
            }
        }
        #endregion

        #region ConstantExpression
        public override Value Visit( ConstantExpression constant )
        {
            return Context.CreateConstant( constant.Value );
        }
        #endregion

        #region BinaryOperatorExpression
        public override Value Visit( BinaryOperatorExpression binaryOperator )
        {
            switch( binaryOperator.Op )
            {
            case BuiltInOperatorKind.Less:
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan
                                                        , binaryOperator.Left.Accept( this )
                                                        , binaryOperator.Right.Accept( this )
                                                        ).RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case BuiltInOperatorKind.Pow:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow
                                                  , binaryOperator.Left.Accept( this )
                                                  , binaryOperator.Right.Accept( this )
                                                  ).RegisterName( "powtmp" );
                }

            case BuiltInOperatorKind.Add:
                return InstructionBuilder.FAdd( binaryOperator.Left.Accept( this )
                                              , binaryOperator.Right.Accept( this )
                                              ).RegisterName( "addtmp" );

            case BuiltInOperatorKind.Subtract:
                return InstructionBuilder.FSub( binaryOperator.Left.Accept( this )
                                              , binaryOperator.Right.Accept( this )
                                              ).RegisterName( "subtmp" );

            case BuiltInOperatorKind.Multiply:
                return InstructionBuilder.FMul( binaryOperator.Left.Accept( this )
                                              , binaryOperator.Right.Accept( this )
                                              ).RegisterName( "multmp" );

            case BuiltInOperatorKind.Divide:
                return InstructionBuilder.FDiv( binaryOperator.Left.Accept( this )
                                              , binaryOperator.Right.Accept( this )
                                              ).RegisterName( "divtmp" );

            case BuiltInOperatorKind.Assign:
                Alloca target = LookupVariable( ( ( VariableReferenceExpression )binaryOperator.Left ).Name );
                Value value = binaryOperator.Right.Accept( this );
                InstructionBuilder.Store( value, target );
                return value;

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        #endregion

        #region FunctionCallExpression
        public override Value Visit( FunctionCallExpression functionCall )
        {
            string targetName = functionCall.FunctionPrototype.Name;
            IrFunction function;

            // try for an extern function declaration
            if( RuntimeState.FunctionDeclarations.TryGetValue( targetName, out Prototype target ) )
            {
                function = GetOrDeclareFunction( target );
            }
            else
            {
                function = Module.GetFunction( targetName ) ?? throw new CodeGeneratorException( $"Definition for function {targetName} not found" );
            }

            var args = functionCall.Arguments.Select( ctx => ctx.Accept( this ) ).ToArray( );
            return InstructionBuilder.Call( function, args ).RegisterName( "calltmp" );
        }
        #endregion

        #region FunctionDefinition
        public override Value Visit( FunctionDefinition definition )
        {
            var function = GetOrDeclareFunction( definition.Signature );
            if( !function.IsDeclaration )
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

            try
            {
                var entryBlock = function.AppendBasicBlock( "entry" );
                InstructionBuilder.PositionAtEnd( entryBlock );
                using( NamedValues.EnterScope( ) )
                {
                    foreach( var param in definition.Signature.Parameters )
                    {
                        var argSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                        .RegisterName( param.Name );
                        InstructionBuilder.Store( function.Parameters[ param.Index ], argSlot );
                        NamedValues[ param.Name ] = argSlot;
                    }

                    foreach( LocalVariableDeclaration local in definition.LocalVariables )
                    {
                        var localSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                          .RegisterName( local.Name );
                        NamedValues[ local.Name ] = localSlot;
                    }

                    EmitBranchToNewBlock( "body" );

                    var funcReturn = definition.Body.Accept( this );
                    InstructionBuilder.Return( funcReturn );
                    function.Verify( );

                    FunctionPassManager.Run( function );
                    return function;
                }
            }
            catch( CodeGeneratorException )
            {
                function.EraseFromParent( );
                throw;
            }
        }
        #endregion

        #region VariableReferenceExpression
        public override Value Visit( VariableReferenceExpression reference )
        {
            var value = LookupVariable( reference.Name );

            // since the Alloca is created as a non-opaque pointer it is OK to just use the
            // ElementType. If full opaque pointer support was used, then the Lookup map
            // would need to include the type of the value allocated.
            return InstructionBuilder.Load( value.ElementType, value )
                                     .RegisterName( reference.Name );
        }
        #endregion

        #region ConditionalExpression
        public override Value Visit( ConditionalExpression conditionalExpression )
        {
            var result = LookupVariable( conditionalExpression.ResultVariable.Name );

            var condition = conditionalExpression.Condition.Accept( this );
            if( condition == null )
            {
                return null;
            }

            var condBool = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, condition, Context.CreateConstant( 0.0 ) )
                                             .RegisterName( "ifcond" );

            var function = InstructionBuilder.InsertBlock.ContainingFunction;

            var thenBlock = function.AppendBasicBlock( "then" );
            var elseBlock = function.AppendBasicBlock( "else" );
            var continueBlock = function.AppendBasicBlock( "ifcont" );
            InstructionBuilder.Branch( condBool, thenBlock, elseBlock );

            // generate then block instructions
            InstructionBuilder.PositionAtEnd( thenBlock );
            var thenValue = conditionalExpression.ThenExpression.Accept( this );
            if( thenValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( thenValue, result );
            InstructionBuilder.Branch( continueBlock );

            // generate else block
            InstructionBuilder.PositionAtEnd( elseBlock );
            var elseValue = conditionalExpression.ElseExpression.Accept( this );
            if( elseValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( elseValue, result );
            InstructionBuilder.Branch( continueBlock );

            // generate continue block
            InstructionBuilder.PositionAtEnd( continueBlock );

            // since the Alloca is created as a non-opaque pointer it is OK to just use the
            // ElementType. If full opaque pointer support was used, then the Lookup map
            // would need to include the type of the value allocated.
            return InstructionBuilder.Load( result.ElementType, result )
                                     .RegisterName( "ifresult" );
        }
        #endregion

        #region ForInExpression
        public override Value Visit( ForInExpression forInExpression )
        {
            var function = InstructionBuilder.InsertBlock.ContainingFunction;
            string varName = forInExpression.LoopVariable.Name;
            Alloca allocaVar = LookupVariable( varName );

            // Emit the start code first, without 'variable' in scope.
            Value startVal;
            if( forInExpression.LoopVariable.Initializer != null )
            {
                startVal = forInExpression.LoopVariable.Initializer.Accept( this );
                if( startVal == null )
                {
                    return null;
                }
            }
            else
            {
                startVal = Context.CreateConstant( 0.0 );
            }

            // store the value into allocated location
            InstructionBuilder.Store( startVal, allocaVar );

            // Make the new basic block for the loop header.
            var loopBlock = function.AppendBasicBlock( "loop" );

            // Insert an explicit fall through from the current block to the loopBlock.
            InstructionBuilder.Branch( loopBlock );

            // Start insertion in loopBlock.
            InstructionBuilder.PositionAtEnd( loopBlock );

            // Within the loop, the variable is defined equal to the PHI node.
            // So, push a new scope for it and any values the body might set
            using( NamedValues.EnterScope( ) )
            {
                EmitBranchToNewBlock( "ForInScope" );

                // Emit the body of the loop.  This, like any other expression, can change the
                // current BB.  Note that we ignore the value computed by the body, but don't
                // allow an error.
                if( forInExpression.Body.Accept( this ) == null )
                {
                    return null;
                }

                Value stepValue = forInExpression.Step.Accept( this );
                if( stepValue == null )
                {
                    return null;
                }

                // Compute the end condition.
                Value endCondition = forInExpression.Condition.Accept( this );
                if( endCondition == null )
                {
                    return null;
                }

                // since the Alloca is created as a non-opaque pointer it is OK to just use the
                // ElementType. If full opaque pointer support was used, then the Lookup map
                // would need to include the type of the value allocated.
                var curVar = InstructionBuilder.Load( allocaVar.ElementType, allocaVar )
                                               .RegisterName( varName );
                var nextVar = InstructionBuilder.FAdd( curVar, stepValue )
                                                .RegisterName( "nextvar" );
                InstructionBuilder.Store( nextVar, allocaVar );

                // Convert condition to a bool by comparing non-equal to 0.0.
                endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, Context.CreateConstant( 0.0 ) )
                                                 .RegisterName( "loopcond" );

                // Create the "after loop" block and insert it.
                var afterBlock = function.AppendBasicBlock( "afterloop" );

                // Insert the conditional branch into the end of LoopEndBB.
                InstructionBuilder.Branch( endCondition, loopBlock, afterBlock );
                InstructionBuilder.PositionAtEnd( afterBlock );

                // for expression always returns 0.0 for consistency, there is no 'void'
                return Context.DoubleType.GetNullValue( );
            }
        }
        #endregion

        #region VarInExpression
        public override Value Visit( VarInExpression varInExpression )
        {
            using( NamedValues.EnterScope( ) )
            {
                EmitBranchToNewBlock( "VarInScope" );
                foreach( var localVar in varInExpression.LocalVariables )
                {
                    Alloca alloca = LookupVariable( localVar.Name );
                    Value initValue = Context.CreateConstant( 0.0 );
                    if( localVar.Initializer != null )
                    {
                        initValue = localVar.Initializer.Accept( this );
                    }

                    InstructionBuilder.Store( initValue, alloca );
                }

                return varInExpression.Body.Accept( this );
            }
        }
        #endregion

        private Alloca LookupVariable( string name )
        {
            if( !NamedValues.TryGetValue( name, out Alloca value ) )
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {name}" );
            }

            return value;
        }

        private void EmitBranchToNewBlock( string blockName )
        {
            var newBlock = InstructionBuilder.InsertBlock.ContainingFunction.AppendBasicBlock( blockName );
            InstructionBuilder.Branch( newBlock );
            InstructionBuilder.PositionAtEnd( newBlock );
        }

        #region InitializeModuleAndPassManager
        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            Module.Layout = JIT.TargetMachine.TargetData;
            FunctionPassManager = new FunctionPassManager( Module );
            FunctionPassManager.AddPromoteMemoryToRegisterPass( );

            if( !DisableOptimizations )
            {
                FunctionPassManager.AddInstructionCombiningPass( )
                                   .AddReassociatePass( )
                                   .AddGVNPass( )
                                   .AddCFGSimplificationPass( );
            }

            FunctionPassManager.Initialize( );
        }
        #endregion

        #region GetOrDeclareFunction

        // Retrieves a Function for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private IrFunction GetOrDeclareFunction( Prototype prototype )
        {
            var function = Module.GetFunction( prototype.Name );
            if( function != null )
            {
                return function;
            }

            var llvmSignature = Context.GetFunctionType( Context.DoubleType, prototype.Parameters.Select( _ => Context.DoubleType ) );
            var retVal = Module.AddFunction( prototype.Name, llvmSignature );

            int index = 0;
            foreach( var argId in prototype.Parameters )
            {
                retVal.Parameters[ index ].Name = argId.Name;
                ++index;
            }

            return retVal;
        }
        #endregion

        #region CloneAndRenameFunction
        private static FunctionDefinition CloneAndRenameFunction( FunctionDefinition definition )
        {
            // clone the definition with a new name, note that this is really
            // a shallow clone so there's minimal overhead for the cloning.
            var newSignature = new Prototype( definition.Signature.Location
                                            , definition.Signature.Name + "$impl"
                                            , definition.Signature.Parameters
                                            );

            var implDefinition = new FunctionDefinition( definition.Location
                                                       , newSignature
                                                       , definition.Body
                                                       , definition.LocalVariables.ToImmutableArray( )
                                                       );
            return implDefinition;
        }
        #endregion

        #region PrivateMembers
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Alloca> NamedValues = new ScopeStack<Alloca>( );
        private FunctionPassManager FunctionPassManager;
        private readonly bool DisableOptimizations;
        private BitcodeModule Module;
        private readonly KaleidoscopeJIT JIT = new KaleidoscopeJIT( );
        #endregion
    }
}
