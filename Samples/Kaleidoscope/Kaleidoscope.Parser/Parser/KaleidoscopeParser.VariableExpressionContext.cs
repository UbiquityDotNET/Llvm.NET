// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.VariableExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class VariableExpressionContext
        {
            public string Name => Identifier( ).GetText( );
        }
    }
}
