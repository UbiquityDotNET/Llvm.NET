// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.PrototypeContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Kaleidoscope.Grammar.ANTLR
{
    public partial class KaleidoscopeParser
    {
        // base type for all prototypes (technically abstract, but the generator doesn't apply that)
        public partial class PrototypeContext
        {
            public virtual IEnumerable<(string Name, int Index, SourceSpan Span)> Parameters
                => Enumerable.Empty<(string, int, SourceSpan)>( );

            public virtual string Name => string.Empty;
        }
    }
}
