// -----------------------------------------------------------------------
// <copyright file="DataLayoutTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.DataLayoutBindings;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class DataLayoutTests
    {
        [TestMethod]
        public void TestParseKnownBad( )
        {
            LazyEncodedString utf8Span = "badlayout"u8;
            using var errorRef = LibLLVMParseDataLayout(utf8Span, out LLVMTargetDataRef retVal);
            using(retVal)
            {
                Assert.IsTrue(retVal.IsInvalid);
                Assert.IsTrue(errorRef.Failed);
                string errMsg = errorRef.ToString();
                Assert.IsFalse(string.IsNullOrWhiteSpace(errMsg), "Failure should have an error message");
            }
        }
    }
}
