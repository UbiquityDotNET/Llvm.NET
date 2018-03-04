// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.JIT;
using Llvm.NET.Transforms;
using Llvm.NET.Values;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace Kaleidoscope
{
    /// <summary>Static extension methods to perform LLVM IR Code generation from the Kaleidoscope AST</summary>
    internal sealed class CodeGenerator
        : KaleidoscopeBaseVisitor<Value>
        , IDisposable
        , IKaleidoscopeCodeGenerator<Value>
    {
        public CodeGenerator( DynamicRuntimeState globalState )
        {
            RuntimeState = globalState;
            Context = new Context( );
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
            JIT = new KaleidoscopeJIT( );
            FunctionPrototypes = new PrototypeCollection( );
            FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );
            NamedValues = new ScopeStack<Value>( );
        }

        public bool DisableOptimizations { get; set; }

        public void Dispose( )
        {
            JIT.Dispose( );
            Context.Dispose( );
        }

        public Value Generate( Parser parser, IParseTree tree, DiagnosticRepresentations additionalDiagnostics )
        {
            if( parser.NumberOfSyntaxErrors > 0 )
            {
                return null;
            }

            return Visit( tree );
        }

        public override Value VisitParenExpression( [NotNull] ParenExpressionContext context )
        {
            return context.Expression.Accept( this );
        }

        public override Value VisitConstExpression( [NotNull] ConstExpressionContext context )
        {
            return Context.CreateConstant( context.Value );
        }

        public override Value VisitVariableExpression( [NotNull] VariableExpressionContext context )
        {
            string varName = context.Name;
            if( !NamedValues.TryGetValue( varName, out Value value ) )
            {
                throw new CodeGeneratorException( $"Unknown variable name: {context}" );
            }

           return value;
        }

        public override Value VisitFunctionCallExpression( [NotNull] FunctionCallExpressionContext context )
        {
            var function = GetFunction( context.CaleeName );
            if( function == null )
            {
                throw new CodeGeneratorException( $"function '{context.CaleeName}' is unknown" );
            }

            var args = context.Args.Select( ctx => ctx.Accept( this ) ).ToArray( );
            return InstructionBuilder.Call( function, args ).RegisterName( "calltmp" );
        }

        public override Value VisitExternalDeclaration( [NotNull] ExternalDeclarationContext context )
        {
            return context.Signature.Accept( this );
        }

        public override Value VisitFunctionPrototype( [NotNull] FunctionPrototypeContext context )
        {
            return GetOrDeclareFunction( new Prototype( context ) );
        }

        public override Value VisitFunctionDefinition( [NotNull] FunctionDefinitionContext context )
        {
            return DefineFunction( ( Function )context.Signature.Accept( this )
                                 , context.BodyExpression
                                 ).Function;
        }

        public override Value VisitTopLevelExpression( [NotNull] TopLevelExpressionContext context )
        {
            var proto = new Prototype( $"anon_expr_{AnonNameIndex++}" );
            var function = GetOrDeclareFunction( proto, isAnonymous: true );

            var (_, jitHandle) = DefineFunction( function, context.expression( ) );

            var nativeFunc = JIT.GetDelegateForFunction<AnonExpressionFunc>( proto.Identifier.Name );
            var retVal = Context.CreateConstant( nativeFunc( ) );
            JIT.RemoveModule( jitHandle );
            return retVal;
        }

        public override Value VisitExpression( [NotNull] ExpressionContext context )
        {
            // Expression: PrimaryExpression (op expression)*
            // the sub-expressions are in evaluation order
            var lhs = context.Atom.Accept( this );
            foreach( var (op, rhs) in context.OperatorExpressions )
            {
                lhs = EmitBinaryOperator( lhs, op, rhs );
            }

            return lhs;
        }

        public override Value VisitConditionalExpression( [NotNull] ConditionalExpressionContext context )
        {
            var condition = context.Condition.Accept( this );
            if( condition == null )
            {
                return null;
            }

            var condBool = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, condition, Context.CreateConstant( 0.0 ) )
                                             .RegisterName( "ifcond" );

            var function = InstructionBuilder.InsertBlock.ContainingFunction;

            var thenBlock = Context.CreateBasicBlock( "then", function );
            var elseBlock = Context.CreateBasicBlock( "else" );
            var phiMergeBlock = Context.CreateBasicBlock( "ifcont" );
            InstructionBuilder.Branch( condBool, thenBlock, elseBlock );

            // generate then block
            InstructionBuilder.PositionAtEnd( thenBlock );
            var thenValue = context.ThenExpression.Accept( this );
            if( thenValue == null )
            {
                return null;
            }

            InstructionBuilder.Branch( phiMergeBlock );

            // capture the insert in case generating thenExpression adds new blocks
            thenBlock = InstructionBuilder.InsertBlock;

            // generate else block
            function.BasicBlocks.Add( elseBlock );
            InstructionBuilder.PositionAtEnd( elseBlock );
            var elseValue = context.ElseExpression.Accept( this );
            if( elseValue == null )
            {
                return null;
            }

            InstructionBuilder.Branch( phiMergeBlock );
            elseBlock = InstructionBuilder.InsertBlock;

            // generate PHI merge block
            function.BasicBlocks.Add( phiMergeBlock );
            InstructionBuilder.PositionAtEnd( phiMergeBlock );
            var phiNode = InstructionBuilder.PhiNode( function.Context.DoubleType )
                                            .RegisterName( "iftmp" );

            phiNode.AddIncoming( thenValue, thenBlock );
            phiNode.AddIncoming( elseValue, elseBlock );
            return phiNode;
        }

        /*
        // Output for-loop as:
        //   ...
        //   start = startexpr
        //   goto loop
        // loop:
        //   variable = phi [start, loopheader], [nextvariable, loopend]
        //   ...
        //   bodyexpr
        //   ...
        // loopend:
        //   step = stepexpr
        //   nextvariable = variable + step
        //   endcond = endexpr
        //   br endcond, loop, endloop
        // outloop:
        */
        public override Value VisitForExpression( [NotNull] ForExpressionContext context )
        {
            var function = InstructionBuilder.InsertBlock.ContainingFunction;
            string varName = context.Initializer.Name;

            // Emit the start code first, without 'variable' in scope.
            Value startVal = null;
            if( context.Initializer.Value != null )
            {
                startVal = context.Initializer.Value.Accept( this );
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
            var loopBlock = Context.CreateBasicBlock( "loop", function );

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

                // Emit the body of the loop.  This, like any other expr, can change the
                // current BB.  Note that we ignore the value computed by the body, but don't
                // allow an error.
                if( context.BodyExpression.Accept( this ) == null )
                {
                    return null;
                }

                Value stepValue = Context.CreateConstant( 1.0 );

                if( context.StepExpression != null )
                {
                    stepValue = context.StepExpression.Accept( this );
                    if( stepValue == null )
                    {
                        return null;
                    }
                }

                var nextVar = InstructionBuilder.FAdd( variable, stepValue)
                                                .RegisterName( "nextvar" );

                // Compute the end condition.
                Value endCondition = context.EndExpression.Accept( this );
                if( endCondition == null )
                {
                    return null;
                }

                // Convert condition to a bool by comparing non-equal to 0.0.
                endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, Context.CreateConstant( 0.0 ) )
                                                 .RegisterName( "loopcond" );

                // Create the "after loop" block and insert it.
                var loopEndBlock = InstructionBuilder.InsertBlock;
                var afterBlock = Context.CreateBasicBlock( "afterloop", function );

                // Insert the conditional branch into the end of LoopEndBB.
                InstructionBuilder.Branch( endCondition, loopBlock, afterBlock );
                InstructionBuilder.PositionAtEnd( afterBlock );

                // Add a new entry to the PHI node for the back-edge.
                variable.AddIncoming( nextVar, loopEndBlock );

                // for expr always returns 0.0 for consistency, there is no 'void'
                return Context.DoubleType.GetNullValue( );
            }
        }

        protected override Value DefaultResult => null;

        private Value EmitBinaryOperator( Value lhs, BinaryopContext op, IParseTree rightTree )
        {
            var rhs = rightTree.Accept( this );
            if( lhs == null || rhs == null )
            {
                return null;
            }

            switch( op.Token.Type )
            {
            case LEFTANGLE:
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case CARET:
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow, lhs, rhs )
                                             .RegisterName( "powtmp" );
                }

            case PLUS:
                return InstructionBuilder.FAdd( lhs, rhs ).RegisterName( "addtmp" );

            case MINUS:
                return InstructionBuilder.FSub( lhs, rhs ).RegisterName( "subtmp" );

            case ASTERISK:
                return InstructionBuilder.FMul( lhs, rhs ).RegisterName( "multmp" );

            case SLASH:
                return InstructionBuilder.FDiv( lhs, rhs ).RegisterName( "divtmp" );

            default:
                throw new CodeGeneratorException( $"Invalid binary operator {op.Token.Text}" );
            }
        }

        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            FunctionPassManager = new FunctionPassManager( Module );
            FunctionPassManager.AddInstructionCombiningPass( )
                               .AddReassociatePass( )
                               .AddGVNPass( )
                               .AddCFGSimplificationPass( )
                               .Initialize( );
        }

        private Function GetFunction( string name )
        {
            if( FunctionPrototypes.TryGetValue( name, out var signature ) )
            {
                return GetOrDeclareFunction( signature );
            }

            Function retVal = Module.GetFunction( name );
            if( retVal != null )
            {
                return retVal;
            }

            return null;
        }

        private Function GetOrDeclareFunction( Prototype prototype, bool isAnonymous = false )
        {
            var function = Module.GetFunction( prototype.Identifier.Name );
            if( function != null )
            {
                return function;
            }

            var llvmSignature = Context.GetFunctionType( Context.DoubleType, prototype.Parameters.Select( _ => Context.DoubleType ) );

            var retVal = Module.AddFunction( prototype.Identifier.Name, llvmSignature );
            retVal.Linkage( Linkage.External );

            int index = 0;
            foreach( var argId in prototype.Parameters )
            {
                retVal.Parameters[ index ].Name = argId.Name;
                ++index;
            }

            if( !isAnonymous )
            {
                FunctionPrototypes.AddOrReplaceItem( prototype );
            }

            return retVal;
        }

        private (Function Function, IJitModuleHandle JitHandle) DefineFunction( Function function, ExpressionContext body )
        {
            if( !function.IsDeclaration )
            {
                throw new CodeGeneratorException( $"Function {function.Name} cannot be redefined in the same module" );
            }

            // Destroy any previously generated module for this function.
            // This allows re-definition as the new module will provide the
            // implementation. This is needed, otherwise both the MCJIT
            // and OrcJit engines will resolve to the original module, despite
            // claims to the contrary in the official tutorial text. (Though,
            // to be fare it may have been true in the original JIT and might
            // still be true for the interpreter)
            if( FunctionModuleMap.Remove( function.Name, out IJitModuleHandle handle ) )
            {
                JIT.RemoveModule( handle );
            }

            var basicBlock = function.AppendBasicBlock( "entry" );
            InstructionBuilder.PositionAtEnd( basicBlock );
            using( NamedValues.EnterScope( ) )
            {
                foreach( var arg in function.Parameters )
                {
                    NamedValues[ arg.Name ] = arg;
                }

                var funcReturn = body.Accept( this );
                if( funcReturn == null )
                {
                    function.EraseFromParent( );
                    return (null, default);
                }

                InstructionBuilder.Return( funcReturn );
                function.Verify( );
            }

            if( !DisableOptimizations )
            {
                FunctionPassManager.Run( function );
            }

            var jitHandle = JIT.AddModule( Module );
            FunctionModuleMap.Add( function.Name, jitHandle );
            InitializeModuleAndPassManager( );
            return (function, jitHandle);
        }

        // <PrivateMembers>
        private readonly DynamicRuntimeState RuntimeState;
        private static int AnonNameIndex;
        private readonly Context Context;
        private BitcodeModule Module;
        private readonly InstructionBuilder InstructionBuilder;
        private readonly ScopeStack<Value> NamedValues;
        private readonly KaleidoscopeJIT JIT;
        private readonly Dictionary<string, IJitModuleHandle> FunctionModuleMap;
        private FunctionPassManager FunctionPassManager;
        private readonly PrototypeCollection FunctionPrototypes;

        /// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
        /// <returns>Result of evaluating the expression</returns>
        [UnmanagedFunctionPointer( System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private delegate double AnonExpressionFunc( );
        // </PrivateMembers>
    }
}
