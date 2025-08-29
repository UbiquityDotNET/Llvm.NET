// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
