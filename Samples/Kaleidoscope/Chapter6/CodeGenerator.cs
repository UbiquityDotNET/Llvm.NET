// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
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

namespace Kaleidoscope.Chapter6
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
            if( globalState.LanguageLevel > LanguageLevel.UserDefinedOperators )
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

        public void Dispose( )
        {
            JIT.Dispose( );
            Module.Dispose( );
            Context.Dispose( );
        }

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

                InitializeModuleAndPassManager( );

                // Destroy any previously generated module for this function.
                // This allows re-definition as the new module will provide the
                // implementation. This is needed, otherwise both the MCJIT
                // and OrcJit engines will resolve to the original module, despite
                // claims to the contrary in the official tutorial text. (Though,
                // to be fair it may have been true in the original JIT and might
                // still be true for the interpreter)
                if( FunctionModuleMap.Remove( definition.Name, out IJitModuleHandle handle ) )
                {
                    JIT.RemoveModule( handle );
                }

                var function = ( Function )definition.Accept( this );
                var jitHandle = JIT.AddModule( function.ParentModule );
                if( definition.IsAnonymous )
                {
                    var nativeFunc = JIT.GetFunctionDelegate<KaleidoscopeJIT.CallbackHandler0>( function.Name );
                    var retVal = Context.CreateConstant( nativeFunc( ) );
                    JIT.RemoveModule( jitHandle );
                    return retVal;
                }

                FunctionModuleMap.Add( function.Name, jitHandle );
                return function;
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
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case BuiltInOperatorKind.Pow:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow, binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) )
                                             .RegisterName( "powtmp" );
                }

            case BuiltInOperatorKind.Add:
                return InstructionBuilder.FAdd( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "addtmp" );

            case BuiltInOperatorKind.Subtract:
                return InstructionBuilder.FSub( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "subtmp" );

            case BuiltInOperatorKind.Multiply:
                return InstructionBuilder.FMul( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "multmp" );

            case BuiltInOperatorKind.Divide:
                return InstructionBuilder.FDiv( binaryOperator.Left.Accept( this ), binaryOperator.Right.Accept( this ) ).RegisterName( "divtmp" );

            default:
                throw new CodeGeneratorException( $"ICE: Invalid binary operator {binaryOperator.Op}" );
            }
        }
        #endregion

        #region FunctionCallExpression
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
                        NamedValues[ param.Name ] = function.Parameters[ param.Index ];
                    }

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
            if( !NamedValues.TryGetValue( reference.Name, out Value value ) )
            {
                // Source input is validated by the parser and AstBuilder, therefore
                // this is the result of an internal error in the generator rather
                // then some sort of user error.
                throw new CodeGeneratorException( $"ICE: Unknown variable name: {reference.Name}" );
            }

            return value;
        }
        #endregion

        #region ConditionalExpression
        public override Value Visit( ConditionalExpression conditionalExpression )
        {
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

            // generate then block
            InstructionBuilder.PositionAtEnd( thenBlock );
            var thenValue = conditionalExpression.ThenExpression.Accept( this );
            if( thenValue == null )
            {
                return null;
            }

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

            InstructionBuilder.Branch( continueBlock );
            elseBlock = InstructionBuilder.InsertBlock;

            // generate continue block
            function.BasicBlocks.Add( continueBlock );
            InstructionBuilder.PositionAtEnd( continueBlock );
            var phiNode = InstructionBuilder.PhiNode( function.Context.DoubleType )
                                            .RegisterName( "iftmp" );

            phiNode.AddIncoming( thenValue, thenBlock );
            phiNode.AddIncoming( elseValue, elseBlock );
            return phiNode;
        }
        #endregion

        #region ForInExpression
        public override Value Visit( ForInExpression forInExpression )
        {
            var function = InstructionBuilder.InsertBlock.ContainingFunction;
            string varName = forInExpression.LoopVariable.Name;

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

            // Make the new basic block for the loop header, inserting after current
            // block.
            var preHeaderBlock = InstructionBuilder.InsertBlock;
            var loopBlock = function.AppendBasicBlock( "loop" );

            // Insert an explicit fall through from the current block to the loopBlock.
            InstructionBuilder.Branch( loopBlock );

            // Start insertion in loopBlock.
            InstructionBuilder.PositionAtEnd( loopBlock );

            // Start the PHI node with an entry for Start.
            var variable = InstructionBuilder.PhiNode( Context.DoubleType )
                                             .RegisterName( varName );

            variable.AddIncoming( startVal, preHeaderBlock );

            // Within the loop, the variable is defined equal to the PHI node.
            // So, push a new scope for it and any values the body might set
            using( NamedValues.EnterScope( ) )
            {
                NamedValues[ varName ] = variable;

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

                var nextVar = InstructionBuilder.FAdd( variable, stepValue)
                                                .RegisterName( "nextvar" );

                // Compute the end condition.
                Value endCondition = forInExpression.Condition.Accept( this );
                if( endCondition == null )
                {
                    return null;
                }

                // Convert condition to a bool by comparing non-equal to 0.0.
                endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, Context.CreateConstant( 0.0 ) )
                                                 .RegisterName( "loopcond" );

                // Create the "after loop" block and insert it.
                var loopEndBlock = InstructionBuilder.InsertBlock;
                var afterBlock = function.AppendBasicBlock( "afterloop" );

                // Insert the conditional branch into the end of LoopEndBB.
                InstructionBuilder.Branch( endCondition, loopBlock, afterBlock );
                InstructionBuilder.PositionAtEnd( afterBlock );

                // Add a new entry to the PHI node for the back-edge.
                variable.AddIncoming( nextVar, loopEndBlock );

                // for expression always returns 0.0 for consistency, there is no 'void'
                return Context.DoubleType.GetNullValue( );
            }
        }
        #endregion

        #region InitializeModuleAndPassManager
        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            Module.Layout = JIT.TargetMachine.TargetData;
            FunctionPassManager = new FunctionPassManager( Module );

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
        #endregion

        #region PrivateMembers
        private readonly DynamicRuntimeState RuntimeState;
        private readonly Context Context;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Value> NamedValues = new ScopeStack<Value>( );
        private FunctionPassManager FunctionPassManager;
        private readonly bool DisableOptimizations;
        private BitcodeModule Module;
        private readonly KaleidoscopeJIT JIT = new KaleidoscopeJIT( );
        private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );
        #endregion
    }
}
