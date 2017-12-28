// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

namespace Kaleidoscope
{
    public static class Program
    {
        /// <summary>C# version of the LLVM Kaleidoscope language tutorial</summary>
        /// <param name="args">Ignored...</param>
        [SuppressMessage( "Redundancies in Symbol Declarations", "RECS0154:Parameter is never used", Justification = "Standard required signature" )]
        public static void Main( string[ ] args )
        {
            // Using language level that includes the complete set
            // of language features to allow exploring and verifying
            // the parser support for the whole language.
            var parser = new ReplParserStack( LanguageLevel.MutableVariables );
            using( var generator = new CodeGenerator( parser.GlobalState ) )
            {
                Console.WriteLine( "LLVM Kaleidoscope Syntax Viewer - {0}", parser.LanguageLevel );

                var replLoop = new ReplLoop<int>( generator, parser, DiagnosticRepresentations.Xml | DiagnosticRepresentations.Dgml );
                replLoop.ReadyStateChanged += ( s, e ) => Console.Write( e.PartialParse ? ">" : "Ready>" );
                replLoop.GeneratedResultAvailable += OnGeneratedResultAvailable;

                replLoop.Run( );
            }
        }

        private static void OnGeneratedResultAvailable( object sender, GeneratedResultAvailableArgs<int> e )
        {
            var docListener = new XDocumentListener( e.Recognizer );
            ParseTreeWalker.Default.Walk( docListener, e.ParseTree );
            Console.WriteLine( "Parsed:" );
            docListener.Document.Save( Console.Out );
            Console.WriteLine( );
        }
    }
}
