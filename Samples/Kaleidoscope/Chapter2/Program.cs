// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

namespace Kaleidoscope.Chapter2
{
    public static class Program
    {
        #region Main

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 2)</summary>
        public static void Main( )
        {
            var repl = new ReplEngine( );

            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Parse evaluator - {repl.LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );

            repl.Run( Console.In, Grammar.DiagnosticRepresentations.Dgml );
        }
        #endregion
    }
}
