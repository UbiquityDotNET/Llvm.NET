// -----------------------------------------------------------------------
// <copyright file="IAstNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Root interface for nodes in the Abstract Syntax Tree</summary>
    public interface IAstNode
    {
        /// <summary>Gets the source location covering the original source for the node</summary>
        SourceLocation Location { get; }

        /// <summary>Gets a collection of children for the node</summary>
        IEnumerable<IAstNode> Children { get; }

        /// <summary>Visitor pattern support for implementations to dispatch the concrete node type to a visitor</summary>
        /// <typeparam name="TResult">Result type for the visitor</typeparam>
        /// <param name="visitor">Visitor to dispatch the concrete type to</param>
        /// <returns>Result of visiting this node</returns>
        TResult? Accept<TResult>( IAstVisitor<TResult> visitor );

        /// <summary>Visitor pattern support for implementations to dispatch the concrete node type to a visitor</summary>
        /// <typeparam name="TResult">Result type for the visitor</typeparam>
        /// <typeparam name="TArg">Type of the argument to pass on to the visitor</typeparam>
        /// <param name="visitor">Visitor to dispatch the concrete type to</param>
        /// <param name="arg">Argument to pass to the concrete type as a readonly ref</param>
        /// <returns>Result of visiting this node</returns>
        TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TArg : struct, allows ref struct;
    }
}
