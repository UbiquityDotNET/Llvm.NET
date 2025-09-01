// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;

using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class RootNode
        : AstNode
        , IAstNode
    {
        public RootNode( SourceRange location, IAstNode child )
            : this( location, [ child ] )
        {
        }

        public RootNode( SourceRange location, IEnumerable<IAstNode> children )
            : base( location )
        {
            ChildNodes = [ .. children ];
        }

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

        public override IEnumerable<IAstNode> Children => ChildNodes;

        public override string ToString( )
        {
            return string.Join( ' ', Children );
        }

        private readonly ImmutableArray<IAstNode> ChildNodes;
    }
}
