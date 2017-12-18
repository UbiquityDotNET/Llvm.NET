// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
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
            var generator = new CodeGenerator( LanguageLevel.MutableVariables );
            RunReplLoop( generator );
        }

        /// <summary>Runs the REPL loop for the language</summary>
        /// <param name="generator">Generator for generating code</param>
        /// <remarks>
        /// Since ANTLR doesn't have an "interactive" input stream, this sort of fakes
        /// it by using the <see cref="ReplLoopExtensions.ReadStatements(System.IO.TextReader)"/>
        /// extension to provide an enumeration of lines that may be partial statements read in.
        /// This is consistent with the behavior of the official LLVM C++ version and allows
        /// for full use of ANTLR4 instead of wrting a parser by hand.
        /// </remarks>
        private static void RunReplLoop( CodeGenerator generator )
        {
            Console.WriteLine( "LLVM Kaleidoscope Syntax Viewer - {0}", generator.ParserStack.LanguageLevel );
            Console.Write( "Ready>" );
            foreach( var (Txt, IsPartial) in Console.In.ReadStatements( ) )
            {
                if( !IsPartial )
                {
                    var parseTree = generator.ParserStack.ReplParse( Txt );
                    if( parseTree != null )
                    {
                        // pass the tree through simplistic generator to track user defined ops so that
                        // subsequent references parse successfully. No code generation in this version.
                        generator.Visit( parseTree );

                        // This departs a tad from the official C++ version for this "chapter"
                        // by printing generating a representation of the complete parse tree
                        // as opposed to the official version's simplistic "parsed an XYZ"
                        // message.

                        // This provides much more detailed information about the actual parse
                        // to help in diagnosing issues. Whenever, adding functionality to
                        // the grammar itself it is useful to come back to this to verify what
                        // the parser is actually producing for a given input.
#if NET47
                        // For desktop, generate a DGML from the parse tree. This is useful when modifying
                        // or debugging the gramar in general as you can open the DGML in VS
                        // and as each new tree is parsed VS can auto update the visual graph
                        string path = System.IO.Path.GetFullPath( "parsetree.dgml" );
                        generator.ParserStack.GenerateDgml( parseTree, path );
                        Console.WriteLine( "Generated {0}", path );
#endif
                        Console.WriteLine( "Parsed:\n{0}", generator.ParserStack.GenerateXmlTree( parseTree ) );
                    }
                }

                Console.Write( IsPartial ? ">" : "Ready>" );
            }
        }
    }
}
