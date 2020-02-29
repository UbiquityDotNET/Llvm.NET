// -----------------------------------------------------------------------
// <copyright file="RootNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Ubiquity.ArgValidators;

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

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            return visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );
        }

        public IEnumerable<IAstNode> Children => ChildNodes;

        private readonly ImmutableArray<IAstNode> ChildNodes;
    }
}
