// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Text;

using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class VarInExpression
        : AstNode
        , IExpression
    {
        public VarInExpression( SourceRange location, IEnumerable<LocalVariableDeclaration> localVariables, IExpression body )
            : base( location )
        {
            LocalVariables = localVariables;
            Body = body;
        }

        public IEnumerable<LocalVariableDeclaration> LocalVariables { get; }

        public IExpression Body { get; }

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
                foreach(var local in LocalVariables)
                {
                    yield return local;
                }

                yield return Body;
            }
        }

        public override string ToString( )
        {
            var bldr = new StringBuilder( "VarIn{" );
            bldr.AppendJoin( ',', LocalVariables )
                .Append( "}(" )
                .Append( Body )
                .Append( ')' );
            return bldr.ToString();
        }
    }
}
