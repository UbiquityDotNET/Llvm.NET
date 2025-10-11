// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Kaleidoscope AST node for a <c>ForIn</c> expression</summary>
    public sealed class ForInExpression
        : AstNode
        , IExpression
    {
        /// <summary>Initializes a new instance of the <see cref="ForInExpression"/> class.</summary>
        /// <param name="location">Location of the expression in the source input</param>
        /// <param name="loopVariable">Declaration of the variable to use for the loop</param>
        /// <param name="condition">Exit Condition for the loop</param>
        /// <param name="step">Step value to increment <paramref name="loopVariable"/> by with each iteration of the loop</param>
        /// <param name="body">The body of the loop to invoke on each iteration</param>
        public ForInExpression( SourceRange location
                              , LocalVariableDeclaration loopVariable
                              , IExpression condition
                              , IExpression step
                              , IExpression body
                              )
            : base( location )
        {
            LoopVariable = loopVariable;
            Condition = condition;
            Step = step;
            Body = body;
        }

        /// <summary>Gets the declaration of the variable to use for the loop</summary>
        public LocalVariableDeclaration LoopVariable { get; }

        /// <summary>Gets the exit Condition for the loop</summary>
        public IExpression Condition { get; }

        /// <summary>Gets the step value to increment <see cref="LoopVariable"/> by with each iteration of the loop</summary>
        public IExpression Step { get; }

        /// <summary>Gets the body of the loop to invoke on each iteration</summary>
        public IExpression Body { get; }

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
                yield return LoopVariable;
                yield return Condition;
                yield return Step;
                yield return Body;
            }
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"for({LoopVariable}, {Condition}, {Step}, {Body})";
        }
    }
}
