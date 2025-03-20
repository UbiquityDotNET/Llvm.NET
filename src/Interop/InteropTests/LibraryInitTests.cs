// -----------------------------------------------------------------------
// <copyright file="LibraryInitTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class LibraryInitTests
    {
        [TestMethod]
        public void TestLibraryInit( )
        {
            using var lib = Library.InitializeLLVM();
            Assert.IsNotNull(lib);
        }

        [TestMethod]
        public void TestLibraryReInit( )
        {
            using(var lib = Library.InitializeLLVM())
            {
                Assert.IsNotNull(lib);
                lib.RegisterTarget(CodeGenTarget.Native);
            }

            // re-initialize the library without unloading it
            // (There is no API to allow unloading it at present)
            using(var lib = Library.InitializeLLVM())
            {
                Assert.IsNotNull(lib);
                lib.RegisterTarget(CodeGenTarget.ARM);
            }
        }
    }
}
