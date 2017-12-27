// <copyright file="Parser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Kaleidoscope.Grammar
{
    public partial class KaleidoscopeParser
    {
        public partial class BinaryPrototypeContext
        {
            public char Op => opsymbol( ).GetText( )[ 0 ];

            public override string Name => $"$binary${Op}";

            public override IReadOnlyList<(string Name, SourceSpan Span)> Parameters
                => Identifier( ).Select( i => (i.GetText( ), i.GetSourceSpan( )) ).ToList( );

            public int Precedence => ( int )double.Parse( Number( ).GetText( ) );
        }
    }
}
