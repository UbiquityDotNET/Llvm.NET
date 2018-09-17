// <copyright file="RootNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Kaleidoscope.Grammar.AST
{
    public class RootNode
        : IAstNode
    {
        public RootNode( SourceSpan location, IEnumerable<IAstNode> children )
        {
            Location = location;
            ChildNodes = children.ToImmutableArray( );
        }

        public SourceSpan Location { get; }

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.Visit( this );

        public IEnumerable<IAstNode> Children => ChildNodes;

        private readonly ImmutableArray<IAstNode> ChildNodes;
    }
}
