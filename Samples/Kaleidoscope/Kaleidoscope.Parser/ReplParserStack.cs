// <copyright file="ReplParserStack.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Xml.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
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
            GlobalState = new DynamicRuntimeState( level );
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
            GlobalState = new DynamicRuntimeState( level );
            LexErrorListener = lexErrorListener;
            ParseErrorListener = parseErrorListener;
            ErrorStrategy = new ReplErrorStrategy( );
            InitializeParser( string.Empty );
        }

        /// <summary>Gets the language level for this parser</summary>
        public LanguageLevel LanguageLevel => GlobalState.LanguageLevel;

        /// <summary>Gets the global state for this parser stack</summary>
        public DynamicRuntimeState GlobalState { get; }

        /* TODO: Convert this to a proper .NET style Tryxxx pattern */

        /// <summary>Parses a single block of text from the parser that may be incomplete</summary>
        /// <param name="txt">Text to parse</param>
        /// <returns>Parsed context</returns>
        public ReplContext ReplParse( string txt )
        {
            try
            {
                InitializeParser( txt );
                var retVal = Parser.repl( );
                if( retVal != null )
                {
                    var span = retVal.GetCharInterval( );
                }

                return retVal;
            }
            catch(ParseCanceledException)
            {
                return null;
            }
        }

// the DGML library used supports desktop and NETCOREAPP2 but NOT .NET STANDARD [Sigh...]
#if NET47
        /// <summary>Generate a DGML diagram file from the tree</summary>
        /// <param name="parseTree">Parse tree to generate the diagram for</param>
        /// <param name="path">path of the file to generate</param>
        public void GenerateDgml( IParseTree parseTree, string path )
        {
            if( Parser != null )
            {
                var dgmlGenerator = new DgmlGenerator( Parser );
                ParseTreeWalker.Default.Walk( dgmlGenerator, parseTree );
                dgmlGenerator.WriteDgmlGraph( path );
            }
        }
#endif

        /// <summary>Generate the parse tree as an <see cref="XDocument"/></summary>
        /// <param name="parseTree">Parse tree to generate</param>
        /// <returns><see cref="XDocument"/></returns>
        public XDocument GenerateXmlTree( IParseTree parseTree )
        {
            var docListener = new XDocumentListener( Parser );
            ParseTreeWalker.Default.Walk( docListener, parseTree );
            return docListener.Document;
        }

        private void InitializeParser( string txt )
        {
            Lexer = new KaleidoscopeLexer( new AntlrInputStream( txt ) )
            {
                GlobalState = GlobalState
            };

            if( LexErrorListener != null )
            {
                Lexer.AddErrorListener( LexErrorListener );
            }

            TokenStream = new CommonTokenStream( Lexer );

            Parser = new KaleidoscopeParser( TokenStream )
            {
                BuildParseTree = true,
                ErrorHandler = ErrorStrategy,
                GlobalState = GlobalState
            };

#if TRACE_PARSER
            Parser.AddParseListener( new DebugTraceListener( Parser ) );
#endif
            if( ParseErrorListener != null )
            {
                Parser.RemoveErrorListeners( );
                Parser.AddErrorListener( ParseErrorListener );
            }
        }

        private CommonTokenStream TokenStream;
        private KaleidoscopeLexer Lexer;
        private KaleidoscopeParser Parser;

        private readonly IAntlrErrorListener<int> LexErrorListener;
        private readonly IAntlrErrorListener<IToken> ParseErrorListener;
        private readonly IAntlrErrorStrategy ErrorStrategy;
    }
}
