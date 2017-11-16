// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Kaleidoscope.Grammar;
using Llvm.NET;
using Llvm.NET.Instructions;
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
            Module = new BitcodeModule( Context, "Kaleidoscope" );
            InstructionBuilder = new InstructionBuilder( Context );
            NamedValues = new Dictionary<string, Value>( );
            ParserStack = new ReplParserStack( level );
        }

        public Context Context { get; }

        public BitcodeModule Module { get; }

        public InstructionBuilder InstructionBuilder { get; }

        public IDictionary<string, Value> NamedValues { get; }

        public ReplParserStack ParserStack { get; }

        public void Dispose( )
        {
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
            if( NamedValues.TryGetValue( varName, out Value value ) )
            {
                return value;
            }

            throw new ArgumentException( nameof( context ) );
        }

        public override Value VisitBinaryOpExpression( [NotNull] BinaryOpExpressionContext context )
        {
            var lhs = context.Lhs.Accept( this );
            var rhs = context.Rhs.Accept( this );
            if( lhs == null || rhs == null )
            {
                return null;
            }

            switch( context.Op )
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
                    var (Name, Parameters) = CreateSyntheticPrototype( "llvm.pow.f64", "value", "power" );
                    var pow = GetOrDeclareFunction( Name, Parameters );
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
                throw new ArgumentException( $"Invalid binary operator {context.Op}", nameof( context ) );
            }
        }

        public override Value VisitFunctionCallExpression( [NotNull] FunctionCallExpressionContext context )
        {
            var function = GetFunction( context.CaleeName );
            if( function == null )
            {
                throw new ArgumentException( $"Unknown function reference {context.CaleeName}", nameof( context ) );
            }

            var args = context.Args.Select( ctx => ctx.Accept( this ) ).ToArray( );
            return InstructionBuilder.Call( function, args ).RegisterName("calltmp");
        }

        public override Value VisitPrototype( [NotNull] PrototypeContext context )
        {
            return GetOrDeclareFunction( context );
        }

        public override Value VisitFunctionDefinition( [NotNull] FunctionDefinitionContext context )
        {
            var function = (Function)context.Signature.Accept( this );
            return DefineFunction( function, context.BodyExpression );
        }

        public override Value VisitTopLevelExpression( [NotNull] TopLevelExpressionContext context )
        {
            var (Name, Parameters) = CreateSyntheticPrototype( $"anon_expr_{AnonNameIndex++}" );

            var function = GetOrDeclareFunction( Name, Parameters );
            return DefineFunction( function, context.expression() );
        }

        protected override Value DefaultResult => null;

        private Function GetFunction( string name )
        {
            return Module.GetFunction( name );
        }

        private Function GetOrDeclareFunction( PrototypeContext signature )
        {
            return GetOrDeclareFunction( GetPrototypeName( signature ), signature.Parameters );
        }

        private Function GetOrDeclareFunction( string name, IReadOnlyList<string> argNames )
        {
            var function = GetFunction( name );
            if( function != null )
            {
                return function;
            }

            var llvmSignature = Context.GetFunctionType( Context.DoubleType, argNames.Select( _ => Context.DoubleType ) );

            var retVal = Module.AddFunction( name, llvmSignature );
            retVal.Linkage( Linkage.External );

            int index = 0;
            foreach( string argName in argNames )
            {
                retVal.Parameters[ index ].Name = argName;
                ++index;
            }

            return retVal;
        }

        private Function DefineFunction( PrototypeContext signature, ExpressionContext body )
        {
            return DefineFunction( GetOrDeclareFunction( signature ), body );
        }

        private Function DefineFunction( Function function, ExpressionContext body )
        {
            if( !function.IsDeclaration )
            {
                throw new ArgumentException( $"Function {function.Name} cannot be redefined", nameof( function ) );
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
                return null;
            }

            InstructionBuilder.Return( funcReturn );
            function.Verify( );

            return function;
        }

        private static string GetPrototypeName( PrototypeContext protoType )
        {
            switch( protoType )
            {
            case FunctionProtoTypeContext func:
                return func.Name;

            default:
                throw new ArgumentException( "unknown prototype" );
            }
        }

        private static (string Name, IReadOnlyList<string> Parameters) CreateSyntheticPrototype( string name, params string[] argNames )
        {
            return (name, argNames);
        }

        private static int AnonNameIndex;
    }
}
