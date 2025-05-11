#if PROBABLY_DELETE_ME_LATER_AS_LAZYENCODEDSTRING_IS_BETTER
using System;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.InteropHelpers.Tests
{
    [TestClass()]
    public class ExecutionEncodingStringMarshallerTests
    {
        [TestMethod()]
        public void ReadOnlySpanFromNullTerminatedTest( )
        {
            var testMsg = "Testing 1,2,3"u8;
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference(testMsg))
                {
                    var span = ExecutionEncodingStringMarshaller.ReadOnlySpanFromNullTerminated(pMsg);
                    Assert.AreEqual(testMsg.Length, span.Length);
                    fixed(byte* pSpan = &MemoryMarshal.GetReference(span))
                    {
                        Assert.AreEqual((nint)pMsg, (nint)pSpan, "spans should contain the same pointer");
                    }

                    Assert.IsTrue(span.SequenceEqual(testMsg));
                }
            }
        }

        [TestMethod()]
        public void ConvertToUnmanagedTest( )
        {
            var nativeTestMsg = "Testing 1,2,3"u8;
            string managedTestMsg = "Testing 1,2,3";
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference(nativeTestMsg))
                {
                    byte* nullPtr = ExecutionEncodingStringMarshaller.ConvertToUnmanaged(null);

#pragma warning disable MSTEST0037 // Use proper 'Assert' methods
                    // It is proper - recommendation is a syntax error!
                    Assert.IsTrue(nullPtr is null);
#pragma warning restore MSTEST0037 // Use proper 'Assert' methods

                    byte* pNativeMsg = ExecutionEncodingStringMarshaller.ConvertToUnmanaged(managedTestMsg);
                    try
                    {
                        var nativeSpan = ExecutionEncodingStringMarshaller.ReadOnlySpanFromNullTerminated(pNativeMsg);
                        Assert.IsTrue(nativeTestMsg.SequenceEqual(nativeSpan));
                    }
                    finally
                    {
                        ExecutionEncodingStringMarshaller.Free(pNativeMsg);
                    }
                }
            }
        }

        [TestMethod()]
        public void ConvertToManagedTest( )
        {
            var nativeTestMsg = "Testing 1,2,3"u8;
            string managedTestMsg = "Testing 1,2,3";
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference(nativeTestMsg))
                {
                    string? nullMsg = ExecutionEncodingStringMarshaller.ConvertToManaged(null);
                    Assert.IsNull(nullMsg);

                    string? msg = ExecutionEncodingStringMarshaller.ConvertToManaged(pMsg);
                    Assert.IsNotNull(msg);
                    Assert.AreEqual(managedTestMsg, msg);
                }
            }
        }

        [TestMethod]
        public void ManagedToUnmanagedInTests( )
        {
            string managedTest = "Testing 1,2,3";
            var expectedNativeTest = "Testing 1,2,3"u8;

            unsafe
            {
                var marshaller = new ExecutionEncodingStringMarshaller.ManagedToUnmanagedIn();
                byte* buffer = stackalloc byte[ExecutionEncodingStringMarshaller.ManagedToUnmanagedIn.BufferSize];
                marshaller.FromManaged(managedTest, new(buffer, ExecutionEncodingStringMarshaller.ManagedToUnmanagedIn.BufferSize));
                try
                {
                    byte* pNative = marshaller.ToUnmanaged();

#pragma warning disable MSTEST0037 // Use proper 'Assert' methods
                    // It is proper - recommendation is a syntax error!
                    Assert.IsFalse(pNative is null);
#pragma warning restore MSTEST0037 // Use proper 'Assert' methods

                    var span = ExecutionEncodingStringMarshaller.ReadOnlySpanFromNullTerminated(pNative);
                    Assert.IsTrue(expectedNativeTest.SequenceEqual(span));
                }
                finally
                {
                    marshaller.Free();
                }
            }
        }
    }
}
#endif
