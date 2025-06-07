// -----------------------------------------------------------------------
// <copyright file="FunctionCallExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public sealed class FunctionCallExpression
        : AstNode
        , IExpression
    {
        public FunctionCallExpression( SourceLocation location, Prototype functionPrototype, params IEnumerable<IExpression> args )
            : base( location )
        {
            FunctionPrototype = functionPrototype;
            Arguments = [ .. args ];
        }

        public Prototype FunctionPrototype { get; }

        public IReadOnlyList<IExpression> Arguments { get; }

        public override IEnumerable<IAstNode> Children
        {
            get
            {
                yield return FunctionPrototype;
            }
        }

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

        public override string ToString( )
        {
            return Arguments.Count == 0
                ? $"Call({FunctionPrototype})"
                : $"Call({FunctionPrototype}, {string.Join( ",", Arguments.Select( a => a.ToString() ) )})";
        }
    }
}
