// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Llvm.NET.Values;

using static Kaleidoscope.Runtime.Utilities;
using static Llvm.NET.StaticState;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace Kaleidoscope
{
    public static class Program
    {
        private const LanguageLevel LanguageFeatureLevel = LanguageLevel.MutableVariables;

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
            string helloMsg = $"Llvm.NET Kaleidoscope Interpreter - {LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );
            WaitForDebugger( args.Length > 0 && string.Compare( args[0], "waitfordebugger", StringComparison.InvariantCultureIgnoreCase ) == 0 );

            using( InitializeLLVM( ) )
            {
                RegisterNative( );

                // <generatorloop>
                var parser = new ParserStack( LanguageFeatureLevel );
                using( var generator = new CodeGenerator( parser.GlobalState ) )
                {
                    var replLoop = new ReplLoop<Value>( generator, parser );
                    replLoop.ReadyStateChanged += ( s, e ) => Console.Write( e.PartialParse ? ">" : "Ready>" );
                    replLoop.GeneratedResultAvailable += OnGeneratedResultAvailable;
                    replLoop.CodeGenerationError += OnGeneratorError;

                    replLoop.Run( );
                }
                // </generatorloop>
            }
        }

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

        // <ResultProcessing>
        private static void OnGeneratedResultAvailable( object sender, GeneratedResultAvailableArgs<Value> e )
        {
            var source = ( ReplLoop<Value> )sender;

            switch( e.Result )
            {
            case ConstantFP result:
                if( Console.CursorLeft > 0 )
                {
                    Console.WriteLine( );
                }
                Console.WriteLine( "Evaluated to {0}", result.Value );
                break;

            case Function function:
#if GENERATE_LLVM_IR
                function.ParentModule.WriteToTextFile( System.IO.Path.ChangeExtension( GetSafeFileName( function.Name ), "ll" ), out string ignoredMsg );
#endif
                break;
            }
        }
        // </ResultProcessing>
    }
}
