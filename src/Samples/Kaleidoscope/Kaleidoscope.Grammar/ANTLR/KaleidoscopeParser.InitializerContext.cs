// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.InitializerContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class InitializerContext
        {
            public string Name => Identifier().GetText();

            public ExpressionContext Value => expression();
        }
    }
}
