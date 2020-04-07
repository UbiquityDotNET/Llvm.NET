// -----------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Runtime
{
    public static class EnumerableExtensions
    {
        public static IAsyncEnumerable<IAstNode> ParseWith( [ValidatedNotNull] this IAsyncEnumerable<string> source
                                                          , IKaleidoscopeParser parser
                                                          )
        {
            source.ValidateNotNull( nameof( source ) );
            parser.ValidateNotNull( nameof( parser ) );

            return from txt in source
                   select parser.Parse( txt );
        }
    }
}
