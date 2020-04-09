// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using Kaleidoscope.Grammar;
using Kaleidoscope.Grammar.AST;
using Kaleidoscope.Runtime;

namespace Kaleidoscope.Chapter2
{
    public static class Program
    {
        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 2)</summary>
        public static void Main( )
        {
            string helloMsg = "Ubiquity.NET.Llvm Kaleidoscope parse evaluator";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );

            ShowPrompt( ReadyState.StartExpression );
            var parser = new Parser( LanguageLevel.MutableVariables );

            // Create sequence of parsed AST nodes to feed the loop
            var nodes = from stmt in Console.In.ToStatements( ShowPrompt )
                        select parser.Parse( stmt );

            // Read, Parse, Print loop
            foreach( IAstNode node in nodes )
            {
                Console.WriteLine( "PARSED: {0}", node );
            }
        }

        public static void ShowPrompt( ReadyState state )
        {
            Console.Write( state == ReadyState.StartExpression ? "Ready>" : ">" );
        }
    }
}
