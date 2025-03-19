// -----------------------------------------------------------------------
// <copyright file="FunctionCallExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kaleidoscope.Grammar.AST
{
    public class FunctionCallExpression
        : IExpression
    {
        public FunctionCallExpression( SourceSpan location, Prototype functionPrototype, IEnumerable<IExpression> args )
        {
            Location = location;
            FunctionPrototype = functionPrototype;
            Arguments = args.ToImmutableArray( );
        }

        public FunctionCallExpression( SourceSpan location, Prototype functionPrototype, params IExpression[ ] args )
            : this( location, functionPrototype, ( IEnumerable<IExpression> )args )
        {
        }

        public SourceSpan Location { get; }

        public Prototype FunctionPrototype { get; }

        public IReadOnlyList<IExpression> Arguments { get; }

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

        public IEnumerable<IAstNode> Children
        {
            get
            {
                yield return FunctionPrototype;
            }
        }

        public override string ToString( )
        {
            return Arguments.Count == 0
                ? $"Call({FunctionPrototype})"
                : $"Call({FunctionPrototype}, {string.Join( ",", Arguments.Select( a => a.ToString( ) ) )})";
        }
    }
}
