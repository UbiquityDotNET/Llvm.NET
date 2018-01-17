// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.IO;
using Kaleidoscope.Grammar;

namespace Kaleidoscope.Runtime
{
    public class ReplLoop<TResult>
    {
        public ReplLoop( IKaleidoscopeCodeGenerator<TResult> generator, LanguageLevel languageLevel )
            : this( generator, new ReplParserStack( languageLevel ), DiagnosticRepresentations.None, Console.In )
        {
        }

        public ReplLoop( IKaleidoscopeCodeGenerator<TResult> generator, IKaleidoscopeParser parser )
            : this( generator, parser, DiagnosticRepresentations.None, Console.In )
        {
        }

        public ReplLoop( IKaleidoscopeCodeGenerator<TResult> generator, IKaleidoscopeParser parser, DiagnosticRepresentations additionalDiagnostics )
            : this( generator, parser, additionalDiagnostics, Console.In)
        {
        }

        public ReplLoop( IKaleidoscopeCodeGenerator<TResult> generator
                       , IKaleidoscopeParser parser
                       , DiagnosticRepresentations additionalDiagnostics
                       , TextReader inputReader
                       )
        {
            Parser = parser;
            Generator = generator;
            Input = inputReader;
            AdditionalDiagnostics = additionalDiagnostics;
        }

        public event EventHandler<GeneratedResultAvailableArgs<TResult>> GeneratedResultAvailable = (s,e)=> { };

        public event EventHandler<ReadyStateChangedArgs> ReadyStateChanged = (s,e)=> { };

        public bool Ready { get; private set; } = true;

        public DiagnosticRepresentations AdditionalDiagnostics { get; }

        /// <summary>Runs the REPL loop for the language</summary>
        /// <remarks>
        /// Since ANTLR doesn't have an "interactive" input stream, this sort of fakes
        /// it by using the <see cref="TextReaderExtensions.ReadStatements(System.IO.TextReader)"/>
        /// extension to provide an enumeration of lines that may be partial statements read in.
        /// This is consistent with the behavior of the official LLVM C++ version and allows
        /// for full use of ANTLR4 instead of writing a parser by hand.
        /// </remarks>
        public void Run( )
        {
            Ready = true;
            ReadyStateChanged( this, ReadyStateChangedArgs.CompleteParseArgs );
            foreach( var (Txt, IsPartial) in Input.ReadStatements( ) )
            {
                if( !IsPartial )
                {
                    var (parseTree, recognizer) = Parser.Parse( Txt, AdditionalDiagnostics );
                    TResult value = Generator.Generate( recognizer, parseTree, AdditionalDiagnostics );
                    GeneratedResultAvailable( this, new GeneratedResultAvailableArgs<TResult>( value, recognizer, parseTree ) );
                }

                /*
                Ready | IsPartial | blank | new value for Ready
                true  | false     | false | true
                true  | false     | true  | <INVALID> (assert)
                true  | true      | false | false
                true  | true      | true  | true

                false | false     | x     | READY
                false | true      | x     | CONTINUE
                */
                if( Ready )
                {
                    bool isBlank = String.IsNullOrWhiteSpace( Txt );

                    Debug.Assert( !( Ready && !IsPartial && isBlank ), "Invalid internal state" );
                    Ready = IsPartial == isBlank;
                }
                else
                {
                    Ready = !IsPartial;
                }

                ReadyStateChanged( this, Ready ? ReadyStateChangedArgs.CompleteParseArgs : ReadyStateChangedArgs.PartialParseArgs );
            }
        }

        private readonly IKaleidoscopeParser Parser;
        private readonly IKaleidoscopeCodeGenerator<TResult> Generator;
        private readonly TextReader Input;
    }
}
