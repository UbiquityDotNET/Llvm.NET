// -----------------------------------------------------------------------
// <copyright file="ConditionalExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class ConditionalExpression
        : AstNode
        , IExpression
    {
        public ConditionalExpression( SourceLocation location
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

        public IExpression Condition { get; }

        public IExpression ThenExpression { get; }

        public IExpression ElseExpression { get; }

        // Compiler generated result variable supports building conditional
        // expressions without the need for SSA form in the AST/Code generation
        // by using mutable variables. The result is assigned a value from both
        // sides of the branch. In pure SSA form this isn't needed as a PHI node
        // would be used instead.
        public LocalVariableDeclaration ResultVariable { get; }

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
                yield return Condition;
                yield return ThenExpression;
                yield return ElseExpression;
            }
        }

        public override string ToString( )
        {
            return $"Conditional({Condition}, {ThenExpression}, {ElseExpression})";
        }
    }
}
