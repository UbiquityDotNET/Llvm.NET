// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
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
                using( var generator = new CodeGenerator( ) )
                {
                    RunReplLoop( LanguageLevel.SimpleExpressions, generator );
                }
            }
        }

        /// <summary>Runs the REPL loop for the language</summary>
        /// <param name="level">Language level to use</param>
        /// <param name="generator">Generator to generate code</param>
        /// <remarks>
        /// Since ANTLR doesn't have an "interactive" input stream, this sort of fakes
        /// it by using the <see cref="ReplLoopExtensions.ReadStatements(System.IO.TextReader)"/>
        /// extension to provide an enumeration of lines that may be partial statements read in.
        /// This is consistent with the behavior of the official LLVM C++ version and allows
        /// for full use of ANTLR4 instead of wrting a parser by hand.
        /// </remarks>
        private static void RunReplLoop( LanguageLevel level, CodeGenerator generator )
        {
            var parseStack = new ReplParserStack( level );

            Console.WriteLine( "LLVM Kaleidoscope Interpreter - {0}", level );
            Console.Write( "Ready>" );
            foreach( var lineInfo in Console.In.ReadStatements( ) )
            {
                if( !lineInfo.IsPartial )
                {
                    var parseTree = parseStack.ReplParse( lineInfo.Txt );
                    Value value = generator.Visit( parseTree );
                    if( value is ConstantFP result )
                    {
                        Console.WriteLine( result.Value );
                    }
                }

                Console.Write( lineInfo.IsPartial ? ">" : "Ready>" );
            }
        }
    }
}
