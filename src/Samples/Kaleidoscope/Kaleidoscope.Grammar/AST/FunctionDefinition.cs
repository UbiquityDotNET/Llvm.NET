// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;

using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

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
        /// <summary>Initializes a new instance of the <see cref="FunctionDefinition"/> class.</summary>
        /// <param name="location">Location of the node in the input source</param>
        /// <param name="signature">Signature of the definition</param>
        /// <param name="body">Expression for the body of the function</param>
        /// <param name="isAnonymous">Flag to indicate whether this function is anonymous</param>
        public FunctionDefinition( SourceRange location
                                 , Prototype signature
                                 , IExpression body
                                 , bool isAnonymous = false
                                 )
            : this( location, signature, body, ImmutableArray.Create<LocalVariableDeclaration>(), isAnonymous )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FunctionDefinition"/> class.</summary>
        /// <param name="location">Location of the node in the input source</param>
        /// <param name="signature">Signature of the definition</param>
        /// <param name="body">Expression for the body of the function</param>
        /// <param name="localVariables">Local variables for this function</param>
        /// <param name="isAnonymous">Flag to indicate whether this function is anonymous</param>
        public FunctionDefinition( SourceRange location
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
        /// <remarks>
        /// This is useful during generation as anonymous expressions are discardable once they are generated and invoked
        /// or inlined/merged in AOT scenarios.
        /// </remarks>
        public bool IsAnonymous { get; }

        /// <summary>Gets the name of this function definition</summary>
        public string Name => Signature.Name;

        /// <summary>Gets the parameters for this function definition</summary>
        public IReadOnlyList<ParameterDeclaration> Parameters => Signature.Parameters;

        /// <summary>Gets the local variables for this function definition</summary>
        public IReadOnlyList<LocalVariableDeclaration> LocalVariables { get; }

        /// <inheritdoc cref="BinaryOperatorExpression.Accept{TResult}(IAstVisitor{TResult})"/>
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
                yield return Signature;
                yield return Body;
            }
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"{Signature}{{{Body}}}";
        }
    }
}
