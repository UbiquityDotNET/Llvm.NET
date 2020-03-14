// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Linq;
using System.Reflection;

using Kaleidoscope.Grammar;
using Kaleidoscope.Runtime;
using Ubiquity.NET.Llvm.Values;

using static Kaleidoscope.Runtime.Utilities;
using static Ubiquity.NET.Llvm.Interop.Library;

namespace Kaleidoscope.Chapter7
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Design", "CA1062:Validate arguments of public methods", Justification = "Provided by platform" )]
        public static void Main( string[ ] args )
        {
            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Interpreter - {LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );
            WaitForDebugger( args.Length > 0 && string.Compare( args[ 0 ], "waitfordebugger", StringComparison.OrdinalIgnoreCase ) == 0 );

            using( InitializeLLVM( ) )
            {
                RegisterNative( );

                #region GeneratorLoop
                var parser = new Parser( LanguageFeatureLevel );
                using var generator = new CodeGenerator( parser.GlobalState );
                var readyState = new ReadyStateManager( );

                // Create Observable chain to provide the REPL implementation
                var replSeq = parser.Parse( Console.In.ToObservableStatements( ShowPrompt ), ShowCodeGenError )
                                    .GenerateResults( generator, ShowCodeGenError );

                // Run the sequence
                using( replSeq.Subscribe( ShowResults ) )
                {
                }
                #endregion
            }
        }

        #region ShowPrompt
        private static void ShowPrompt( ReadyState state )
        {
            Console.Write( state == ReadyState.StartExpression ? "Ready>" : ">" );
        }
        #endregion

        #region ErrorHandling
        private static void ShowCodeGenError( CodeGeneratorException ex )
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine( ex.Message );
            Console.ForegroundColor = color;
        }
        #endregion

        #region ShowResults
        private static void ShowResults( Value? resultValue )
        {
            switch( resultValue )
            {
            case ConstantFP result:
                if( Console.CursorLeft > 0 )
                {
                    Console.WriteLine( );
                }

                Console.WriteLine( "Evaluated to {0}", result.Value );
                break;

            case IrFunction function:
#if SAVE_LLVM_IR
                function.ParentModule.WriteToTextFile( System.IO.Path.ChangeExtension( GetSafeFileName( function.Name ), "ll" ), out string ignoredMsg );
#endif
                break;

            default:
                throw new InvalidOperationException( );
            }
        }
        #endregion
    }
}
