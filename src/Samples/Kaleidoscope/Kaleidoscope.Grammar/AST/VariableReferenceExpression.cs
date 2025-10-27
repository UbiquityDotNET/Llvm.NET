// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Kaleidoscope AST node for a reference to a variable</summary>
    public sealed class VariableReferenceExpression
        : AstNode
        , IExpression
    {
        /// <summary>Initializes a new instance of the <see cref="VariableReferenceExpression"/> class.</summary>
        /// <param name="location">Location of the node in the input source</param>
        /// <param name="declaration">Declaration of the variable referenced</param>
        public VariableReferenceExpression( SourceRange location, IVariableDeclaration declaration )
            : base( location )
        {
            Declaration = declaration;
        }

        /// <summary>Gets the declaration this reference refers to</summary>
        public IVariableDeclaration Declaration { get; }

        /// <summary>Gets the name of the variable referenced</summary>
        public string Name => Declaration.Name;

        /// <inheritdoc cref="BinaryOperatorExpression.Accept{TResult, TArg}(IAstVisitor{TResult, TArg}, ref readonly TArg)"/>
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
                yield return Declaration;
            }
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"Load({Name})";
        }
    }
}
