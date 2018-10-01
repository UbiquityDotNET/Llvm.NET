// <copyright file="AstVisitor.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar.AST
{
    public interface IAstVisitor<TResult>
    {
        TResult Visit( RootNode root );

        TResult Visit( Prototype prototype );

        TResult Visit( FunctionDefinition definition );

        TResult Visit( ConstantExpression constant );

        TResult Visit( VariableReferenceExpression reference );

        TResult Visit( FunctionCallExpression functionCall );

        TResult Visit( BinaryOperatorExpression binaryOperator );

        TResult Visit( VarInExpression varInExpression );

        TResult Visit( ParameterDeclaration parameterDeclaration );

        TResult Visit( ConditionalExpression conditionalExpression );

        TResult Visit( ForInExpression forInExpression );

        TResult Visit( LocalVariableDeclaration localVariableDeclaration );
    }
}
