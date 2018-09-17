// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class FunctionCallExpressionContext
        {
            public string CaleeName => Identifier( ).GetText( );

            public IReadOnlyList<ExpressionContext> Args => expression( );
        }
    }
}
