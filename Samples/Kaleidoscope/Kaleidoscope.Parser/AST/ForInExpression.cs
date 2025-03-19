// -----------------------------------------------------------------------
// <copyright file="ForInExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Kaleidoscope.Grammar.AST
{
    public class ForInExpression
        : IExpression
    {
        public ForInExpression( SourceSpan location
                              , LocalVariableDeclaration loopVariable
                              , IExpression condition
                              , IExpression step
                              , IExpression body
                              )
        {
            Location = location;
            LoopVariable = loopVariable;
            Condition = condition;
            Step = step;
            Body = body;
        }

        public SourceSpan Location { get; }

        public LocalVariableDeclaration LoopVariable { get; }

        public IExpression Condition { get; }

        public IExpression Step { get; }

        public IExpression Body { get; }

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
                yield return LoopVariable;
                yield return Condition;
                yield return Step;
                yield return Body;
            }
        }

        public override string ToString( )
        {
            return $"for({LoopVariable}, {Condition}, {Step}, {Body})";
        }
    }
}
