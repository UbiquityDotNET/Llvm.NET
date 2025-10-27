// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Text;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Kaleidoscope AST node for a <c>Var-In</c> expression</summary>
    public sealed class VarInExpression
        : AstNode
        , IExpression
    {
        /// <summary>Initializes a new instance of the <see cref="VarInExpression"/> class.</summary>
        /// <param name="location">Location of the node in the input source</param>
        /// <param name="localVariables">Local variables for the expression</param>
        /// <param name="body">Body of the expression</param>
        public VarInExpression( SourceRange location, IEnumerable<LocalVariableDeclaration> localVariables, IExpression body )
            : base( location )
        {
            LocalVariables = localVariables;
            Body = body;
        }

        /// <summary>Gets the local variables for the expression</summary>
        public IEnumerable<LocalVariableDeclaration> LocalVariables { get; }

        /// <summary>Gets the body for the expression</summary>
        public IExpression Body { get; }

        /// <inheritdoc cref="BinaryOperatorExpression.Accept{TResult, TArg}(IAstVisitor{TResult, TArg}, ref readonly TArg)"/>
        public override TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult> klsVisitor
                   ? klsVisitor.Visit( this )
                   : visitor.Visit( this );
        }

        /// <inheritdoc cref="BinaryOperatorExpression.Accept{TResult, TArg}(IAstVisitor{TResult, TArg}, ref readonly TArg)"/>
        public override TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult, TArg> klsVisitor
                   ? klsVisitor.Visit( this, in arg )
                   : visitor.Visit( this, in arg );
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
