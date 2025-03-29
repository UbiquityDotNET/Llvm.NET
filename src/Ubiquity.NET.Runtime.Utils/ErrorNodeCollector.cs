// -----------------------------------------------------------------------
// <copyright file="ErrorNodeCollector.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>AST visitor that collects all errors for a given node</summary>
    public class ErrorNodeCollector
        : AstVisitorBase<ImmutableArray<ErrorNode>>
    {
        /// <summary>Initializes a new instance of <see cref="ErrorNodeCollector"/></summary>
        public ErrorNodeCollector( )
            : base( [] )
        {
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This implementation will aggregate the node to the results of errors if it is an
        /// <see cref="ErrorNode"/> before visiting all children of the node, which will, in
        /// turn, add any <see cref="ErrorNode"/>s to the collected results. Thus resulting
        /// in a final array of errors.
        /// </remarks>
        public override ImmutableArray<ErrorNode> Visit( IAstNode node )
        {
            return node is ErrorNode errNode
                 ? AggregateResult([ errNode ], base.Visit( node ) )
                 : base.Visit( node );
        }

        /// <inheritdoc/>
        protected override ImmutableArray<ErrorNode> AggregateResult( ImmutableArray<ErrorNode> aggregate, ImmutableArray<ErrorNode> newResult )
        {
            return aggregate.AddRange( newResult );
        }
    }
}
