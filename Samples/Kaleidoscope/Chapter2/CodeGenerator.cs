// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

namespace Kaleidoscope
{
    /// <summary>Static extension methods to perform LLVM IR Code generation from the Kaleidoscope AST</summary>
    /// <remarks>
    /// This doesn't actually generate any code. It simply satisfies the generator contract with an
    /// effective NOP
    /// </remarks>
    internal sealed class CodeGenerator
        : KaleidoscopeBaseVisitor<int>
        , IDisposable
        , IKaleidoscopeCodeGenerator<int>
    {
        public void Dispose( )
        {
        }

        public int Generate( Parser parser, IParseTree tree, DiagnosticRepresentations additionalDiagnostics )
        {
            if( parser.NumberOfSyntaxErrors > 0 )
            {
                return 1;
            }

            return 0;
        }

        protected override int DefaultResult => 0;
    }
}
