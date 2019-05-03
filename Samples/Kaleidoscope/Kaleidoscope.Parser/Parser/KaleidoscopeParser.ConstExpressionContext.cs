// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Globalization;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class ConstExpressionContext
        {
            public double Value => double.Parse( Number( ).GetText( ), CultureInfo.InvariantCulture );
        }
    }
}
