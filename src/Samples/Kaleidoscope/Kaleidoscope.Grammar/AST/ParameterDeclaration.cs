// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>AST Node for a parameter declaration</summary>
    public sealed class ParameterDeclaration
        : AstNode
        , IVariableDeclaration
    {
        /// <summary>Initializes a new instance of the <see cref="ParameterDeclaration"/> class.</summary>
        /// <param name="location">Location of the source of the node in the input</param>
        /// <param name="name">Name of the parameter</param>
        /// <param name="index">Index of the parameter in the function signature</param>
        public ParameterDeclaration( SourceRange location, string name, int index )
            : base( location )
        {
            Name = name;
            Index = index;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <summary>Gets the index of the parameter in the parameter list it belongs too.</summary>
        public int Index { get; }

        /// <inheritdoc/>
        public bool CompilerGenerated => false;

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
        public override IEnumerable<IAstNode> Children => [];

        /// <inheritdoc/>
        public override string ToString( )
        {
            return Name;
        }
    }
}
