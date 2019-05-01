// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Nesting is generated, this is just the non-generated partial" )]
        public partial class BinaryopContext
        {
            public IToken OpToken => Start;
        }
    }
}
