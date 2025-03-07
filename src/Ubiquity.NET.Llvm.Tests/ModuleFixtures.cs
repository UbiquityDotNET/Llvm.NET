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

            LibLLVM = Library.InitializeLLVM();
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
