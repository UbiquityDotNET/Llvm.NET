// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class ParenExpressionContext
        {
            public ExpressionContext Expression => ( ExpressionContext )GetChild( 1 );
        }
    }
}
