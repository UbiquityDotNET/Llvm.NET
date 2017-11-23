// <copyright file="Enumerations.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Native;

// TEMP: disable this until all values are properly doc'd
#pragma warning disable SA1602 // Enumeration items must be documented

namespace Llvm.NET
{
    /// <summary>Enumeration to indicate the behavior of module level flags metadata sharing the same name in a <see cref="BitcodeModule"/></summary>
    [SuppressMessage( "Microsoft.Naming"
                    , "CA1726:UsePreferredTerms"
                    , MessageId = "Flag"
                    , Justification = "Enum for the behavior of the LLVM ModuleFlag (Flag in middle doesn't imply the enum is Bit Flags)" )]
    public enum ModuleFlagBehavior
    {
        /// <summary>Invalid value (default value for this enumeration)</summary>
        Invalid = 0,

        /// <summary>Emits an error if two values disagree, otherwise the resulting value is that of the operands</summary>
        Error = LLVMModFlagBehavior.Error,

        /// <summary>Emits a warning if two values disagree. The result will be the operand for the flag from the first module being linked</summary>
        Warning = LLVMModFlagBehavior.Warning,

        /// <summary>Adds a requirement that another module flag be present and have a specified value after linking is performed</summary>
        /// <remarks>
        /// The value must be a metadata pair, where the first element of the pair is the ID of the module flag to be restricted, and the
        /// second element of the pair is the value the module flag should be restricted to. This behavior can be used to restrict the
        /// allowable results (via triggering of an error) of linking IDs with the <see cref="Override"/> behavior
        /// </remarks>
        Require = LLVMModFlagBehavior.Require,

        /// <summary>Uses the specified value, regardless of the behavior or value of the other module</summary>
        /// <remarks>If both modules specify Override, but the values differ, and error will be emitted</remarks>
        Override = LLVMModFlagBehavior.Override,

        /// <summary>Appends the two values, which are required to be metadata nodes</summary>
        Append = LLVMModFlagBehavior.Append,

        /// <summary>Appends the two values, which are required to be metadata nodes dropping duplicate entries in the second list</summary>
        AppendUnique = LLVMModFlagBehavior.AppendUnique,

        /// <summary>Takes the max of the two values, which are required to be integers</summary>
        Max = 7 /*LLVMModFlagBehavior.Max*/
    }

    /// <summary>LLVM Instruction opcodes</summary>
    /// <remarks>
    /// These are based on the "C" API and therefore more stable as changes in the underlying instruction ids are remapped in the C API layer
    /// </remarks>
    [SuppressMessage( "Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not actually flags" )]
    public enum OpCode
    {
        Invalid = 0,
        /* Terminator Instructions */
        Return = LLVMOpcode.LLVMRet,
        Branch = LLVMOpcode.LLVMBr,
        Switch = LLVMOpcode.LLVMSwitch,
        IndirectBranch = LLVMOpcode.LLVMIndirectBr,
        Invoke = LLVMOpcode.LLVMInvoke,
        Unreachable = LLVMOpcode.LLVMUnreachable,

        /* Standard Binary Operators */
        Add = LLVMOpcode.LLVMAdd,
        FAdd = LLVMOpcode.LLVMFAdd,
        Sub = LLVMOpcode.LLVMSub,
        FSub = LLVMOpcode.LLVMFSub,
        Mul = LLVMOpcode.LLVMMul,
        FMul = LLVMOpcode.LLVMFMul,
        UDiv = LLVMOpcode.LLVMUDiv,
        SDiv = LLVMOpcode.LLVMSDiv,
        FDiv = LLVMOpcode.LLVMFDiv,
        URem = LLVMOpcode.LLVMURem,
        SRem = LLVMOpcode.LLVMSRem,
        FRem = LLVMOpcode.LLVMFRem,

        /* Logical Operators */
        Shl = LLVMOpcode.LLVMShl,
        LShr = LLVMOpcode.LLVMLShr,
        AShr = LLVMOpcode.LLVMAShr,
        And = LLVMOpcode.LLVMAnd,
        Or = LLVMOpcode.LLVMOr,
        Xor = LLVMOpcode.LLVMXor,

        /* Memory Operators */
        Alloca = LLVMOpcode.LLVMAlloca,
        Load = LLVMOpcode.LLVMLoad,
        Store = LLVMOpcode.LLVMStore,
        GetElementPtr = LLVMOpcode.LLVMGetElementPtr,

        /* Cast Operators */
        Trunc = LLVMOpcode.LLVMTrunc,
        ZeroExtend = LLVMOpcode.LLVMZExt,
        SignExtend = LLVMOpcode.LLVMSExt,
        FPToUI = LLVMOpcode.LLVMFPToUI,
        FPToSI = LLVMOpcode.LLVMFPToSI,
        UIToFP = LLVMOpcode.LLVMUIToFP,
        SIToFP = LLVMOpcode.LLVMSIToFP,
        FPTrunc = LLVMOpcode.LLVMFPTrunc,
        FPExt = LLVMOpcode.LLVMFPExt,
        PtrToInt = LLVMOpcode.LLVMPtrToInt,
        IntToPtr = LLVMOpcode.LLVMIntToPtr,
        BitCast = LLVMOpcode.LLVMBitCast,
        AddrSpaceCast = LLVMOpcode.LLVMAddrSpaceCast,

        /* Other Operators */
        ICmp = LLVMOpcode.LLVMICmp,
        FCmp = LLVMOpcode.LLVMFCmp,
        Phi = LLVMOpcode.LLVMPHI,
        Call = LLVMOpcode.LLVMCall,
        Select = LLVMOpcode.LLVMSelect,
        UserOp1 = LLVMOpcode.LLVMUserOp1,
        UserOp2 = LLVMOpcode.LLVMUserOp2,
        VaArg = LLVMOpcode.LLVMVAArg,
        ExtractElement = LLVMOpcode.LLVMExtractElement,
        InsertElement = LLVMOpcode.LLVMInsertElement,
        ShuffleVector = LLVMOpcode.LLVMShuffleVector,
        ExtractValue = LLVMOpcode.LLVMExtractValue,
        InsertValue = LLVMOpcode.LLVMInsertValue,

        /* Atomic operators */
        Fence = LLVMOpcode.LLVMFence,
        AtomicCmpXchg = LLVMOpcode.LLVMAtomicCmpXchg,
        AtomicRMW = LLVMOpcode.LLVMAtomicRMW,

        /* Exception Handling Operators */
        Resume = LLVMOpcode.LLVMResume,
        LandingPad = LLVMOpcode.LLVMLandingPad,
        CleanupRet = LLVMOpcode.LLVMCleanupRet,
        CatchRet = LLVMOpcode.LLVMCatchRet,
        CatchPad = LLVMOpcode.LLVMCatchPad,
        CleanupPad = LLVMOpcode.LLVMCleanupPad,
        CatchSwitch = LLVMOpcode.LLVMCatchSwitch
    }

    /// <summary>Basic kind of a type</summary>
    public enum TypeKind
    {
        /// <summary>Type with no size</summary>
        Void = LLVMTypeKind.LLVMVoidTypeKind,

        /// <summary>16 bit floating point type</summary>
        Float16 = LLVMTypeKind.LLVMHalfTypeKind,

        /// <summary>32 bit floating point type</summary>
        Float32 = LLVMTypeKind.LLVMFloatTypeKind,

        /// <summary>64 bit floating point type</summary>
        Float64 = LLVMTypeKind.LLVMDoubleTypeKind,

        /// <summary>80 bit floating point type (X87)</summary>
        X86Float80 = LLVMTypeKind.LLVMX86_FP80TypeKind,

        /// <summary>128 bit floating point type (112-bit mantissa)</summary>
        Float128m112 = LLVMTypeKind.LLVMFP128TypeKind,

        /// <summary>128 bit floating point type (two 64-bits)</summary>
        Float128 = LLVMTypeKind.LLVMPPC_FP128TypeKind,

        /// <summary><see cref="Llvm.NET.Values.BasicBlock"/> instruction label</summary>
        Label = LLVMTypeKind.LLVMLabelTypeKind,

        /// <summary>Arbitrary bit width integers</summary>
        Integer = LLVMTypeKind.LLVMIntegerTypeKind,

        /// <summary><see cref="Llvm.NET.Types.IFunctionType"/></summary>
        Function = LLVMTypeKind.LLVMFunctionTypeKind,

        /// <summary><see cref="Llvm.NET.Types.IStructType"/></summary>
        Struct = LLVMTypeKind.LLVMStructTypeKind,

        /// <summary><see cref="Llvm.NET.Types.IArrayType"/></summary>
        Array = LLVMTypeKind.LLVMArrayTypeKind,

        /// <summary><see cref="Llvm.NET.Types.IPointerType"/></summary>
        Pointer = LLVMTypeKind.LLVMPointerTypeKind,

        /// <summary>SIMD 'packed' format, or other <see cref="Llvm.NET.Types.IVectorType"/> implementation</summary>
        Vector = LLVMTypeKind.LLVMVectorTypeKind,

        /// <summary><see cref="Llvm.NET.LlvmMetadata"/></summary>
        Metadata = LLVMTypeKind.LLVMMetadataTypeKind,

        /// <summary>x86 MMX data type</summary>
        X86MMX = LLVMTypeKind.LLVMX86_MMXTypeKind,

        /// <summary>Exception handler token</summary>
        Token = LLVMTypeKind.LLVMTokenTypeKind
    }

    /* valuse for this enum come directly from LLVM's CallingConv.h
    // rather then the mapped C API version as the C version is not
    // a complete set.
    */

    /// <summary>Calling Convention for functions</summary>
    public enum LlvmCallingConvention
    {
        C = 0,

        // [gap]
        FastCall = 8,
        ColdCall = 9,
        GlasgowHaskellCompiler = 10,
        HiPE = 11,
        WebKitJS = 12,
        AnyReg = 13,
        PreserveMost = 14,
        PreserveAll = 15,
        Swift = 16,
        CxxFastTls = 17,

        // [Gap]
        FirstTargetSPecific = 64, // [marker]
        X86StdCall = 64,
        X86FastCall = 65,
        ArmAPCS = 66, // Generally considered obsolete but some older targets use this
        ArmAAPCS = 67,
        ArmAAPCSVfp = 68,
        MSP430Interrupt = 69,
        X86ThisCall = 70,
        PtxKernel = 71,
        PtxDevice = 72,
        SpirFunction = 75,
        SpirKernel = 76,
        IntelOpenCLBuiltIn = 77,
        X86x64SysV = 78,
        X86x64Win64 = 79,
        X86VectorCall = 80,
        HHVM = 81,
        HHVMCCall = 82,
        X86Interrupt = 83,
        AVRInterrupt = 84,
        AVRSignal = 85,
        AVRBuiltIn = 86,
        AMDGpuVetexShader = 87,
        AMDGpuGeometryShader = 88,
        AMDGpuPixelShader = 89,
        AMDGpuComputeShader = 90,
        AMDGpuKernel = 91,
        X86RegCall = 92,
        AMDGpuHullShader = 93,
        MSP430BuiltIn = 94,
        MaxCallingConvention = 1023
    }

    /// <summary>Linkage specification for functions and globals</summary>
    public enum Linkage
    {
        External = LLVMLinkage.LLVMExternalLinkage,    /*< Externally visible function */
        AvailableExternally = LLVMLinkage.LLVMAvailableExternallyLinkage,
        LinkOnceAny = LLVMLinkage.LLVMLinkOnceAnyLinkage, /*< Keep one copy of function when linking (inline)*/
        LinkOnceODR = LLVMLinkage.LLVMLinkOnceODRLinkage, /*< Same, but only replaced by something equivalent. */

        // LLVMLinkage.LLVMLinkOnceODRAutoHideLinkage, /**< Obsolete */
        Weak = LLVMLinkage.LLVMWeakAnyLinkage,     /*< Keep one copy of function when linking (weak) */
        WeakODR = LLVMLinkage.LLVMWeakODRLinkage,     /*< Same, but only replaced by something equivalent. */
        Append = LLVMLinkage.LLVMAppendingLinkage,   /*< Special purpose, only applies to global arrays */
        Internal = LLVMLinkage.LLVMInternalLinkage,    /*< Rename collisions when linking (static functions) */
        Private = LLVMLinkage.LLVMPrivateLinkage,     /*< Like Internal, but omit from symbol table */
        DllImport = LLVMLinkage.LLVMDLLImportLinkage,   /*< Function to be imported from DLL */
        DllExport = LLVMLinkage.LLVMDLLExportLinkage,   /*< Function to be accessible from DLL */
        ExternalWeak = LLVMLinkage.LLVMExternalWeakLinkage,/*< ExternalWeak linkage description */

        // LLVMLinkage.LLVMGhostLinkage,       /*< Obsolete */
        Common = LLVMLinkage.LLVMCommonLinkage,      /*< Tentative definitions */
        LinkerPrivate = LLVMLinkage.LLVMLinkerPrivateLinkage, /*< Like Private, but linker removes. */
        LinkerPrivateWeak = LLVMLinkage.LLVMLinkerPrivateWeakLinkage /*< Like LinkerPrivate, but is weak. */
    }

    /// <summary>Enumeration for the visibility of a global value</summary>
    public enum Visibility
    {
        Default = LLVMVisibility.LLVMDefaultVisibility,  /*< The GV is visible */
        Hidden = LLVMVisibility.LLVMHiddenVisibility,   /*< The GV is hidden */
        Protected = LLVMVisibility.LLVMProtectedVisibility /*< The GV is protected */
    }

    /// <summary>Unified predicate enumeration</summary>
    /// <remarks>
    /// Underneath the C API this is what LLVM uses. For some reason the C API
    /// split it into the integer and float predicate enumerations.
    /// </remarks>
    [SuppressMessage( "Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not flags and shouldn't be marked as such" )]
    public enum Predicate
    {
        False = LLVMRealPredicate.LLVMRealPredicateFalse,
        OrderedAndEqual = LLVMRealPredicate.LLVMRealOEQ,
        OrderedAndGreaterThan = LLVMRealPredicate.LLVMRealOGT,
        OrderedAndGreaterThanOrEqual = LLVMRealPredicate.LLVMRealOGE,
        OrderedAndLessThan = LLVMRealPredicate.LLVMRealOLT,
        OrderedAndLessThanOrEqual = LLVMRealPredicate.LLVMRealOLE,
        OrderedAndNotEqual = LLVMRealPredicate.LLVMRealONE,
        Ordered = LLVMRealPredicate.LLVMRealORD,
        Unordered = LLVMRealPredicate.LLVMRealUNO,
        UnorderedAndEqual = LLVMRealPredicate.LLVMRealUEQ,
        UnorderedOrGreaterThan = LLVMRealPredicate.LLVMRealUGT,
        UnorderedOrGreaterThanOrEqual = LLVMRealPredicate.LLVMRealUGE,
        UnorderedOrLessThan = LLVMRealPredicate.LLVMRealULT,
        UnorderedOrLessThanOrEqual = LLVMRealPredicate.LLVMRealULE,
        UnorderedOrNotEqual = LLVMRealPredicate.LLVMRealUNE,
        True = LLVMRealPredicate.LLVMRealPredicateTrue,
        FirstFcmpPredicate = False,
        LastFcmpPredicate = True,

        /// <summary>Any value Greater than or equal to this is not valid for Fcmp operations</summary>
        BadFcmpPredicate = LastFcmpPredicate + 1,

        Equal = LLVMIntPredicate.LLVMIntEQ,
        NotEqual = LLVMIntPredicate.LLVMIntNE,
        UnsignedGreater = LLVMIntPredicate.LLVMIntUGT,
        UnsignedGreaterOrEqual = LLVMIntPredicate.LLVMIntUGE,
        UnsignedLess = LLVMIntPredicate.LLVMIntULT,
        UnsignedLessOrEqual = LLVMIntPredicate.LLVMIntULE,
        SignedGreater = LLVMIntPredicate.LLVMIntSGT,
        SignedGreaterOrEqual = LLVMIntPredicate.LLVMIntSGE,
        SignedLess = LLVMIntPredicate.LLVMIntSLT,
        SignedLessOrEqual = LLVMIntPredicate.LLVMIntSLE,
        FirstIcmpPredicate = Equal,
        LastIcmpPredicate = SignedLessOrEqual,

        /// <summary>Any value Greater than or equal to this is not valid for Icmp operations</summary>
        BadIcmpPredicate = LastIcmpPredicate + 1
    }

    /// <summary>Predicate enumeration for integer comparison</summary>
    public enum IntPredicate
    {
        False = LLVMRealPredicate.LLVMRealPredicateFalse,
        Equal = LLVMIntPredicate.LLVMIntEQ,
        NotEqual = LLVMIntPredicate.LLVMIntNE,
        UnsignedGreater = LLVMIntPredicate.LLVMIntUGT,
        UnsignedGreaterOrEqual = LLVMIntPredicate.LLVMIntUGE,
        UnsignedLess = LLVMIntPredicate.LLVMIntULT,
        UnsignedLessOrEqual = LLVMIntPredicate.LLVMIntULE,
        SignedGreater = LLVMIntPredicate.LLVMIntSGT,
        SignedGreaterOrEqual = LLVMIntPredicate.LLVMIntSGE,
        SignedLess = LLVMIntPredicate.LLVMIntSLT,
        SignedLessOrEqual = LLVMIntPredicate.LLVMIntSLE
    }

    /// <summary>Predicate enumeration for integer comparison</summary>
    public enum RealPredicate
    {
        False = LLVMRealPredicate.LLVMRealPredicateFalse,
        OrderedAndEqual = LLVMRealPredicate.LLVMRealOEQ,
        OrderedAndGreaterThan = LLVMRealPredicate.LLVMRealOGT,
        OrderedAndGreaterThanOrEqual = LLVMRealPredicate.LLVMRealOGE,
        OrderedAndLessThan = LLVMRealPredicate.LLVMRealOLT,
        OrderedAndLessThanOrEqual = LLVMRealPredicate.LLVMRealOLE,
        OrderedAndNotEqual = LLVMRealPredicate.LLVMRealONE,
        Ordered = LLVMRealPredicate.LLVMRealORD,
        Unordered = LLVMRealPredicate.LLVMRealUNO,
        UnorderedAndEqual = LLVMRealPredicate.LLVMRealUEQ,
        UnorderedOrGreaterThan = LLVMRealPredicate.LLVMRealUGT,
        UnorderedOrGreaterThanOrEqual = LLVMRealPredicate.LLVMRealUGE,
        UnorderedOrLessThan = LLVMRealPredicate.LLVMRealULT,
        UnorderedOrLessThanOrEqual = LLVMRealPredicate.LLVMRealULE,
        UnorderedOrNotEqual = LLVMRealPredicate.LLVMRealUNE,
        True = LLVMRealPredicate.LLVMRealPredicateTrue
    }

    /// <summary>Optimization level for target code generation</summary>
    public enum CodeGenOpt
    {
        None = LLVMCodeGenOptLevel.LLVMCodeGenLevelNone,
        Less = LLVMCodeGenOptLevel.LLVMCodeGenLevelLess,
        Default = LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault,
        Aggressive = LLVMCodeGenOptLevel.LLVMCodeGenLevelAggressive
    }

    /// <summary>Relocation type for target code generation</summary>
    public enum Reloc
    {
        Default = LLVMRelocMode.LLVMRelocDefault,
        Static = LLVMRelocMode.LLVMRelocStatic,
        PositionIndependent = LLVMRelocMode.LLVMRelocPIC,
        Dynamic = LLVMRelocMode.LLVMRelocDynamicNoPic
    }

    /// <summary>Code model to use for target code generation</summary>
    public enum CodeModel
    {
        Default = LLVMCodeModel.LLVMCodeModelDefault,
        JitDefault = LLVMCodeModel.LLVMCodeModelJITDefault,
        Small = LLVMCodeModel.LLVMCodeModelSmall,
        Kernel = LLVMCodeModel.LLVMCodeModelKernel,
        Medium = LLVMCodeModel.LLVMCodeModelMedium,
        Large = LLVMCodeModel.LLVMCodeModelLarge
    }

    /// <summary>Output file type for target code generation</summary>
    public enum CodeGenFileType
    {
        AssemblySource = LLVMCodeGenFileType.LLVMAssemblyFile,
        ObjectFile = LLVMCodeGenFileType.LLVMObjectFile
    }

    /// <summary>Byte ordering for target code generation and data type layout</summary>
    public enum ByteOrdering
    {
        LittleEndian = LLVMByteOrdering.LLVMLittleEndian,
        BigEndian = LLVMByteOrdering.LLVMBigEndian
    }

    /// <summary>Function index for attributes</summary>
    /// <remarks>
    /// Attributes on functions apply to the function itself, the return type
    /// or one of the function's parameters. This enumeration is used to
    /// identify where the attribute applies.
    /// </remarks>
    public enum FunctionAttributeIndex
    {
        /// <summary>The attribute applies to the function itself</summary>
        Function = -1,

        /// <summary>The attribute applies to the return type of the function</summary>
        ReturnType = 0,

        /// <summary>The attribute applies to the first parameter of the function</summary>
        /// <remarks>
        /// Additional parameters can identified by simply adding an integer value to
        /// this value. (i.e. FunctionAttributeIndex.Parameter0 + 1 )
        /// </remarks>
        Parameter0 = 1
    }

    /// <summary>Enumeration for the Architecture portion of a target triple</summary>
    [SuppressMessage( "Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not actually flags" )]
    public enum TripleArchType
    {
        /// <summary>Invalid or unknown architecture</summary>
        UnknownArch = LLVMTripleArchType.UnknownArch,

        /// <summary>ARM (little endian): arm, armv.*, xscale</summary>
        Arm = LLVMTripleArchType.arm,

        /// <summary>ARM (big endian): armeb</summary>
        Armeb = LLVMTripleArchType.armeb,

        /// <summary>AArch64 (little endian): aarch64</summary>
        Aarch64 = LLVMTripleArchType.aarch64,

        /// <summary>AArch64 (big endian): aarch64_be</summary>
        Aarch64_be = LLVMTripleArchType.aarch64_be,

        /// <summary>AVR: Atmel AVR microcontroller</summary>
        Avr = LLVMTripleArchType.avr,

        /// <summary>eBPF or extended BPF or 64-bit BPF (little endian)</summary>
        BPFel = LLVMTripleArchType.bpfel,

        /// <summary>eBPF or extended BPF or 64-bit BPF (big endian)</summary>
        BPFeb = LLVMTripleArchType.bpfeb,

        /// <summary>Hexagon processor</summary>
        Hexagon = LLVMTripleArchType.hexagon,

        /// <summary>MIPS: mips, mipsallegrex</summary>
        MIPS = LLVMTripleArchType.mips,

        /// <summary>MIPSEL: mipsel, mipsallegrexel</summary>
        MIPSel = LLVMTripleArchType.mipsel,
        MIPS64 = LLVMTripleArchType.mips64,         // MIPS64: mips64
        MIPS64el = LLVMTripleArchType.mips64el,       // MIPS64EL: mips64el
        MSP430 = LLVMTripleArchType.msp430,         // MSP430: msp430
        PPC = LLVMTripleArchType.ppc,            // PPC: powerpc
        PPC64 = LLVMTripleArchType.ppc64,          // PPC64: powerpc64, ppu
        PPC64le = LLVMTripleArchType.ppc64le,        // PPC64LE: powerpc64le
        R600 = LLVMTripleArchType.r600,           // R600: AMD GPUs HD2XXX - HD6XXX
        AMDGCN = LLVMTripleArchType.amdgcn,         // AMDGCN: AMD GCN GPUs
        RiscV32 = LLVMTripleArchType.riscV32,        // RISC-V (32-bit): riscv32
        RiscV64 = LLVMTripleArchType.riscV64,        // RISC-V (64-bit): riscv64
        Sparc = LLVMTripleArchType.sparc,          // Sparc: sparc
        Sparcv9 = LLVMTripleArchType.sparcv9,        // Sparcv9: Sparcv9
        Sparcel = LLVMTripleArchType.sparcel,        // Sparc: (endianness = little). NB: 'Sparcle' is a CPU variant
        SystemZ = LLVMTripleArchType.systemz,        // SystemZ: s390x
        TCE = LLVMTripleArchType.tce,            // TCE (http://tce.cs.tut.fi/): tce
        TCEle = LLVMTripleArchType.tcele,          // TCE little endian (http://tce.cs.tut.fi/): tcele
        Thumb = LLVMTripleArchType.thumb,          // Thumb (little endian): thumb, thumbv.*
        Thumbeb = LLVMTripleArchType.thumbeb,        // Thumb (big endian): thumbeb
        X86 = LLVMTripleArchType.x86,            // X86: i[3-9]86
        X86_64 = LLVMTripleArchType.x86_64,         // X86-64: amd64, x86_64
        Xcore = LLVMTripleArchType.xcore,          // XCore: xcore
        Nvptx = LLVMTripleArchType.nvptx,          // NVPTX: 32-bit
        Nvptx64 = LLVMTripleArchType.nvptx64,        // NVPTX: 64-bit
        Le32 = LLVMTripleArchType.le32,           // le32: generic little-endian 32-bit CPU (PNaCl)
        Le64 = LLVMTripleArchType.le64,           // le64: generic little-endian 64-bit CPU (PNaCl)
        Amdil = LLVMTripleArchType.amdil,          // AMDIL
        Amdil64 = LLVMTripleArchType.amdil64,        // AMDIL with 64-bit pointers
        Hsail = LLVMTripleArchType.hsail,          // AMD HSAIL
        Hsail64 = LLVMTripleArchType.hsail64,        // AMD HSAIL with 64-bit pointers
        Spir = LLVMTripleArchType.spir,           // SPIR: standard portable IR for OpenCL 32-bit version
        Spir64 = LLVMTripleArchType.spir64,         // SPIR: standard portable IR for OpenCL 64-bit version
        Kalimba = LLVMTripleArchType.kalimba,        // Kalimba: generic kalimba
        Shave = LLVMTripleArchType.shave,          // SHAVE: Movidius vector VLIW processors
        Lanai = LLVMTripleArchType.lanai,          // Lanai: Lanai 32-bit
        Wasm32 = LLVMTripleArchType.wasm32,         // WebAssembly with 32-bit pointers
        Wasm64 = LLVMTripleArchType.wasm64,         // WebAssembly with 64-bit pointers
        Renderscript32 = LLVMTripleArchType.renderscript32, // 32-bit RenderScript
        Renderscript64 = LLVMTripleArchType.renderscript64, // 64-bit RenderScript
        LastArchType = Renderscript64
    }

    public enum TripleSubArchType
    {
        NoSubArch = LLVMTripleSubArchType.NoSubArch,
        ARMSubArch_v8_2a = LLVMTripleSubArchType.ARMSubArch_v8_2a,
        ARMSubArch_v8_1a = LLVMTripleSubArchType.ARMSubArch_v8_1a,
        ARMSubArch_v8 = LLVMTripleSubArchType.ARMSubArch_v8,
        ARMSubArch_v8r = LLVMTripleSubArchType.ARMSubArch_v8r,
        ARMSubArch_v8m_baseline = LLVMTripleSubArchType.ARMSubArch_v8m_baseline,
        ARMSubArch_v8m_mainline = LLVMTripleSubArchType.ARMSubArch_v8m_mainline,
        ARMSubArch_v7 = LLVMTripleSubArchType.ARMSubArch_v7,
        ARMSubArch_v7em = LLVMTripleSubArchType.ARMSubArch_v7em,
        ARMSubArch_v7m = LLVMTripleSubArchType.ARMSubArch_v7m,
        ARMSubArch_v7s = LLVMTripleSubArchType.ARMSubArch_v7s,
        ARMSubArch_v7k = LLVMTripleSubArchType.ARMSubArch_v7k,
        ARMSubArch_v7ve = LLVMTripleSubArchType.ARMSubArch_v7ve,
        ARMSubArch_v6 = LLVMTripleSubArchType.ARMSubArch_v6,
        ARMSubArch_v6m = LLVMTripleSubArchType.ARMSubArch_v6m,
        ARMSubArch_v6k = LLVMTripleSubArchType.ARMSubArch_v6k,
        ARMSubArch_v6t2 = LLVMTripleSubArchType.ARMSubArch_v6t2,
        ARMSubArch_v5 = LLVMTripleSubArchType.ARMSubArch_v5,
        ARMSubArch_v5te = LLVMTripleSubArchType.ARMSubArch_v5te,
        ARMSubArch_v4t = LLVMTripleSubArchType.ARMSubArch_v4t,
        KalimbaSubArch_v3 = LLVMTripleSubArchType.KalimbaSubArch_v3,
        KalimbaSubArch_v4 = LLVMTripleSubArchType.KalimbaSubArch_v4,
        KalimbaSubArch_v5 = LLVMTripleSubArchType.KalimbaSubArch_v5
    }

    public enum TripleVendorType
    {
        UnknownVendor = LLVMTripleVendorType.UnknownVendor,
        Apple = LLVMTripleVendorType.Apple,
        PC = LLVMTripleVendorType.PC,
        SCEI = LLVMTripleVendorType.SCEI,
        BGP = LLVMTripleVendorType.BGP,
        BGQ = LLVMTripleVendorType.BGQ,
        Freescale = LLVMTripleVendorType.Freescale,
        IBM = LLVMTripleVendorType.IBM,
        ImaginationTechnologies = LLVMTripleVendorType.ImaginationTechnologies,
        MipsTechnologies = LLVMTripleVendorType.MipsTechnologies,
        NVIDIA = LLVMTripleVendorType.NVIDIA,
        CSR = LLVMTripleVendorType.CSR,
        Myriad = LLVMTripleVendorType.Myriad,
        AMD = LLVMTripleVendorType.AMD,
        Mesa = LLVMTripleVendorType.Mesa
    }

    public enum TripleOSType
    {
        UnknownOS = LLVMTripleOSType.UnknownOS,
        Ananas = LLVMTripleOSType.Ananas,
        CloudABI = LLVMTripleOSType.CloudABI,
        Darwin = LLVMTripleOSType.Darwin,
        DragonFly = LLVMTripleOSType.DragonFly,
        FreeBSD = LLVMTripleOSType.FreeBSD,
        Fuchsia = LLVMTripleOSType.Fuchsia,
        IOS = LLVMTripleOSType.IOS,
        KFreeBSD = LLVMTripleOSType.KFreeBSD,
        Linux = LLVMTripleOSType.Linux,
        Lv2 = LLVMTripleOSType.Lv2,
        MacOSX = LLVMTripleOSType.MacOSX,
        NetBSD = LLVMTripleOSType.NetBSD,
        OpenBSD = LLVMTripleOSType.OpenBSD,
        Solaris = LLVMTripleOSType.Solaris,
        Win32 = LLVMTripleOSType.Win32,
        Haiku = LLVMTripleOSType.Haiku,
        Minix = LLVMTripleOSType.Minix,
        RTEMS = LLVMTripleOSType.RTEMS,
        NaCl = LLVMTripleOSType.NaCl,
        CNK = LLVMTripleOSType.CNK,
        Bitrig = LLVMTripleOSType.Bitrig,
        AIX = LLVMTripleOSType.AIX,
        CUDA = LLVMTripleOSType.CUDA,
        NVCL = LLVMTripleOSType.NVCL,
        AMDHSA = LLVMTripleOSType.AMDHSA,
        PS4 = LLVMTripleOSType.PS4,
        ELFIAMCU = LLVMTripleOSType.ELFIAMCU,
        TvOS = LLVMTripleOSType.TvOS,
        WatchOS = LLVMTripleOSType.WatchOS,
        Mesa3D = LLVMTripleOSType.Mesa3D,
        Contiki = LLVMTripleOSType.Contiki
    }

    public enum TripleEnvironmentType
    {
        UnknownEnvironment = LLVMTripleEnvironmentType.UnknownEnvironment,
        GNU = LLVMTripleEnvironmentType.GNU,
        GNUABI64 = LLVMTripleEnvironmentType.GNUABI64,
        GNUEABI = LLVMTripleEnvironmentType.GNUEABI,
        GNUEABIHF = LLVMTripleEnvironmentType.GNUEABIHF,
        GNUX32 = LLVMTripleEnvironmentType.GNUX32,
        CODE16 = LLVMTripleEnvironmentType.CODE16,
        EABI = LLVMTripleEnvironmentType.EABI,
        EABIHF = LLVMTripleEnvironmentType.EABIHF,
        Android = LLVMTripleEnvironmentType.Android,
        Musl = LLVMTripleEnvironmentType.Musl,
        MuslEABI = LLVMTripleEnvironmentType.MuslEABI,
        MuslEABIHF = LLVMTripleEnvironmentType.MuslEABIHF,
        MSVC = LLVMTripleEnvironmentType.MSVC,
        Itanium = LLVMTripleEnvironmentType.Itanium,
        Cygnus = LLVMTripleEnvironmentType.Cygnus,
        AMDOpenCL = LLVMTripleEnvironmentType.AMDOpenCL,
        CoreCLR = LLVMTripleEnvironmentType.CoreCLR,
        OpenCL = LLVMTripleEnvironmentType.OpenCL
    }

    public enum TripleObjectFormatType
    {
        UnknownObjectFormat = LLVMTripleObjectFormatType.UnknownObjectFormat,
        COFF = LLVMTripleObjectFormatType.COFF,
        ELF = LLVMTripleObjectFormatType.ELF,
        MachO = LLVMTripleObjectFormatType.MachO,
        Wasm = LLVMTripleObjectFormatType.Wasm
    }

    public enum ComdatKind
    {
        Any = LLVMComdatSelectionKind.ANY,
        ExactMatch = LLVMComdatSelectionKind.EXACTMATCH,
        Largest = LLVMComdatSelectionKind.LARGEST,
        NoDuplicates = LLVMComdatSelectionKind.NODUPLICATES,
        SameSize = LLVMComdatSelectionKind.SAMESIZE
    }
}
