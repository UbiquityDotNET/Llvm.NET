// -----------------------------------------------------------------------
// <copyright file="CallTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
