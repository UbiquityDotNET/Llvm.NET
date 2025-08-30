// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public class KaleidoscopeAstVisitorBase<TResult>
        : IKaleidoscopeAstVisitor<TResult>
        , IAstVisitor<TResult>
    {
        public TResult? Visit( IAstNode node )
        {
            Debug.WriteLine( "WARNING using low performance ambiguous type Visit method" );
            return node switch
            {
                RootNode n => Visit( n ),
                ErrorNode<DiagnosticCode> n => Visit( n ),
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

        public virtual TResult? Visit( RootNode root ) => VisitChildren( root );

        public virtual TResult? Visit( ErrorNode<DiagnosticCode> errorNode ) => default;

        public virtual TResult? Visit( Prototype prototype ) => VisitChildren( prototype );

        public virtual TResult? Visit( FunctionDefinition definition ) => VisitChildren( definition );

        public virtual TResult? Visit( ConstantExpression constant ) => VisitChildren( constant );

        public virtual TResult? Visit( VariableReferenceExpression reference ) => VisitChildren( reference );

        public virtual TResult? Visit( FunctionCallExpression functionCall ) => VisitChildren( functionCall );

        public virtual TResult? Visit( BinaryOperatorExpression binaryOperator ) => VisitChildren( binaryOperator );

        public virtual TResult? Visit( VarInExpression varInExpression ) => VisitChildren( varInExpression );

        public virtual TResult? Visit( ParameterDeclaration parameterDeclaration ) => VisitChildren( parameterDeclaration );

        public virtual TResult? Visit( ConditionalExpression conditionalExpression ) => VisitChildren( conditionalExpression );

        public virtual TResult? Visit( ForInExpression forInExpression ) => VisitChildren( forInExpression );

        public virtual TResult? Visit( LocalVariableDeclaration localVariableDeclaration ) => VisitChildren( localVariableDeclaration );

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

        protected KaleidoscopeAstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        protected TResult? DefaultResult { get; }
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Closely related" )]
    public class KaleidoscopeAstVisitorBase<TResult, TArg>
        : IKaleidoscopeAstVisitor<TResult, TArg>
        , IAstVisitor<TResult, TArg>
#if NET9_0_OR_GREATER
        where TArg : struct, allows ref struct
#else
        where TArg : struct
#endif
    {
        public TResult? Visit( IAstNode node, ref readonly TArg arg )
        {
            Debug.WriteLine( "WARNING using low performance ambiguous type Visit method" );
            return node switch
            {
                RootNode n => Visit( n, in arg ),
                ErrorNode<DiagnosticCode> n => Visit( n, in arg ),
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

        public virtual TResult? Visit( RootNode root, ref readonly TArg arg ) => VisitChildren( root, in arg );

        public virtual TResult? Visit( ErrorNode<DiagnosticCode> errorNode, ref readonly TArg arg ) => default;

        public virtual TResult? Visit( Prototype prototype, ref readonly TArg arg ) => VisitChildren( prototype, in arg );

        public virtual TResult? Visit( FunctionDefinition definition, ref readonly TArg arg ) => VisitChildren( definition, in arg );

        public virtual TResult? Visit( ConstantExpression constant, ref readonly TArg arg ) => VisitChildren( constant, in arg );

        public virtual TResult? Visit( VariableReferenceExpression reference, ref readonly TArg arg ) => VisitChildren( reference, in arg );

        public virtual TResult? Visit( FunctionCallExpression functionCall, ref readonly TArg arg ) => VisitChildren( functionCall, in arg );

        public virtual TResult? Visit( BinaryOperatorExpression binaryOperator, ref readonly TArg arg ) => VisitChildren( binaryOperator, in arg );

        public virtual TResult? Visit( VarInExpression varInExpression, ref readonly TArg arg ) => VisitChildren( varInExpression, in arg );

        public virtual TResult? Visit( ParameterDeclaration parameterDeclaration, ref readonly TArg arg ) => VisitChildren( parameterDeclaration, in arg );

        public virtual TResult? Visit( ConditionalExpression conditionalExpression, ref readonly TArg arg ) => VisitChildren( conditionalExpression, in arg );

        public virtual TResult? Visit( ForInExpression forInExpression, ref readonly TArg arg ) => VisitChildren( forInExpression, in arg );

        public virtual TResult? Visit( LocalVariableDeclaration localVariableDeclaration, ref readonly TArg arg ) => VisitChildren( localVariableDeclaration, in arg );

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

        protected KaleidoscopeAstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        protected TResult? DefaultResult { get; }
    }
}
