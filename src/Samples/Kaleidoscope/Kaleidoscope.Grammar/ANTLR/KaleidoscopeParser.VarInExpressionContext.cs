﻿// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.VarInExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class VarInExpressionContext
        {
            public IReadOnlyList<InitializerContext> Initiaizers => initializer( );

            public ExpressionContext Scope => expression( );
        }
    }
}
