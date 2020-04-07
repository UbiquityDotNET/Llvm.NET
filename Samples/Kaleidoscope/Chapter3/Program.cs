// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.Library;

namespace Kaleidoscope.Chapter3
{
    public static class Program
    {
        private const LanguageLevel LanguageFeatureLevel = LanguageLevel.SimpleExpressions;

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 3)</summary>
        /// <returns>Async task</returns>
        public static async Task Main( )
        {
            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Interpreter - {LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );

            using( InitializeLLVM( ) )
            {
                RegisterNative( );

                #region GeneratorLoop
                var parser = new Parser( LanguageFeatureLevel );
                using var generator = new CodeGenerator( parser.GlobalState );

                ShowPrompt( ReadyState.StartExpression );

                // Create sequence to feed the REPL loop
                var replSeq = from stmt in Console.In.ToStatements( ShowPrompt )
                              let node = parser.Parse( stmt )
                              where !ShowParseErrors( node )
                              select node;

                await foreach( IAstNode node in replSeq )
                {
                    try
                    {
                        ShowResults( generator.Generate( node ) );
                    }
                    catch( CodeGeneratorException ex )
                    {
                        // This is an internal error that is not recoverable.
                        // Show the error and stop additional processing
                        ShowError( ex );
                        break;
                    }
                }

                Console.WriteLine( generator.Module.WriteToString( ) );
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
        private static bool ShowParseErrors( this IAstNode node )
        {
            if( node is ErrorNode errNode )
            {
                ShowError( errNode.Error );
                return true;
            }

            if( node.Errors.Count > 0 )
            {
                foreach( var err in node.Errors )
                {
                    ShowError( err.Error );
                }

                return true;
            }

            return false;
        }

        private static void ShowError( CodeGeneratorException ex )
        {
            ShowError( ex.ToString( ) );
        }

        private static void ShowError( string msg )
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine( msg );
            Console.ForegroundColor = color;
        }
        #endregion

        #region ShowResults
        private static void ShowResults( OptionalValue<Value> resultValue )
        {
            if( resultValue.HasValue )
            {
                switch( resultValue.Value )
                {
                case IrFunction function:
                    Console.WriteLine( "Defined function: {0}", function.Name );
                    Console.WriteLine( function );
                    break;

                default:
                    throw new InvalidOperationException( );
                }
            }
        }
        #endregion
    }
}
