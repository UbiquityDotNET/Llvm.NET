// -----------------------------------------------------------------------
// <copyright file="RootNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class RootNode
        : AstNode
        , IAstNode
    {
        public RootNode( SourceSpan location, IAstNode child )
            : this( location, [child] )
        {
        }

        public RootNode( SourceSpan location, IEnumerable<IAstNode> children )
            : base(location)
        {
            ChildNodes = [ .. children ];
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

        public override IEnumerable<IAstNode> Children => ChildNodes;

        public override string ToString( )
        {
            return string.Join( ' ', Children );
        }

        private readonly ImmutableArray<IAstNode> ChildNodes;
    }
}
