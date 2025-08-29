// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Globalization;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class ConstExpressionContext
        {
            public double Value => double.Parse( Number().GetText(), CultureInfo.InvariantCulture );
        }
    }
}
