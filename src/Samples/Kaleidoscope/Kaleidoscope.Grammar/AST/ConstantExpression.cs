// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;

using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Kaleidoscope AST node for a constant</summary>
    public sealed class ConstantExpression
        : AstNode
        , IExpression
    {
        /// <summary>Initializes a new instance of the <see cref="ConstantExpression"/> class.</summary>
        /// <param name="location">Location of the expression in the original source</param>
        /// <param name="value">Value of the constant</param>
        public ConstantExpression( SourceRange location, double value )
            : base( location )
        {
            Value = value;
        }

        /// <summary>Gets the value of this constant expression</summary>
        public double Value { get; }

        /// <inheritdoc cref="BinaryOperatorExpression.Accept{TResult}(IAstVisitor{TResult})"/>
        public override TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult> klsVisitor
                   ? klsVisitor.Visit( this )
                   : visitor.Visit( this );
        }

        /// <inheritdoc cref="BinaryOperatorExpression.Accept{TResult, TArg}(IAstVisitor{TResult, TArg}, ref readonly TArg)"/>
        public override TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult, TArg> klsVisitor
                   ? klsVisitor.Visit( this, in arg )
                   : visitor.Visit( this, in arg );
        }

        /// <inheritdoc/>
        public override IEnumerable<IAstNode> Children => [];

        /// <inheritdoc/>
        public override string ToString( )
        {
            return Value.ToString( CultureInfo.CurrentCulture );
        }
    }
}
