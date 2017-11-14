// <copyright file="ReplParserStack.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Diagnostics;
using Antlr4.Runtime;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope.Grammar
{
    /// <summary>Combined Lexer and Parser support for REPL useage</summary>
    public class ReplParserStack
    {
        /// <summary>Initializes a new instance of the <see cref="ReplParserStack"/> class configured for the specified language level</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        public ReplParserStack( LanguageLevel level )
        {
            LanguageLevel = level;
            var listener = new FormattedConsoleErrorListener( );
            LexErrorListener = listener;
            ParseErrorListener = listener;
            ErrorStrategy = new ReplErrorStrategy( );
            InitializeParser( string.Empty );
        }

        /// <summary>Initializes a new instance of the <see cref="ReplParserStack"/> class.</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        /// <param name="lexErrorListener">Error listener for Lexer errors</param>
        /// <param name="parseErrorListener">Error listener for parer errors</param>
        public ReplParserStack( LanguageLevel level
                              , IAntlrErrorListener<int> lexErrorListener
                              , IAntlrErrorListener<IToken> parseErrorListener
                              )
        {
            LanguageLevel = level;
            LexErrorListener = lexErrorListener;
            ParseErrorListener = parseErrorListener;
            ErrorStrategy = new ReplErrorStrategy( );
            InitializeParser( string.Empty );
        }

        public LanguageLevel LanguageLevel { get; }

        public KaleidoscopeParser Parser { get; }

        public ReplContext ReplParse( string txt )
        {
            Trace.TraceInformation( "Parsing: {0}", txt );
            Lexer.SetInputStream( new AntlrInputStream( txt ) );
            TokenStream.SetTokenSource( Lexer );
            Parser.SetInputStream( TokenStream );

            var retVal = Parser.repl( );
            if( retVal != null )
            {
                Trace.TraceInformation( "Matched {0} {1}", Parser.RuleNames[ retVal.RuleIndex ], retVal.GetText() );
            }

            return retVal;
        }

        private void InitializeParser( string txt )
        {
            Lexer = new KaleidoscopeLexer( new AntlrInputStream( txt ) );
            if( LexErrorListener != null )
            {
                Lexer.AddErrorListener( LexErrorListener );
            }

            TokenStream = new CommonTokenStreamFix( Lexer );

            Parser = new KaleidoscopeParser( TokenStream )
            {
                LanguageLevel = LanguageLevel,
                BuildParseTree = true,
                ErrorHandler = ErrorStrategy,
            };

            if( ParseErrorListener != null )
            {
                Parser.RemoveErrorListeners( );
                Parser.AddErrorListener( ParseErrorListener );
            }
        }

        private CommonTokenStreamFix TokenStream;
        private KaleidoscopeLexer Lexer;

        private readonly IAntlrErrorListener<int> LexErrorListener;
        private readonly IAntlrErrorListener<IToken> ParseErrorListener;
        private readonly IAntlrErrorStrategy ErrorStrategy;
    }
}
