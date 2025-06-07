// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.FunctionDefinitionContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class FunctionDefinitionContext
        {
            public PrototypeContext Signature => prototype();

            public ExpressionContext BodyExpression => expression();
        }
    }
}
