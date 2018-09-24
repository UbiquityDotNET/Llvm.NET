// <copyright file="IVariableDeclaration.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar.AST
{
    public interface IVariableDeclaration
        : IAstNode
    {
        string Name { get; }

        bool CompilerGenerated { get; }
    }
}
