// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
    /// <para>In frameworks that support it the <typeparamref name="TArg"/> is typically used for a <c>byref-like</c>
    /// type where the type may NOT be stored on the heap and MUST be passed via <c>ref readonly</c>. (Such support
    /// requires at least .NET 9 to support <c>allows ref struct</c>, which requires runtime support.)</para>
    /// </remarks>
    public interface IAstVisitor<out TResult, TArg>
#if NET9_0_OR_GREATER
        where TArg : struct, allows ref struct
#else
        where TArg : struct
#endif
    {
        /// <summary>Visits a given node to produce a result</summary>
        /// <param name="node">Node to visit</param>
        /// <param name="arg">Arg associated with this node and visit operation</param>
        /// <returns>Result of the visit</returns>
        TResult? Visit( IAstNode node, ref readonly TArg arg );
    }
}
