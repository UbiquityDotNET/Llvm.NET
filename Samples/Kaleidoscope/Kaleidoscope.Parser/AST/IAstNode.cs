// <copyright file="IAstNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Root interface for nodes in the Abstract Syntax Tree</summary>
    public interface IAstNode
    {
        /// <summary>Source location covering the original source for the node</summary>
        SourceSpan Location { get; }

        /// <summary>Gets a collection of children for the node</summary>
        IEnumerable<IAstNode> Children { get; }

        /// <summary>VIsitor pattern support for implementations to dispatch the concrete node type to a visitor</summary>
        /// <typeparam name="TResult">Result type for the visitor</typeparam>
        /// <param name="visitor">Visitor to dispatch the concrete type to</param>
        /// <returns>Result of visiting this node</returns>
        TResult Accept<TResult>( IAstVisitor<TResult> visitor );
    }
}
