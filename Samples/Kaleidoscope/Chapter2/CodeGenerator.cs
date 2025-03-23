// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

namespace Kaleidoscope.Chapter2
{
    internal sealed class CodeGenerator
        : IKaleidoscopeCodeGenerator<IAstNode>
    {
        public void Dispose( )
        {
        }

        public IAstNode? Generate( IAstNode ast )
        {
            return ast;
        }
    }
}
