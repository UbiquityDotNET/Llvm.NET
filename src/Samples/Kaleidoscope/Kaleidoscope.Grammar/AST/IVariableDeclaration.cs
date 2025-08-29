// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public interface IVariableDeclaration
        : IAstNode
    {
        string Name { get; }

        bool CompilerGenerated { get; }
    }
}
