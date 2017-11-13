// <copyright file="ContextDeconstructors.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope.Grammar
{
    /// <summary>Static utility class to provide deconstruction of various parser rule contexts</summary>
    public static class ContextDeconstructors
    {
        public static string GetName( this VariableExpressionContext context )
        {
            return ((IdentifierContext)context.GetChild(0)).GetName();
        }

        public static double GetValue( this ConstExpressionContext context )
        {
            return double.Parse( context.Number().GetText() );
        }

        public static ExpressionContext GetExpression( this ParenExpressionContext context )
        {
            return (ExpressionContext)context.GetChild( 1 );
        }

        public static string GetName( this IdentifierContext context )
        {
            return context.Identifier().ToString( );
        }

        public static void Deconstruct( this FunctionCallExpressionContext context, out string calleeName, out IList<ExpressionContext> args )
        {
            calleeName = ( ( IdentifierContext )context.GetChild( 0 ) ).GetName( );
            args = context.expression( );
        }

        public static void Deconstruct( this InitializerContext context, out string Name, out ExpressionContext Value )
        {
            Name = ( ( IdentifierContext )context.GetChild( 0 ) ).GetName( );
            Value = context.expression();
        }

        public static void Deconstruct( this VarInExpressionContext context
                                      , out IList<(string Name, ExpressionContext Value)> Initializers
                                      , out ExpressionContext Scope
                                      )
        {
            Initializers = context.initializer( )
                                  .Select( i =>
                                    {
                                        var (name, value) = i;
                                        return (name, value);
                                    })
                                  .ToList( );
            Scope = context.GetRuleContext<ExpressionContext>( context.ChildCount - 1 );
        }

        public static void Deconstruct( this ConditionalExpressionContext context
                                      , out ExpressionContext Condition
                                      , out ExpressionContext ThenExpression
                                      , out ExpressionContext ElseExpression
                                      )
        {
            Condition = context.GetRuleContext<ExpressionContext>( 1 );
            ThenExpression = context.GetRuleContext<ExpressionContext>( 3 );
            ElseExpression = context.GetRuleContext<ExpressionContext>( 5 );
        }

        public static void Deconstruct( this ForExpressionContext context
                                      , out InitializerContext Initializer
                                      , out ExpressionContext EndExpression
                                      , out ExpressionContext StepExpression
                                      , out ExpressionContext BodyExpression
                                      )
        {
            Initializer = context.GetRuleContext<InitializerContext>( 1 );
            var expressions = context.expression( );
            EndExpression = expressions[ 0 ];
            StepExpression = expressions[ 1 ];
            BodyExpression = expressions[ 2 ];
        }

        public static void Deconstruct( this AssignmentExpressionContext context
                                      , out string VariableName
                                      , out ExpressionContext Value
                                      )
        {
            VariableName = context.identifier( ).GetName( );
            Value = context.expression( );
        }

        public static void Deconstruct( this UnaryOpExpressionContext context, out char Op, out ExpressionContext Rhs )
        {
            Op = context.LETTER( ).GetText( )[ 0 ];
            Rhs = context.expression( );
        }

        public static void Deconstruct( this BinaryOpExpressionContext context
                                      , out ExpressionContext Lhs
                                      , out char Op
                                      , out ExpressionContext Rhs
                                      )
        {
            Lhs = context.expression( 0 );
            Op = context.LETTER( ).GetText( )[ 0 ];
            Rhs = context.expression( 1 );
        }

        public static void Deconstruct( this PrototypeContext context
                                      , out string Name
                                      , out IList<string> Parameters
                                      )
        {
            switch( context )
            {
            case UnaryProtoTypeContext unaryDef:
                Deconstruct( unaryDef, out Name, out Parameters );
                break;

            case BinaryProtoTypeContext binaryDef:
                Deconstruct( binaryDef, out Name, out Parameters );
                break;

            case FunctionProtoTypeContext funcDef:
                Deconstruct( funcDef, out Name, out Parameters );
                break;

            default:
                throw new ArgumentException( "Unknown PrototypeContext" );
            }
        }

        public static void Deconstruct( this BinaryProtoTypeContext context
                                      , out string Name
                                      , out IList<string> Parameters
                                      )
        {
            Name = $"$binary{context.LETTER( ).GetText( )}";
            Parameters = context.identifier( ).Select( i => i.GetName( ) ).ToList( );
        }

        public static void Deconstruct( this UnaryProtoTypeContext context
                                      , out string Name
                                      , out IList<string> Parameters
                                      )
        {
            Name = $"$unary{context.LETTER( ).GetText( )}";
            Parameters = new List<string> { context.identifier( ).GetText( ) };
        }

        public static void Deconstruct( this FunctionProtoTypeContext context
                                      , out string Name
                                      , out IList<string> Parameters
                                      )
        {
            Name = context.identifier( 0 ).GetName( );
            Parameters = context.identifier( ).Skip( 1 ).Select( i => i.GetName( ) ).ToList( );
        }

        public static void Deconstruct( this FunctionDefinitionContext context
                                      , out PrototypeContext Signature
                                      , out ExpressionContext BodyExpression
                                      )
        {
            Signature = context.prototype( );
            BodyExpression = context.expression( );
        }
    }
}
