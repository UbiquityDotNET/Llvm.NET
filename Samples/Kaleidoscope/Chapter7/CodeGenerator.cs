// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Antlr4.Runtime.Misc;
using Kaleidoscope.Grammar;
using Llvm.NET;
using Llvm.NET.Instructions;
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
        public CodeGenerator( )
        {
            Context = new Context( );
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
            JIT = new KaleidoscopeJIT( );
            NamedValues = new Dictionary<string, Alloca>( );
            FunctionProtoTypes = new PrototypeCollection( );
        }

        public Context Context { get; }

        public BitcodeModule Module { get; private set; }

        public FunctionPassManager FunctionPassManager { get; private set; }

        public InstructionBuilder InstructionBuilder { get; }

        public IDictionary<string, Alloca> NamedValues { get; }

        public KaleidoscopeJIT JIT { get; }

        public PrototypeCollection FunctionProtoTypes { get; }

        public void Dispose( )
        {
            Context.Dispose( );
        }

        public override Value VisitParenExPression( [NotNull] ParenExPressionContext context )
        {
            return context.GetExpression( ).Accept( this );
        }

        public override Value VisitConstExpression( [NotNull] ConstExpressionContext context )
        {
            return Context.CreateConstant( context.GetValue() );
        }

        public override Value VisitVariableExpression( [NotNull] VariableExpressionContext context )
        {
            string varName = context.GetName( );
            if( !NamedValues.TryGetValue( varName, out Alloca value ) )
            {
                throw new ArgumentException( "Unknown variable name", nameof( context ) );
            }

            return InstructionBuilder.Load( value )
                                     .RegisterName( varName );
        }

        public override Value VisitBinaryOpExpression( [NotNull] BinaryOpExpressionContext context )
        {
            var (lhsExpr, op, rhsExpr) = context;

            var lhs = lhsExpr.Accept( this );
            var rhs = rhsExpr.Accept( this );
            if( lhs == null || rhs == null )
            {
                return null;
            }

            switch( op )
            {
            case '<':
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrLessThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case '>':
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrGreaterThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case '^':
                {
                    var proto = CreateSyntheticProtoTypeContext( "llvm.pow.f64", "value", "power" );
                    var pow = DeclareFunction( proto );
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
                throw new ArgumentException( $"Invalid binary operator {op}", nameof( context ) );
            }
        }

        public override Value VisitFunctionCallExpression( [NotNull] FunctionCallExpressionContext context )
        {
            var (calleeName, argExprs) = context;

            var function = GetFunction( calleeName );
            if( function == null )
            {
                throw new ArgumentException( $"Unknown function reference {calleeName}", nameof( context ) );
            }

            var args = argExprs.Select( ctx => ctx.Accept( this ) ).ToArray( );
            return InstructionBuilder.Call( function, args ).RegisterName("calltmp");
        }

        public override Value VisitPrototype( [NotNull] PrototypeContext context )
        {
            var (name, parameters) = context;
            var retVal = DeclareFunction( (name, parameters) );
            FunctionProtoTypes.AddOrReplaceItem( (name, parameters) );
            return retVal;
        }

        public override Value VisitFunctionDefinition( [NotNull] FunctionDefinitionContext context )
        {
            var (signature, body) = context;
            var funcAndHandle = FunctionDefinition( signature, body );
            return funcAndHandle.Function;
        }

        public override Value VisitTopLevelExpression( [NotNull] TopLevelExpressionContext context )
        {
            string name = $"anon_expr_{AnonNameIndex++}";
            var proto = CreateSyntheticProtoTypeContext( name );

            var def = DeclareFunction( proto, persistentSymbol: false );
            var function = FunctionDefinition( proto, context.expression() );

            var nativeFunc = JIT.GetDelegateForFunction<AnonExpressionFunc>( name );
            var retVal = Context.CreateConstant( nativeFunc( ) );
            JIT.RemoveModule( function.JitHandle );
            return retVal;
        }

        public override Value VisitConditionalExpression( [NotNull] ConditionalExpressionContext context )
        {
            var (condExpr, thenExpr, elseExpr) = context;
            var condition = condExpr.Accept( this );
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
            var thenValue = thenExpr.Accept( this );
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
            var elseValue = elseExpr.Accept( this );
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
            var (startExpr, endExpr, stepExpr, bodyExpr) = context;

            var function = InstructionBuilder.InsertBlock.ContainingFunction;
            string varName = startExpr.identifier( ).GetName( );
            var allocaVar = CreateEntryBlockAlloca( function, varName );

            // Emit the start code first, without 'variable' in scope.
            Value startVal = null;
            if( startExpr.expression( ) != null )
            {
                startVal = startExpr.expression().Accept( this );
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
            if( bodyExpr.Accept(this) == null)
            {
                return null;
            }

            Value stepValue = Context.CreateConstant( 1.0 );

            // DEBUG: How does ANTLR represent optional context (Null or IsEmpty == true)
            if( stepExpr != null )
            {
                stepValue = stepExpr.Accept( this );
                if( stepValue == null )
                {
                    return null;
                }
            }

            // Compute the end condition.
            Value endCondition =endExpr.Accept( this );
            if( endCondition == null )
            {
                return null;
            }

            var curVar = InstructionBuilder.Load( allocaVar )
                                           .RegisterName( varName );
            var nextVar = InstructionBuilder.FAdd( curVar, stepValue)
                                            .RegisterName( "nextvar" );
            InstructionBuilder.Store( nextVar, allocaVar );

            // Convert condition to a bool by comparing non-equal to 0.0.
            endCondition = InstructionBuilder.Compare( RealPredicate.OrderedAndNotEqual, endCondition, Context.CreateConstant( 1.0 ) )
                                             .RegisterName("loopcond");

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

        public override Value VisitAssignmentExpression( [NotNull] AssignmentExpressionContext context )
        {
            var (varName, value) = context;
            var rhs = value.Accept( this );
            if( rhs == null )
            {
                return null;
            }

            if( !NamedValues.TryGetValue( varName, out Alloca varSlot ) )
            {
                throw new ArgumentException( "Unknown variable name" );
            }

            InstructionBuilder.Store( rhs, varSlot );
            return rhs;
        }

        public override Value VisitVarInExpression( [NotNull] VarInExpressionContext context )
        {
            var (initializers, scope) = context;

            IList<Alloca> oldBindings = new List<Alloca>( );
            Function function = InstructionBuilder.InsertBlock.ContainingFunction;
            foreach( var initializer in initializers )
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

            var bodyVal = scope.Accept( this );
            if( bodyVal == null )
            {
                return null;
            }

            for( int i = 0; i < initializers.Count; ++i )
            {
                var initializer = initializers[ i ];
                NamedValues[ initializer.Name ] = oldBindings[ i ];
            }

            return bodyVal;
        }

        protected override Value DefaultResult => null;

        private void InitializeModuleAndPassManager( )
        {
            Module = new BitcodeModule( Context, "Kaleidoscope" );
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
            Function retVal = Module.GetFunction( name );
            if( retVal != null )
            {
                return retVal;
            }

            if( FunctionProtoTypes.TryGetValue( name, out var signature ) )
            {
                return DeclareFunction( signature );
            }

            return null;
        }

        private Function DeclareFunction( PrototypeContext signature, bool persistentSymbol = true )
        {
            var (name, parameters) = signature;
            return DeclareFunction( (name, parameters), persistentSymbol );
        }

        private Function DeclareFunction( (string Name, IList<string> Parameters) signature, bool persistentSymbol = true )
        {
            var (name, argNames) = signature;

            var llvmSignature = Context.GetFunctionType( Context.DoubleType, argNames.Select( _ => Context.DoubleType ) );

            var retVal = Module.AddFunction( name, llvmSignature );
            retVal.Linkage( Linkage.External );

            int index = 0;
            foreach( string argName in argNames )
            {
                retVal.Parameters[ index ].Name = argName;
                ++index;
            }

            if( persistentSymbol )
            {
                FunctionProtoTypes.AddOrReplaceItem( signature );
            }

            return retVal;
        }

        private (Function Function, int JitHandle) FunctionDefinition( PrototypeContext signature, ExpressionContext body )
        {
            var (name, parameters) = signature;
            return FunctionDefinition( (name, parameters), body );
        }

        private (Function Function, int JitHandle) FunctionDefinition( (string Name, IList<string> Parameters) signature, ExpressionContext body )
        {
            var (name, argNames) = signature;

            var function = GetFunction( name );
            if( function == null )
            {
                function = DeclareFunction( signature );
            }

            if( function == null )
            {
                return (null, default);
            }

            if( !function.IsDeclaration )
            {
                throw new ArgumentException( $"Function {name} cannot be redefined", nameof( signature ) );
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
            Trace.TraceInformation( function.ToString( ) );

            FunctionPassManager.Run( function );
            int jitHandle = JIT.AddModule( Module );
            InitializeModuleAndPassManager( );
            return (function, jitHandle);
        }

        private (string Name, IList<string> Parameters) CreateSyntheticProtoTypeContext( string name, params string[] argNames )
        {
            return (name, argNames);
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
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        private delegate double AnonExpressionFunc( );

        private static int AnonNameIndex;
    }
}
