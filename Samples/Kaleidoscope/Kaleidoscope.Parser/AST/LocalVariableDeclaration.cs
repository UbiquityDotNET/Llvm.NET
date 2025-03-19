// -----------------------------------------------------------------------
// <copyright file="LocalVariableDeclaration.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Kaleidoscope.Grammar.AST
{
    public class LocalVariableDeclaration
        : IVariableDeclaration
    {
        public LocalVariableDeclaration( SourceSpan location, string name, IExpression? initializer, bool compilerGenerated = false )
        {
            Location = location;
            Name = name;
            Initializer = initializer;
            CompilerGenerated = compilerGenerated;
        }

        public SourceSpan Location { get; }

        public string Name { get; }

        public IExpression? Initializer { get; }

        public bool CompilerGenerated { get; }

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
                if( Initializer != null )
                {
                    yield return Initializer;
                }
            }
        }

        public override string ToString( )
        {
            var bldr = new StringBuilder();
            if( CompilerGenerated )
            {
                bldr.Append( "[CompilerGenerated]" );
            }

            bldr.Append( "Declare(" );
            bldr.Append( Name );
            if( Initializer != null )
            {
                bldr.Append( ", " );
                bldr.Append( Initializer );
            }

            bldr.Append( ')' );
            return bldr.ToString( );
        }
    }
}
