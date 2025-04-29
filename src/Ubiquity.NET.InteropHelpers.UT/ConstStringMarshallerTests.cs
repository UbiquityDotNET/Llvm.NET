using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.InteropHelpers.UT
{
    [TestClass()]
    public class ConstStringMarshallerTests
    {
        [TestMethod()]
        public void ConvertToManagedTest( )
        {
            var testMsg = "testing1,2,3"u8;
            string? convertedValue = null;
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference(testMsg))
                {
                    convertedValue = ConstStringMarshaller.ConvertToManaged(pMsg);
                }
            }

            Assert.IsFalse(string.IsNullOrWhiteSpace(convertedValue));
            Assert.AreEqual("testing1,2,3", convertedValue);
        }

        [TestMethod()]
        public void FreeTest( )
        {
            var testMsg = "testing1,2,3"u8;
            var testMsg2 = "testing1,2,3"u8;
            Assert.IsTrue(testMsg.SequenceEqual(testMsg2), "should be same sequence before test");
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference(testMsg))
                {
                    // Should be a NOP that doesn't crash, corrupt or alter the input pointer
                    // in any way...
                    ConstStringMarshaller.Free(pMsg);
                    Assert.IsTrue(testMsg.SequenceEqual(testMsg2), "should be same sequence after test");
                }
           }
        }
    }
}
