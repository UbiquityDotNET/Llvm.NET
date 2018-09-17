// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class UnaryPrototypeContext
        {
            public override IEnumerable<(string Name, int Index, SourceSpan Span)> Parameters
            {
                get
                {
                    yield return (Identifier( ).GetText( ), 1, Identifier().GetSourceSpan());
                }
            }

            public IToken OpToken => unaryop( ).Start;
        }
    }
}
