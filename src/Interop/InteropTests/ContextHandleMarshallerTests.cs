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
            Assert.IsFalse(handle.IsNull);
            Assert.AreEqual(NativeABIValue, handle.DangerousGetHandle());
            Assert.AreEqual(NativeABIValue, (nint)handle);
        }

        [TestMethod]
        public void ConvertToUnmanagedTest( )
        {
            var handle = LibLLVMMDOperandRef.FromABI(NativeABIValue);
            // Validate FromABI() method AND verify assumptions made in subsequent asserts...
            Assert.AreEqual(NativeABIValue, handle.DangerousGetHandle());
            nint abiHandleVal = ContextHandleMarshaller<LibLLVMMDOperandRef>.ConvertToUnmanaged(handle);
            Assert.AreEqual(NativeABIValue, abiHandleVal);
        }

        private const nint NativeABIValue = 0x12345678;
    }
}
