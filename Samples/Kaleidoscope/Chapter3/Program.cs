// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Ubiquity.NET.Llvm;
using static Ubiquity.NET.Llvm.Library;

namespace Kaleidoscope.Chapter3
{
    public static class Program
    {
        #region Main

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 3)</summary>
        public static void Main( )
        {
            var repl = new ReplEngine( );

            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Interpreter - {repl.LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );

            using var libLlvm = InitializeLLVM( );
            libLlvm.RegisterTarget( CodeGenTarget.Native );
            repl.Run( Console.In );
        }
        #endregion
    }
}
