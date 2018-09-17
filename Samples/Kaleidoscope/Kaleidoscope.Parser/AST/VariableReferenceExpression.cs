// <copyright file="IVariableReferenceExpression.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

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

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.Visit( this );

        public IEnumerable<IAstNode> Children
        {
            get
            {
                yield return Declaration;
            }
        }
    }
}
