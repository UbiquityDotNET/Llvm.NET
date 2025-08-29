// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Text;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class LocalVariableDeclaration
        : AstNode
        , IVariableDeclaration
    {
        public LocalVariableDeclaration( SourceRange location, string name, IExpression? initializer, bool compilerGenerated = false )
            : base( location )
        {
            Name = name;
            Initializer = initializer;
            CompilerGenerated = compilerGenerated;
        }

        public string Name { get; }

        public IExpression? Initializer { get; }

        public bool CompilerGenerated { get; }

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

        public override IEnumerable<IAstNode> Children
        {
            get
            {
                if(Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }

        public override string ToString( )
        {
            var bldr = new StringBuilder();
            if(CompilerGenerated)
            {
                bldr.Append( "[CompilerGenerated]" );
            }

            bldr.Append( "Declare(" );
            bldr.Append( Name );
            if(Initializer != null)
            {
                bldr.Append( ", " );
                bldr.Append( Initializer );
            }

            bldr.Append( ')' );
            return bldr.ToString();
        }
    }
}
