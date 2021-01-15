// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.BinaryopContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar.ANTLR
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
