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

            case '>':
                {
                    var tmp = InstructionBuilder.Compare( RealPredicate.UnorderedOrGreaterThan, lhs, rhs )
                                                .RegisterName( "cmptmp" );
                    return InstructionBuilder.UIToFPCast( tmp, InstructionBuilder.Context.DoubleType )
                                             .RegisterName( "booltmp" );
                }

            case '^':
                {
                    var proto = CreateSyntheticPrototype( "llvm.pow.f64", "value", "power" );
                    var pow = DeclareFunction( proto.Name, proto.Parameters );
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
            return DeclareFunction( context );
        }

        public override Value VisitFunctionDefinition( [NotNull] FunctionDefinitionContext context )
        {
            return FunctionDefinition( context.Signature, context.BodyExpression);
        }

        public override Value VisitTopLevelExpression( [NotNull] TopLevelExpressionContext context )
        {
            string name = $"anon_expr_{AnonNameIndex++}";
            var proto = CreateSyntheticPrototype( name );

            var def = DeclareFunction( proto.Name, proto.Parameters);
            return FunctionDefinition( proto.Name, proto.Parameters, context.expression( ) );
        }

        protected override Value DefaultResult => null;

        private Function GetFunction( string name )
        {
            return Module.GetFunction( name );
        }

        private Function DeclareFunction( PrototypeContext signature )
        {
            return DeclareFunction( signature.Name, signature.Parameters );
        }

        private Function DeclareFunction( string name, IReadOnlyList<string> argNames )
        {
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

        private Function FunctionDefinition( PrototypeContext signature, ExpressionContext body )
        {
            return FunctionDefinition( signature.Name, signature.Parameters, body );
        }

        private Function FunctionDefinition( string name, IReadOnlyList<string> argNames, ExpressionContext body )
        {
            var function = GetFunction( name );
            if( function == null )
            {
                function = DeclareFunction( name, argNames );
            }

            if( function == null )
            {
                return null;
            }

            if( !function.IsDeclaration )
            {
                throw new ArgumentException( $"Function {name} cannot be redefined", nameof( name ) );
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

        private (string Name, IReadOnlyList<string> Parameters) CreateSyntheticPrototype( string name, params string[] argNames )
        {
            return (name, argNames);
        }

        private static int AnonNameIndex;
    }
}
