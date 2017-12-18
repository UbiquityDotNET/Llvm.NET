// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Kaleidoscope.Grammar;
using Llvm.NET;
using Llvm.NET.Values;

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
            Utilities.WaitForDebugger( );

            using( StaticState.InitializeLLVM( ) )
            {
                StaticState.RegisterNative( );
                using( var generator = new CodeGenerator( LanguageLevel.MutableVariables ) )
                {
                    RunReplLoop( generator );
                }
            }
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
            Console.WriteLine( "LLVM Kaleidoscope Interpreter - {0}", generator.ParserStack.LanguageLevel );
            Console.Write( "Ready>" );
            foreach( var (Txt, IsPartial) in Console.In.ReadStatements( ) )
            {
                if( !IsPartial )
                {
                    var parseTree = generator.ParserStack.ReplParse( Txt );
                    if( parseTree != null )
                    {
                        Value value = generator.Visit( parseTree );
                        switch( value )
                        {
                        case ConstantFP result:
                            Console.WriteLine( "Evaluated to {0}", result.Value );
                            break;
                        case Function function:
                            Console.WriteLine( "Defined function: {0}", function.Name );
                            var bldr = new System.Text.StringBuilder( function.Name.Length );
                            var invalidChars = Path.GetInvalidFileNameChars( );
                            foreach( char c in function.Name )
                            {
                                bldr.Append( invalidChars.Contains( c ) ? '_' : c );
                            }

                            function.ParentModule.WriteToTextFile( Path.ChangeExtension( bldr.ToString( ), "ll" ), out string ignoredMsg );
#if NET47
                            generator.ParserStack.GenerateDgml( parseTree, Path.ChangeExtension( bldr.ToString( ), "dgml" ) );
#endif
                            break;
                        }
                    }
                }

                Console.Write( IsPartial ? ">" : "Ready>" );
            }
        }
    }
}
