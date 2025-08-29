// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AnalysisBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT
{
    [TestClass]
    public class AnalysisBindingsTests
    {
        [TestMethod]
        public void LibLLVMVerifyFunctionExTest( )
        {
            using LLVMContextRef ctx = LLVMContextCreate();
            using LLVMModuleRef module = LLVMModuleCreateWithNameInContext("testModule"u8, ctx);
            LLVMTypeRef intType = LLVMInt32TypeInContext(ctx);
            LLVMTypeRef funcType = LLVMFunctionType(intType, [], 0, false);

            // declare a test func to work with and create an entry block for it...
            LLVMValueRef goodFunc = LLVMAddFunction(module, "goodfunc"u8, funcType);
            ImplementFunction( goodFunc, intType, createBroken: false );
            LLVMStatus goodStatus = LibLLVMVerifyFunctionEx(goodFunc, llvm_c.LLVMVerifierFailureAction.LLVMPrintMessageAction, out string goodmsg);
            Assert.AreEqual( 0, goodStatus.ErrorCode );
            Assert.IsFalse( goodStatus.Failed );
            Assert.IsTrue( goodStatus.Succeeded );

            // test for each state of a string in order to produce a distinct failure point for each condition
            Assert.IsNotNull( goodmsg, "Error message should not be null" );
            Assert.IsTrue( string.IsNullOrEmpty( goodmsg ), "Error message should be empty, for successful validation" );

            LLVMValueRef badFunc = LLVMAddFunction(module, "badfunc"u8, funcType);
            ImplementFunction( badFunc, intType, createBroken: true );
            LLVMStatus badStatus = LibLLVMVerifyFunctionEx(badFunc, llvm_c.LLVMVerifierFailureAction.LLVMPrintMessageAction, out string badmsg);
            Assert.AreNotEqual( 0, badStatus.ErrorCode );
            Assert.IsTrue( badStatus.Failed );
            Assert.IsFalse( badStatus.Succeeded );

            // test for each state of a string in order to produce a distinct failure point for each condition
            Assert.IsNotNull( badmsg, "Error message should not be null" );
            Assert.IsFalse( string.IsNullOrEmpty( badmsg ), "Error message should not be empty" );
            Assert.IsFalse( string.IsNullOrWhiteSpace( badmsg ), "Error message should not be all whitespace" );
        }

        private static void ImplementFunction( LLVMValueRef func, LLVMTypeRef intType, bool createBroken = false )
        {
            LLVMContextRefAlias ctx = LLVMGetValueContext(func);

            // Create an instruction builder to work with and connect it to
            // a new entry block for the test function
            using LLVMBuilderRef testBuilder = LLVMCreateBuilderInContext(ctx);
            LLVMPositionBuilderAtEnd( testBuilder, LLVMAppendBasicBlockInContext( ctx, func, "entry"u8 ) );

            // Now build out some code...
            LLVMValueRef constOne = LLVMConstInt(intType, 1, false);
            LLVMValueRef r0 = LLVMBuildAdd(testBuilder, constOne, constOne, string.Empty);

            // BB without terminator is a good example of a bad function
            if(!createBroken)
            {
                LLVMBuildRet( testBuilder, r0 );
            }
        }
    }
}
