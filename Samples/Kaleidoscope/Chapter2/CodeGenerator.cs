// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Chapter2
{
    internal sealed class CodeGenerator
        : ICodeGenerator<IAstNode>
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
