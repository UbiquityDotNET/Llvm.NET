// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>This is a grouping interface for all AST nodes that are valid expressions</summary>
    /// <remarks>
    /// This is a grouping interface to allow other parts of the system to distinguish between
    /// an arbitrary node and one that is ultimately an expression. This helps to ensure correctness
    /// (e.g. a function declaration is not valid as an argument to an operator, only an expression is.
    /// </remarks>
    public interface IExpression
        : IAstNode
    {
    }
}
