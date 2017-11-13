// <copyright file="CommonTokenStreamFix.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    // Workaround: for https://github.com/tunnelvisionlabs/antlr4cs/pull/136
    // which, despite being closed as resolved, still exists.
    // While antlr4 is officially up to 4.7.x the MSBUILD generator support
    // for C# is behind at 4.6.5 and not compatible with the 4.7.1 runtime.
    internal class CommonTokenStreamFix
        : CommonTokenStream
    {
        public CommonTokenStreamFix(ITokenSource ts)
            : base(ts)
        {
        }

        public CommonTokenStreamFix(ITokenSource ts, int channel)
            : base( ts, channel)
        {
        }

        public override void SetTokenSource( ITokenSource tokenSource )
        {
            base.SetTokenSource( tokenSource );
            fetchedEOF = false;
        }
    }
}
