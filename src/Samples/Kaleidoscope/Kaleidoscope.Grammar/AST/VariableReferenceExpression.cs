// -----------------------------------------------------------------------
// <copyright file="VariableReferenceExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class VariableReferenceExpression
        : AstNode
        , IExpression
    {
        public VariableReferenceExpression( SourceLocation location, IVariableDeclaration declaration )
            : base( location )
        {
            Declaration = declaration;
        }

        public IVariableDeclaration Declaration { get; }

        public string Name => Declaration.Name;

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
                yield return Declaration;
            }
        }

        public override string ToString( )
        {
            return $"Load({Name})";
        }
    }
}
