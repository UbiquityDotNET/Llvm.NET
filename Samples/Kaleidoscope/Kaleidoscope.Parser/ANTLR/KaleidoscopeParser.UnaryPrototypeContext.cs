// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.UnaryPrototypeContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
                    yield return (Identifier( ).GetText( ), 0, Identifier( ).GetSourceLocation( ));
                }
            }

            public IToken OpToken => unaryop( ).Start;
        }
    }
}
