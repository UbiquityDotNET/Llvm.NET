// -----------------------------------------------------------------------
// <copyright file="ContextBindingsTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ContextBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT
{
    [TestClass]
    public class ContextBindingsTests
    {
        [TestMethod]
        public void LibLLVMContextGetIsODRUniquingDebugTypesTest( )
        {
            using LLVMContextRef ctx = LLVMContextCreate();
            Assert.IsFalse( LibLLVMContextGetIsODRUniquingDebugTypes( ctx ) );
        }

        [TestMethod]
        public void LibLLVMContextSetIsODRUniquingDebugTypesTest( )
        {
            using LLVMContextRef ctx = LLVMContextCreate();
            LibLLVMContextSetIsODRUniquingDebugTypes( ctx, true );
            Assert.IsTrue( LibLLVMContextGetIsODRUniquingDebugTypes( ctx ) );
        }
    }
}
