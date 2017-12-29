// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Llvm.NET.Values;

using static Kaleidoscope.Runtime.Utilities;
using static Llvm.NET.StaticState;

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
            WaitForDebugger( );

            using( InitializeLLVM( ) )
            {
                RegisterNative( );
                var parser = new ReplParserStack( LanguageLevel.SimpleExpressions );
                using( var generator = new CodeGenerator( parser.GlobalState ) )
                {
                    Console.WriteLine( "LLVM Kaleidoscope Interpreter - {0}", parser.LanguageLevel );

                    var replLoop = new ReplLoop<Value>( generator, parser );
                    replLoop.ReadyStateChanged += ( s, e ) => Console.Write( e.PartialParse ? ">" : "Ready>" );
                    replLoop.GeneratedResultAvailable += OnGeneratedResultAvailable;

                    replLoop.Run( );
                }
            }
        }

        private static void OnGeneratedResultAvailable( object sender, GeneratedResultAvailableArgs<Value> e )
        {
            var source = ( ReplLoop<Value> )sender;

            switch( e.Result )
            {
            case ConstantFP result:
                Console.WriteLine( "Evaluated to {0}", result.Value );
                break;

            case Function function:
                if( source.AdditionalDiagnostics.HasFlag( DiagnosticRepresentations.LlvmIR ) )
                {
                    function.ParentModule.WriteToTextFile( Path.ChangeExtension( GetSafeFileName( function.Name ), "ll" ), out string ignoredMsg );
                }

                Console.WriteLine( "Defined function: {0}", function.Name );
                Console.WriteLine( function );
                break;
            }
        }
    }
}
