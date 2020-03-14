// -----------------------------------------------------------------------
// <copyright file="FunctionDefinition.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>AST type for a function definition</summary>
    /// <remarks>
    /// This supports construction of anonymous functions
    /// that do not have an explicit prototype in the source language
    /// </remarks>
    public class FunctionDefinition
        : IAstNode
    {
        public FunctionDefinition( SourceSpan location
                                 , Prototype signature
                                 , IExpression body
                                 , bool isAnonymous = false
                                 )
            : this( location, signature, body, ImmutableArray.Create<LocalVariableDeclaration>( ), isAnonymous )
        {
        }

        public FunctionDefinition( SourceSpan location
                                 , Prototype signature
                                 , IExpression body
                                 , IImmutableList<LocalVariableDeclaration> localVariables
                                 , bool isAnonymous = false
                                 )
        {
            Signature = signature;
            Body = body;
            IsAnonymous = isAnonymous;
            Location = location;
            LocalVariables = localVariables;
        }

        public SourceSpan Location { get; }

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

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            return visitor.ValidateNotNull( nameof( visitor ) ).Visit( this );
        }

        public IEnumerable<IAstNode> Children
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
