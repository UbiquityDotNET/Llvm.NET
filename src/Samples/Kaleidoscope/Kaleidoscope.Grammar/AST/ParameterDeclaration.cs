// -----------------------------------------------------------------------
// <copyright file="ParameterDeclaration.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class ParameterDeclaration
        : AstNode
        , IVariableDeclaration
    {
        public ParameterDeclaration( SourceLocation location, string name, int index )
            : base( location )
        {
            Name = name;
            Index = index;
        }

        public string Name { get; }

        public int Index { get; }

        public bool CompilerGenerated => false;

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

        public override IEnumerable<IAstNode> Children => [];

        public override string ToString( )
        {
            return Name;
        }
    }
}
