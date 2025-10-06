// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.DataLayoutBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Error;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT
{
    [TestClass]
    public class DataLayoutBindingsTests
    {
        [TestMethod]
        public void LibLLVMParseDataLayoutTest( )
        {
            using(LLVMErrorRef errorRef = LibLLVMParseDataLayout( "badlayout"u8, out LLVMTargetDataRef retVal ))
            {
                Assert.IsTrue( retVal.IsNull );
                Assert.IsTrue( errorRef.Failed );
                Assert.IsFalse( errorRef.Success);
                Assert.IsFalse( errorRef.IsNull );
                Assert.IsTrue( errorRef.IsString );
                Assert.AreEqual(LLVMGetStringErrorTypeId(), errorRef.TypeId);
                Assert.AreNotEqual( 0, errorRef.DangerousGetHandle() );
                string errMsg = errorRef.ToString();
                Assert.IsFalse( string.IsNullOrWhiteSpace( errMsg ), "Failure should have an error message" );
            }

            // should match SPARC but as long as it is valid syntax the semantics don't matter.
            LazyEncodedString goodLayout = "E-p:32:32-f128:128:128"u8;
            using(var errorRef = LibLLVMParseDataLayout( goodLayout, out LLVMTargetDataRef retVal ))
            using(retVal)
            {
                Assert.IsFalse( errorRef.Failed );
                Assert.IsTrue( errorRef.Success );
                Assert.IsFalse( retVal.IsNull );
                string errMsg = errorRef.ToString();
                Assert.IsTrue( string.IsNullOrWhiteSpace( errMsg ), "Valid layout should NOT have an error message" );
            }
        }

        [TestMethod]
        public void LibLLVMGetDataLayoutStringTest( )
        {
            // should match SPARC but as long as it is valid syntax the semantics don't matter.
            LazyEncodedString goodLayout = "E-p:32:32-f128:128:128"u8;
            using(var errorRef = LibLLVMParseDataLayout( goodLayout, out LLVMTargetDataRef retVal ))
            using(retVal)
            {
                // status of parse assumed correct, behavior is validated in LibLLVMParseDataLayoutTest()

                LazyEncodedString? retrievedLayout = LibLLVMGetDataLayoutString(retVal);
                Assert.IsNotNull( retrievedLayout );
                Assert.IsFalse( LazyEncodedString.IsNullOrEmpty( retrievedLayout ) );
                Assert.IsFalse( LazyEncodedString.IsNullOrWhiteSpace( retrievedLayout ) );

                // This assert is a bit dodgy, as there's no formal canonicalization of the layout strings
                // and multiple strings can describe the same layout... BUT, the retrieval returns the
                // EXACT string used to create the layout, thus, for this test it is a legit thing to do.
                Assert.AreEqual( goodLayout, retrievedLayout );
            }
        }
    }
}
