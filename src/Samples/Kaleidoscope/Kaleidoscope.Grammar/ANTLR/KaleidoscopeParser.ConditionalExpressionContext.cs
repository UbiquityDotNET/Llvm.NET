// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class ConditionalExpressionContext
        {
            public ExpressionContext Condition => expression( 0 );

            public ExpressionContext ThenExpression => expression( 1 );

            public ExpressionContext ElseExpression => expression( 2 );
        }
    }
}
