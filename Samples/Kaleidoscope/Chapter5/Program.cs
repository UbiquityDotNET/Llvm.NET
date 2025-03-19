// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Ubiquity.NET.Llvm;

using static Ubiquity.NET.Llvm.Library;

namespace Kaleidoscope.Chapter5
{
    public static class Program
    {
        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 5)</summary>
        public static void Main( )
        {
            var repl = new ReplEngine( );

            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Interpreter - {repl.LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );

            // On Windows the exit keyboard sequence includes the <enter> key press.
            // Any other platform it's still less then obvious, so provide some help.
            Console.WriteLine($"    Use Ctrl-Z{(OperatingSystem.IsWindows() ? " (followed by <Enter>)" : string.Empty)} to exit this application.");

            using var libLlvm = InitializeLLVM( );
            libLlvm.RegisterTarget( CodeGenTarget.Native );

            repl.Run( Console.In );
        }
    }
}
