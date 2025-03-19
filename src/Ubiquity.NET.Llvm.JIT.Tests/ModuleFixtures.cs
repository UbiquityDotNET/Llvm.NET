// -----------------------------------------------------------------------
// <copyright file="ModuleFixtures.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Unit Tests" )]

namespace Ubiquity.NET.LlvmTests
{
    // Provides common location for one time initialization for all tests in this assembly
    [TestClass]
    public static class ModuleFixtures
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize( TestContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );
            LibLLVM?.Dispose();
            LibLLVM = Library.InitializeLLVM( );

            // Testing JIT so the only target of relevance is the native machine
            LibLLVM.RegisterTarget( CodeGenTarget.Native );
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup( )
        {
            LibLLVM?.Dispose( );
        }

        private static ILibLlvm? LibLLVM;
    }
}
