// <copyright file="ReplParserStack.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;

namespace Kaleidoscope.Runtime
{
    /// <summary>Combined Lexer and Parser support for REPL useage</summary>
    public class ReplParserStack
        : IKaleidoscopeParser
    {
        /// <summary>Initializes a new instance of the <see cref="ReplParserStack"/> class configured for the specified language level</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        public ReplParserStack( LanguageLevel level )
            : this( level, new FormattedConsoleErrorListener( ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ReplParserStack"/> class.</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        /// <param name="listener">Combined error listener for lexer and parser errors</param>
        public ReplParserStack( LanguageLevel level, IUnifiedErrorListener listener )
            : this( level, listener, listener)
        {
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
        }

        /// <summary>Gets or sets the language level for this parser</summary>
        public LanguageLevel LanguageLevel
        {
            get => GlobalState.LanguageLevel;
            set => GlobalState.LanguageLevel = value;
        }

        /// <summary>Gets the global state for this parser stack</summary>
        public DynamicRuntimeState GlobalState { get; }

        /// <inheritdoc/>
        public (IParseTree parseTree, Parser recognizer) Parse( string txt, DiagnosticRepresentations aditionalDiagnostics )
        {
            try
            {
                Lexer = new KaleidoscopeLexer( new AntlrInputStream( txt ) )
                {
                    LanguageLevel = GlobalState.LanguageLevel
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

                if( aditionalDiagnostics.HasFlag( DiagnosticRepresentations.DebugTraceParser ) )
                {
                    Parser.AddParseListener( new DebugTraceListener( Parser ) );
                }

                if( ParseErrorListener != null )
                {
                    Parser.RemoveErrorListeners( );
                    Parser.AddErrorListener( ParseErrorListener );
                }

                var parseTree = Parser.repl( );

                if( aditionalDiagnostics.HasFlag( DiagnosticRepresentations.Xml ) )
                {
                    var docListener = new XDocumentListener( Parser );
                    ParseTreeWalker.Default.Walk( docListener, parseTree );
                    docListener.Document.Save("ParseTree.xml");
                }
#if NET47
                if( aditionalDiagnostics.HasFlag( DiagnosticRepresentations.Dgml ) || aditionalDiagnostics.HasFlag( DiagnosticRepresentations.BlockDiag ) )
                {
                    // both forms share the same initial DirectedGraph model as the formats are pretty similar
                    var dgmlGenerator = new DgmlGenerator( Parser );
                    ParseTreeWalker.Default.Walk( dgmlGenerator, parseTree );

                    if( aditionalDiagnostics.HasFlag( DiagnosticRepresentations.Dgml ) )
                    {
                        dgmlGenerator.WriteDgmlGraph( "ParseTree.dgml" );
                    }

                    if( aditionalDiagnostics.HasFlag( DiagnosticRepresentations.BlockDiag ) )
                    {
                        dgmlGenerator.WriteBlockDiag( "ParseTree.diag" );
                    }
                }
#endif
                return (parseTree, Parser);
            }
            catch(ParseCanceledException)
            {
                return default;
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
