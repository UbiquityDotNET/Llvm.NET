// -----------------------------------------------------------------------
// <copyright file="IAstVisitor.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Runtime.Utils
{
    /// <inheritdoc cref="IAstVisitor{TResult, TArg}"/>
    /// <remarks>
    /// <para>This interface is sued for visiting an AST, typically for
    /// generating code but may also be used to detect errors in the
    /// AST etc..</para>
    /// </remarks>
    public interface IAstVisitor<out TResult>
    {
        /// <inheritdoc cref="IAstVisitor{TResult, TArg}.Visit(IAstNode, ref readonly TArg)"/>
        TResult? Visit( IAstNode node );
    }

    /// <summary>Interface for implementing the Visitor pattern with <see cref="IAstNode"/></summary>
    /// <typeparam name="TResult">Result type of the visit</typeparam>
    /// <typeparam name="TArg">Argument type to pass to all visit methods</typeparam>
    /// <remarks>
    /// <para>This interface is used for visiting an AST, typically for generating code but may also
    /// be used to detect errors in the AST etc..</para>
    /// <para>The <typeparamref name="TArg"/> is typically used for a Byref-like type where the type
    /// may NOT be stored on the heap and MUST be passed via `ref readonly`.</para>
    /// </remarks>
    public interface IAstVisitor<out TResult, TArg>
        where TArg : struct, allows ref struct
    {
        /// <summary>Visits a given node to produce a result</summary>
        /// <param name="node">Node to visit</param>
        /// <param name="arg">Arg associated with this node and visit operation</param>
        /// <returns>Result of the visit</returns>
        TResult? Visit( IAstNode node, ref readonly TArg arg );
    }
}
