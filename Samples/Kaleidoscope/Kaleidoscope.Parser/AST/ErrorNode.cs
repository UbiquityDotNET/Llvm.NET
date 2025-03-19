// -----------------------------------------------------------------------
// <copyright file="ErrorNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

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

        /// <inheritdoc/>
        public virtual TResult? Accept<TResult, TArg>(IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : class
            where TArg : struct, allows ref struct
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit( this, in arg );
        }

        public override string ToString( ) => $"<{Location}:{Error}>";
    }
}
