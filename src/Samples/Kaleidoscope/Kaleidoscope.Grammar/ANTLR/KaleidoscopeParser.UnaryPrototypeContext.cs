// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Antlr4.Runtime;

using Ubiquity.NET.ANTLR.Utils;
using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.ANTLR
{
    internal partial class KaleidoscopeParser
    {
        internal partial class UnaryPrototypeContext
        {
            public override IEnumerable<(string Name, int Index, SourceLocation Span)> Parameters
            {
                get
                {
                    yield return (Identifier().GetText(), 0, Identifier().GetSourceLocation());
                }
            }

            public IToken OpToken => unaryop().Start;
        }
    }
}
