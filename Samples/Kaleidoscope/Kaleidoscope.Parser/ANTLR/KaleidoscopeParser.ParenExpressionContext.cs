// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.ParenExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.ANTLR
{
    public partial class KaleidoscopeParser
    {
        public partial class ParenExpressionContext
        {
            public ExpressionContext Expression => ( ExpressionContext )GetChild( 1 );
        }
    }
}
