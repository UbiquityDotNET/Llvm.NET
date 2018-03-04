// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Llvm.NET;
using Llvm.NET.Values;
using static Kaleidoscope.Runtime.Utilities;
using static Llvm.NET.StaticState;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

namespace Kaleidoscope
{
    public static class Program
    {
        /// <summary>C# version of the LLVM Kaleidoscope language tutorial</summary>
        /// <param name="args">Command line arguments to the application</param>
        /// <remarks>
        /// The only supported command line option at present is 'WaitForDebugger'
        /// This parameter is optional and if used must be the first parameter.
        /// Setting 'WaitForDebugger' will trigger a wait loop in Main() to wait
        /// for an attached debugger if one is not yet attached. This is useful
        /// for mixed mode native+managed debugging as the SDK project system does
        /// not support that on launch.
        /// </remarks>
        public static void Main( string[ ] args )
        {
            WaitForDebugger( args.Length > 0 && string.Compare( args[0], "waitfordebugger", StringComparison.InvariantCultureIgnoreCase ) == 0 );

            using( InitializeLLVM( ) )
            {
                RegisterNative( );
                var machine = new TargetMachine( Triple.HostTriple );
                var parser = new ReplParserStack( LanguageLevel.MutableVariables );
                using( var generator = new CodeGenerator( parser.GlobalState, machine ) )
                {
                    var replLoop = new ReplLoop<Value>( generator, parser );
                    replLoop.ReadyStateChanged += ( s, e ) => Console.Write( e.PartialParse ? ">" : "Ready>" );
                    replLoop.Run( );
                    if( !generator.Module.Verify(out string errMsg ) )
                    {
                        Console.Error.WriteLine( errMsg );
                    }
                    else
                    {
                        machine.EmitToFile( generator.Module, "kls.o", CodeGenFileType.ObjectFile );
                        Console.WriteLine( generator.Module.WriteToString( ) );
                        Console.WriteLine( "Wrote module to kls.o" );
                    }
                }
            }
        }
    }
}
