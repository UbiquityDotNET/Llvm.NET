// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class InitializerContext
        {
            public string Name => Identifier( ).GetText( );

            public ExpressionContext Value => expression( );
        }
    }
}
