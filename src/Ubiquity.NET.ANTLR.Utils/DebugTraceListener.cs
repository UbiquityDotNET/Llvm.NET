// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Ubiquity.NET.ANTLR.Utils
{
    /// <summary>Provides debug <see cref="Trace.TraceInformation(string)"/> notification of all rule processing while parsing</summary>
    public class DebugTraceListener
        : IParseTreeListener
    {
        /// <summary>Initializes a new instance of the <see cref="DebugTraceListener"/> class.</summary>
        /// <param name="parser">Parser to use to resolve names when generating messages</param>
        public DebugTraceListener( Antlr4.Runtime.Parser parser )
        {
            Parser = parser;
        }

        /// <inheritdoc/>
        public virtual void EnterEveryRule( ParserRuleContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );

            Trace.TraceInformation( $"enter[{ctx.SourceInterval}] {Parser.RuleNames[ ctx.RuleIndex ]} [{ctx.GetType().Name}] Lt(1)='{((ITokenStream)Parser.InputStream).LT( 1 ).Text}'" );
        }

        /// <inheritdoc/>
        public virtual void ExitEveryRule( ParserRuleContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );

            Trace.TraceInformation( $"exit[{ctx.SourceInterval}] {Parser.RuleNames[ ctx.RuleIndex ]} [{ctx.GetType().Name}] Lt(1)='{((ITokenStream)Parser.InputStream).LT( 1 ).Text}'" );
        }

        /// <inheritdoc/>
        public virtual void VisitErrorNode( IErrorNode node )
        {
            ArgumentNullException.ThrowIfNull( node );

            Trace.TraceInformation( "Error: '{0}'", node.ToStringTree() );
        }

        /// <inheritdoc/>
        public virtual void VisitTerminal( ITerminalNode node )
        {
            ArgumentNullException.ThrowIfNull( node );

            var parserRuleContext = ( ParserRuleContext )node.Parent.RuleContext;
            IToken symbol = node.Symbol;
            Trace.TraceInformation( "Terminal: '{0}' rule {1}", symbol, Parser.RuleNames[ parserRuleContext.RuleIndex ] );
        }

        private readonly Antlr4.Runtime.Parser Parser;
    }
}
