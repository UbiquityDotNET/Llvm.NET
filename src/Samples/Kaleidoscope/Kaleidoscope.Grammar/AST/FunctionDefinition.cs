// -----------------------------------------------------------------------
// <copyright file="FunctionDefinition.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>AST type for a function definition</summary>
    /// <remarks>
    /// This supports construction of anonymous functions
    /// that do not have an explicit prototype in the source language
    /// </remarks>
    public sealed class FunctionDefinition
        : AstNode
        , IAstNode
    {
        public FunctionDefinition( SourceLocation location
                                 , Prototype signature
                                 , IExpression body
                                 , bool isAnonymous = false
                                 )
            : this( location, signature, body, ImmutableArray.Create<LocalVariableDeclaration>(), isAnonymous )
        {
        }

        public FunctionDefinition( SourceLocation location
                                 , Prototype signature
                                 , IExpression body
                                 , IImmutableList<LocalVariableDeclaration> localVariables
                                 , bool isAnonymous = false
                                 )
            : base( location )
        {
            Signature = signature;
            Body = body;
            IsAnonymous = isAnonymous;
            LocalVariables = localVariables;
        }

        /// <summary>Gets the prototype signature for the function</summary>
        public Prototype Signature { get; }

        /// <summary>Gets the body of the function as an expression tree</summary>
        public IExpression Body { get; }

        /// <summary>Gets a value indicating whether this function is an anonymous top level expression</summary>
        /// <remarks>This is useful during generation as anonymous expressions are discardable once they are generated</remarks>
        public bool IsAnonymous { get; }

        public string Name => Signature.Name;

        public IReadOnlyList<ParameterDeclaration> Parameters => Signature.Parameters;

        public IReadOnlyList<LocalVariableDeclaration> LocalVariables { get; }

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
                yield return Signature;
                yield return Body;
            }
        }

        public override string ToString( )
        {
            return $"{Signature}{{{Body}}}";
        }
    }
}
