// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class ForInExpression
        : AstNode
        , IExpression
    {
        public ForInExpression( SourceLocation location
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

        public LocalVariableDeclaration LoopVariable { get; }

        public IExpression Condition { get; }

        public IExpression Step { get; }

        public IExpression Body { get; }

        public override TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult> klsVisitor
                   ? klsVisitor.Visit( this )
                   : visitor.Visit( this );
        }

        public override TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult, TArg> klsVisitor
                   ? klsVisitor.Visit( this, in arg )
                   : visitor.Visit( this, in arg );
        }

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

        public override string ToString( )
        {
            return $"for({LoopVariable}, {Condition}, {Step}, {Body})";
        }
    }
}
