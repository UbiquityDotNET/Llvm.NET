using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class ArchitectureExtensionsTests
    {
        [TestMethod]
        public void GetLibLLVMTargetTest( )
        {
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_ARM, Architecture.Arm.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_AArch64, Architecture.Arm64.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_ARM, Architecture.Armv6.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_LoongArch, Architecture.LoongArch64.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_PowerPC, Architecture.Ppc64le.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_RISCV, Architecture.RiscV64.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_SystemZ, Architecture.S390x.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_WebAssembly, Architecture.Wasm.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_X86, Architecture.X64.GetLibLLVMTarget());
            Assert.AreEqual(LibLLVMCodeGenTarget.CodeGenTarget_X86, Architecture.X86.GetLibLLVMTarget());
        }
    }
}
