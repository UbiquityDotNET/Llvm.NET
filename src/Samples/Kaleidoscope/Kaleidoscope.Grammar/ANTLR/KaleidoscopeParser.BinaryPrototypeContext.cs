// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

            public override IEnumerable<(string Name, int Index, SourceRange Span)> Parameters
                => Identifier().Select( ( id, i ) => (id.GetText(), i, id.GetSourceRange()) );

            public int Precedence => (int)double.Parse( Number().GetText(), CultureInfo.InvariantCulture );
        }
    }
}
