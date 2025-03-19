// -----------------------------------------------------------------------
// <copyright file="VariableReferenceExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Kaleidoscope.Grammar.AST
{
    public class VariableReferenceExpression
        : IExpression
    {
        public VariableReferenceExpression( SourceSpan location, IVariableDeclaration declaration )
        {
            Location = location;
            Declaration = declaration;
        }

        public SourceSpan Location { get; }

        public IVariableDeclaration Declaration { get; }

        public string Name => Declaration.Name;

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
                yield return Declaration;
            }
        }

        public override string ToString( )
        {
            return $"Load({Name})";
        }
    }
}
