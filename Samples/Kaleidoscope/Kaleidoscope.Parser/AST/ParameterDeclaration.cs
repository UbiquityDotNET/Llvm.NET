// -----------------------------------------------------------------------
// <copyright file="ParameterDeclaration.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ubiquity.ArgValidators;

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

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            return visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );
        }

        public IEnumerable<IAstNode> Children => Enumerable.Empty<IAstNode>( );

        public override string ToString( )
        {
            return Name;
        }
    }
}
