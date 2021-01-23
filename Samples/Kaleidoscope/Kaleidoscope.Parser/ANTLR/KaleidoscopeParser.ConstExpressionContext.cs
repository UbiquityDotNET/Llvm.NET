// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.ConstExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace Kaleidoscope.Grammar.ANTLR
{
    public partial class KaleidoscopeParser
    {
        public partial class ConstExpressionContext
        {
            public double Value => double.Parse( Number( ).GetText( ), CultureInfo.InvariantCulture );
        }
    }
}
