// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Interface for a visitor of a Kaleidoscope AST</summary>
    /// <typeparam name="TResult">Type of the result of a visit</typeparam>
    public interface IKaleidoscopeAstVisitor<out TResult>
    {
        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(RootNode, ref readonly TArg)"/>
        TResult? Visit( RootNode root );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(ErrorNode, ref readonly TArg)"/>
        TResult? Visit( ErrorNode errorNode );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(Prototype, ref readonly TArg)"/>
        TResult? Visit( Prototype prototype );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(FunctionDefinition, ref readonly TArg)"/>
        TResult? Visit( FunctionDefinition definition );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(ConstantExpression, ref readonly TArg)"/>
        TResult? Visit( ConstantExpression constant );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(VariableReferenceExpression, ref readonly TArg)"/>
        TResult? Visit( VariableReferenceExpression reference );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(FunctionCallExpression, ref readonly TArg)"/>
        TResult? Visit( FunctionCallExpression functionCall );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(BinaryOperatorExpression, ref readonly TArg)"/>
        TResult? Visit( BinaryOperatorExpression binaryOperator );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(VarInExpression, ref readonly TArg)"/>
        TResult? Visit( VarInExpression varInExpression );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(ParameterDeclaration, ref readonly TArg)"/>
        TResult? Visit( ParameterDeclaration parameterDeclaration );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(ConditionalExpression, ref readonly TArg)"/>
        TResult? Visit( ConditionalExpression conditionalExpression );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(ForInExpression, ref readonly TArg)"/>
        TResult? Visit( ForInExpression forInExpression );

        /// <inheritdoc cref="IKaleidoscopeAstVisitor{TResult, TArg}.Visit(LocalVariableDeclaration, ref readonly TArg)"/>
        TResult? Visit( LocalVariableDeclaration localVariableDeclaration );
    }

    /// <summary>Interface for a visitor of a Kaleidoscope AST that uses an argument</summary>
    /// <typeparam name="TResult">Type of the result of a visit</typeparam>
    /// <typeparam name="TArg">Argument to pass to the visitors</typeparam>
    public interface IKaleidoscopeAstVisitor<out TResult, TArg>
        where TArg : struct
#if NET9_0_OR_GREATER
                   , allows ref struct
#endif
    {
        /// <summary>Visits a root node and all of it's children recursively</summary>
        /// <param name="root">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <returns>Result of the visit</returns>
        TResult? Visit( RootNode root, ref readonly TArg arg );

        /// <summary>Visits an error node and all of it's children recursively</summary>
        /// <param name="errorNode">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( ErrorNode errorNode, ref readonly TArg arg );

        /// <summary>Visits a prototype node and all of it's children recursively</summary>
        /// <param name="prototype">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( Prototype prototype, ref readonly TArg arg );

        /// <summary>Visits a function definition node and all of it's children recursively</summary>
        /// <param name="definition">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( FunctionDefinition definition, ref readonly TArg arg );

        /// <summary>Visits a constant expression node and all of it's children recursively</summary>
        /// <param name="constant">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( ConstantExpression constant, ref readonly TArg arg );

        /// <summary>Visits a variable reference expression node and all of it's children recursively</summary>
        /// <param name="reference">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( VariableReferenceExpression reference, ref readonly TArg arg );

        /// <summary>Visits a function call node and all of it's children recursively</summary>
        /// <param name="functionCall">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( FunctionCallExpression functionCall, ref readonly TArg arg );

        /// <summary>Visits a binary operator node and all of it's children recursively</summary>
        /// <param name="binaryOperator">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( BinaryOperatorExpression binaryOperator, ref readonly TArg arg );

        /// <summary>Visits a <c>VarIn</c> expression node and all of it's children recursively</summary>
        /// <param name="varInExpression">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( VarInExpression varInExpression, ref readonly TArg arg );

        /// <summary>Visits a parameter declaration node and all of it's children recursively</summary>
        /// <param name="parameterDeclaration">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( ParameterDeclaration parameterDeclaration, ref readonly TArg arg );

        /// <summary>Visits a conditional expression node and all of it's children recursively</summary>
        /// <param name="conditionalExpression">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( ConditionalExpression conditionalExpression, ref readonly TArg arg );

        /// <summary>Visits a <c>ForIn</c> expression node and all of it's children recursively</summary>
        /// <param name="forInExpression">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( ForInExpression forInExpression, ref readonly TArg arg );

        /// <summary>Visits a local variable declaration node and all of it's children recursively</summary>
        /// <param name="localVariableDeclaration">node to visit</param>
        /// <param name="arg">argument value for the visit</param>
        /// <inheritdoc cref="Visit(RootNode, ref readonly TArg)" path="/returns"/>
        TResult? Visit( LocalVariableDeclaration localVariableDeclaration, ref readonly TArg arg );
    }
}
