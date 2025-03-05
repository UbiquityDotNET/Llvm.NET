// -----------------------------------------------------------------------
// <copyright file="ErrorNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaleidoscope.Grammar.AST
{
    public class ErrorNode
        : IAstNode
        , IExpression
    {
        public ErrorNode( SourceSpan location, string err )
        {
            ArgumentNullException.ThrowIfNull(err);

            Location = location;
            Error = err;
        }

        public SourceSpan Location { get; }

        public string Error { get; }

        public IEnumerable<IAstNode> Children { get; } = [];

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit( this );
        }

        public override string ToString( ) => $"<{Location}:{Error}>";
    }
}
