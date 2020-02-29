// -----------------------------------------------------------------------
// <copyright file="ForInExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ubiquity.ArgValidators;

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
            return visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );
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
