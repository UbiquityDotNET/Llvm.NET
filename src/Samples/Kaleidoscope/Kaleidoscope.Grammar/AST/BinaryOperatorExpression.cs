// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Identifier for the kinds of built-in operators</summary>
    public enum BuiltInOperatorKind
    {
        /// <summary>Default value (Invalid)</summary>
        Invalid,

        /// <summary>Assignment operator</summary>
        Assign,

        /// <summary>Addition operator</summary>
        Add,

        /// <summary>Subtraction operator</summary>
        Subtract,

        /// <summary>Multiplication operator</summary>
        Multiply,

        /// <summary>Division operator</summary>
        Divide,

        /// <summary>Comparison operator (less)</summary>
        Less,

        /// <summary>Exponentiation operator</summary>
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
        public BinaryOperatorExpression( SourceRange location, IExpression lhs, BuiltInOperatorKind op, IExpression rhs )
            : base( location )
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
        public string Name => Op.ToString();

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

        /// <summary>Visitor pattern 'Accept' of a visitors that don't need a parameter</summary>
        /// <typeparam name="TResult">Type of the result from the visitor</typeparam>
        /// <param name="visitor">Visitor to apply for each node in this expression</param>
        /// <returns>Result of visiting this expression</returns>
        public override TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult> klsVisitor
                   ? klsVisitor.Visit( this )
                   : visitor.Visit( this );
        }

        /// <summary>Visitor pattern 'Accept' of a visitors that need a parameter</summary>
        /// <typeparam name="TResult">Type of the result from the visitor</typeparam>
        /// <typeparam name="TArg">Type of the argument for the visit</typeparam>
        /// <param name="visitor">Visitor to apply for each node in this expression</param>
        /// <param name="arg">Argument to pass to each method of the visit</param>
        /// <returns>Result of visiting this expression</returns>
        public override TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult, TArg> klsVisitor
                   ? klsVisitor.Visit( this, in arg )
                   : visitor.Visit( this, in arg );
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"{Name}({Left},{Right})";
        }
    }
}
