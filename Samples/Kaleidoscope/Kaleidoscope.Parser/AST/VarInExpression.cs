// -----------------------------------------------------------------------
// <copyright file="VarInExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;

using Ubiquity.NET.ArgValidators;

namespace Kaleidoscope.Grammar.AST
{
    public class VarInExpression
        : IExpression
    {
        public VarInExpression( SourceSpan location, IEnumerable<LocalVariableDeclaration> localVariables, IExpression body )
        {
            Location = location;
            LocalVariables = localVariables;
            Body = body;
        }

        public SourceSpan Location { get; }

        public IEnumerable<LocalVariableDeclaration> LocalVariables { get; }

        public IExpression Body { get; }

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            return visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );
        }

        public IEnumerable<IAstNode> Children
        {
            get
            {
                foreach( var local in LocalVariables )
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
            return bldr.ToString( );
        }
    }
}
