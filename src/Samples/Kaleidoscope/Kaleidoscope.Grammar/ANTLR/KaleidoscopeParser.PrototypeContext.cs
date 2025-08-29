// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        // base type for all prototypes (technically abstract, but the generator doesn't apply that)
        internal partial class PrototypeContext
        {
            public virtual IEnumerable<(string Name, int Index, SourceLocation Span)> Parameters
                => Enumerable.Empty<(string, int, SourceLocation)>();

            public virtual string Name => string.Empty;
        }
    }
}
