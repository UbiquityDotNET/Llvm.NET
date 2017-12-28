// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class BinaryPrototypeContext
        {
            public IToken OpToken => userdefinedop( ).start;

            public override IReadOnlyList<(string Name, SourceSpan Span)> Parameters
                => Identifier( ).Select( i => (i.GetText( ), i.GetSourceSpan( )) ).ToList( );

            public int Precedence => ( int )double.Parse( Number( ).GetText( ) );

            public override string Name => GetOperatorFunctionName( OpToken );

            public static string GetOperatorFunctionName( IToken token ) => $"$binary${token.Text}";
        }
    }
}
