// -----------------------------------------------------------------------
// <copyright file="ModuleFixtures.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.UT
{
    // Provides common location for one time initialization for all tests in this assembly
    [TestClass]
    public static class ModuleFixtures
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext ctx)
        {
            ArgumentNullException.ThrowIfNull( ctx );

            LibLLVM?.Dispose();

            // Native is assumed, Tests also use Cortex-M3; so load that variant of
            // the interop APIs.
            LibLLVM = Library.InitializeLLVM( CodeGenTarget.ARM );
            Assert.AreEqual(2, LibLLVM.Targets.Length);
            Assert.AreEqual(Library.NativeTarget, LibLLVM.Targets[0]);
            Assert.AreEqual(CodeGenTarget.ARM, LibLLVM.Targets[1]);
            LibLLVM.RegisterTarget( CodeGenTarget.All );
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            LibLLVM?.Dispose();
        }

        private static ILibLlvm? LibLLVM;
    }
}
