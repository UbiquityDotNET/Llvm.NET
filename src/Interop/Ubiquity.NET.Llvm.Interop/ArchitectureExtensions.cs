// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly, marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be supressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx) don't support the new syntax yet and it isn't clear if they will in the future.
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

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
                Architecture.RiscV64 => LibLLVMCodeGenTarget.CodeGenTarget_RISCV, // 64bit is the only form supported by .NET
#endif
                Architecture.S390x => LibLLVMCodeGenTarget.CodeGenTarget_SystemZ,
                _ => throw new NotSupportedException( "Native code gen target is unknown" )
            };
        }
    }
}
