// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <inheritdoc cref="AstVisitorBase{TResult, TArg}"/>
    public class AstVisitorBase<TResult>
        : IAstVisitor<TResult>
    {
        /// <inheritdoc cref="AstVisitorBase{TResult, TArg}.Visit(IAstNode, ref readonly TArg)"/>
        /// <remarks>
        /// The implementation for this base class does nothing beyond visiting each child via
        /// a call to <see cref="VisitChildren(IAstNode)"/>
        /// </remarks>
        public virtual TResult? Visit( IAstNode node )
        {
            return VisitChildren( node );
        }

        /// <inheritdoc cref="AstVisitorBase{TResult, TArg}.VisitChildren(IAstNode, ref readonly TArg)"/>
        public virtual TResult? VisitChildren( IAstNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            TResult? aggregate = DefaultResult;
            foreach(var child in node.Children)
            {
                aggregate = AggregateResult( aggregate, child.Accept( this ) );
            }

            return aggregate;
        }

        /// <summary>Initializes a new instance of the <see cref="AstVisitorBase{TResult}"/> class</summary>
        /// <param name="defaultResult">Default result to use for visitation</param>
        protected AstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        /// <inheritdoc cref="AstVisitorBase{TResult, TArg}.AggregateResult"/>
        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        /// <inheritdoc cref="AstVisitorBase{TResult, TArg}.DefaultResult"/>
        protected TResult? DefaultResult { get; }
    }

    /// <summary>Common base implementation of visitor for <see cref="IAstNode"/></summary>
    /// <typeparam name="TResult">Result of the visit</typeparam>
    /// <typeparam name="TArg">Type of the argument to pass to visit methods</typeparam>
    /// <remarks>
    /// <para>DSL languages will typically define the language specific node types to test for a
    /// language specific implementation of this interface in their <see cref="IAstNode.Accept{TResult}(IAstVisitor{TResult})"/>
    /// and <see cref="IAstNode.Accept{TResult, TArg}(IAstVisitor{TResult, TArg}, ref readonly TArg)"/> implementations.
    /// This allows them to re-route the visitation to the language specific node type directly. If they don't do this
    /// then the default behavior is to visit ANY node via <see cref="Visit"/>. The default implementation of that here
    /// does nothing of it's own beyond visiting each child. While that can be made to work in a language specific form
    /// it requires such a visitor to implement a "type" switch on the node and then call the correct Visit method for
    /// that type. This is tedious, error prone and performance overhead that is avoided if each node type contains a
    /// redirecting implementation of the Accept methods.</para>
    /// <para>An alternative is that each language specific node already contains a "kind" property (perhaps to make binary
    /// serialization more efficient.) Then no re-directing to a custom interface is needed as a switch on the "kind" can
    /// provide all the information needed to adjust the visit behavior. A number of possibilities exist for customization,
    /// though the typical implementation is to use a simple redirecting Accept. (As shown in the example).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// public interface IMyAstVisitor<out TResult>
    /// {
    ///     // Declare Visit methods for other node types here...
    ///
    ///     TResult? Visit( MyLangNode node );
    /// }
    ///
    /// public interface IMyAstVisitor<out TResult, TArg>
    ///     where TArg : struct, allows ref struct
    /// {
    ///     TResult? Visit( MyLangNode node, ref readonly TArg arg );
    /// }
    ///
    /// // [...]
    /// public class MyLangAstVisitor<TResult>
    ///     : AstVisitorBase<TResult>
    /// {
    ///     // ...
    ///
    ///     TResult? Visit( MyLangNode node )
    ///     {
    ///         // Do something interesting with specific MyLangNode type for `node`
    ///         // and return the result...
    ///         return default;
    ///     }
    /// }
    ///
    /// // [...]
    ///
    /// public class MyLangAstVisitor<TResult, TArg>
    ///     : AstVisitorBase<TResult>
    ///     where TArg : struct, allows ref struct
    /// {
    ///     // ...
    ///
    ///     TResult? Visit( MyLangNode node, ref readonly TArg arg )
    ///     {
    ///         // Do something interesting with specific MyLangNode type for `node`
    ///         // and input arg (if passing it to another function use `in`). Then
    ///         // return the result...
    ///         return default;
    ///     }
    /// }
    ///
    /// // [ ... ]
    ///
    /// public sealed class MyLangNode
    ///     : AstNode
    /// {
    ///     // ...
    ///
    ///     public override TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
    ///         where TResult : default
    ///     {
    ///         return visitor is IMyAstVisitor<TResult> myVisitor
    ///                ? myVisitor.Visit(this)
    ///                : visitor.Visit(this);
    ///     }
    ///
    ///     public override TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
    ///         where TResult : default
    ///     {
    ///         return visitor is IMyAstVisitor<TResult, TArg> myVisitor
    ///                ? myVisitor.Visit(this, in arg)
    ///                : visitor.Visit(this, in arg);
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Related Generic - split file names is just confusing" )]
    public class AstVisitorBase<TResult, TArg>
        : IAstVisitor<TResult, TArg>
#if NET9_0_OR_GREATER
        where TArg : struct, allows ref struct
#else
        where TArg : struct
#endif
    {
        /// <summary>Visit a node and all of it's children</summary>
        /// <param name="node">The node to visit</param>
        /// <param name="arg">Argument to pass to the visit for use in association with the node</param>
        /// <returns>Result of the visitation</returns>
        /// <remarks>
        /// The implementation for this base class does nothing beyond visiting each child via
        /// a call to <see cref="VisitChildren(IAstNode, ref readonly TArg)"/>
        /// </remarks>
        public virtual TResult? Visit( IAstNode node, ref readonly TArg arg )
        {
            return VisitChildren( node, in arg );
        }

        /// <summary>Visits each child and aggregates the results as the return value</summary>
        /// <param name="node">Node to visit children of</param>
        /// <param name="arg">Argument to pass to the visit for use in association with the node</param>
        /// <returns>Result of the visitation</returns>
        /// <remarks>
        /// The protected virtual method <see cref="AggregateResult"/> is responsible for aggregation
        /// of results. The default is to simply replace the results with that of the new visitation.
        /// </remarks>
        public virtual TResult? VisitChildren( IAstNode node, ref readonly TArg arg )
        {
            ArgumentNullException.ThrowIfNull( node );
            TResult? aggregate = DefaultResult;
            foreach(var child in node.Children)
            {
                aggregate = AggregateResult( aggregate, child.Accept( this, in arg ) );
            }

            return aggregate;
        }

        /// <summary>Initializes a new instance of the <see cref="AstVisitorBase{TResult, TArg}"/> class</summary>
        /// <param name="defaultResult">Default result to use for visitation</param>
        protected AstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        /// <summary>Performs result aggregation for <see cref="VisitChildren"/></summary>
        /// <param name="aggregate">Current aggregate result</param>
        /// <param name="newResult">New result to aggregate into the results</param>
        /// <returns>Aggregated result</returns>
        /// <remarks>
        /// The result type is deliberately ambiguous to allow for simple replacement to
        /// capturing all results in a sequence of some sort as needed by a given implementation.
        /// The default behavior is to simply replace the results but an override is allowed to do
        /// pretty much whatever it wants as long as the types are consistent.
        /// </remarks>
        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        /// <summary>Gets the default value for a result provided in the constructor</summary>
        protected TResult? DefaultResult { get; }
    }
}
