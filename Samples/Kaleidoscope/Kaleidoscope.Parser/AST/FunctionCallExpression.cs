// <copyright file="IFunctionCall.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kaleidoscope.Grammar.AST
{
    public class FunctionCallExpression
        : IExpression
    {
        public FunctionCallExpression(SourceSpan location, Prototype functionPrototype, IEnumerable<IExpression> args )
        {
            Location = location;
            FunctionPrototype = functionPrototype;
            Arguments = args.ToImmutableArray( );
        }

        public FunctionCallExpression( SourceSpan location, Prototype functionPrototype, params IExpression[] args )
            : this(location, functionPrototype, (IEnumerable<IExpression>)args)
        {
        }

        public SourceSpan Location { get; }

        public Prototype FunctionPrototype { get; }

        public IReadOnlyList<IExpression> Arguments { get; }

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.Visit( this );

        public IEnumerable<IAstNode> Children
        {
            get
            {
                yield return FunctionPrototype;
            }
        }

        public override string ToString( )
        {
            if( Arguments.Count == 0 )
            {
                return $"Call({FunctionPrototype})";
            }

            return $"Call({FunctionPrototype}, {string.Join(",", Arguments.Select(a=>a.ToString()))})";
        }
    }
}
