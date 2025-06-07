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

using Ubiquity.NET.ANTLR.Utils;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Nesting is generated, this is just the non-generated partial" )]
        internal partial class BinaryPrototypeContext
        {
            public IToken OpToken => userdefinedop().Start;

            public override IEnumerable<(string Name, int Index, SourceLocation Span)> Parameters
                => Identifier().Select( ( id, i ) => (id.GetText(), i, id.GetSourceLocation()) );

            public int Precedence => (int)double.Parse( Number().GetText(), CultureInfo.InvariantCulture );
        }
    }
}
