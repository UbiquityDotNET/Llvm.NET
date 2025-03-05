// -----------------------------------------------------------------------
// <copyright file="LocationExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Kaleidoscope.Grammar
{
    internal static class LocationExtensions
    {
        public static SourceSpan GetSourceSpan( this ParserRuleContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );
            return new SourceSpan( ctx.Start.Line
                                 , ctx.Start.Column
                                 , ctx.Stop.Line
                                 , ctx.Stop.Column
                                 );
        }

        public static SourceSpan GetSourceSpan( this RuleContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );
            if( ctx is ParserRuleContext ruleCtx )
            {
                GetSourceSpan( ruleCtx );
            }

            return default;
        }

        public static SourceSpan GetSourceSpan( this ITerminalNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            return GetSourceSpan( node.Symbol );
        }

        public static SourceSpan GetSourceSpan( this IToken token )
        {
            ArgumentNullException.ThrowIfNull( token );
            return new SourceSpan( token.Line, token.Column, token.Line, token.Column + token.Text.Length );
        }
    }
}
