// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Base class implementation of a visitor for the Kaleidoscope language</summary>
    /// <typeparam name="TResult">Type of the result for visitation</typeparam>
    public class KaleidoscopeAstVisitorBase<TResult>
        : IKaleidoscopeAstVisitor<TResult>
        , IAstVisitor<TResult>
    {
        /// <inheritdoc/>
        public TResult? Visit( IAstNode node )
        {
            Debug.WriteLine( "WARNING using low performance ambiguous type Visit method" );
            return node switch
            {
                RootNode n => Visit( n ),
                ErrorNode n => Visit( n ),
                Prototype n => Visit( n ),
                FunctionDefinition n => Visit( n ),
                ConstantExpression n => Visit( n ),
                VariableReferenceExpression n => Visit( n ),
                FunctionCallExpression n => Visit( n ),
                BinaryOperatorExpression n => Visit( n ),
                VarInExpression n => Visit( n ),
                ParameterDeclaration n => Visit( n ),
                ConditionalExpression n => Visit( n ),
                ForInExpression n => Visit( n ),
                LocalVariableDeclaration n => Visit( n ),
                _ => DefaultResult
            };
        }

        /// <inheritdoc/>
        public virtual TResult? Visit( RootNode root ) => VisitChildren( root );

        /// <inheritdoc/>
        public virtual TResult? Visit( ErrorNode errorNode ) => default;

        /// <inheritdoc/>
        public virtual TResult? Visit( Prototype prototype ) => VisitChildren( prototype );

        /// <inheritdoc/>
        public virtual TResult? Visit( FunctionDefinition definition ) => VisitChildren( definition );

        /// <inheritdoc/>
        public virtual TResult? Visit( ConstantExpression constant ) => VisitChildren( constant );

        /// <inheritdoc/>
        public virtual TResult? Visit( VariableReferenceExpression reference ) => VisitChildren( reference );

        /// <inheritdoc/>
        public virtual TResult? Visit( FunctionCallExpression functionCall ) => VisitChildren( functionCall );

        /// <inheritdoc/>
        public virtual TResult? Visit( BinaryOperatorExpression binaryOperator ) => VisitChildren( binaryOperator );

        /// <inheritdoc/>
        public virtual TResult? Visit( VarInExpression varInExpression ) => VisitChildren( varInExpression );

        /// <inheritdoc/>
        public virtual TResult? Visit( ParameterDeclaration parameterDeclaration ) => VisitChildren( parameterDeclaration );

        /// <inheritdoc/>
        public virtual TResult? Visit( ConditionalExpression conditionalExpression ) => VisitChildren( conditionalExpression );

        /// <inheritdoc/>
        public virtual TResult? Visit( ForInExpression forInExpression ) => VisitChildren( forInExpression );

        /// <inheritdoc/>
        public virtual TResult? Visit( LocalVariableDeclaration localVariableDeclaration ) => VisitChildren( localVariableDeclaration );

        /// <summary>Visits children of a node</summary>
        /// <param name="node">node to visit the children</param>
        /// <returns>Result of visit</returns>
        public virtual TResult? VisitChildren( IAstNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            TResult? aggregate = DefaultResult;
            foreach(var child in node.Children)
            {
                aggregate = AggregateResult( aggregate, child.Accept( this ) );
            }

            return aggregate;
        }

        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeAstVisitorBase{TResult}"/> class.</summary>
        /// <param name="defaultResult">Default value for a result</param>
        protected KaleidoscopeAstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        /// <summary>Aggregates a result into the final result for a visitation</summary>
        /// <param name="aggregate">Current aggregate value of the result</param>
        /// <param name="newResult">New result to aggregate into the result</param>
        /// <returns>Aggregate result</returns>
        /// <remarks>
        /// The default base implementation simply returns <paramref name="newResult"/>
        /// ignoring <paramref name="aggregate"/>. Derived types can provide an override
        /// for different behavior as needed.
        /// </remarks>
        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        /// <summary>Gets the default result value that starts the aggregation process</summary>
        protected TResult? DefaultResult { get; }
    }

    /// <summary>Base class implementation of a visitor for the Kaleidoscope language with an argument</summary>
    /// <typeparam name="TResult">Type of the result for visitation</typeparam>
    /// <typeparam name="TArg">Type of the argument</typeparam>
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Closely related" )]
    public class KaleidoscopeAstVisitorBase<TResult, TArg>
        : IKaleidoscopeAstVisitor<TResult, TArg>
        , IAstVisitor<TResult, TArg>
        where TArg : struct
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        /// <inheritdoc/>
        public TResult? Visit( IAstNode node, ref readonly TArg arg )
        {
            Debug.WriteLine( "WARNING using low performance ambiguous type Visit method" );
            return node switch
            {
                RootNode n => Visit( n, in arg ),
                ErrorNode n => Visit( n, in arg ),
                Prototype n => Visit( n, in arg ),
                FunctionDefinition n => Visit( n, in arg ),
                ConstantExpression n => Visit( n, in arg ),
                VariableReferenceExpression n => Visit( n, in arg ),
                FunctionCallExpression n => Visit( n, in arg ),
                BinaryOperatorExpression n => Visit( n, in arg ),
                VarInExpression n => Visit( n, in arg ),
                ParameterDeclaration n => Visit( n, in arg ),
                ConditionalExpression n => Visit( n, in arg ),
                ForInExpression n => Visit( n, in arg ),
                LocalVariableDeclaration n => Visit( n, in arg ),
                _ => DefaultResult
            };
        }

        /// <inheritdoc/>
        public virtual TResult? Visit( RootNode root, ref readonly TArg arg ) => VisitChildren( root, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( ErrorNode errorNode, ref readonly TArg arg ) => default;

        /// <inheritdoc/>
        public virtual TResult? Visit( Prototype prototype, ref readonly TArg arg ) => VisitChildren( prototype, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( FunctionDefinition definition, ref readonly TArg arg ) => VisitChildren( definition, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( ConstantExpression constant, ref readonly TArg arg ) => VisitChildren( constant, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( VariableReferenceExpression reference, ref readonly TArg arg ) => VisitChildren( reference, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( FunctionCallExpression functionCall, ref readonly TArg arg ) => VisitChildren( functionCall, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( BinaryOperatorExpression binaryOperator, ref readonly TArg arg ) => VisitChildren( binaryOperator, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( VarInExpression varInExpression, ref readonly TArg arg ) => VisitChildren( varInExpression, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( ParameterDeclaration parameterDeclaration, ref readonly TArg arg ) => VisitChildren( parameterDeclaration, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( ConditionalExpression conditionalExpression, ref readonly TArg arg ) => VisitChildren( conditionalExpression, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( ForInExpression forInExpression, ref readonly TArg arg ) => VisitChildren( forInExpression, in arg );

        /// <inheritdoc/>
        public virtual TResult? Visit( LocalVariableDeclaration localVariableDeclaration, ref readonly TArg arg ) => VisitChildren( localVariableDeclaration, in arg );

        /// <summary>Visits children of a node</summary>
        /// <param name="node">node to visit the children</param>
        /// <param name="arg">argument value to provide to use for visiting each child</param>
        /// <returns>Result of visit</returns>
        public virtual TResult? VisitChildren( IAstNode node, ref readonly TArg arg )
        {
            ArgumentNullException.ThrowIfNull( node );
            TResult? aggregate = DefaultResult;
            foreach(var child in node.Children)
            {
                aggregate = AggregateResult( aggregate, child.Accept( this, in arg ) );
            }

            return aggregate;
        }

        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeAstVisitorBase{TResult, TArg}"/> class.</summary>
        /// <param name="defaultResult">Default value for a result</param>
        protected KaleidoscopeAstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        /// <summary>Aggregates a result into the final result for a visitation</summary>
        /// <param name="aggregate">Current aggregate value of the result</param>
        /// <param name="newResult">New result to aggregate into the result</param>
        /// <returns>Aggregate result</returns>
        /// <remarks>
        /// The default base implementation simply returns <paramref name="newResult"/>
        /// ignoring <paramref name="aggregate"/>. Derived types can provide an override
        /// for different behavior as needed.
        /// </remarks>
        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        /// <summary>Gets the default result value that starts the aggregation process</summary>
        protected TResult? DefaultResult { get; }
    }
}
