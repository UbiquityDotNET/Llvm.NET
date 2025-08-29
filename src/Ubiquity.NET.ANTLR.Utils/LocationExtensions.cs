// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Ubiquity.NET.Runtime.Utils;

namespace Ubiquity.NET.ANTLR.Utils
{
    /// <summary>Utility class to provide extensions for translation of ANTLR location data into a <see cref="SourceLocation"/></summary>
    public static class LocationExtensions
    {
        /// <summary>Gets the <see cref="SourceLocation"/> for a given <see cref="ParserRuleContext"/></summary>
        /// <param name="ctx">Parser rule context to get the location from</param>
        /// <returns><see cref="SourceLocation"/> for the context</returns>
        public static SourceLocation GetSourceLocation( this ParserRuleContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );
            return new SourceLocation( ctx.Start.Line
                                     , ctx.Start.Column
                                     , ctx.Stop.Line
                                     , ctx.Stop.Column
                                     );
        }

        /// <summary>Attempts to retrieve the <see cref="SourceLocation"/> from a <see cref="RuleContext"/></summary>
        /// <param name="ctx">Context to get the span from</param>
        /// <returns><see cref="SourceLocation"/> for this input context or a default constructed one</returns>
        /// <remarks>
        /// Not all <see cref="RuleContext"/> derived types will support line+col location information. This
        /// only tests for a <see cref="ParserRuleContext"/> and retrieves the location from that. The base
        /// RuleContext and other derived types simply get a default constructed location as the location is
        /// not known. (They only store location as an integral interval without the line+col information)
        /// </remarks>
        public static SourceLocation GetSourceLocation( this RuleContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );
            if(ctx is ParserRuleContext ruleCtx)
            {
                ruleCtx.GetSourceLocation();
            }

            // NOTE other RuleContext types may track position but a RuleContext itself
            // only tracks the integral position as an Interval (no line+col info!)
            return default;
        }

        /// <summary>Gets the source location information for a token an <see cref="ITerminalNode"/> represents</summary>
        /// <param name="node">Terminal node</param>
        /// <returns>Source span for the terminal's token</returns>
        public static SourceLocation GetSourceLocation( this ITerminalNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            return node.Symbol.GetSourceLocation();
        }

        /// <summary>Gets the <see cref="SourceLocation"/> from an <see cref="IToken"/></summary>
        /// <param name="token">Token to get the location information for</param>
        /// <returns>SourceLocation</returns>
        public static SourceLocation GetSourceLocation( this IToken token )
        {
            ArgumentNullException.ThrowIfNull( token );

            // TODO: Q: Should this account for a newline in the token?
            //       A: Probably not, as a token can't span a newline.
            return new SourceLocation( token.Line, token.Column, token.Line, token.Column + token.Text.Length );
        }
    }
}
