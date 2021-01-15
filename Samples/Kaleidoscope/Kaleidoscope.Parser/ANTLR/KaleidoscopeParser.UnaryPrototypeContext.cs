// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.UnaryPrototypeContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar.ANTLR
{
    public partial class KaleidoscopeParser
    {
        public partial class UnaryPrototypeContext
        {
            public override IEnumerable<(string Name, int Index, SourceSpan Span)> Parameters
            {
                get
                {
                    yield return (Identifier( ).GetText( ), 0, Identifier( ).GetSourceSpan( ));
                }
            }

            public IToken OpToken => unaryop( ).Start;
        }
    }
}
