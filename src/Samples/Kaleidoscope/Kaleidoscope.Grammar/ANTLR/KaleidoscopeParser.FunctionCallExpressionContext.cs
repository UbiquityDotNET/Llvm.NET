// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class FunctionCallExpressionContext
        {
            public string CaleeName => Identifier().GetText();

            public IReadOnlyList<ExpressionContext> Args => expression();
        }
    }
}
