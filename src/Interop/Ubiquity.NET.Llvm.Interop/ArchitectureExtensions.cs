// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Utility class to implement extensions of <see cref="Architecture"/></summary>
    public static class ArchitectureExtensions
    {
        /// <summary>Converts an <see cref="Architecture"/> to a <see cref="LibLLVMCodeGenTarget"/></summary>
        /// <param name="arch">Value to convert</param>
        /// <returns>LLVM code generation target</returns>
        /// <exception cref="NotSupportedException">Native LLVM form of target is unknown</exception>
        public static LibLLVMCodeGenTarget AsLLVMTarget( this Architecture arch )
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
#if NET9_0_OR_GREATER
                Architecture.RiscV64 => LibLLVMCodeGenTarget.CodeGenTarget_RISCV, // 64 vs 32 bit distinction is a CPU/Feature of the target
#endif
                Architecture.S390x => LibLLVMCodeGenTarget.CodeGenTarget_SystemZ,
                _ => throw new NotSupportedException( "Native code gen target is unknown" )
            };
        }
    }
}
