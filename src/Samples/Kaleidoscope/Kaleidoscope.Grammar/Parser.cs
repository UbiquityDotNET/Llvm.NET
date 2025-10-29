// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using Kaleidoscope.Grammar.ANTLR;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Grammar.Visualizers;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar
{
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
        /// <param name="visualizer">Visualizer to use for the parse (Includes possible error nodes)</param>
        public Parser( LanguageLevel level, IVisualizer? visualizer = null )
            : this( new DynamicRuntimeState( level, functionRedefinitionIsAnError: false ), visualizer )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
        /// <param name="level"><see cref="LanguageLevel"/> for the parser</param>
        /// <param name="functionRedefinitionIsAnError">flag to indicate if redefinition of a function is an error</param>
        /// <param name="visualizer">Visualizer to use for the parse (Includes possible error nodes)</param>
        public Parser( LanguageLevel level, bool functionRedefinitionIsAnError, IVisualizer? visualizer = null )
            : this( new DynamicRuntimeState( level, functionRedefinitionIsAnError ), visualizer )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
        /// <param name="globalState"><see cref="DynamicRuntimeState"/> for the parse</param>
        /// <param name="visualizer">Visualizer to use for the parse (Includes possible error nodes)</param>
        public Parser( DynamicRuntimeState globalState
                     , IVisualizer? visualizer = null
                     )
        {
            ArgumentNullException.ThrowIfNull( globalState );

            GlobalState = globalState;
            Visualizer = visualizer;
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
        public IAstNode? Parse( string txt )
        {
            return Parse( (txt ?? string.Empty).ToCharArray(), ParseMode.ReplLoop );
        }

        /// <inheritdoc/>
        public IAstNode? Parse( TextReader reader )
        {
            ArgumentNullException.ThrowIfNull( reader );
            return Parse( reader.ReadToEnd().ToCharArray(), ParseMode.FullSource );
        }

        /// <summary>Specifies the parse mode for parsing Kaleidoscope source input</summary>
        internal enum ParseMode
        {
            /// <summary>Parse for a REPL</summary>
            ReplLoop,

            /// <summary>Full text source parse</summary>
            FullSource
        }

        private IAstNode? Parse( char[] input, ParseMode mode )
        {
            try
            {
                var errCollector = new ParseErrorCollector();

                (KaleidoscopeParser antlrParser, IParseTree parseTree) = CoreParse( input, mode, errCollector );

                if(Visualizer is not null && Visualizer.VisualizationKind.HasFlag( VisualizationKind.Xml ))
                {
                    var docListener = new XDocumentListener( antlrParser );
                    ParseTreeWalker.Default.Walk( docListener, parseTree );
                    Visualizer.VisualizeParseTree( docListener.Document );
                }

                IAstNode retVal;
                if(errCollector.ErrorNodes.Length > 0)
                {
                    retVal = new RootNode( default, errCollector.ErrorNodes );
                }
                else
                {
                    var astBuilder = new AstBuilder( GlobalState );
                    retVal = astBuilder.Visit( parseTree );
                }

                if(Visualizer is not null && (Visualizer.VisualizationKind.HasFlag( VisualizationKind.Dgml ) || Visualizer.VisualizationKind.HasFlag( VisualizationKind.BlockDiag )))
                {
                    // both forms share the same initial DirectedGraph model as the formats are pretty similar
                    // blockDiag doesn't need the DGML (XML) form so that is only produced if needed.
                    var dgmlGenerator = new DgmlGenerator( antlrParser );
                    ParseTreeWalker.Default.Walk( dgmlGenerator, parseTree );

                    if(Visualizer.VisualizationKind.HasFlag( VisualizationKind.Dgml ))
                    {
                        using var writer = new StringWriter(CultureInfo.CurrentCulture);
                        Visualizer.VisualizeAstDgml( dgmlGenerator.Graph.ToXml() );
                    }

                    if(Visualizer.VisualizationKind.HasFlag( VisualizationKind.BlockDiag ))
                    {
                        using var writer = new StringWriter(CultureInfo.CurrentCulture);
                        dgmlGenerator.Graph.WriteAsBlockDiag( writer );
                        Visualizer.VisualizeBlockDiag( writer.ToString() );
                    }
                }

                return retVal;
            }
            catch(ParseCanceledException)
            {
                return new RootNode( default, new ErrorNode( default, (int)DiagnosticCode.ParseCanceled, "Parse canceled" ) );
            }
        }

        private (KaleidoscopeParser Op, IParseTree RHS) CoreParse( char[] input, ParseMode mode, ParseErrorCollector errCollector )
        {
            var lexer = new KaleidoscopeLexer( input, GlobalState.LanguageLevel, errCollector );

            var tokenStream = new CommonTokenStream( lexer );

            var antlrParser = new KaleidoscopeParser( tokenStream
                                                    , GlobalState
                                                    , errCollector
                                                    , Visualizer?.VisualizationKind.HasFlag( VisualizationKind.DebugTraceParser ) ?? false
                                                    );

            var parseTree = mode == ParseMode.ReplLoop ? (IParseTree)antlrParser.repl() : antlrParser.fullsrc();

            return (antlrParser, parseTree);
        }

        private readonly IVisualizer? Visualizer;
    }
}
