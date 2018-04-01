// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Values;

using static Kaleidoscope.Runtime.Utilities;
using static Llvm.NET.StaticState;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace Kaleidoscope
{
    public static class Program
    {
        // <Main>

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial</summary>
        /// <param name="args">Command line arguments to the application</param>
        /// <returns>0 on success; non-zero on error</returns>
        /// <remarks>
        /// The command line options at present are 'WaitForDebugger' and the source file name
        ///
        /// Specifying 'WaitForDebugger' will trigger a wait loop in Main() to wait
        /// for an attached debugger if one is not yet attached. This is useful
        /// for mixed mode native+managed debugging as the SDK project system does
        /// not support that on launch.
        /// </remarks>
        public static int Main( string[ ] args )
        {
            (string sourceFilePath, bool waitForDebugger, int exitCode) = ProcessArgs( args );
            if( exitCode != 0 )
            {
                return exitCode;
            }

            string objFilePath = Path.ChangeExtension( sourceFilePath, ".o" );
            string irFilePath = Path.ChangeExtension( sourceFilePath, ".ll" );

            using( TextReader rdr = File.OpenText( sourceFilePath ) )
            using( InitializeLLVM( ) )
            {
                RegisterNative( );

                var machine = new TargetMachine( Triple.HostTriple );
                var parser = new ReplParserStack( LanguageLevel.MutableVariables );
                using( var generator = new CodeGenerator( parser.GlobalState, machine ) )
                {
                    Console.WriteLine( "Llvm.NET Kaleidoscope Compiler - {0}", parser.LanguageLevel );
                    Console.WriteLine( "Compiling {0}", sourceFilePath );

                    var replLoop = new ReplLoop<Value>( generator, parser, DiagnosticRepresentations.None, rdr );
                    replLoop.CodeGenerationError += OnGeneratorError;

                    // time the parse and code generation
                    var timer = System.Diagnostics.Stopwatch.StartNew( );
                    replLoop.Run( );
                    if( !generator.Module.Verify( out string errMsg ) )
                    {
                        Console.Error.WriteLine( errMsg );
                    }
                    else
                    {
                        machine.EmitToFile( generator.Module, objFilePath, CodeGenFileType.ObjectFile );
                        timer.Stop( );

                        Console.WriteLine( "Wrote {0}", objFilePath );
                        if( !generator.Module.WriteToTextFile( irFilePath, out string msg ) )
                        {
                            Console.Error.WriteLine( msg );
                            return -1;
                        }
                        Console.WriteLine( "CopmilationTiorTime: {0}", timer.Elapsed );
                    }
                }
            }

            return 0;
        }
        // </Main>

        // <ErrorHandling>
        private static void OnGeneratorError( object sender, CodeGenerationExceptionArgs e )
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            try
            {
                Console.Error.WriteLine( e.Exception.Message );
            }
            finally
            {
                Console.ForegroundColor = color;
            }
        }
        // </ErrorHandling>

        // <ProcessArgs>
        // really simple command line handling, just loops through the args
        private static (string SourceFilePath, bool WaitForDebugger, int ExitCode) ProcessArgs( string[ ] args )
        {
            bool waitforDebugger = false;
            string sourceFilePath = string.Empty;
            foreach( string arg in args )
            {
                if( string.Compare( arg, "waitfordebugger", StringComparison.InvariantCultureIgnoreCase ) == 0 )
                {
                    waitforDebugger = true;
                }
                else
                {
                    if( !string.IsNullOrWhiteSpace( sourceFilePath ) )
                    {
                        Console.Error.WriteLine( "Source path already provided, unrecognized option: '{0}'", arg );
                    }
                    sourceFilePath = Path.GetFullPath( arg );
                }
            }

            WaitForDebugger( waitforDebugger );

            if( string.IsNullOrWhiteSpace( sourceFilePath ) )
            {
                Console.Error.WriteLine( "Missing source file name!" );
                return (null, false, -1);
            }

            if( !File.Exists( sourceFilePath ) )
            {
                Console.Error.WriteLine( "Source file '{0}' - not found!", sourceFilePath );
                return (null, false, -2);
            }

            return (sourceFilePath, waitforDebugger, 0);
        }
        // </ProcessArgs>
    }
}
