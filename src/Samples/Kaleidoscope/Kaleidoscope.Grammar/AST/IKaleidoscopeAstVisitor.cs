// -----------------------------------------------------------------------
// <copyright file="IKaleidoscopeAstVisitor.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public interface IKaleidoscopeAstVisitor<out TResult>
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

    public interface IKaleidoscopeAstVisitor<out TResult, TArg>
        where TArg : struct, allows ref struct
    {
        TResult? Visit( RootNode root, ref readonly TArg arg );

        TResult? Visit( ErrorNode errorNode, ref readonly TArg arg );

        TResult? Visit( Prototype prototype, ref readonly TArg arg );

        TResult? Visit( FunctionDefinition definition, ref readonly TArg arg );

        TResult? Visit( ConstantExpression constant, ref readonly TArg arg );

        TResult? Visit( VariableReferenceExpression reference, ref readonly TArg arg );

        TResult? Visit( FunctionCallExpression functionCall, ref readonly TArg arg );

        TResult? Visit( BinaryOperatorExpression binaryOperator, ref readonly TArg arg );

        TResult? Visit( VarInExpression varInExpression, ref readonly TArg arg );

        TResult? Visit( ParameterDeclaration parameterDeclaration, ref readonly TArg arg );

        TResult? Visit( ConditionalExpression conditionalExpression, ref readonly TArg arg );

        TResult? Visit( ForInExpression forInExpression, ref readonly TArg arg );

        TResult? Visit( LocalVariableDeclaration localVariableDeclaration, ref readonly TArg arg );
    }
}
