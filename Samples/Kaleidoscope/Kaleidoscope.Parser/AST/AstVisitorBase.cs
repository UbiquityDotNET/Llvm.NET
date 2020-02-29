// -----------------------------------------------------------------------
// <copyright file="AstVisitorBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.AST
{
    public class AstVisitorBase<TResult>
        : IAstVisitor<TResult>
        where TResult : class
    {
        public virtual TResult? Visit( RootNode root ) => VisitChildren( root );

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
            node.ValidateNotNull( nameof( node ) );
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
}
