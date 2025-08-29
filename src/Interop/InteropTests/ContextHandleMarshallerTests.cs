// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class ContextHandleMarshallerTests
    {
        [TestMethod]
        public void ConvertToManagedTest( )
        {
            var handle = ContextHandleMarshaller<LibLLVMMDOperandRef>.ConvertToManaged(NativeABIValue);
            Assert.IsFalse( handle.IsNull );
            Assert.AreEqual( NativeABIValue, handle.DangerousGetHandle() );
            Assert.AreEqual( NativeABIValue, (nint)handle );
        }

        [TestMethod]
        public void ConvertToUnmanagedTest( )
        {
            var handle = LibLLVMMDOperandRef.FromABI(NativeABIValue);

            // Validate FromABI() method AND verify assumptions made in subsequent asserts...
            Assert.AreEqual( NativeABIValue, handle.DangerousGetHandle() );
            nint abiHandleVal = ContextHandleMarshaller<LibLLVMMDOperandRef>.ConvertToUnmanaged(handle);
            Assert.AreEqual( NativeABIValue, abiHandleVal );
        }

        private const nint NativeABIValue = 0x12345678;
    }
}
