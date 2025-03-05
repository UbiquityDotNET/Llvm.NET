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
