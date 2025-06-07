// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.ParenExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class ParenExpressionContext
        {
            public ExpressionContext Expression => (ExpressionContext)GetChild( 1 );
        }
    }
}
