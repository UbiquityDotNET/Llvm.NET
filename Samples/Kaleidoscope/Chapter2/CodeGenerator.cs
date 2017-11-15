// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Antlr4.Runtime.Misc;
using Kaleidoscope.Grammar;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope
{
    /// <summary>Static extension methods to perform LLVM IR Code generation from the Kaledoscope AST</summary>
    internal sealed class CodeGenerator
        : KaleidoscopeBaseVisitor<int>
    {
        public CodeGenerator( LanguageLevel level )
        {
            ParserStack = new ReplParserStack( level );
        }

        public ReplParserStack ParserStack { get; }

        public override int VisitBinaryProtoType( [NotNull] BinaryProtoTypeContext context )
        {
            if( !ParserStack.Parser.TryAddOperator( context.Op, OperatorKind.InfixLeftAssociative, context.Precedence ) )
            {
                throw new ArgumentException( "Cannot replace built-in operators", nameof( context ) );
            }

            return 0;
        }

        public override int VisitUnaryProtoType( [NotNull] UnaryProtoTypeContext context )
        {
            if( !ParserStack.Parser.TryAddOperator( context.Op, OperatorKind.PreFix, 0 ) )
            {
                throw new ArgumentException( "Cannot replace built-in operators", nameof( context ) );
            }

            return 0;
        }

        protected override int DefaultResult => 0;
    }
}
