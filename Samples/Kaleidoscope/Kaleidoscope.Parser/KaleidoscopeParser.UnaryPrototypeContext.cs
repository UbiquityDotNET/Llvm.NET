// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class UnaryPrototypeContext
        {
            public char Op => opsymbol( ).GetText( )[ 0 ];

            public override string Name => $"$unary${Op}";

            public override IReadOnlyList<(string Name, SourceSpan Span)> Parameters
                => new List<(string Name, SourceSpan Span)>
                    { (Identifier( ).GetText( ), Identifier().GetSourceSpan()) };
        }
    }
}
