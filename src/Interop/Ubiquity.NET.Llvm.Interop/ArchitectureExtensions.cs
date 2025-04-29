// <copyright file="ArchitectureExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

namespace Ubiquity.NET.Llvm.Interop
{
    public static class ArchitectureExtensions
    {
        public static LibLLVMCodeGenTarget GetLibLLVMTarget( this Architecture arch)
        {
            return arch switch
            {
                Architecture.X86 or
                Architecture.X64 => LibLLVMCodeGenTarget.CodeGenTarget_X86, // 64 vs 32 bit distinction is a CPU/feature of the target
                Architecture.Arm or
                Architecture.Armv6 => LibLLVMCodeGenTarget.CodeGenTarget_ARM, // Distinction is a CPU/Feature of the target
                Architecture.Arm64 => LibLLVMCodeGenTarget.CodeGenTarget_AArch64,
                Architecture.Wasm => LibLLVMCodeGenTarget.CodeGenTarget_WebAssembly,
                Architecture.LoongArch64 => LibLLVMCodeGenTarget.CodeGenTarget_LoongArch,
                Architecture.Ppc64le => LibLLVMCodeGenTarget.CodeGenTarget_PowerPC,
                Architecture.RiscV64 => LibLLVMCodeGenTarget.CodeGenTarget_RISCV, // 64 vs 32 bit distinction is a CPU/Feature of the target
                Architecture.S390x => LibLLVMCodeGenTarget.CodeGenTarget_SystemZ,
                _ => throw new NotSupportedException( "Native code gen target is unknown" )
            };
        }
    }
}
