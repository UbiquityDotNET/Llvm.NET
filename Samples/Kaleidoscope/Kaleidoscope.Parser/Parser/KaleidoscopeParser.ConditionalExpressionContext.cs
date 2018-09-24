// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class ConditionalExpressionContext
        {
            public ExpressionContext Condition => expression( 0 );

            public ExpressionContext ThenExpression => expression( 1 );

            public ExpressionContext ElseExpression => expression( 2 );
        }
    }
}
