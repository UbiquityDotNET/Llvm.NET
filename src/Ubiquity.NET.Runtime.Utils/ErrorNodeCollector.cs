// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>AST visitor that collects all errors for a given node</summary>
    /// <typeparam name="T">Type of the enum for diagnostic IDs</typeparam>
    public class ErrorNodeCollector<T>
        : AstVisitorBase<ImmutableArray<ErrorNode<T>>>
        where T : struct, Enum
    {
        /// <summary>Initializes a new instance of the <see cref="ErrorNodeCollector{T}"/> class</summary>
        public ErrorNodeCollector( )
            : base( [] )
        {
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This implementation will aggregate the node to the results of errors if it is an
        /// <see cref="ErrorNode{T}"/> before visiting all children of the node, which will, in
        /// turn, add any <see cref="ErrorNode{T}"/>s to the collected results. Thus resulting
        /// in a final array of errors.
        /// </remarks>
        public override ImmutableArray<ErrorNode<T>> Visit( IAstNode node )
        {
            return node is ErrorNode<T> errNode
                 ? AggregateResult( [ errNode ], base.Visit( node ) )
                 : base.Visit( node );
        }

        /// <inheritdoc/>
        protected override ImmutableArray<ErrorNode<T>> AggregateResult(
            ImmutableArray<ErrorNode<T>> aggregate,
            ImmutableArray<ErrorNode<T>> newResult
            )
        {
            return aggregate.AddRange( newResult );
        }
    }
}
