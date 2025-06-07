// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Kaleidoscope.Grammar;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Runtime.Utils;

using static Ubiquity.NET.Llvm.Library;

namespace Kaleidoscope.Chapter9
{
    public static class Program
    {
        #region Main

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial</summary>
        /// <param name="args">Command line arguments to the application</param>
        /// <returns>0 on success; non-zero on error</returns>
        /// <remarks>
        /// The command line options at present are just the source file name
        /// </remarks>
        [SuppressMessage( "Design", "CA1062:Validate arguments of public methods", Justification = "Provided by Platform" )]
        public static int Main( string[] args )
        {
            (string sourceFilePath, int exitCode) = ProcessArgs( args );
            if(exitCode != 0)
            {
                return exitCode;
            }

            string objFilePath = Path.ChangeExtension( sourceFilePath, ".o" );
            string irFilePath = Path.ChangeExtension( sourceFilePath, ".ll" );
            string asmPath = Path.ChangeExtension( sourceFilePath, ".s" );

            using var rdr = File.OpenText( sourceFilePath );
            using var libLLVM = InitializeLLVM( );
            libLLVM.RegisterTarget( CodeGenTarget.Native );

            using var hostTriple = Triple.GetHostTriple();
            using var machine = new TargetMachine( hostTriple );
            var parser = new Parser( LanguageLevel.MutableVariables );

            using var generator = new CodeGenerator( parser.GlobalState, machine, sourceFilePath );
            Console.WriteLine( "Ubiquity.NET.Llvm Kaleidoscope Compiler - {0}", parser.LanguageLevel );
            Console.WriteLine( "Compiling {0}", sourceFilePath );

            var errorLogger = new ColoredConsoleParseErrorReporter( sourceFilePath );

            // time the parse and code generation
            var timer = System.Diagnostics.Stopwatch.StartNew( );
            var ast = parser.Parse( rdr );
            if(!errorLogger.CheckAndReportParseErrors( ast ))
            {
                Module? module = generator.Generate( ast );
                if(module is null)
                {
                    Console.Error.WriteLine( "No module generated" );
                }
                else if(!module.Verify( out string errMsg ))
                {
                    Console.Error.WriteLine( errMsg );
                }
                else
                {
                    machine.EmitToFile( module, objFilePath, CodeGenFileKind.ObjectFile );
                    timer.Stop(); // only time Object file generation (the rest is just useful reference)

                    Console.WriteLine( "Wrote {0}", objFilePath );
                    if(!module.WriteToTextFile( irFilePath, out string msg ))
                    {
                        Console.Error.WriteLine( msg );
                        return -1;
                    }

                    Console.WriteLine( "Wrote {0}", irFilePath );

                    machine.EmitToFile( module, asmPath, CodeGenFileKind.AssemblySource );
                    Console.WriteLine( "Wrote {0}", asmPath );

                    Console.WriteLine( "Compilation Time: {0}", timer.Elapsed );
                }
            }

            return 0;
        }
        #endregion

        #region ProcessArgs

        // really simple command line handling, just loops through the input arguments
        private static (string SourceFilePath, int ExitCode) ProcessArgs( string[] args )
        {
            string sourceFilePath = string.Empty;
            foreach(string arg in args)
            {
                if(!string.IsNullOrWhiteSpace( sourceFilePath ))
                {
                    Console.Error.WriteLine( "Source path already provided, unrecognized option: '{0}'", arg );
                }

                sourceFilePath = Path.GetFullPath( arg );
            }

            if(string.IsNullOrWhiteSpace( sourceFilePath ))
            {
                Console.Error.WriteLine( "Missing source file name!" );
                return (string.Empty, -1);
            }

            if(!File.Exists( sourceFilePath ))
            {
                Console.Error.WriteLine( "Source file '{0}' - not found!", sourceFilePath );
                return (string.Empty, -2);
            }

            return (sourceFilePath, 0);
        }
        #endregion
    }
}
