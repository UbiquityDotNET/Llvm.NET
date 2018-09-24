// <copyright file="LocalVariableDeclaration.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.AST
{
    public class LocalVariableDeclaration
        : IVariableDeclaration
    {
        public LocalVariableDeclaration( SourceSpan location, string name, IExpression initializer, bool compilerGenerated = false )
        {
            Location = location;
            Name = name;
            Initializer = initializer;
            CompilerGenerated = compilerGenerated;
        }

        public SourceSpan Location { get; }

        public string Name { get; }

        public IExpression Initializer { get; }

        public bool CompilerGenerated { get; }

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.Visit( this );

        public IEnumerable<IAstNode> Children
        {
            get
            {
                if( Initializer != null )
                {
                    yield return Initializer;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
