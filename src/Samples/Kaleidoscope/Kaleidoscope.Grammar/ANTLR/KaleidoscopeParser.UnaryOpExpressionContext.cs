// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.UnaryOpExpressionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class UnaryOpExpressionContext
        {
            public IToken OpToken => unaryop().Start;

            public int Op => OpToken.Type;

            public ExpressionContext Rhs => expression();
        }
    }
}
