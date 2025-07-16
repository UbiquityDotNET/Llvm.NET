// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Ubiquity.NET.Llvm;

using static Ubiquity.NET.Llvm.Library;

namespace Kaleidoscope.Chapter5
{
    public static class Program
    {
        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 5)</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation of the program.</returns>
        public static async Task Main( )
        {
            var repl = new ReplEngine( );

            using CancellationTokenSource cts = new();
            Console.CancelKeyPress += ( _, e ) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Interpreter - {repl.LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly().GetName()}: {helloMsg}";
            Console.WriteLine( helloMsg );

            using var libLlvm = InitializeLLVM( );
            libLlvm.RegisterTarget( CodeGenTarget.Native );
            await repl.Run( Console.In, cts.Token );

            Console.WriteLine();
            Console.WriteLine( "good bye!" );
        }
    }
}
