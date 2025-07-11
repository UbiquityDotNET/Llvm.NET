// -----------------------------------------------------------------------
// <copyright file="ArchitectureExtensionsTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class ArchitectureExtensionsTests
    {
        [TestMethod]
        [TestProperty( "Description", "Validates the Ubiquity.NET.Llvm.Interop.ArchitectureExtensions.AsLLVMTarget() extension method" )]
        public void GetLibLLVMTargetTest( )
        {
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_ARM, Architecture.Arm.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_AArch64, Architecture.Arm64.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_ARM, Architecture.Armv6.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_LoongArch, Architecture.LoongArch64.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_PowerPC, Architecture.Ppc64le.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_RISCV, Architecture.RiscV64.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_SystemZ, Architecture.S390x.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_WebAssembly, Architecture.Wasm.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_X86, Architecture.X64.AsLLVMTarget() );
            Assert.AreEqual( LibLLVMCodeGenTarget.CodeGenTarget_X86, Architecture.X86.AsLLVMTarget() );
        }
    }
}
