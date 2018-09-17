// <copyright file="CodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

namespace Kaleidoscope
{
    /// <summary>Visitor to perform final syntax error checks</summary>
    /// <remarks>
    /// This doesn't actually generate any code. It simply satisfies the generator contract with an
    /// effective NOP
    /// </remarks>
    internal sealed class CodeGenerator
        : IKaleidoscopeCodeGenerator<int>
    {
        public int Generate( IAstNode ast )
        {
            return 0;
        }
    }
}
