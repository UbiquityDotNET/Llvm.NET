// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.ExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Antlr4.Runtime.Tree;

namespace Kaleidoscope.Grammar.ANTLR
{
    public partial class KaleidoscopeParser
    {
        public partial class ExpressionContext
        {
            public PrimaryExpressionContext Atom => primaryExpression( );

            public bool IsAssignment => binaryop( ).Length > 0 && binaryop( )[ 0 ].Start.Type == ASSIGN;

            public VariableExpressionContext? AssignmentTarget => IsAssignment ? GetChild<VariableExpressionContext>( 0 ) : null;

            public IEnumerable<(BinaryopContext op, IParseTree rhs)> OperatorExpressions
            {
                get
                {
                    // Expression: PrimaryExpression (op expression)*
                    for( int i = 1; i < ChildCount - 1; i += 2 )
                    {
                        yield return (( BinaryopContext )children[ i ], children[ i + 1 ]);
                    }
                }
            }
        }
    }
}
