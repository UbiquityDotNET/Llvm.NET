// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm;

namespace Kaleidoscope.Tests
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
            LibLLVM = Library.InitializeLLVM();
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
