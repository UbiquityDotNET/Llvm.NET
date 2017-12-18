// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Antlr4.Runtime.Tree;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class ExpressionContext
        {
            public PrimaryExpressionContext Atom => primaryExpression( );

            public IEnumerable<(OpsymbolContext op, IParseTree rhs)> OperatorExpressions
            {
                get
                {
                    // Expression: PrimaryExpression (op expression)*
                    for( int i = 1; i < ChildCount - 1; i += 2 )
                    {
                        yield return ((OpsymbolContext)children[ i ], children[ i + 1 ]);
                    }
                }
            }
        }
    }
}
