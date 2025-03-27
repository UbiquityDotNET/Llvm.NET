// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.FunctionPrototypeContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
            public override string Name => Identifier( 0 ).GetText( );

            public override IEnumerable<(string Name, int Index, SourceSpan Span)> Parameters
                => Identifier( ).Skip( 1 ).Select( ( id, i ) => (id.GetText( ), i, id.GetSourceSpan( )) );
        }
    }
}
