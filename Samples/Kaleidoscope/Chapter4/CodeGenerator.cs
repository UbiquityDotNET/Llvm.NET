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
        public CodeGenerator( LanguageLevel level )
        {
            Context = new Context( );
            InitializeModuleAndPassManager( );
            InstructionBuilder = new InstructionBuilder( Context );
            JIT = new KaleidoscopeJIT( );
            NamedValues = new Dictionary<string, Value>( );
            FunctionProtoTypes = new PrototypeCollection( );
            ParserStack = new ReplParserStack( level );
        }

        public ReplParserStack ParserStack { get; }

        public Context Context { get; }

        public BitcodeModule Module { get; private set; }

        public FunctionPassManager FunctionPassManager { get; private set; }

        public InstructionBuilder InstructionBuilder { get; }

        public IDictionary<string, Value> NamedValues { get; }

        public KaleidoscopeJIT JIT { get; }

        public PrototypeCollection FunctionProtoTypes { get; }

        public void Dispose( )
        {
            Context.Dispose( );
        }

        public override Value VisitParenExpression( [NotNull] ParenExpressionContext context )
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
            if( NamedValues.TryGetValue( varName, out Value value ) )
            {
                return value;
            }

            throw new ArgumentException( nameof( context ) );
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

        protected override Value DefaultResult => null;

        private void InitializeModuleAndPassManager( )
        {
            Module = new BitcodeModule( Context, "Kaleidoscope" );
            FunctionPassManager = new FunctionPassManager( Module );
            FunctionPassManager.AddInstructionCombiningPass( )
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

        /// <summary>Delegate type to allow execution of a JIT'd TopLevelExpression</summary>
        /// <returns>Result of evaluating the expression</returns>
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        private delegate double AnonExpressionFunc( );

        private static int AnonNameIndex;
    }
}
