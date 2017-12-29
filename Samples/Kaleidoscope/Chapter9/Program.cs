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
        /// <param name="args">Ignored...</param>
        [SuppressMessage( "Redundancies in Symbol Declarations", "RECS0154:Parameter is never used", Justification = "Standard required signature" )]
        public static void Main( string[ ] args )
        {
            WaitForDebugger( );

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
                    generator.Module.DIBuilder.Finish( );
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
