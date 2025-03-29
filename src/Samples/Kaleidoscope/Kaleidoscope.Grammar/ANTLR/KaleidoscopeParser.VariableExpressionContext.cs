﻿// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.VariableExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class VariableExpressionContext
        {
            public string Name => Identifier( ).GetText( );
        }
    }
}
