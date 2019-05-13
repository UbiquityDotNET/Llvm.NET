// -----------------------------------------------------------------------
// <copyright file="ConstantExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kaleidoscope.Grammar.AST
{
    public class ConstantExpression
        : IExpression
    {
        public ConstantExpression( SourceSpan location, double value )
        {
            Value = value;
            Location = location;
        }

        public double Value { get; }

        public SourceSpan Location { get; }

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.Visit( this );

        public IEnumerable<IAstNode> Children => Enumerable.Empty<IAstNode>( );

        public override string ToString( )
        {
            return Value.ToString( CultureInfo.CurrentCulture );
        }
    }
}
