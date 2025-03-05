// -----------------------------------------------------------------------
// <copyright file="RootNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Kaleidoscope.Grammar.AST
{
    public class RootNode
        : IAstNode
    {
        public RootNode( SourceSpan location, IAstNode child )
            : this( location, new IAstNode[ ] { child } )
        {
        }

        public RootNode( SourceSpan location, IEnumerable<IAstNode> children )
        {
            Location = location;
            ChildNodes = children.ToImmutableArray( );
        }

        public SourceSpan Location { get; }

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit( this );
        }

        public IEnumerable<IAstNode> Children => ChildNodes;

        public override string ToString( )
        {
            return string.Join( ' ', Children );
        }

        private readonly ImmutableArray<IAstNode> ChildNodes;
    }
}
