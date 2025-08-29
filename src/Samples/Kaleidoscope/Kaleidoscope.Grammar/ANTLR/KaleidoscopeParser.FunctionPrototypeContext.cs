// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Ubiquity.NET.ANTLR.Utils;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class FunctionPrototypeContext
        {
            public override string Name => Identifier( 0 ).GetText();

            public override IEnumerable<(string Name, int Index, SourceLocation Span)> Parameters
                => Identifier().Skip( 1 ).Select( ( id, i ) => (id.GetText(), i, id.GetSourceLocation()) );
        }
    }
}
