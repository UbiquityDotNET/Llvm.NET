// -----------------------------------------------------------------------
// <copyright file="BinaryOperatorExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public enum BuiltInOperatorKind
    {
        Invalid,
        Assign,
        Add,
        Subtract,
        Multiply,
        Divide,
        Less,
        Pow
    }

    /// <summary>AST Expression node for a binary operator</summary>
    public sealed class BinaryOperatorExpression
        : AstNode
        , IExpression
    {
        /// <summary>Initializes a new instance of the <see cref="BinaryOperatorExpression"/> class.</summary>
        /// <param name="location">Source location of the operator expression</param>
        /// <param name="lhs">Left hand side expression for the operator</param>
        /// <param name="op">Operator type</param>
        /// <param name="rhs">Right hand side expression for the operator</param>
        public BinaryOperatorExpression( SourceSpan location, IExpression lhs, BuiltInOperatorKind op, IExpression rhs )
            : base(location)
        {
            Left = lhs;
            Op = op;
            Right = rhs;
        }

        /// <summary>Gets the left hand side expression</summary>
        public IExpression Left { get; }

        /// <summary>Gets the operator kind for this operator</summary>
        public BuiltInOperatorKind Op { get; }

        /// <summary>Gets the Operator name for this expression</summary>
        public string Name => Op.ToString( );

        /// <summary>Gets the Right hand side expression</summary>
        public IExpression Right { get; }

        /// <inheritdoc/>
        public sealed override IEnumerable<IAstNode> Children
        {
            get
            {
                yield return Left;
                yield return Right;
            }
        }

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

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"{Name}({Left},{Right})";
        }
    }
}
