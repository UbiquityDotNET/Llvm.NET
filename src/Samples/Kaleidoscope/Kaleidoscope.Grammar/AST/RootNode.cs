// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Root node for the Kaleidoscope AST</summary>
    public sealed class RootNode
        : AstNode
        , IAstNode
    {
        /// <summary>Initializes a new instance of the <see cref="RootNode"/> class.</summary>
        /// <param name="location">Location of the node in the source input</param>
        /// <param name="child">Child node</param>
        public RootNode( SourceRange location, IAstNode child )
            : this( location, [ child ] )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="RootNode"/> class.</summary>
        /// <param name="location">Location of the node in the source input</param>
        /// <param name="children">Child nodes of the root</param>
        public RootNode( SourceRange location, IEnumerable<IAstNode> children )
            : base( location )
        {
            ChildNodes = [ .. children ];
        }

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
        public override IEnumerable<IAstNode> Children => ChildNodes;

        /// <inheritdoc/>
        public override string ToString( )
        {
            return string.Join( ' ', Children );
        }

        private readonly ImmutableArray<IAstNode> ChildNodes;
    }
}
