// <copyright file="LocationExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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

        public static SourceSpan GetSourceSpan( this ITerminalNode node )
        {
            return GetSourceSpan( node.Symbol );
        }

        public static SourceSpan GetSourceSpan( this IToken token )
        {
            return new SourceSpan( token.Line, token.Column, token.Line, token.Column + token.Text.Length );
        }
    }
}
