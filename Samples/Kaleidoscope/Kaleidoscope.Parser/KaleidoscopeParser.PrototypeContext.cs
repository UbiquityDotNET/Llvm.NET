// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        // base type for all prototypes (technically abstract, but the generator doesn't apply that)
        public partial class PrototypeContext
        {
            public virtual IReadOnlyList<(string Name, SourceSpan Span)> Parameters
                => new List<(string Name, SourceSpan Span)>( );

            public virtual string Name => string.Empty;
        }
    }
}
