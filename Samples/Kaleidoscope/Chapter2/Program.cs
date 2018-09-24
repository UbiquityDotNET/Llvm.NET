// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace Kaleidoscope
{
    public static class Program
    {
        // Using language level that includes the complete set for exploration of pare trees and AST
        private const LanguageLevel LanguageFeatureLevel = LanguageLevel.MutableVariables;

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial</summary>
        /// <param name="args">Ignored...</param>
        [SuppressMessage( "Redundancies in Symbol Declarations", "RECS0154:Parameter is never used", Justification = "Standard required signature" )]
        public static void Main( string[ ] args )
        {
            string helloMsg = $"Llvm.NET Kaleidoscope Explorer - {LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );
            // <generatorloop>
            var parser = new ParserStack( LanguageFeatureLevel );
            var generator = new CodeGenerator( );
            {
                // generate hopefully helpful representations of parse trees
                var replLoop = new ReplLoop<int>( generator
                                                , parser
                                                , DiagnosticRepresentations.Xml | DiagnosticRepresentations.Dgml | DiagnosticRepresentations.BlockDiag
                                                );
                replLoop.ReadyStateChanged += ( s, e ) => Console.Write( e.PartialParse ? ">" : "Ready>" );
                replLoop.GeneratedResultAvailable += OnGeneratedResultAvailable;

                replLoop.Run( );
            }
            // </generatorloop>
        }

        // <ProcessResults>
        private static void OnGeneratedResultAvailable( object sender, GeneratedResultAvailableArgs<int> e )
        {
            Console.WriteLine( "Parsed {0}", e.Result.GetType( ).Name );
        }
        // </ProcessResults>
    }
}
