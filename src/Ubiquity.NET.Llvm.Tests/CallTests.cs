﻿// -----------------------------------------------------------------------
// <copyright file="CallTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Tests
{
    [TestClass]
    public class CallTests
    {
        [TestMethod]
        [TestCategory( "Instruction" )]
        public void Create_varargs_function_with_arbitrary_params_succeeds()
        {
            using var ctx = new Context();
            var module = ctx.CreateBitcodeModule();
            var varArgsSig = ctx.GetFunctionType(ctx.VoidType, Enumerable.Empty<ITypeRef>(), true);
            var callerSig = ctx.GetFunctionType(ctx.VoidType);

            var function = module.CreateFunction( "VarArgFunc", varArgsSig );
            var entryBlock = function.PrependBasicBlock("entry");
            var bldr = new InstructionBuilder(entryBlock);
            bldr.Return( );

            var caller = module.CreateFunction("CallVarArgs", callerSig);
            entryBlock = caller.PrependBasicBlock( "entry" );
            bldr = new InstructionBuilder( entryBlock );
            var callInstruction = bldr.Call( function, ctx.CreateConstant( 0 ), ctx.CreateConstant( 1 ), ctx.CreateConstant( 2 ) );
            Assert.IsNotNull( callInstruction );

            // operands of call are all arguments, plus the target function
            Assert.AreEqual( 4, callInstruction.Operands.Count );
            Assert.AreEqual( 0UL, callInstruction.Operands.GetOperand<ConstantInt>( 0 )!.ZeroExtendedValue );
            Assert.AreEqual( 1UL, callInstruction.Operands.GetOperand<ConstantInt>( 1 )!.ZeroExtendedValue );
            Assert.AreEqual( 2UL, callInstruction.Operands.GetOperand<ConstantInt>( 2 )!.ZeroExtendedValue );
        }
    }
}
