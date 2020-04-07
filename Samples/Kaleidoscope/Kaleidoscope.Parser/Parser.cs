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

using Kaleidoscope.Grammar.ANTLR;
using Kaleidoscope.Grammar.AST;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar
{
    /// <summary>Enumeration to define the kinds of diagnostic intermediate data to generate for each function definition</summary>
    [Flags]
    public enum DiagnosticRepresentations
    {
        /// <summary>No diagnostics</summary>
        None,

        /// <summary>Generate an XML representation of the parse tree</summary>
        Xml,

        /// <summary>Generate a DGML representation of the parse tree</summary>
        Dgml,

        /// <summary>Generates a BlockDiag representation of the parse tree</summary>
        BlockDiag,

        /// <summary>Emits debug tracing during the parse to an attached debugger</summary>
        DebugTraceParser
    }

    /// <summary>Common parse stack for the Kaleidoscope language</summary>
    /// <remarks>
    /// A great value of ANTLR is its flexibility, but that comes at the cost
    /// of increased complexity. This class encapsulates the complexity as
    /// it is needed for Kaleidoscope code generation with Ubiquity.NET.Llvm.
    /// </remarks>
    public class Parser
        : IKaleidoscopeParser
    {
        /// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        /// <param name="diagnostics">Diagnostic representations to generate when parsing</param>
        public Parser( LanguageLevel level, DiagnosticRepresentations diagnostics = DiagnosticRepresentations.None )
            : this( new DynamicRuntimeState( level ), diagnostics )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
        /// <param name="globalState"><see cref="DynamicRuntimeState"/> for the parse</param>
        /// <param name="diagnostics">Diagnostic representations to generate when parsing</param>
        public Parser( DynamicRuntimeState globalState
                     , DiagnosticRepresentations diagnostics
                     )
        {
            GlobalState = globalState.ValidateNotNull( nameof( globalState ) );
            Diagnostics = diagnostics;
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
            return Parse( ( txt ?? string.Empty ).ToCharArray( ), ParseMode.ReplLoop );
        }

        /// <inheritdoc/>
        public IAstNode Parse( TextReader reader )
        {
            reader.ValidateNotNull( nameof( reader ) );
            return Parse( reader.ReadToEnd( ).ToCharArray( ), ParseMode.FullSource );
        }

        /// <summary>Specifies the parse mode for parsing Kaleidoscope source input</summary>
        internal enum ParseMode
        {
            /// <summary>Parse for a REPL</summary>
            ReplLoop,

            /// <summary>Full text source parse</summary>
            FullSource
        }

        private IAstNode Parse( char[ ] input, ParseMode mode )
        {
            try
            {
                var errCollector = new ParseErrorCollector();

                var lexer = new KaleidoscopeLexer( input, GlobalState.LanguageLevel, errCollector );

                var tokenStream = new CommonTokenStream( lexer );

                var antlrParser = new KaleidoscopeParser( tokenStream
                                                        , GlobalState
                                                        , errCollector
                                                        , Diagnostics.HasFlag( DiagnosticRepresentations.DebugTraceParser )
                                                        );

                var parseTree = mode == ParseMode.ReplLoop ? ( IParseTree )antlrParser.repl( ) : antlrParser.fullsrc( );

                if( Diagnostics.HasFlag( DiagnosticRepresentations.Xml ) )
                {
                    var docListener = new XDocumentListener( antlrParser );
                    ParseTreeWalker.Default.Walk( docListener, parseTree );
                    docListener.Document.Save( "ParseTree.xml" );
                }

                IAstNode retVal;
                if( errCollector.ErrorNodes.Count > 0 )
                {
                    retVal = new RootNode( default, errCollector.ErrorNodes );
                }
                else
                {
                    var astBuilder = new AstBuilder( GlobalState );
                    retVal = astBuilder.Visit( parseTree );
                }

#if NET47
                if( Diagnostics.HasFlag( DiagnosticRepresentations.Dgml ) || Diagnostics.HasFlag( DiagnosticRepresentations.BlockDiag ) )
                {
                    // both forms share the same initial DirectedGraph model as the formats are pretty similar
                    var dgmlGenerator = new DgmlGenerator( AntlrParser );
                    ParseTreeWalker.Default.Walk( dgmlGenerator, parseTree );
                    var astGraphGenereator = new AstGraphGenerator( );
                    retVal.Accept( astGraphGenereator );

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
                return retVal;
            }
            catch( ParseCanceledException )
            {
                return new RootNode( default, new ErrorNode( default, "Parse canceled" ) );
            }
        }
    }
}
