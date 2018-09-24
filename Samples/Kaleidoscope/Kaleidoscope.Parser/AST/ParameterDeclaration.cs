// <copyright file="ParameterDeclaration.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Kaleidoscope.Grammar.AST
{
    public class ParameterDeclaration
        : IVariableDeclaration
    {
        public ParameterDeclaration( SourceSpan location, string name, int index )
        {
            Location = location;
            Name = name;
            Index = index;
        }

        public SourceSpan Location { get; }

        public string Name { get; }

        public int Index { get; }

        public bool CompilerGenerated => false;

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.Visit( this );

        public IEnumerable<IAstNode> Children => Enumerable.Empty<IAstNode>( );
    }
}
