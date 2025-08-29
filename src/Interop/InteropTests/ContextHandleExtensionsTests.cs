// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class ContextHandleExtensionsTests
    {
        [TestMethod]
        public void ThrowIfInvalidTest( )
        {
            var invalidTestHandle = LibLLVMMDOperandRef.FromABI(0);
            Assert.IsTrue( invalidTestHandle.IsNull );

            var ex = Assert.ThrowsExactly<UnexpectedNullHandleException>(
                ()=> invalidTestHandle.ThrowIfInvalid("message", "memberName", "sourceFilePath", 1234)
            );
            Assert.AreEqual( "[memberName] - sourceFilePath@1234 message", ex.Message );

            var validTestHandle = LibLLVMMDOperandRef.FromABI(12345678);
            Assert.IsFalse( validTestHandle.IsNull );
            validTestHandle.ThrowIfInvalid(); // Not expected to throw!
        }
    }
}
