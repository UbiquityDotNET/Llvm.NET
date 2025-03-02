// -----------------------------------------------------------------------
// <copyright file="LocationExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Ubiquity.NET.ArgValidators;

namespace Kaleidoscope.Grammar
{
    internal static class LocationExtensions
    {
        public static SourceSpan GetSourceSpan( [ValidatedNotNull] this ParserRuleContext ctx )
        {
            ctx.ValidateNotNull( nameof( ctx ) );
            return new SourceSpan( ctx.Start.Line
                                 , ctx.Start.Column
                                 , ctx.Stop.Line
                                 , ctx.Stop.Column
                                 );
        }

        public static SourceSpan GetSourceSpan( [ValidatedNotNull]this RuleContext ctx )
        {
            ctx.ValidateNotNull( nameof( ctx ) );
            if( ctx is ParserRuleContext ruleCtx )
            {
                GetSourceSpan( ruleCtx );
            }

            return default;
        }

        public static SourceSpan GetSourceSpan( [ValidatedNotNull] this ITerminalNode node )
        {
            node.ValidateNotNull( nameof( node ) );
            return GetSourceSpan( node.Symbol );
        }

        public static SourceSpan GetSourceSpan( [ValidatedNotNull] this IToken token )
        {
            token.ValidateNotNull( nameof( token ) );
            return new SourceSpan( token.Line, token.Column, token.Line, token.Column + token.Text.Length );
        }
    }
}
