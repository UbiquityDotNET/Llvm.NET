// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Kaleidoscope.Grammar;

using Ubiquity.NET.CommandLine;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

using static Ubiquity.NET.Llvm.Library;

// error CA1506: 'Main' is coupled with '41' different types from '15' different namespaces...
// Total BS. Maybe at the IL level, but hardly true at the source level
#pragma warning disable CA1506

namespace Kaleidoscope.Chapter8
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
            var reporter = new ColoredConsoleReporter(MsgLevel.Information);
            if(!ArgsParsing.TryParse<Options>( args, reporter, out Options? options, out int exitCode ))
            {
                return exitCode;
            }

            // TODO: Reset Command line option to support reporting level from command line (if different)

            string sourceFilePath = options.SourcePath.FullName;

            string objFilePath = Path.ChangeExtension( sourceFilePath, ".o" );
            string irFilePath = Path.ChangeExtension( sourceFilePath, ".ll" );
            string asmPath = Path.ChangeExtension( sourceFilePath, ".s" );

            using var libLLVM = InitializeLLVM( );
            libLLVM.RegisterTarget( CodeGenTarget.Native );

            using var hostTriple = Triple.GetHostTriple();
            using var machine = new TargetMachine( hostTriple );
            var parser = new Parser( LanguageLevel.MutableVariables );

            using var generator = new CodeGenerator( parser.GlobalState, machine );
            Console.WriteLine( "Ubiquity.NET.Llvm Kaleidoscope Compiler - {0}", parser.LanguageLevel );
            Console.WriteLine( "Compiling {0}", sourceFilePath );

            // Create adapter to route parse messages for the Kaleidoscope language to the command line reporter for this app
            var errorLogger = new ParseErrorDiagnosticAdapter(reporter, "KLS", new Uri(sourceFilePath) );

            // time the parse and code generation
            var timer = System.Diagnostics.Stopwatch.StartNew( );
            var ast = parser.ParseFrom( sourceFilePath );
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
    }
}
