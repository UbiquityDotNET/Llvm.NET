// <copyright file="IAstNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.AST
{
    /*
    Child nodes defined for higher level constructs only.
    UnaryOpExpressionContext and BinaryOpExpressionCOntext are
    collapsed to just a FunctionCall for user defined operators)
    Similarly for prototypes the distinction from unary, binary
    and regular function are collapsed to just a declared function
    Precedence and paren expressions are no longer relevent once parsed,
    the AST just provides an ordered sequence of expressions.
    The built-in operators don't use the function call form so they
    still have a representation in the AST.

    Thus the node types produced are:
        Function Declaration (including extern)
        Function Definition
        BinaryOperator
        FunctionCall
        VariableReference
        ConstExpression
        ConditionalExpression
        ForExpression
        VarInExpression
    */

    public interface IAstNode
    {
        SourceSpan Location { get; }

        IEnumerable<IAstNode> Children { get; }

        TResult Accept<TResult>( IAstVisitor<TResult> visitor );
    }
}
