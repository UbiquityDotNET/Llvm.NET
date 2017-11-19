// <copyright file="LocationExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    public static class LocationExtensions
    {
        public static SourceSpan GetSourceSpan(this ParserRuleContext ctx)
        {
            return new SourceSpan( ctx.Start.Line
                                 , ctx.Start.Column
                                 , ctx.Stop.Line
                                 , ctx.Stop.Column
                                 );
        }

        public static SourceSpan GetSourceSpan(this RuleContext ctx)
        {
            if( ctx is ParserRuleContext ruleCtx )
            {
                GetSourceSpan( ruleCtx );
            }

            return default;
        }
    }
}
