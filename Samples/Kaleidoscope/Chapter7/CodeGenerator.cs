// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Llvm.NET;
using Llvm.NET.Instructions;
using Llvm.NET.JIT;
using Llvm.NET.Transforms;
using Llvm.NET.Values;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope
{
    /// <summary>Static extension methods to perform LLVM IR Code generation from the Kaledoscope AST</summary>
    internal sealed class CodeGenerator
        : KaleidoscopeBaseVisitor<Value>
        , IDisposable
    {
        public CodeGenerator( LanguageLevel level )
        {
            Context = new Context( );
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
            JIT = new KaleidoscopeJIT( );
            NamedValues = new Dictionary<string, Alloca>( );
            FunctionPrototypes = new PrototypeCollection( );
            ParserStack = new ReplParserStack( level );
            FunctionModuleMap = new Dictionary<string, IJitModuleHandle>( );
        }

        public ReplParserStack ParserStack { get; }

        public Context Context { get; }

        public BitcodeModule Module { get; private set; }

        public FunctionPassManager FunctionPassManager { get; private set; }

        public InstructionBuilder InstructionBuilder { get; }

        public IDictionary<string, Alloca> NamedValues { get; }

        public KaleidoscopeJIT JIT { get; }

        public PrototypeCollection FunctionPrototypes { get; }

        public Dictionary<string, IJitModuleHandle> FunctionModuleMap { get; }

        public void Dispose( )
        {
            JIT.Dispose( );
            Context.Dispose( );
        }

        public override Value VisitParenExpression( [NotNull] ParenExpressionContext context )
        {
            return context.Expression.Accept( this );
        }

        public override Value VisitConstExpression( [NotNull] ConstExpressionContext context )
        {
            return Context.CreateConstant( context.Value );
        }

        public override Value VisitExternalDeclaration( [NotNull] ExternalDeclarationContext context )
        {
            return context.Signature.Accept( this );
        }

        public override Value VisitVariableExpression( [NotNull] VariableExpressionContext context )
        {
            string varName = context.Name;
            if( !NamedValues.TryGetValue( varName, out Alloca value ) )
            {
                throw new CodeGeneratorException( $"Unknown variable name: {context}" );
            }

            return InstructionBuilder.Load( value )
                                     .RegisterName( varName );
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
            var function = GetOrDeclareFunction( proto
                                               , isAnonymous: true
                                               );

            var (_, jitHandle) = DefineFunction( function, context.expression() );

            var nativeFunc = JIT.GetDelegateForFunction<AnonExpressionFunc>( proto.Identifier.Name );
            var retVal = Context.CreateConstant( nativeFunc( ) );
            JIT.RemoveModule( jitHandle );
            return retVal;
        }

        public override Value VisitExpression( [NotNull] ExpressionContext context )
        {
            // Expression: PrimaryExpression (op expression)*
            // the sub-expressions are in evaluation order
            var lhs = context.primaryExpression( ).Accept( this );
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
            var allocaVar = CreateEntryBlockAlloca( function, varName );

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

            // Start the PHI node with an entry for Start.
            var variable = InstructionBuilder.PhiNode( Context.DoubleType )
                                             .RegisterName( varName );

            variable.AddIncoming( startVal, preHeaderBlock );

            // Within the loop, the variable is defined equal to the PHI node.  If it
            // shadows an existing variable, we have to restore it, so save it now.
            NamedValues.TryGetValue( varName, out Alloca oldValue );
            NamedValues[ varName ] = allocaVar;

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

            // Compute the end condition.
            Value endCondition = context.EndExpression.Accept( this );
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

            // Add a new entry to the PHI node for the backedge.
            variable.AddIncoming( nextVar, loopEndBlock );

            // Restore the unshadowed variable.
            if( oldValue != null )
            {
                NamedValues[ varName ] = oldValue;
            }
            else
            {
                NamedValues.Remove( varName );
            }

            // for expr always returns 0.0 for consistency, there is no 'void'
            return Context.DoubleType.GetNullValue( );
        }

        public override Value VisitUnaryOpExpression( [NotNull] UnaryOpExpressionContext context )
        {
            // verify the operator was previously defined
            var opKind = ParserStack.GlobalState.GetUnaryOperatorInfo( context.Op ).Kind;
            if( opKind == OperatorKind.None )
            {
                throw new CodeGeneratorException( $"invalid unary operator {context.Op}" );
            }

            string calleeName = $"$unary{context.Op}";
            var function = GetFunction( calleeName );
            if( function == null )
            {
                throw new CodeGeneratorException( $"Unknown function reference {calleeName}" );
            }

            var arg = context.Rhs.Accept( this );
            return InstructionBuilder.Call( function, arg ).RegisterName( "calltmp" );
        }

        public override Value VisitBinaryPrototype( [NotNull] BinaryPrototypeContext context )
        {
            if( !ParserStack.GlobalState.TryAddOperator( context.Op, OperatorKind.InfixLeftAssociative, context.Precedence ) )
            {
                throw new CodeGeneratorException( "Cannot replace built-in operators" );
            }

            return GetOrDeclareFunction( new Prototype( context, context.GetPrototypeName( ) ) );
        }

        public override Value VisitUnaryPrototype( [NotNull] UnaryPrototypeContext context )
        {
            if( !ParserStack.GlobalState.TryAddOperator( context.Op, OperatorKind.PreFix, 0 ) )
            {
                throw new CodeGeneratorException( "Cannot replace built-in operators" );
            }

            return GetOrDeclareFunction( new Prototype( context, context.GetPrototypeName( ) ) );
        }

        public override Value VisitAssignmentExpression( [NotNull] AssignmentExpressionContext context )
        {
            var rhs = context.Value.Accept( this );
            if( rhs == null )
            {
                return null;
            }

            if( !NamedValues.TryGetValue( context.VariableName, out Alloca varSlot ) )
            {
                throw new CodeGeneratorException( $"Unknown variable name {context.VariableName}" );
            }

            InstructionBuilder.Store( rhs, varSlot );
            return rhs;
        }

        public override Value VisitVarInExpression( [NotNull] VarInExpressionContext context )
        {
            IList<Alloca> oldBindings = new List<Alloca>( );
            Function function = InstructionBuilder.InsertBlock.ContainingFunction;
            foreach( var initializer in context.Initiaizers )
            {
                Value initValue = Context.CreateConstant( 0.0 );
                if( initializer.Value != null )
                {
                    initValue = initializer.Value.Accept( this );
                }

                var alloca = CreateEntryBlockAlloca( function, initializer.Name );
                InstructionBuilder.Store( initValue, alloca );
                oldBindings.Add( NamedValues[ initializer.Name ] );
                NamedValues[ initializer.Name ] = alloca;
            }

            var bodyVal = context.Scope.Accept( this );
            if( bodyVal == null )
            {
                return null;
            }

            for( int i = 0; i < context.Initiaizers.Count; ++i )
            {
                var initializer = context.Initiaizers[ i ];
                NamedValues[ initializer.Name ] = oldBindings[ i ];
            }

            return bodyVal;
        }

        protected override Value DefaultResult => null;

        private Value EmitBinaryOperator( Value lhs, OpsymbolContext opSymbol, IParseTree rightTree )
        {
            var rhs = rightTree.Accept( this );
            if( lhs == null || rhs == null )
            {
                return null;
            }

            switch( opSymbol.Op )
            {
            case '<':
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case '^':
                {
                    var pow = GetOrDeclareFunction( new Prototype( "llvm.pow.f64", "value", "power" ) );
                    return InstructionBuilder.Call( pow, lhs, rhs )
                                             .RegisterName( "powtmp" );
                }

            case '+':
                return InstructionBuilder.FAdd( lhs, rhs ).RegisterName( "addtmp" );

            case '-':
                return InstructionBuilder.FSub( lhs, rhs ).RegisterName( "subtmp" );

            case '*':
                return InstructionBuilder.FMul( lhs, rhs ).RegisterName( "multmp" );

            case '/':
                return InstructionBuilder.FDiv( lhs, rhs ).RegisterName( "divtmp" );

            default:
                {
                    // User defined op?
                    var opKind = ParserStack.GlobalState.GetBinOperatorInfo( opSymbol.Op ).Kind;
                    if( opKind != OperatorKind.InfixLeftAssociative && opKind != OperatorKind.InfixRightAssociative )
                    {
                        throw new CodeGeneratorException( $"Invalid binary operator {opSymbol.Op}" );
                    }

                    string calleeName = $"$binary{opSymbol.Op}";
                    var function = GetFunction( calleeName );
                    if( function == null )
                    {
                        throw new CodeGeneratorException( $"Unknown function reference {calleeName}" );
                    }

                    return InstructionBuilder.Call( function, lhs, rhs ).RegisterName( "calltmp" );
                }
            }
        }

        private void InitializeModuleAndPassManager( )
        {
            Module = Context.CreateBitcodeModule( );
            FunctionPassManager = new FunctionPassManager( Module );
            FunctionPassManager.AddPromoteMemoryToRegisterPass()
                               .AddInstructionCombiningPass( )
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
            NamedValues.Clear( );
            foreach( var arg in function.Parameters )
            {
                var argSlot = CreateEntryBlockAlloca( function, arg.Name );
                InstructionBuilder.Store( arg, argSlot );
                NamedValues[ arg.Name ] = argSlot;
            }

            var funcReturn = body.Accept( this );
            if( funcReturn == null )
            {
                function.EraseFromParent( );
                return (null, default);
            }

            InstructionBuilder.Return( funcReturn );
            function.Verify( );

            FunctionPassManager.Run( function );
            var jitHandle = JIT.AddModule( Module );
            FunctionModuleMap.Add( function.Name, jitHandle );
            InitializeModuleAndPassManager( );
            return (function, jitHandle);
        }

        private static Alloca CreateEntryBlockAlloca( Function theFunction, string varName )
        {
            var tmpBldr = new InstructionBuilder( theFunction.EntryBlock );
            if( theFunction.EntryBlock.FirstInstruction != null )
            {
                tmpBldr.PositionBefore( theFunction.EntryBlock.FirstInstruction );
            }

            return tmpBldr.Alloca( theFunction.Context.DoubleType )
                          .RegisterName( varName );
        }

        /// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
        /// <returns>Result of evaluating the expression</returns>
        [UnmanagedFunctionPointer( System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private delegate double AnonExpressionFunc( );

        private static int AnonNameIndex;
    }
}
