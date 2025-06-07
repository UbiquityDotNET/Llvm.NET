// -----------------------------------------------------------------------
// <copyright file="ModuleFixtures.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    // Provides common location for one time initialization for all tests in this assembly
    [TestClass]
    public static class ModuleFixtures
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize( TestContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );

            // ctx.WriteLine("Hello world!"); // Goes to great bit pool in the sky... [Sort-of.]
            // reality: ctx.Write*() calls go to a string writer, the results of that are captured
            // stored and then ignored, UNLESS the result of THIS initializer is not success.
            // Thus, in the real world those APIs are completely useless in an assembly initializer.
            // Instead an implementation can use the DisplayMessage() method to write data to the
            // "tests" pane in VS (Where, or if, that is reported in other environments is unknown)
            // ctx.DisplayMessage(MessageLevel.Informational, nameof(AssemblyInitialize)); // Goes to "Tests" pane in VS
            LibLLVM?.Dispose();

            LibLLVM = Library.InitializeLLVM();

            // Native is assumed, Tests also use Cortex-M3; so load that variant of
            // the interop APIs.
            // NOTE: Target tests may need to register all, but that's OK as it includes
            //       these.
            LibLLVM.RegisterTarget( LibLLVMCodeGenTarget.CodeGenTarget_Native );
            LibLLVM.RegisterTarget( LibLLVMCodeGenTarget.CodeGenTarget_ARM );
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup( )
        {
            LibLLVM?.Dispose();
        }

        internal static ILibLlvm? LibLLVM { get; private set; }
    }
}
