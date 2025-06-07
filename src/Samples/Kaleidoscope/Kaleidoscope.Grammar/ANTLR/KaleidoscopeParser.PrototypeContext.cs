// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.PrototypeContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
