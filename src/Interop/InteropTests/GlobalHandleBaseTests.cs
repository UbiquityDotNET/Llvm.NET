using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    public class TestGlobalHandle
        : GlobalHandleBase
    {
        public TestGlobalHandle()
            : base(true)
        {
        }

        public TestGlobalHandle(nint abi, bool owned = true)
            : base(abi, owned)
        {
        }

        protected override bool ReleaseHandle( )
        {
            ++ReleaseHandleCount;
            return true;
        }

        public int ReleaseHandleCount { get; private set; } = 0;
    }

    [TestClass()]
    public class GlobalHandleBaseTests
    {
        [TestMethod()]
        [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP017:Prefer using", Justification = "Not appropriate for a test" )]
        [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP016:Don't use disposed instance", Justification = "Not appropriate for a test" )]
        public void BasicStatusTest( )
        {
            TestGlobalHandle ownedInst1 = new(NativeHandleValue, owned: true);
            Assert.IsTrue(ownedInst1.IsOwned);
            Assert.IsFalse(ownedInst1.IsClosed);
            Assert.IsFalse(ownedInst1.IsInvalid);
            Assert.AreEqual(NativeHandleValue, ownedInst1.DangerousGetHandle());

            ownedInst1.Dispose();
            Assert.AreEqual(1, ownedInst1.ReleaseHandleCount);
            Assert.IsTrue(ownedInst1.IsOwned);
            Assert.IsTrue(ownedInst1.IsClosed);
            // Disposal/Close does NOT alter the underlying handle value
            // so the IsInvalid should report false.
            Assert.IsFalse(ownedInst1.IsInvalid);
            Assert.AreEqual(NativeHandleValue, ownedInst1.DangerousGetHandle());

            // Additional calls to Close()/Dispose() while a bug, have no detrimental side effects
            ownedInst1.Close();
            // Second Dispose does not release the handle again!
            Assert.AreEqual(1, ownedInst1.ReleaseHandleCount);
            Assert.IsTrue(ownedInst1.IsOwned);
            Assert.IsTrue(ownedInst1.IsClosed);
            // Disposal/Close does NOT alter the underlying handle value
            // so the IsInvalid should report false.
            Assert.IsFalse(ownedInst1.IsInvalid);
            Assert.AreEqual(NativeHandleValue, ownedInst1.DangerousGetHandle());
        }

        [TestMethod]
        public void AreSameTest( )
        {
            using TestGlobalHandle ownedInst1 = new(NativeHandleValue, owned: true);
            Assert.IsTrue(ownedInst1.IsOwned);
            Assert.IsFalse(ownedInst1.IsClosed);
            Assert.AreEqual(NativeHandleValue, ownedInst1.DangerousGetHandle());

            // create an unowned alias with same underlying ABI value
            // NOTE: There is no way to correctly support TWO instances created with
            //       owned == true! Such a thing is bound to create failures unless
            //       the underlying native API contract is for a ref counted object
            //       and the release is decrementing the ref count in native code.
            using TestGlobalHandle unownedInst1 = new(NativeHandleValue, owned: false);
            Assert.IsFalse(unownedInst1.IsOwned);
            Assert.IsFalse(unownedInst1.IsClosed);
            Assert.AreEqual(NativeHandleValue, unownedInst1.DangerousGetHandle());
            Assert.IsTrue(unownedInst1.AreSame(ownedInst1));
        }

        private const nint NativeHandleValue = 0x12345678;
    }
}
