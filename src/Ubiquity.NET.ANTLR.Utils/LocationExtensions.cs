// -----------------------------------------------------------------------
// <copyright file="LocationExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Ubiquity.NET.Runtime.Utils;

namespace Ubiquity.NET.ANTLR.Utils
{
    public static class LocationExtensions
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
                ruleCtx.GetSourceSpan( );
            }

            return default;
        }

        public static SourceSpan GetSourceSpan( this ITerminalNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            return node.Symbol.GetSourceSpan( );
        }

        public static SourceSpan GetSourceSpan( this IToken token )
        {
            ArgumentNullException.ThrowIfNull( token );
            return new SourceSpan( token.Line, token.Column, token.Line, token.Column + token.Text.Length );
        }
    }
}
