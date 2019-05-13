// -----------------------------------------------------------------------
// <copyright file="IVariableDeclaration.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.AST
{
    public interface IVariableDeclaration
        : IAstNode
    {
        string Name { get; }

        bool CompilerGenerated { get; }
    }
}
