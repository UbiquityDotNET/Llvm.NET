// -----------------------------------------------------------------------
// <copyright file="DataLayoutTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.DataLayoutBindings;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class DataLayoutTests
    {
        [TestMethod]
        public void TestParseKnownBad( )
        {
            ReadOnlySpan<byte> utf8Span = "badlayout"u8;
            unsafe
            {
                fixed(byte* p = &MemoryMarshal.GetReference(utf8Span))
                {
                    using var ErrorRef = LibLLVMParseDataLayout(p, utf8Span.Length, out LLVMTargetDataRef retVal);
                    using(retVal)
                    {
                        Assert.IsTrue(retVal.IsInvalid);
                        Assert.IsTrue(ErrorRef.Failed);
                        string errMsg = ErrorRef.ToString();
                        Assert.IsFalse(string.IsNullOrWhiteSpace(errMsg));
                    }
                }
            }
        }
    }
}
