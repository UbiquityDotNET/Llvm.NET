﻿// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.ConditionalExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
