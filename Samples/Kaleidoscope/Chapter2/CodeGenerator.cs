// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope
{
    /// <summary>Static extension methods to perform LLVM IR Code generation from the Kaledoscope AST</summary>
    internal sealed class CodeGenerator
        : KaleidoscopeBaseVisitor<int>
        , IDisposable
        , IKaleidoscopeCodeGenerator<int>
    {
        public CodeGenerator( DynamicRuntimeState globalState )
        {
            RuntimeState = globalState;
        }

        public void Dispose( )
        {
        }

        public int Generate( Parser parser, IParseTree tree, DiagnosticRepresentations additionalDiagnostics )
        {
            return Visit( tree );
        }

        public override int VisitBinaryPrototype( [NotNull] BinaryPrototypeContext context )
        {
            if( !RuntimeState.TryAddOperator( context.OpToken, OperatorKind.InfixLeftAssociative, context.Precedence ) )
            {
                throw new CodeGeneratorException( "Cannot replace built-in operators" );
            }

            return 0;
        }

        public override int VisitUnaryPrototype( [NotNull] UnaryPrototypeContext context )
        {
            if( !RuntimeState.TryAddOperator( context.OpToken, OperatorKind.PreFix, 0 ) )
            {
                throw new CodeGeneratorException( "Cannot replace built-in operators" );
            }

            return 0;
        }

        protected override int DefaultResult => 0;

        private readonly DynamicRuntimeState RuntimeState;
    }
}
