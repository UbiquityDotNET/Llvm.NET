// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Types;

namespace Ubiquity.NET.Llvm.UT.Instructions
{
    [TestClass]
    public class AllocaTests
    {
        // Point of this test case is to VERIFY nullability checks behavior
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        [TestMethod]
        public void Generating_alloca_with_null_args_throws( )
        {
            using var ctx = new Context();
            var testBlock = ctx.CreateBasicBlock("testBlock"u8);

            using var irBuilder = new InstructionBuilder(testBlock);
            var argNullEx = Assert.ThrowsExactly<ArgumentNullException>(
                ()=>
                {
                    _ = irBuilder.Alloca(null);
                }
            );
            Assert.AreEqual( "typeRef", argNullEx.ParamName );

            argNullEx = Assert.ThrowsExactly<ArgumentNullException>(
                ( ) =>
                {
                    _ = irBuilder.Alloca( null, ctx.CreateConstant( 2 ) );
                }
            );
            Assert.AreEqual( "typeRef", argNullEx.ParamName );

            argNullEx = Assert.ThrowsExactly<ArgumentNullException>(
                ( ) =>
                {
                    _ = irBuilder.Alloca( ctx.DoubleType, (Values.ConstantInt)null );
                }
            );
            Assert.AreEqual( "elements", argNullEx.ParamName );

            Assert.ThrowsExactly<InvalidOperationException>(
                ( ) =>
                {
                    _ = irBuilder.Alloca( ctx.DoubleType, ctx.CreateConstant(2) );
                }
                , "no data layout attached should generate an exception"
            );
        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        public void Alloca_reflects_correct_element_type( )
        {
            using var ctx = new Context();
            var voidFuncSig = ctx.GetFunctionType(ctx.VoidType);
            using var module = ctx.CreateBitcodeModule();
            var func = module.CreateFunction("testfunc"u8, voidFuncSig);
            var testBlock = func.AppendBasicBlock("testBlock"u8);

            using var irBuilder = new InstructionBuilder(testBlock);
            Alloca alloca = irBuilder.Alloca(ctx.Int128Type);
            Assert.AreEqual( testBlock, alloca.ContainingBlock );
            Assert.IsFalse( alloca.HasDebugRecords );
            Assert.IsInstanceOfType<IPointerType>( alloca.NativeType );

            Assert.AreEqual( ctx.Int128Type, alloca.ElementType );
            Assert.AreEqual( 1, alloca.ElementCount );
            Assert.IsFalse( alloca.IsArrayAllocation );

            const Int64 arraySize = 3;
            alloca = irBuilder.Alloca(ctx.Int128Type, ctx.CreateConstant(arraySize));
            Assert.AreEqual( testBlock, alloca.ContainingBlock );
            Assert.IsFalse( alloca.HasDebugRecords );
            Assert.IsInstanceOfType<IPointerType>( alloca.NativeType );

            Assert.AreEqual( ctx.Int128Type, alloca.ElementType );
            Assert.AreEqual( arraySize, alloca.ElementCount );
            Assert.IsTrue( alloca.IsArrayAllocation );
        }

        [TestMethod]
        public void Alloca_with_address_space_reflects_correct_element_type( )
        {
            using var ctx = new Context();
            var voidFuncSig = ctx.GetFunctionType(ctx.VoidType);
            using var module = ctx.CreateBitcodeModule();
            var func = module.CreateFunction("testfunc"u8, voidFuncSig);
            var testBlock = func.AppendBasicBlock("testBlock"u8);

            const UInt32 testAddressSpace = 256;
            using var irBuilder = new InstructionBuilder(testBlock);
            Alloca alloca = irBuilder.Alloca(ctx.Int128Type, testAddressSpace);
            Assert.AreEqual( testBlock, alloca.ContainingBlock );
            Assert.IsFalse( alloca.HasDebugRecords );
            Assert.IsInstanceOfType<IPointerType>( alloca.NativeType );
            Assert.AreEqual(testAddressSpace, ((IPointerType)alloca.NativeType).AddressSpace );

            Assert.AreEqual( ctx.Int128Type, alloca.ElementType );
            Assert.AreEqual( 1, alloca.ElementCount );
            Assert.IsFalse( alloca.IsArrayAllocation );

            const Int64 arraySize = 3;
            alloca = irBuilder.Alloca(ctx.Int128Type, ctx.CreateConstant(arraySize), testAddressSpace);
            Assert.IsInstanceOfType<IPointerType>( alloca.NativeType );
            Assert.AreEqual(testAddressSpace, ((IPointerType)alloca.NativeType).AddressSpace );
            Assert.AreEqual( testBlock, alloca.ContainingBlock );
            Assert.IsFalse( alloca.HasDebugRecords );

            Assert.AreEqual( ctx.Int128Type, alloca.ElementType );
            Assert.AreEqual( arraySize, alloca.ElementCount );
            Assert.IsTrue( alloca.IsArrayAllocation );
        }
    }
}
