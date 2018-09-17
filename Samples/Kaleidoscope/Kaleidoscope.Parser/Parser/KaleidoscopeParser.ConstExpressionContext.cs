// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class ConstExpressionContext
        {
            public double Value => double.Parse( Number( ).GetText( ) );
        }
    }
}
