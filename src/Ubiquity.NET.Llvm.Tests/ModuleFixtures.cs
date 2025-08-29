// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.UT
{
    // Provides common location for one time initialization for all tests in this assembly
    [TestClass]
    public static class ModuleFixtures
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize( TestContext ctx )
        {
            ArgumentNullException.ThrowIfNull(ctx);

            LibLLVM?.Dispose();

            LibLLVM = Library.InitializeLLVM();

            // Native is assumed, Tests also use Cortex-M3; so load that variant of
            // the interop APIs.
            // NOTE: Target tests may need to register all, but that's OK as it includes
            //       these.
            LibLLVM.RegisterTarget(CodeGenTarget.Native);
            LibLLVM.RegisterTarget(CodeGenTarget.ARM);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup( )
        {
            LibLLVM?.Dispose();
        }

        internal static ILibLlvm? LibLLVM { get; private set; }
    }
}
