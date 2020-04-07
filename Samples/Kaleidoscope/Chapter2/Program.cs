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

namespace Kaleidoscope.Chapter2
{
    public static class Program
    {
        // Using language level that includes the complete set for exploration of pare trees and AST
        private const LanguageLevel LanguageFeatureLevel = LanguageLevel.MutableVariables;

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial</summary>
        /// <returns>Async task</returns>
        public static async Task Main( )
        {
            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Explorer - {LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );

            #region GeneratorLoop
            var parser = new Parser( LanguageFeatureLevel );

            ShowPrompt( ReadyState.StartExpression );

            // Create sequence to feed the REPL loop
            var replSeq = from stmt in Console.In.ToStatements( ShowPrompt )
                          let node = parser.Parse( stmt )
                          where !ShowParseErrors( node )
                          select node;

            await foreach( IAstNode node in replSeq )
            {
                ShowResults( node );
            }

            Console.WriteLine( "Bye!" );
            #endregion
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

        private static void ShowError( string msg )
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine( msg );
            Console.ForegroundColor = color;
        }
        #endregion

        #region ShowResults
        private static void ShowResults( IAstNode node )
        {
            Console.WriteLine( "Parsed {0}", node.GetType( ).Name );
        }
        #endregion
    }
}
