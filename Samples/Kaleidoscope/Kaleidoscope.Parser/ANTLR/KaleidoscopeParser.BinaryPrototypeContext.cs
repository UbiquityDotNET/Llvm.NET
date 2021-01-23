// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.BinaryPrototypeContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar.ANTLR
{
    public partial class KaleidoscopeParser
    {
        [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Nesting is generated, this is just the non-generated partial" )]
        public partial class BinaryPrototypeContext
        {
            public IToken OpToken => userdefinedop( ).start;

            public override IEnumerable<(string Name, int Index, SourceSpan Span)> Parameters
                => Identifier( ).Select( ( id, i ) => (id.GetText( ), i, id.GetSourceSpan( )) );

            public int Precedence => ( int )double.Parse( Number( ).GetText( ), CultureInfo.InvariantCulture );
        }
    }
}
