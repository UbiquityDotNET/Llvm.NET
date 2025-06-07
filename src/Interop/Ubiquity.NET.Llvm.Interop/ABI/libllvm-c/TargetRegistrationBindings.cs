// -----------------------------------------------------------------------
// <copyright file="TargetRegistrationBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public enum LibLLVMCodeGenTarget
    {
        CodeGenTarget_None = 0,    // Invalid value
        CodeGenTarget_Native,      // Generic value for the native architecture of the current runtime (generally used for local JIT execution)
        CodeGenTarget_AArch64,     // ARM 64 bit Architecture
        CodeGenTarget_AMDGPU,      // AMD GPUs
        CodeGenTarget_ARM,         // ARM 32 bit (including thumb mode)
        CodeGenTarget_AVR,         // Atmel AVR Micro controller
        CodeGenTarget_BPF,         // Berkeley Packet Filter (Including eBPF)
        CodeGenTarget_Hexagon,     // Qualcom Hexagon DSP/NPU family
        CodeGenTarget_Lanai,       // Un[der]documented Google (Myricom) processor (see: https://q3k.org/lanai.html)
        CodeGenTarget_LoongArch,   // Loongson Custom ISA CPU (see: https://en.wikipedia.org/wiki/Loongson)
        CodeGenTarget_MIPS,        // MIPS based CPU
        CodeGenTarget_MSP430,      // TI MSP430 Mixed-signal microcontroller
        CodeGenTarget_NVPTX,       // Nvidia Parallel Thread Execution (Nvidia GPUs)
        CodeGenTarget_PowerPC,     // Apple/IBM/Motorola CPU
        CodeGenTarget_RISCV,       // Open Source RISC Architecture
        CodeGenTarget_Sparc,       // Sun Microsystems SPARC CPU
        CodeGenTarget_SPIRV,       // Standard Portable Intermediate Representation (see: https://en.wikipedia.org/wiki/Standard_Portable_Intermediate_Representation)
        CodeGenTarget_SystemZ,     // z/Architecture (IBM 64 bit CISC) (see: https://en.wikipedia.org/wiki/Z/Architecture)
        CodeGenTarget_VE,          // NEC's Vector Engine
        CodeGenTarget_WebAssembly, // Browser interpreted/JIT execution
        CodeGenTarget_X86,         // Intel X86 and AMD64
        CodeGenTarget_XCore,       // XMOS core (see: https://en.wikipedia.org/wiki/XMOS)
        CodeGenTarget_All = int.MaxValue
    }

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "It has one, just not the name the analyzer likes..." )]
    public enum LibLLVMTargetRegistrationKind
    {
        TargetRegistration_None = 0x00,
        TargetRegistration_Target = 0x01,
        TargetRegistration_TargetInfo = 0x02,
        TargetRegistration_TargetMachine = 0x04,
        TargetRegistration_AsmPrinter = 0x08,
        TargetRegistration_Disassembler = 0x10,
        TargetRegistration_AsmParser = 0x20,
        TargetRegistration_CodeGen = TargetRegistration_Target | TargetRegistration_TargetInfo | TargetRegistration_TargetMachine,
        TargetRegistration_All = TargetRegistration_CodeGen | TargetRegistration_AsmPrinter | TargetRegistration_Disassembler | TargetRegistration_AsmParser
    }

    public static partial class TargetRegistrationBindings
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMRegisterTarget( LibLLVMCodeGenTarget target, LibLLVMTargetRegistrationKind registrations );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial Int32 LibLLVMGetNumTargets( );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMErrorRef LibLLVMGetRuntimeTargets( LibLLVMCodeGenTarget[] targets )
        {
            return LibLLVMGetRuntimeTargets( targets, targets.Length );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMErrorRef LibLLVMGetRuntimeTargets(
            [Out, In]
            LibLLVMCodeGenTarget[] targets, Int32 lengthOfArray
        );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LibLLVMGetVersion( );
    }
}
