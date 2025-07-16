// -----------------------------------------------------------------------
// <copyright file="ILibLLVM.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// This is cribbed from the interop library to prevent the need for applications to take a direct dependency on the interop library
using System.Collections.Immutable;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Code gen target to register/initialize</summary>
    public enum CodeGenTarget
    {
        /// <summary>Default invalid target; No/None is never a valid target</summary>
        None = LibLLVMCodeGenTarget.CodeGenTarget_None,

        /// <summary>Native target of the host system, generally used for JIT execution</summary>
        Native = LibLLVMCodeGenTarget.CodeGenTarget_Native,

        /// <summary>ARM AArch64 target</summary>
        AArch64 = LibLLVMCodeGenTarget.CodeGenTarget_AArch64,

        /// <summary>AMD GPU target</summary>
        AMDGPU = LibLLVMCodeGenTarget.CodeGenTarget_AMDGPU,

        /// <summary>ARM 32 bit targets</summary>
        ARM = LibLLVMCodeGenTarget.CodeGenTarget_ARM,

        /// <summary>Berkeley Packet Filter (BPF) target</summary>
        BPF = LibLLVMCodeGenTarget.CodeGenTarget_BPF,

        /// <summary>QUALCOMM Hexagon DSP/NPU family</summary>
        Hexagon = LibLLVMCodeGenTarget.CodeGenTarget_Hexagon,

        /// <summary>Un[der]documented Google (Myricom) processor</summary>
        /// <seealso href="https://q3k.org/lanai.html"/>
        Lanai = LibLLVMCodeGenTarget.CodeGenTarget_Lanai,

        /// <summary>MIPS target</summary>
        MIPS = LibLLVMCodeGenTarget.CodeGenTarget_MIPS,

        /// <summary>TI MSP430 Mixed-signal micro-controller</summary>
        MSP430 = LibLLVMCodeGenTarget.CodeGenTarget_MSP430,

        /// <summary>Nvidia Parallel Thread Execution (Nvidia GPUs)</summary>
        NVPTX = LibLLVMCodeGenTarget.CodeGenTarget_NVPTX,

        /// <summary>PowerPC target</summary>
        PowerPC = LibLLVMCodeGenTarget.CodeGenTarget_PowerPC,

        /// <summary>RISC-V target</summary>
        RISCV = LibLLVMCodeGenTarget.CodeGenTarget_RISCV,

        /// <summary>Sparc target</summary>
        Sparc = LibLLVMCodeGenTarget.CodeGenTarget_Sparc,

        /// <summary>Standard Portable Intermediate Representation [Generic GPU][Vulcan and later DirectX]</summary>
        /// <seealso href="https://en.wikipedia.org/wiki/Standard_Portable_Intermediate_Representation"/>
        SPIRV = LibLLVMCodeGenTarget.CodeGenTarget_SPIRV,

        /// <summary>z/Architecture (IBM 64 bit CISC)</summary>
        /// <seealso href="https://en.wikipedia.org/wiki/Z/Architecture"/>
        SystemZ = LibLLVMCodeGenTarget.CodeGenTarget_SystemZ,

        /// <summary>NEC's Vector Engine</summary>
        VE = LibLLVMCodeGenTarget.CodeGenTarget_VE,

        /// <summary>WebAssembly target</summary>
        WebAssembly = LibLLVMCodeGenTarget.CodeGenTarget_WebAssembly,

        /// <summary>X86 target</summary>
        X86 = LibLLVMCodeGenTarget.CodeGenTarget_X86,

        /// <summary>XMOS core</summary>
        /// <seealso href="https://en.wikipedia.org/wiki/XMOS"/>
        XCore,

        /// <summary>All available targets</summary>
        All = int.MaxValue
    }

    /// <summary>Target tools to register/enable</summary>
    [Flags]
    public enum TargetRegistration
    {
        /// <summary>Register nothing</summary>
        None = LibLLVMTargetRegistrationKind.TargetRegistration_None,

        /// <summary>Register the Target class</summary>
        Target = LibLLVMTargetRegistrationKind.TargetRegistration_Target,

        /// <summary>Register the Target info for the target</summary>
        TargetInfo = LibLLVMTargetRegistrationKind.TargetRegistration_TargetInfo,

        /// <summary>Register the target machine(s) for a target</summary>
        TargetMachine = LibLLVMTargetRegistrationKind.TargetRegistration_TargetMachine,

        /// <summary>Registers the assembly source code generator for a target</summary>
        AsmPrinter = LibLLVMTargetRegistrationKind.TargetRegistration_AsmPrinter,

        /// <summary>Registers the Disassembler for a target</summary>
        Disassembler = LibLLVMTargetRegistrationKind.TargetRegistration_Disassembler,

        /// <summary>Registers the assembly source parser for a target</summary>
        AsmParser = LibLLVMTargetRegistrationKind.TargetRegistration_AsmParser,

        /// <summary>Registers all the code generation components</summary>
        CodeGen = Target | TargetInfo | TargetMachine,

        /// <summary>Registers all components</summary>
        All = CodeGen | AsmPrinter | Disassembler | AsmParser
    }

    /// <summary>Interface to the core LLVM library itself</summary>
    /// <remarks>
    /// When this instance is disposed the LLVM libraries are no longer usable in the process
    /// <note type="important">
    /// It is important to note that the LLVM library does NOT currently support re-initialization in
    /// the same process. Therefore, it is recommended that initialization is done once at process startup
    /// and then the resulting interface disposed just before the process exits.
    /// </note>
    /// </remarks>
    public interface ILibLlvm
        : IDisposable
    {
        /// <summary>Registers components for ARM AArch64 target(s)</summary>
        /// <param name="target">Target architecture to register/initialize</param>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        void RegisterTarget( CodeGenTarget target, TargetRegistration registrations = TargetRegistration.All );

        /// <summary>Gets the supported targets for this library</summary>
        /// <remarks>
        /// This is a simple set of enumerated values for the known targets supported by the library. It
        /// is distinct from the registered targets. Registration of each top level enumerated target may indeed
        /// register support for more targets (e.g., ARM includes thumb big and little endian targets).
        /// </remarks>
        ImmutableArray<CodeGenTarget> SupportedTargets { get; }

        /// <summary>Gets a map of all known attributes to this build</summary>
        /// <remarks>
        /// This map includes all enumerated attributes AND all well-known string attributes. Additional
        /// string attributes are always valid as various passes and target machines may use custom
        /// attributes not yet known to, or considered stable by, the LLVM core native code.
        /// </remarks>
        ImmutableDictionary<LazyEncodedString, AttributeInfo> AttributeMap { get; }

        /// <summary>Gets the current debug metadata version for this library</summary>
        uint DebugMetadataVersion { get; }
    }
}
