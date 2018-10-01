// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.IO;
using Kaleidoscope.Grammar;

namespace Kaleidoscope.Runtime
{
    /// <summary>Implementation of common REPL infrastructure for interactive Kaleidoscope implementations</summary>
    /// <typeparam name="TResult">result type of the generator type</typeparam>
    /// <remarks>
    /// <para>This class serves to isolate and generalize the REPL infrastructure for the Kaleidoscope language
    /// implementations. Aside from the general encapsulation and isolation, this also helps to keep the
    /// individual chapters lean and focused on the use of Llvm.NET to generate executable code and the
    /// JIT engine support to run it.</para>
    /// <para>Since ANTLR doesn't have an "interactive" input stream, this sort of fakes
    /// it by using the <see cref="TextReaderExtensions.ReadStatements(System.IO.TextReader)"/>
    /// extension to provide an enumeration of lines that may be partial statements read in.
    /// This is consistent with the behavior of the official LLVM C++ version and allows
    /// for full use of ANTLR4 instead of writing a parser by hand.</para>
    /// </remarks>
    public class ReplLoop<TResult>
    {
        public ReplLoop( IKaleidoscopeCodeGenerator<TResult> generator, LanguageLevel languageLevel )
            : this( generator, new ParserStack( languageLevel ), DiagnosticRepresentations.None, Console.In )
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

        public event EventHandler<CodeGenerationExceptionArgs> CodeGenerationError = (s,e) => { };

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
                    try
                    {
                        var parseResult = Parser.Parse( Txt, AdditionalDiagnostics );
                        if( parseResult != null )
                        {
                            TResult value = Generator.Generate( parseResult );
                            GeneratedResultAvailable( this, new GeneratedResultAvailableArgs<TResult>( value ) );
                        }
                    }
                    catch(CodeGeneratorException ex)
                    {
                        CodeGenerationError( this, new CodeGenerationExceptionArgs( ex ) );
                    }
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
                    bool isBlank = string.IsNullOrWhiteSpace( Txt );

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
