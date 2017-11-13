// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;

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
            RunReplLoop( LanguageLevel.MutableVariables );
        }

        /// <summary>Runs the REPL loop for the language</summary>
        /// <param name="level">Langage Level</param>
        /// <remarks>
        /// Since ANTLR doesn't have an "interactive" input stream, this sort of fakes
        /// it by using the <see cref="ReplLoopExtensions.ReadStatements(System.IO.TextReader)"/>
        /// extension to provide an enumeration of lines that may be partial statements read in.
        /// This is consistent with the behavior of the official LLVM C++ version and allows
        /// for full use of ANTLR4 instead of wrting a parser by hand.
        /// </remarks>
        private static void RunReplLoop( LanguageLevel level )
        {
            var parseStack = new ReplParserStack( level );

            Console.WriteLine( "LLVM Kaleidoscope Syntax Viewer - {0}", level );
            Console.Write( "Ready>" );
            foreach( var lineInfo in Console.In.ReadStatements( ) )
            {
                if( !lineInfo.IsPartial )
                {
                    var parseTree = parseStack.ReplParse( lineInfo.Txt );

                    // no code generation in this version.
                    // This departs a tad from the official C++ version for this "chapter"
                    // by printing out an XML representation of the complete parse tree
                    // as opposed to the official version's "parsed an XYZ" message.
                    // This provides much more detailed information about the actual parse
                    // to help in diagnosing issues. Whenever, adding functionality to
                    // the grammar itself it is useful to come back to this to verify what
                    // the parser is actually producing for a given input.
                    var docListener = new XDocumentListener( );
                    ParseTreeWalker.Default.Walk( docListener, parseTree );
                    Console.WriteLine( "Parsed:\n{0}", docListener.Document.ToString( ) );
                }

                Console.Write( lineInfo.IsPartial ? ">" : "Ready>" );
            }
        }
    }
}
