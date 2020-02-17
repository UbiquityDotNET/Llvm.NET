// -----------------------------------------------------------------------
// <copyright file="VarInExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.AST
{
    public class VarInExpression
        : IExpression
    {
        public VarInExpression( SourceSpan location, IEnumerable<LocalVariableDeclaration> localVariables, IExpression body)
        {
            Location = location;
            LocalVariables = localVariables;
            Body = body;
        }

        public SourceSpan Location { get; }

        public IEnumerable<LocalVariableDeclaration> LocalVariables { get; }

        public IExpression Body { get; }

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );

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
            foreach(var local in LocalVariables)
            {
                bldr.Append( local );
            }

            bldr.Append( "}(" );
            bldr.Append( Body );
            bldr.Append( ')' );
            return bldr.ToString( );
        }
    }
}
