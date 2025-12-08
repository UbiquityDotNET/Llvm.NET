// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.UT
{
    // This tests the https://github.com/UbiquityDotNET/Llvm.NET/issues/380
    // the behavior is split to multiple test cases but the validation
    // is performed for each case.

    [TestClass]
    public class GepTests
    {
        [TestMethod]
        public void GetElementPtr_does_not_throw( )
        {
            using var ctx = new Context();

            // struct MyStruct { int64_t a; double y; MyStruct* p}; // a, y and p are debug names not used here
            var structType = ctx.CreateStructType("MyStruct"u8);
            structType.SetBody( packed: false, ctx.Int64Type, ctx.DoubleType, structType.CreatePointerType() );
            var funcSig = ctx.GetFunctionType(ctx.VoidType, structType.CreatePointerType());

            var constZero = ctx.CreateConstant(0);
            var constOne = ctx.CreateConstant(1);
            var constTwo = ctx.CreateConstant(2);
            var constThree = ctx.CreateConstant(3);

            using var module = ctx.CreateBitcodeModule();
            var func = module.CreateFunction("testfunc"u8, funcSig);
            var block = func.AppendBasicBlock("entry"u8);

            using var irBuilder = new InstructionBuilder(block);
            var pInstance = func.Parameters[ 0 ];

            // this has a distinct validation so ensure it works.
            Value gep1 = irBuilder.GetStructElementPointer(structType, pInstance, 1); // get `pInstance[0].y`
            Assert.IsNotNull( gep1 );
            Assert.IsInstanceOfType<GetElementPtr>( gep1 );

            // This will get the value of the nested MyStruct* p which might be used in another
            // call to GetElementPtr. Since, it is an OPAQUE pointer the type is not carried with it.
            // Even with a known type for the pointer one shouldn't attempt arbitrary math on the
            // pointers as it could be nullptr. Next leg of this test validates it throws.
            Value gep2 = irBuilder.GetElementPtr(structType, pInstance, constZero, constTwo); // get `pInstance[0].p`
            Assert.IsNotNull( gep2 );
            Assert.IsInstanceOfType<GetElementPtr>( gep2 );
            var instGep2 = (GetElementPtr)gep2;
            Assert.AreEqual( pInstance, instGep2.Base );
            var indeces = instGep2.IndexValues;
            Assert.HasCount( 2, indeces );
            Assert.AreEqual( constZero, indeces[ 0 ] );
            Assert.AreEqual( constTwo, indeces[ 1 ] );
        }

        [TestMethod]
        public void GetElementPtr_indexing_through_a_pointer_throws( )
        {
            using var ctx = new Context();

            // struct MyStruct { int64_t a; double y; MyStruct* p}; // a, y and p are debug names not used here
            var structType = ctx.CreateStructType("MyStruct"u8);
            structType.SetBody( packed: false, ctx.Int64Type, ctx.DoubleType, structType.CreatePointerType() );
            var funcSig = ctx.GetFunctionType(ctx.VoidType, structType.CreatePointerType());

            var constZero = ctx.CreateConstant(0);
            var constOne = ctx.CreateConstant(1);
            var constTwo = ctx.CreateConstant(2);

            using var module = ctx.CreateBitcodeModule();
            var func = module.CreateFunction("testfunc"u8, funcSig);
            var block = func.AppendBasicBlock("entry"u8);

            using var irBuilder = new InstructionBuilder(block);
            var pInstance = func.Parameters[ 0 ];

            // attempting to index through a pointer should generate an error.
            // NOTE: underlying LLVM seems to allow this, but it's a very dangerous
            // so the managed wrapper will detect this case and throw.
            Assert.ThrowsExactly<ArgumentException>(
                ( ) =>
                {
                    _ = irBuilder.GetElementPtr( structType, pInstance, constZero, constTwo, constZero, constOne ); // get `pInstance[0].p[0].y`
                } );
        }

        [TestMethod]
        public void GetElementPtr_indexing_beyond_declared_members_throws( )
        {
            using var ctx = new Context();

            // struct MyStruct { int64_t a; double y; MyStruct* p}; // a, y and p are debug names not used here
            var structType = ctx.CreateStructType("MyStruct"u8);
            structType.SetBody( packed: false, ctx.Int64Type, ctx.DoubleType, structType.CreatePointerType() );
            var funcSig = ctx.GetFunctionType(ctx.VoidType, structType.CreatePointerType());

            var constZero = ctx.CreateConstant(0);
            var constOne = ctx.CreateConstant(1);
            var constTwo = ctx.CreateConstant(2);
            var constThree = ctx.CreateConstant(3);

            using var module = ctx.CreateBitcodeModule();
            var func = module.CreateFunction("testfunc"u8, funcSig);
            var block = func.AppendBasicBlock("entry"u8);

            using var irBuilder = new InstructionBuilder(block);
            var pInstance = func.Parameters[ 0 ];

            Assert.ThrowsExactly<ArgumentException>(
                ( ) =>
                {
                    // index of 3 is out of range for the struct
                    _ = irBuilder.GetElementPtr( structType, pInstance, constZero, constThree ); // get `pInstance[0].<undefined>`
                } );
        }
    }
}
