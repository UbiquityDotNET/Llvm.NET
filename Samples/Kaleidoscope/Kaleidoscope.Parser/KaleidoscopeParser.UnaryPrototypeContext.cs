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
            public IToken OpToken => unaryop( ).Start;

            public override IReadOnlyList<(string Name, SourceSpan Span)> Parameters
                => new List<(string Name, SourceSpan Span)>
                    { (Identifier( ).GetText( ), Identifier().GetSourceSpan()) };

            public override string Name => GetOperatorFunctionName( OpToken );

            public static string GetOperatorFunctionName( IToken token ) => $"$unary${token.Text}";
        }
    }
}
