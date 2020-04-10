// -----------------------------------------------------------------------
// <copyright file="ModuleFixtures.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop;

namespace Kaleidosocope.Tests
{
    // Provides common location for one time initialization for all tests in this assembly
    [TestClass]
    public static class ModuleFixtures
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize( TestContext ctx )
        {
            if( ctx == null )
            {
                throw new ArgumentNullException( nameof( ctx ) );
            }

            // LLVM really doesn't like being re-initialized in the same module
            // the LLVM-C un-init seems to undo things more aggressively than the init
            // does. (e.g. it wipes out things assumed init via static construction so
            // they are not re-initialized [Not verified, but that would explain the behavior]
            LibLLVM = Library.InitializeLLVM( );
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
