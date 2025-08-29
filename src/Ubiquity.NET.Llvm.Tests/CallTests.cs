// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class CallTests
    {
        [TestMethod]
        [TestCategory( "Instruction" )]
        public void Create_varargs_function_with_arbitrary_params_succeeds( )
        {
            using var ctx = new Context();
            using var module = ctx.CreateBitcodeModule();
            var varArgsSig = ctx.GetFunctionType(true, ctx.VoidType, []);
            var callerSig = ctx.GetFunctionType(returnType: ctx.VoidType);

            var function = module.CreateFunction( "VarArgFunc", varArgsSig );
            var entryBlock = function.PrependBasicBlock("entry");
            using(var bldr = new InstructionBuilder( entryBlock ))
            {
                bldr.Return();
            }

            var caller = module.CreateFunction("CallVarArgs", callerSig);
            entryBlock = caller.PrependBasicBlock( "entry" );

            using var bldr2 = new InstructionBuilder( entryBlock );
            var callInstruction = bldr2.Call( function, ctx.CreateConstant( 0 ), ctx.CreateConstant( 1 ), ctx.CreateConstant( 2 ) );
            Assert.IsNotNull( callInstruction );

            // operands of call are all arguments, plus the target function
            Assert.AreEqual( 4, callInstruction.Operands.Count );
            Assert.AreEqual( 0UL, callInstruction.Operands.GetOperand<ConstantInt>( 0 )!.ZeroExtendedValue );
            Assert.AreEqual( 1UL, callInstruction.Operands.GetOperand<ConstantInt>( 1 )!.ZeroExtendedValue );
            Assert.AreEqual( 2UL, callInstruction.Operands.GetOperand<ConstantInt>( 2 )!.ZeroExtendedValue );
        }

        [TestMethod]
        [TestCategory( "Instruction" )]
        public void Create_indirect_function_call_succeeds( )
        {
            using var ctx = new Context();
            using var module = ctx.CreateBitcodeModule();
            var voidType = module.Context.VoidType;
            var voidPtrType = voidType.CreatePointerType();

            // Func sig: void (*)(int32_t, double);
            var targetSig = ctx.GetFunctionType(returnType: voidType, ctx.Int32Type, ctx.DoubleType);
            var callerSig = ctx.GetFunctionType(returnType: voidType, voidPtrType);
            var caller = module.CreateFunction("CallIndirect"u8, callerSig);

            using var bldr = new InstructionBuilder( caller.PrependBasicBlock( "entry" ) );

            var callInstruction = bldr.Call( targetSig, caller.Parameters[0], ctx.CreateConstant(1), ctx.CreateConstant(2.0));
            bldr.Return();
            Assert.IsNotNull( callInstruction );
            Assert.IsTrue(module.Verify(out string? msg), msg);
        }
    }
}
