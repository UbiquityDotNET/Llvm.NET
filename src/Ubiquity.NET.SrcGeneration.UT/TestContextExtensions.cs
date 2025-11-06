// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;

namespace Ubiquity.NET.SrcGeneration.UT
{
    [ExcludeFromCodeCoverage]
    internal static class TestContextExtensions
    {
        internal static void Report( this TestContext ctx, string title, ImmutableArray<string> arrayVal )
        {
            ctx.WriteLine( "{0}[{1}]", title, arrayVal.Length );
            for(int i = 0; i < arrayVal.Length; ++i)
            {
                ctx.WriteLine("    [{0}] = {1}", i, arrayVal[i]);
            }
        }
    }
}
