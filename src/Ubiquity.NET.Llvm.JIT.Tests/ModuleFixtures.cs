// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm;

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
            LibLLVM?.Dispose();

            LibLLVM = Library.InitializeLLVM();

            // Testing JIT on native system, so the only target of relevance is the native machine
            LibLLVM.RegisterTarget( CodeGenTarget.Native );
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup( )
        {
            LibLLVM?.Dispose();
        }

        private static ILibLlvm? LibLLVM;
    }
}
