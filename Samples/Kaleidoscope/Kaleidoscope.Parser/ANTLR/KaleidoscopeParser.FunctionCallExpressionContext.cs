﻿// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.FunctionCallExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class FunctionCallExpressionContext
        {
            public string CaleeName => Identifier( ).GetText( );

            public IReadOnlyList<ExpressionContext> Args => expression( );
        }
    }
}
