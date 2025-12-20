// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.UT.Instructions
{
    [TestClass]
    public class PointerCasts
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [TestMethod]
        public void IntToPointer_throws_with_invalid_input( )
        {
            using var ctx = new Context();
            using var module = ctx.CreateBitcodeModule("test"u8);
            var doulbeFuncType = ctx.GetFunctionType(ctx.DoubleType);
            var doubleFunc = module.CreateFunction("test"u8, doulbeFuncType);
            var block = ctx.CreateBasicBlock("testBlock"u8);

            using var irBuilder = new InstructionBuilder(block);
            Value nonConstValue = irBuilder.Call(doubleFunc);
            Value nonIntConstantValue = ctx.CreateConstant(1.23);

            var ptrBoolType = ctx.BoolType.CreatePointerType();
            var constInt = ctx.CreateConstant(0x1234u);
            var argNullEx = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                _ = irBuilder.IntToPointer( null, ptrBoolType );
            });
            Assert.AreEqual( "intValue", argNullEx.ParamName);

            argNullEx = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                _ = irBuilder.IntToPointer( constInt, null );
            });
            Assert.AreEqual( "ptrType", argNullEx.ParamName );

            var argEx = Assert.ThrowsExactly<ArgumentException>( ( ) =>
            {
                _ = irBuilder.IntToPointer( nonIntConstantValue, ptrBoolType );
            } );
            Assert.AreEqual( "intValue", argEx.ParamName );

            argEx = Assert.ThrowsExactly<ArgumentException>( ( ) =>
            {
                _ = irBuilder.IntToPointer( nonConstValue, ptrBoolType );
            } );
            Assert.AreEqual( "intValue", argEx.ParamName );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
