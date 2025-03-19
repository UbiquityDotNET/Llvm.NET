// -----------------------------------------------------------------------
// <copyright file="AstVisitorBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace Kaleidoscope.Grammar.AST
{
    public class AstVisitorBase<TResult>
        : IAstVisitor<TResult>
        where TResult : class
    {
        public virtual TResult? Visit( RootNode root ) => VisitChildren( root );

        public virtual TResult? Visit( ErrorNode errorNode ) => default;

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
            foreach( var child in node.Children )
            {
                aggregate = AggregateResult( aggregate, child.Accept( this ) );
            }

            return aggregate;
        }

        protected AstVisitorBase( [AllowNull] TResult defaultResult )
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
    public class AstVisitorBase<TResult, TArg>
        : IAstVisitor<TResult, TArg>
        where TResult : class
        where TArg : struct, allows ref struct
    {
        public virtual TResult? Visit( RootNode root, ref readonly TArg arg ) => VisitChildren( root, in arg );

        public virtual TResult? Visit( ErrorNode errorNode, ref readonly TArg arg ) => default;

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
            foreach( var child in node.Children )
            {
                aggregate = AggregateResult( aggregate, child.Accept( this, in arg ) );
            }

            return aggregate;
        }

        protected AstVisitorBase( [AllowNull] TResult defaultResult )
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
