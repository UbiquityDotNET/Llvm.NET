// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Kaleidoscope.Chapter2
{
    public static class Program
    {
        #region Main

        /// <summary>C# version of the LLVM Kaleidoscope language tutorial (Chapter 2)</summary>
        public static async Task Main( )
        {
            var repl = new ReplEngine( );

            using CancellationTokenSource cts = new();
            Console.CancelKeyPress += ( _, e ) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine();
                Console.WriteLine("good bye!");
            };

            string helloMsg = $"Ubiquity.NET.Llvm Kaleidoscope Parse evaluator - {repl.LanguageFeatureLevel}";
            Console.Title = $"{Assembly.GetExecutingAssembly( ).GetName( )}: {helloMsg}";
            Console.WriteLine( helloMsg );

            await repl.Run( Console.In, Grammar.DiagnosticRepresentations.Dgml, cts.Token );
        }
        #endregion
    }
}
