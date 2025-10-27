// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>AST node for a conditional expression</summary>
    /// <remarks>
    /// A conditional expression is the language equivalent of a conditional operator in most "C" like languages (ternary operator).
    /// </remarks>
    public sealed class ConditionalExpression
        : AstNode
        , IExpression
    {
        /// <summary>Initializes a new instance of the <see cref="ConditionalExpression"/> class.</summary>
        /// <param name="location">Location of the expression in the source input</param>
        /// <param name="condition">Expression for the condition</param>
        /// <param name="thenExpression">Expression for the `then` clause of the conditional expression</param>
        /// <param name="elseExpression">Expression for the `else` clause of the conditional expression</param>
        /// <param name="resultVar"><see cref="LocalVariableDeclaration"/> to represent the results of the expression</param>
        public ConditionalExpression( SourceRange location
                                    , IExpression condition
                                    , IExpression thenExpression
                                    , IExpression elseExpression
                                    , LocalVariableDeclaration resultVar
                                    )
            : base( location )
        {
            Condition = condition;
            ThenExpression = thenExpression;
            ElseExpression = elseExpression;
            ResultVariable = resultVar;
        }

        /// <summary>Gets the expression for the condition</summary>
        public IExpression Condition { get; }

        /// <summary>Gets the expression for the `then` clause</summary>
        public IExpression ThenExpression { get; }

        /// <summary>Gets the expression for the `else` clause</summary>
        public IExpression ElseExpression { get; }

        /// <summary>Gets the result of the expression as a <see cref="LocalVariableDeclaration"/></summary>
        /// <remarks>
        /// Compiler generated result variable supports building conditional
        /// expressions without the need for SSA form in the AST/Code generation
        /// by using mutable variables. The result is assigned a value from both
        /// sides of the branch. In pure SSA form this isn't needed as a PHI node
        /// would be used instead.
        /// </remarks>
        public LocalVariableDeclaration ResultVariable { get; }

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
        public override IEnumerable<IAstNode> Children
        {
            get
            {
                yield return Condition;
                yield return ThenExpression;
                yield return ElseExpression;
            }
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"Conditional({Condition}, {ThenExpression}, {ElseExpression})";
        }
    }
}
