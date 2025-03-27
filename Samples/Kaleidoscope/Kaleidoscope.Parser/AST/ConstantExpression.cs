// -----------------------------------------------------------------------
// <copyright file="ConstantExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class ConstantExpression
        : AstNode
        , IExpression
    {
        public ConstantExpression( SourceSpan location, double value )
            : base(location)
        {
            Value = value;
        }

        public double Value { get; }

        public override TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult> klsVisitor
                   ? klsVisitor.Visit(this)
                   : visitor.Visit(this);
        }

        public override TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult, TArg> klsVisitor
                   ? klsVisitor.Visit(this, in arg)
                   : visitor.Visit(this, in arg);
        }

        public override IEnumerable<IAstNode> Children => [];

        public override string ToString( )
        {
            return Value.ToString( CultureInfo.CurrentCulture );
        }
    }
}
