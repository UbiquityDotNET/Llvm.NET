// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.JIT;
using Llvm.NET.Transforms;
using Llvm.NET.Values;

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace Kaleidoscope
{
    /// <summary>Performs LLVM IR Code generation from the Kaleidoscope AST</summary>
    internal sealed class CodeGenerator
        : AstVisitorBase<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        // <Initialization>
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
        // </Initialization>

        public void Dispose( )
        {
            JIT.Dispose( );
            Context.Dispose( );
        }

        // <Generate>
        public Value Generate( IAstNode ast )
        {
            // Prototypes, including extern are ignored as AST generation
            // adds them to the RuntimeState so that already has the declarations
            if( !( ast is FunctionDefinition definition ) )
            {
                return null;
            }

            Value retVal = null;

            // Anonymous functions are called immediately then removed from the JIT
            // so no point in setting them up as a lazy compilation item.
            if( definition.IsAnonymous )
            {
                var function = (Function)definition.Accept( this );
                var jitHandle = JIT.AddModule( function.ParentModule );
                var nativeFunc = JIT.GetFunctionDelegate<AnonExpressionFunc>( definition.Name );
                retVal = Context.CreateConstant( nativeFunc( ) );
                JIT.RemoveModule( jitHandle );
            }
            else
            {
                FunctionDefinition implDefinition = CloneAndRenameFunction( definition );

                // register the generator as a stub with the original source name
                JIT.AddLazyFunctionGenerator( definition.Name, ( ) =>
                {
                    InitializeModuleAndPassManager( );
                    var function = ( Function )implDefinition.Accept( this );
                    return (implDefinition.Name, function.ParentModule);
                } );
            }
            return retVal;
        }
        // </Generate>

        // <ConstantExpression>
        public override Value Visit( Kaleidoscope.Grammar.AST.ConstantExpression constant )
        {
            return Context.CreateConstant( constant.Value );
        }
        // </ConstantExpression>

        // <BinaryOperatorExpression>
        public override Value Visit( BinaryOperatorExpression binaryOperator )
        {
            var lhs = binaryOperator.Left.Accept( this );
            var rhs = binaryOperator.Right.Accept( this );

            switch( binaryOperator.Op )
            {
            case BuiltInOperatorKind.Less:
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case BuiltInOperatorKind.Pow:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow, lhs, rhs )
                                             .RegisterName( "powtmp" );
                }

            case BuiltInOperatorKind.Add:
                return InstructionBuilder.FAdd( lhs, rhs ).RegisterName( "addtmp" );

            case BuiltInOperatorKind.Subtract:
                return InstructionBuilder.FSub( lhs, rhs ).RegisterName( "subtmp" );

            case BuiltInOperatorKind.Multiply:
                return InstructionBuilder.FMul( lhs, rhs ).RegisterName( "multmp" );

            case BuiltInOperatorKind.Divide:
                return InstructionBuilder.FDiv( lhs, rhs ).RegisterName( "divtmp" );

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        // </BinaryOperatorExpression>

        // <FunctionCallExpression>
        public override Value Visit( FunctionCallExpression functionCall )
        {
            string targetName = functionCall.FunctionPrototype.Name;
            Function function;
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
        // </FunctionCallExpression>

        // <FunctionDefinition>
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
                    foreach( var arg in function.Parameters )
                    {
                        var argSlot = InstructionBuilder.Alloca( function.Context.DoubleType )
                                                        .RegisterName( arg.Name );
                        InstructionBuilder.Store( arg, argSlot );
                        NamedValues[ arg.Name ] = argSlot;
                    }

                    foreach( var local in definition.LocalVariables )
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
        // </FunctionDefinition>

        // <VariableReferenceExpression>
        public override Value Visit( VariableReferenceExpression reference )
        {
            if( !NamedValues.TryGetValue( reference.Name, out Alloca value ) )
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {reference.Name}" );
            }

            return InstructionBuilder.Load( value )
                                     .RegisterName( reference.Name );
        }
        // </VariableReferenceExpression>

        // <ConditionalExpression>
        public override Value Visit( ConditionalExpression conditionalExpression )
        {
            if( !NamedValues.TryGetValue( conditionalExpression.ResultVariable.Name, out Alloca result ) )
            {
                throw new CodeGeneratorException( $"ICE: allocation for compiler generated variable '{conditionalExpression.ResultVariable.Name}' not found!" );
            }

            var condition = conditionalExpression.Condition.Accept( this );
            if( condition == null )
            {
                return null;
            }

            var condBool = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, condition, Context.CreateConstant( 0.0 ) )
                                             .RegisterName( "ifcond" );

            var function = InstructionBuilder.InsertBlock.ContainingFunction;

            var thenBlock = Context.CreateBasicBlock( "then", function );
            var elseBlock = Context.CreateBasicBlock( "else" );
            var continueBlock = Context.CreateBasicBlock( "ifcont" );
            InstructionBuilder.Branch( condBool, thenBlock, elseBlock );

            // generate then block
            InstructionBuilder.PositionAtEnd( thenBlock );
            var thenValue = conditionalExpression.ThenExpression.Accept( this );
            if( thenValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( thenValue, result );
            InstructionBuilder.Branch( continueBlock );

            // capture the insert in case generating else adds new blocks
            thenBlock = InstructionBuilder.InsertBlock;

            // generate else block
            function.BasicBlocks.Add( elseBlock );
            InstructionBuilder.PositionAtEnd( elseBlock );
            var elseValue = conditionalExpression.ElseExpression.Accept( this );
            if( elseValue == null )
            {
                return null;
            }

            InstructionBuilder.Store( elseValue, result );
            InstructionBuilder.Branch( continueBlock );
            elseBlock = InstructionBuilder.InsertBlock;

            // generate continue block
            function.BasicBlocks.Add( continueBlock );
            InstructionBuilder.PositionAtEnd( continueBlock );
            return InstructionBuilder.Load( result )
                                     .RegisterName( "ifresult" );
        }
        // </ConditionalExpression>

        // <ForInExpression>
        public override Value Visit( ForInExpression forInExpression )
        {
            var function = InstructionBuilder.InsertBlock.ContainingFunction;
            string varName = forInExpression.LoopVariable.Name;
            if( !NamedValues.TryGetValue( varName, out Alloca allocaVar ) )
            {
                throw new CodeGeneratorException( $"ICE: For loop initializer variable allocation not found!" );
            }

            // Emit the start code first, without 'variable' in scope.
            Value startVal = null;
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

            // Make the new basic block for the loop header, inserting after current
            // block.
            var preHeaderBlock = InstructionBuilder.InsertBlock;
            var loopBlock = Context.CreateBasicBlock( "loop", function );

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

                var curVar = InstructionBuilder.Load( allocaVar )
                                               .RegisterName( varName );
                var nextVar = InstructionBuilder.FAdd( curVar, stepValue )
                                                .RegisterName( "nextvar" );
                InstructionBuilder.Store( nextVar, allocaVar );

                // Convert condition to a bool by comparing non-equal to 0.0.
                endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, Context.CreateConstant( 0.0 ) )
                                                 .RegisterName( "loopcond" );

                // Create the "after loop" block and insert it.
                var loopEndBlock = InstructionBuilder.InsertBlock;
                var afterBlock = Context.CreateBasicBlock( "afterloop", function );

                // Insert the conditional branch into the end of LoopEndBB.
                InstructionBuilder.Branch( endCondition, loopBlock, afterBlock );
                InstructionBuilder.PositionAtEnd( afterBlock );

                // for expression always returns 0.0 for consistency, there is no 'void'
                return Context.DoubleType.GetNullValue( );
            }
        }
        // </ForInExpression>

        // <VarInExpression>
        public override Value Visit( VarInExpression varInExpression )
        {
            using( NamedValues.EnterScope( ) )
            {
                EmitBranchToNewBlock( "VarInScope" );
                Function function = InstructionBuilder.InsertBlock.ContainingFunction;
                foreach( var localVar in varInExpression.LocalVariables )
                {
                    if( !NamedValues.TryGetValue( localVar.Name, out Alloca alloca ) )
                    {
                        throw new CodeGeneratorException( $"ICE: Missing allocation for local variable {localVar.Name}" );
                    }

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
        // </VarInExpression>

        // <AssignmentExpression>
        public override Value Visit( AssignmentExpression assignment )
        {
            var targetAlloca = NamedValues[ assignment.Target.Name ];
            var value = assignment.Value.Accept( this );
            InstructionBuilder.Store( value, targetAlloca );
            return value;
        }
        // </AssignmentExpression>

        private void EmitBranchToNewBlock( string blockName )
        {
            var newBlock = InstructionBuilder.InsertBlock.ContainingFunction.AppendBasicBlock( blockName );
            InstructionBuilder.Branch( newBlock );
            InstructionBuilder.PositionAtEnd( newBlock );
        }

        // <InitializeModuleAndPassManager>
        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            Module.Layout = JIT.TargetMachine.TargetData;
            FunctionPassManager = new FunctionPassManager( Module )
                                      .AddPromoteMemoryToRegisterPass( );

            if( !DisableOptimizations )
            {
                FunctionPassManager.AddInstructionCombiningPass( )
                                   .AddReassociatePass( )
                                   .AddGVNPass( )
                                   .AddCFGSimplificationPass( );
            }

            FunctionPassManager.Initialize( );
        }
        // </InitializeModuleAndPassManager>

        // <GetOrDeclareFunction>
        // Retrieves a Function" for a prototype from the current module if it exists,
        // otherwise declares the function and returns the newly declared function.
        private Function GetOrDeclareFunction( Prototype prototype )
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
        // </GetOrDeclareFunction>

        // <CloneAndRenameFunction>
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
        // </CloneAndRenameFunction>

        // <PrivateMembers>
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Alloca> NamedValues = new ScopeStack<Alloca>( );
        private FunctionPassManager FunctionPassManager;
        private readonly bool DisableOptimizations;
        private BitcodeModule Module;
        private readonly KaleidoscopeJIT JIT = new KaleidoscopeJIT( );
        private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );

        /// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
        /// <returns>Result of evaluating the expression</returns>
        [UnmanagedFunctionPointer( System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private delegate double AnonExpressionFunc( );
        // </PrivateMembers>
    }
}
