// -----------------------------------------------------------------------
// <copyright file="Parser.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Ubiquity.ArgValidators;

namespace Kaleidoscope.Runtime
{
    /// <summary>Specifies the parse mode for parsing Kaleidoscope source input</summary>
    public enum ParseMode
    {
        /// <summary>Parse for a REPL</summary>
        ReplLoop,

        /// <summary>Full text source parse</summary>
        FullSource
    }

    /// <summary>Common parse stack for the Kaleidoscope language</summary>
    /// <remarks>
    /// A great value of ANTLR is its flexibility, but that comes at the cost
    /// of increased complexity. This class encapsulates the complexity as
    /// it is needed for Kaleidoscope code generation with Llvm.NET.
    /// </remarks>
    public class Parser
        : IKaleidoscopeParser
    {
        /// <summary>Initializes a new instance of the <see cref="Parser"/> class configured for the specified language level</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        /// <param name="diagnostics">Diagnostic representations to generate when parsing</param>
        public Parser( LanguageLevel level, DiagnosticRepresentations diagnostics = DiagnosticRepresentations.None )
            : this( level, new FormattedConsoleErrorListener( ), diagnostics )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        /// <param name="listener">Combined error listener for lexer and parser errors</param>
        /// <param name="diagnostics">Diagnostic representations to generate when parsing</param>
        public Parser( LanguageLevel level, IUnifiedErrorListener listener, DiagnosticRepresentations diagnostics = DiagnosticRepresentations.None )
            : this( new DynamicRuntimeState( level ), diagnostics, listener, listener )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
        /// <param name="globalState"><see cref="DynamicRuntimeState"/> for the parse</param>
        /// <param name="diagnostics">Diagnostic representations to generate when parsing</param>
        /// <param name="lexErrorListener">Error listener for Lexer errors</param>
        /// <param name="parseErrorListener">Error listener for parer errors</param>
        public Parser( DynamicRuntimeState globalState
                          , DiagnosticRepresentations diagnostics
                          , IAntlrErrorListener<int> lexErrorListener
                          , IAntlrErrorListener<IToken> parseErrorListener
                          )
        {
            GlobalState = globalState.ValidateNotNull( nameof( globalState ) );
            Diagnostics = diagnostics;
            LexErrorListener = lexErrorListener;
            ParseErrorListener = parseErrorListener;
            ErrorStrategy = new ReplErrorStrategy( );
        }

        public DiagnosticRepresentations Diagnostics { get; }

        /// <summary>Gets or sets the language level for this parser</summary>
        public LanguageLevel LanguageLevel
        {
            get => GlobalState.LanguageLevel;
            set => GlobalState.LanguageLevel = value;
        }

        /// <summary>Gets the global state for this parser stack</summary>
        public DynamicRuntimeState GlobalState { get; }

        /// <inheritdoc/>
        public IAstNode Parse( string txt )
        {
            return Parse( new AntlrInputStream( txt ), ParseMode.ReplLoop );
        }

        /// <inheritdoc/>
        public IAstNode Parse( TextReader reader )
        {
            return Parse( new AntlrInputStream( reader ), ParseMode.FullSource );
        }

        /// <inheritdoc/>
        public IObservable<IAstNode> Parse( IObservable<string> inputSource, Action<CodeGeneratorException> errorHandler)
        {
            return inputSource.ParseWith( this, errorHandler );
        }

        private IAstNode Parse( ICharStream inputStream, ParseMode mode )
        {
            try
            {
                Lexer = new KaleidoscopeLexer( inputStream )
                {
                    LanguageLevel = GlobalState.LanguageLevel
                };

                if( LexErrorListener != null )
                {
                    Lexer.AddErrorListener( LexErrorListener );
                }

                TokenStream = new CommonTokenStream( Lexer );

                AntlrParser = new KaleidoscopeParser( TokenStream )
                {
                    BuildParseTree = true,
                    ErrorHandler = ErrorStrategy,
                    GlobalState = GlobalState
                };

                if( Diagnostics.HasFlag( DiagnosticRepresentations.DebugTraceParser ) )
                {
                    AntlrParser.AddParseListener( new DebugTraceListener( AntlrParser ) );
                }

                if( AntlrParser.FeatureUserOperators )
                {
                    AntlrParser.AddParseListener( new KaleidoscopeUserOperatorListener( GlobalState ) );
                }

                if( ParseErrorListener != null )
                {
                    AntlrParser.RemoveErrorListeners( );
                    AntlrParser.AddErrorListener( ParseErrorListener );
                }

                var parseTree = mode == ParseMode.ReplLoop ? ( IParseTree )AntlrParser.repl( ) : AntlrParser.fullsrc( );

                if( Diagnostics.HasFlag( DiagnosticRepresentations.Xml ) )
                {
                    var docListener = new XDocumentListener( AntlrParser );
                    ParseTreeWalker.Default.Walk( docListener, parseTree );
                    docListener.Document.Save( "ParseTree.xml" );
                }

                if( AntlrParser.NumberOfSyntaxErrors > 0 )
                {
                    return null;
                }

                var astBuilder = new AstBuilder( GlobalState );
                var astNode = astBuilder.Visit( parseTree );

#if NET47
                if( Diagnostics.HasFlag( DiagnosticRepresentations.Dgml ) || Diagnostics.HasFlag( DiagnosticRepresentations.BlockDiag ) )
                {
                    // both forms share the same initial DirectedGraph model as the formats are pretty similar
                    var dgmlGenerator = new DgmlGenerator( AntlrParser );
                    ParseTreeWalker.Default.Walk( dgmlGenerator, parseTree );
                    var astGraphGenereator = new AstGraphGenerator( );
                    astNode.Accept( astGraphGenereator );

                    if( Diagnostics.HasFlag( DiagnosticRepresentations.Dgml ) )
                    {
                        dgmlGenerator.WriteDgmlGraph( "ParseTree.dgml" );
                        astGraphGenereator.WriteDgmlGraph( "Ast.dgml" );
                    }

                    if( Diagnostics.HasFlag( DiagnosticRepresentations.BlockDiag ) )
                    {
                        dgmlGenerator.WriteBlockDiag( "ParseTree.diag" );
                        astGraphGenereator.WriteBlockDiag( "Ast.diag" );
                    }
                }
#endif
                return astNode;
            }
            catch( ParseCanceledException )
            {
                return default;
            }
        }

        private CommonTokenStream TokenStream;
        private KaleidoscopeLexer Lexer;
        private KaleidoscopeParser AntlrParser;

        private readonly IAntlrErrorListener<int> LexErrorListener;
        private readonly IAntlrErrorListener<IToken> ParseErrorListener;
        private readonly IAntlrErrorStrategy ErrorStrategy;
    }
}
