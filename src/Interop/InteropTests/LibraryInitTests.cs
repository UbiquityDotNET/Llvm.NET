// -----------------------------------------------------------------------
// <copyright file="LibraryInitTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class LibraryInitTests
    {
        [DistinctProcessTestMethod]
        public void TestLibraryInit( )
        {
            using var lib = Library.InitializeLLVM(LibLLVMCodeGenTarget.CodeGenTarget_Native);
            Assert.IsNotNull(lib);
        }

        [DistinctProcessTestMethod]
        public void TestLibraryReInit( )
        {
            using(var lib = Library.InitializeLLVM(LibLLVMCodeGenTarget.CodeGenTarget_Native))
            {
                Assert.IsNotNull(lib);
            }

            _ = Assert.ThrowsExactly<InvalidOperationException>(()=>
            {
                // attempt to re-initialize the library. This is expected
                // to fail. The runtime has already resolved the address of
                // the import functions and has no way to "invalidate" the
                // resolution to an address. Thus it is not possible to reload
                // the library with a different one, or even unload and then
                // reload again as it might land the methods at a different
                // address.
                using var lib = Library.InitializeLLVM( LibLLVMCodeGenTarget.CodeGenTarget_ARM );
            } );
        }

        [DistinctProcessTestMethod]
        public void TestLibraryInitWithAllFails( )
        {
            _ = Assert.ThrowsExactly<ArgumentOutOfRangeException>(()=>
            {
                using var lib = Library.InitializeLLVM( LibLLVMCodeGenTarget.CodeGenTarget_All );
            } );
        }
    }
}
