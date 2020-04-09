// -----------------------------------------------------------------------
// <copyright file="ErrorNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.AST
{
    public class ErrorNode
        : IAstNode
        , IExpression
    {
        public ErrorNode( SourceSpan location, string err )
        {
            Location = location;
            Error = err.ValidateNotNullOrWhiteSpace( nameof( err ) );
        }

        public SourceSpan Location { get; }

        public string Error { get; }

        public IEnumerable<IAstNode> Children { get; } = Enumerable.Empty<IAstNode>( );

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            return visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );
        }

        public override string ToString( ) => $"<{Location}:{Error}>";
    }
}
