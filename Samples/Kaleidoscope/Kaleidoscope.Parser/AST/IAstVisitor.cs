// -----------------------------------------------------------------------
// <copyright file="IAstVisitor.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.AST
{
    public interface IAstVisitor<out TResult>
        where TResult : class
    {
        TResult? Visit( RootNode root );

        TResult? Visit( ErrorNode errorNode );

        TResult? Visit( Prototype prototype );

        TResult? Visit( FunctionDefinition definition );

        TResult? Visit( ConstantExpression constant );

        TResult? Visit( VariableReferenceExpression reference );

        TResult? Visit( FunctionCallExpression functionCall );

        TResult? Visit( BinaryOperatorExpression binaryOperator );

        TResult? Visit( VarInExpression varInExpression );

        TResult? Visit( ParameterDeclaration parameterDeclaration );

        TResult? Visit( ConditionalExpression conditionalExpression );

        TResult? Visit( ForInExpression forInExpression );

        TResult? Visit( LocalVariableDeclaration localVariableDeclaration );
    }
}
