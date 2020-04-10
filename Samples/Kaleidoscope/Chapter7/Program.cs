// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.Library;

namespace Kaleidoscope.Chapter7
{
    public static class Program
    {
        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 7)</summary>
        public static void Main( )
        {
            var repl = new ReplEngine( );

            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Interpreter - {repl.LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( ).Name}: {helloMsg}";
            Console.WriteLine( helloMsg );

            using var libLlvm = InitializeLLVM( );
            libLlvm.RegisterTarget( CodeGenTarget.Native );
            repl.Run( Console.In );
        }
    }
}
