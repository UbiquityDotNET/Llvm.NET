// This file maps the lower level internal LLVM enumeration names to something
// more compatible with the styles, patterns and conventions familiar to .NET Developers.
// This also keeping the lower level interop namespace internal to prevent mis-use or
// violations of uniqueness rules. Furthermore, this helps insulate the managed code
// realm insulated from the sometimes fluid nature of the underlying LLVM changes.

using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Enumeration to indicate the behavior of module level flags metadata sharing the same name in a <see cref="NativeModule"/></summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag" )]
    public enum ModuleFlagBehavior : uint
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
        AppendUnique = LLVMModFlagBehavior.AppendUnique
    };

    /// <summary>LLVM Instruction opcodes</summary>
    /// <remarks>
    /// These are based on the "C" API and therefore more stable as changes in the underlying instruction ids are remapped in the C API layer
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not actually flags" )]
    public enum OpCode : uint
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
        CleanupPad = LLVMOpcode.LLVMCleandupPad,
        CatchSwitch = LLVMOpcode.LLVMCatchSwitch
    }

    /// <summary>Basic kind of a type</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum TypeKind : uint
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

    /// <summary>Calling Convention for functions</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum CallingConvention : uint
    {
        C = LLVMCallConv.LLVMCCallConv,
        FastCall = LLVMCallConv.LLVMFastCallConv,
        ColdCall = LLVMCallConv.LLVMColdCallConv,
        GlasgowHaskellCompiler = LLVMCallConv.LLVMGHCCallConv,
        HiPE = LLVMCallConv.LLVMHiPECallConv,
        WebKitJS = LLVMCallConv.LLVMWebKitJSCallConv,
        AnyReg = LLVMCallConv.LLVMAnyRegCallConv,
        PreserveMost = LLVMCallConv.LLVMPreserveMostCallConv,
        PreserveSwift = LLVMCallConv.LLVMPreserveSwiftCallConv,
        CxxFastTls = LLVMCallConv.LLVMCxxFasTlsCallConv,
        FirstTargetSPecific = LLVMCallConv.LLVMFirstTargetCallConv,
        X86StdCall = LLVMCallConv.LLVMX86StdcallCallConv,
        X86FastCall = LLVMCallConv.LLVMX86FastcallCallConv,
        ArmAPCS = LLVMCallConv.LLVMArmAPCSCallConv,
        ArmAAPCS = LLVMCallConv.LLVMArmAAPCSCallConv,
        ArmAAPCSVfp = LLVMCallConv.LLVMArmAAPCSVfpCallConv,
        MPS430Interrupt = LLVMCallConv.LLVMMSP430IntrCallConv,
        X86ThisCall = LLVMCallConv.LLVMx86ThisCallConv,
        PtxKernel = LLVMCallConv.LLVMPTXKernelCallConv,
        PtxDevice = LLVMCallConv.LLVMPTXDeviceCallConv,
        SpirFunction = LLVMCallConv.LLVMSpirFuncCallConv,
        SpirKernel = LLVMCallConv.LLVMSpirKernelCallConv,
        IntelOpenCLBuiltIn = LLVMCallConv.LLVMIntelOCLBICallConv,
        X86x64SysV = LLVMCallConv.LLVMx86_64SysVCallConv,
        X86x64Win64 = LLVMCallConv.LLVMx86_64Win64CallConv,
        X86VectorCall = LLVMCallConv.LLVMx86_VectorCallCallConv,
        HHVM = LLVMCallConv.LLVM_HHVMCallConv,
        HHVMCCall = LLVMCallConv.LLVM_HHVM_C_CallConv,
        X86Interrupt = LLVMCallConv.LLVMx86IntrCallConv,
        MaxCallingConvention = LLVMCallConv.LLVM_MaxIDCallConv
    }

    /// <summary>Linkage specification for functions and globals</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum Linkage : uint
    {
        External = LLVMLinkage.LLVMExternalLinkage,    /*< Externally visible function */
        AvailableExternally = LLVMLinkage.LLVMAvailableExternallyLinkage,
        LinkOnceAny = LLVMLinkage.LLVMLinkOnceAnyLinkage, /*< Keep one copy of function when linking (inline)*/
        LinkOnceODR = LLVMLinkage.LLVMLinkOnceODRLinkage, /*< Same, but only replaced by something equivalent. */
        //LLVMLinkage.LLVMLinkOnceODRAutoHideLinkage, /**< Obsolete */
        Weak = LLVMLinkage.LLVMWeakAnyLinkage,     /*< Keep one copy of function when linking (weak) */
        WeakODR = LLVMLinkage.LLVMWeakODRLinkage,     /*< Same, but only replaced by something equivalent. */
        Append = LLVMLinkage.LLVMAppendingLinkage,   /*< Special purpose, only applies to global arrays */
        Internal = LLVMLinkage.LLVMInternalLinkage,    /*< Rename collisions when linking (static functions) */
        Private = LLVMLinkage.LLVMPrivateLinkage,     /*< Like Internal, but omit from symbol table */
        DllImport = LLVMLinkage.LLVMDLLImportLinkage,   /*< Function to be imported from DLL */
        DllExport = LLVMLinkage.LLVMDLLExportLinkage,   /*< Function to be accessible from DLL */
        ExternalWeak = LLVMLinkage.LLVMExternalWeakLinkage,/*< ExternalWeak linkage description */
        //LLVMLinkage.LLVMGhostLinkage,       /*< Obsolete */
        Common = LLVMLinkage.LLVMCommonLinkage,      /*< Tentative definitions */
        LinkerPrivate = LLVMLinkage.LLVMLinkerPrivateLinkage, /*< Like Private, but linker removes. */
        LinkerPrivateWeak = LLVMLinkage.LLVMLinkerPrivateWeakLinkage /*< Like LinkerPrivate, but is weak. */
    }

    ///<summary>Enumeration for the visibility of a global value</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum Visibility : uint
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1027:MarkEnumsWithFlags" )]
    public enum Predicate : uint
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

    ///<summary>Predicate enumeration for integer comparison</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum IntPredicate : uint
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

    ///<summary>Predicate enumeration for integer comparison</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum RealPredicate : uint
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
    }

    /// <summary>Optimization level for target code generation</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum CodeGenOpt : uint
    {
        None = LLVMCodeGenOptLevel.LLVMCodeGenLevelNone,
        Less = LLVMCodeGenOptLevel.LLVMCodeGenLevelLess,
        Default = LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault,
        Aggressive = LLVMCodeGenOptLevel.LLVMCodeGenLevelAggressive
    }

    /// <summary>Relocation type for target code generation</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum Reloc : uint
    {
        Default = LLVMRelocMode.LLVMRelocDefault,
        Static = LLVMRelocMode.LLVMRelocStatic,
        PositionIndependent = LLVMRelocMode.LLVMRelocPIC,
        Dynamic = LLVMRelocMode.LLVMRelocDynamicNoPic
    }

    /// <summary>Code model to use for target code generation</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum CodeModel : uint
    {
        Default = LLVMCodeModel.LLVMCodeModelDefault,
        JitDefault = LLVMCodeModel.LLVMCodeModelJITDefault,
        Small = LLVMCodeModel.LLVMCodeModelSmall,
        Kernel = LLVMCodeModel.LLVMCodeModelKernel,
        Medium = LLVMCodeModel.LLVMCodeModelMedium,
        Large = LLVMCodeModel.LLVMCodeModelLarge
    }

    /// <summary>Output file type for target code generation</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum CodeGenFileType : uint
    {
        AssemblySource = LLVMCodeGenFileType.LLVMAssemblyFile,
        ObjectFile = LLVMCodeGenFileType.LLVMObjectFile
    }

    /// <summary>Byte ordering for target code generation and data type layout</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
    public enum ByteOrdering : uint
    {
        LittleEndian = LLVMByteOrdering.LLVMLittleEndian,
        BigEndian = LLVMByteOrdering.LLVMBigEndian
    }

    /// <summary>Enumeration for well known attribute kinds</summary>
    /// <remarks>
    /// The numeric value of the members of this enumeration are subject
    /// to change from version to version. Therefore code must never
    /// rely on the actual underlying value and use only the symbolic name
    /// </remarks>
    /// <Implementation>
    /// For details on what the values should be see attributes.def in 
    /// the LLVM source. This was added in v3.8.0 along with a change in
    /// the numerical values. (in V3.9.0 it is Attributes.inc)
    /// </Implementation>
    public enum AttributeKind
    {  
        None                        = LLVMAttrKind.None,
        Alignment                   = LLVMAttrKind.Alignment,
        AlwaysInline                = LLVMAttrKind.AlwaysInline,
        ArgMemOnly                  = LLVMAttrKind.ArgMemOnly,
        Builtin                     = LLVMAttrKind.Builtin,
        ByVal                       = LLVMAttrKind.ByVal,
        Cold                        = LLVMAttrKind.Cold,
        Convergent                  = LLVMAttrKind.Convergent,
        Dereferenceable             = LLVMAttrKind.Dereferenceable,
        DereferenceableOrNull       = LLVMAttrKind.DereferenceableOrNull,
        InAlloca                    = LLVMAttrKind.InAlloca,
        InReg                       = LLVMAttrKind.InReg,
        InaccessibleMemOnly         = LLVMAttrKind.InaccessibleMemOnly,
        InaccessibleMemOrArgMemOnly = LLVMAttrKind.InaccessibleMemOrArgMemOnly,
        InlineHint                  = LLVMAttrKind.InlineHint,
        JumpTable                   = LLVMAttrKind.JumpTable,
        MinSize                     = LLVMAttrKind.MinSize,
        Naked                       = LLVMAttrKind.Naked,
        Nest                        = LLVMAttrKind.Nest,
        NoAlias                     = LLVMAttrKind.NoAlias,
        NoBuiltin                   = LLVMAttrKind.NoBuiltin,
        NoCapture                   = LLVMAttrKind.NoCapture,
        NoDuplicate                 = LLVMAttrKind.NoDuplicate,
        NoImplicitFloat             = LLVMAttrKind.NoImplicitFloat,
        NoInline                    = LLVMAttrKind.NoInline,
        NoRecurse                   = LLVMAttrKind.NoRecurse,
        NoRedZone                   = LLVMAttrKind.NoRedZone,
        NoReturn                    = LLVMAttrKind.NoReturn,
        NoUnwind                    = LLVMAttrKind.NoUnwind,
        NonLazyBind                 = LLVMAttrKind.NonLazyBind,
        NonNull                     = LLVMAttrKind.NonNull,
        OptimizeForSize             = LLVMAttrKind.OptimizeForSize,
        OptimizeNone                = LLVMAttrKind.OptimizeNone,
        ReadNone                    = LLVMAttrKind.ReadNone,
        ReadOnly                    = LLVMAttrKind.ReadOnly,
        Returned                    = LLVMAttrKind.Returned,
        ReturnsTwice                = LLVMAttrKind.ReturnsTwice,
        SExt                        = LLVMAttrKind.SExt,
        SafeStack                   = LLVMAttrKind.SafeStack,
        SanitizeAddress             = LLVMAttrKind.SanitizeAddress,
        SanitizeMemory              = LLVMAttrKind.SanitizeMemory,
        SanitizeThread              = LLVMAttrKind.SanitizeThread,
        StackAlignment              = LLVMAttrKind.StackAlignment,
        StackProtect                = LLVMAttrKind.StackProtect,
        StackProtectReq             = LLVMAttrKind.StackProtectReq,
        StackProtectStrong          = LLVMAttrKind.StackProtectStrong,
        StructRet                   = LLVMAttrKind.StructRet,
        SwiftError                  = LLVMAttrKind.SwiftError,
        SwiftSelf                   = LLVMAttrKind.SwiftSelf,
        UWTable                     = LLVMAttrKind.UWTable,
        ZExt                        = LLVMAttrKind.ZExt,
        EndAttrKinds           // Sentinel value useful for loops
    };

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

    public enum TripleArchType
    {
        UnknownArch = LLVMTripleArchType.LlvmTripleArchType_UnknownArch,

        // ARM (little endian): arm, armv.*, xscale
        arm = LLVMTripleArchType.LlvmTripleArchType_arm,
        
        // ARM (big endian): armeb
        armeb = LLVMTripleArchType.LlvmTripleArchType_armeb,
        
        // AArch64 (little endian): aarch64
        aarch64 = LLVMTripleArchType.LlvmTripleArchType_aarch64,
        
        // AArch64 (big endian): aarch64_be
        aarch64_be = LLVMTripleArchType.LlvmTripleArchType_aarch64_be,
        
        // AVR: Atmel AVR microcontroller
        avr = LLVMTripleArchType.LlvmTripleArchType_avr,

        // eBPF or extended BPF or 64-bit BPF (little endian)
        bpfel = LLVMTripleArchType.LlvmTripleArchType_bpfel,

        // eBPF or extended BPF or 64-bit BPF (big endian)
        bpfeb = LLVMTripleArchType.LlvmTripleArchType_bpfeb,
        
        // Hexagon: hexagon
        hexagon = LLVMTripleArchType.LlvmTripleArchType_hexagon,
        
        // MIPS: mips, mipsallegrex
        mips = LLVMTripleArchType.LlvmTripleArchType_mips,
        
        // MIPSEL: mipsel, mipsallegrexel
        mipsel = LLVMTripleArchType.LlvmTripleArchType_mipsel,
        
        // MIPS64: mips64
        mips64 = LLVMTripleArchType.LlvmTripleArchType_mips64,

        // MIPS64EL: mips64el
        mips64el = LLVMTripleArchType.LlvmTripleArchType_mips64el,

        // MSP430: msp430
        msp430 = LLVMTripleArchType.LlvmTripleArchType_msp430,
        
        // PPC: powerpc
        ppc = LLVMTripleArchType.LlvmTripleArchType_ppc,

        // PPC64: powerpc64, ppu
        ppc64 = LLVMTripleArchType.LlvmTripleArchType_ppc64,

        // PPC64LE: powerpc64le
        ppc64le = LLVMTripleArchType.LlvmTripleArchType_ppc64le,

        // R600: AMD GPUs HD2XXX - HD6XXX
        r600 = LLVMTripleArchType.LlvmTripleArchType_r600,

        // AMDGCN: AMD GCN GPUs
        amdgcn = LLVMTripleArchType.LlvmTripleArchType_amdgcn,

        // Sparc: sparc
        sparc = LLVMTripleArchType.LlvmTripleArchType_sparc,

        // Sparcv9: Sparcv9
        sparcv9 = LLVMTripleArchType.LlvmTripleArchType_sparcv9,

        // Sparc: (endianness = little). NB: 'Sparcle' is a CPU variant
        sparcel = LLVMTripleArchType.LlvmTripleArchType_sparcel,

        // SystemZ: s390x
        systemz = LLVMTripleArchType.LlvmTripleArchType_systemz,

        // TCE (http://tce.cs.tut.fi/): tce
        tce = LLVMTripleArchType.LlvmTripleArchType_tce,

        // Thumb (little endian): thumb, thumbv.*
        thumb = LLVMTripleArchType.LlvmTripleArchType_thumb,

        // Thumb (big endian): thumbeb
        thumbeb = LLVMTripleArchType.LlvmTripleArchType_thumbeb,

        // X86: i[3-9]86
        x86 = LLVMTripleArchType.LlvmTripleArchType_x86,

        // X86-64: amd64, x86_64
        x86_64 = LLVMTripleArchType.LlvmTripleArchType_x86_64,

        // XCore: xcore
        xcore = LLVMTripleArchType.LlvmTripleArchType_xcore,

        // NVPTX: 32-bit
        nvptx = LLVMTripleArchType.LlvmTripleArchType_nvptx,

        // NVPTX: 64-bit
        nvptx64 = LLVMTripleArchType.LlvmTripleArchType_nvptx64,

        // le32: generic little-endian 32-bit CPU (PNaCl)
        le32 = LLVMTripleArchType.LlvmTripleArchType_le32,

        // le64: generic little-endian 64-bit CPU (PNaCl)
        le64 = LLVMTripleArchType.LlvmTripleArchType_le64,

        // AMDIL
        amdil = LLVMTripleArchType.LlvmTripleArchType_amdil,

        // AMDIL with 64-bit pointers
        amdil64 = LLVMTripleArchType.LlvmTripleArchType_amdil64,

        // AMD HSAIL
        hsail = LLVMTripleArchType.LlvmTripleArchType_hsail,

        // AMD HSAIL with 64-bit pointers
        hsail64 = LLVMTripleArchType.LlvmTripleArchType_hsail64,

        // SPIR: standard portable IR for OpenCL 32-bit version
        spir = LLVMTripleArchType.LlvmTripleArchType_spir,

        // SPIR: standard portable IR for OpenCL 64-bit version
        spir64 = LLVMTripleArchType.LlvmTripleArchType_spir64,

        // Kalimba: generic kalimba
        kalimba = LLVMTripleArchType.LlvmTripleArchType_kalimba,

        // SHAVE: Movidius vector VLIW processors
        shave = LLVMTripleArchType.LlvmTripleArchType_shave,

        // Lanai: Lanai 32-bit
        lanai = LLVMTripleArchType.LlvmTripleArchType_lanai,

        // WebAssembly with 32-bit pointers
        wasm32 = LLVMTripleArchType.LlvmTripleArchType_wasm32,

        // WebAssembly with 64-bit pointers
        wasm64 = LLVMTripleArchType.LlvmTripleArchType_wasm64,

        // 32-bit RenderScript
        renderscript32 = LLVMTripleArchType.LlvmTripleArchType_renderscript32,

        // 64-bit RenderScript
        renderscript64 = LLVMTripleArchType.LlvmTripleArchType_renderscript64,
    };

    public enum TripleSubArchType
    {
        NoSubArch                = LLVMTripleSubArchType.LlvmTripleSubArchType_NoSubArch,
        ARMSubArch_v8_2a         = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8_2a,
        ARMSubArch_v8_1a         = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8_1a,
        ARMSubArch_v8            = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8,
        ARMSubArch_v8m_baseline  = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8m_baseline,
        ARMSubArch_v8m_mainline  = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8m_mainline,
        ARMSubArch_v7            = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7,
        ARMSubArch_v7em          = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7em,
        ARMSubArch_v7m           = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7m,
        ARMSubArch_v7s           = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7s,
        ARMSubArch_v7k           = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7k,
        ARMSubArch_v6            = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6,
        ARMSubArch_v6m           = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6m,
        ARMSubArch_v6k           = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6k,
        ARMSubArch_v6t2          = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6t2,
        ARMSubArch_v5            = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v5,
        ARMSubArch_v5te          = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v5te,
        ARMSubArch_v4t           = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v4t,
        KalimbaSubArch_v3        = LLVMTripleSubArchType.LlvmTripleSubArchType_KalimbaSubArch_v3,
        KalimbaSubArch_v4        = LLVMTripleSubArchType.LlvmTripleSubArchType_KalimbaSubArch_v4,
        KalimbaSubArch_v5        = LLVMTripleSubArchType.LlvmTripleSubArchType_KalimbaSubArch_v5
    };

    public enum TripleVendorType
    {
        UnknownVendor            = LLVMTripleVendorType.LlvmTripleVendorType_UnknownVendor,
        Apple                    = LLVMTripleVendorType.LlvmTripleVendorType_Apple,
        PC                       = LLVMTripleVendorType.LlvmTripleVendorType_PC,
        SCEI                     = LLVMTripleVendorType.LlvmTripleVendorType_SCEI,
        BGP                      = LLVMTripleVendorType.LlvmTripleVendorType_BGP,
        BGQ                      = LLVMTripleVendorType.LlvmTripleVendorType_BGQ,
        Freescale                = LLVMTripleVendorType.LlvmTripleVendorType_Freescale,
        IBM                      = LLVMTripleVendorType.LlvmTripleVendorType_IBM,
        ImaginationTechnologies  = LLVMTripleVendorType.LlvmTripleVendorType_ImaginationTechnologies,
        MipsTechnologies         = LLVMTripleVendorType.LlvmTripleVendorType_MipsTechnologies,
        NVIDIA                   = LLVMTripleVendorType.LlvmTripleVendorType_NVIDIA,
        CSR                      = LLVMTripleVendorType.LlvmTripleVendorType_CSR,
        Myriad                   = LLVMTripleVendorType.LlvmTripleVendorType_Myriad,
        AMD                      = LLVMTripleVendorType.LlvmTripleVendorType_AMD,
        Mesa                     = LLVMTripleVendorType.LlvmTripleVendorType_Mesa,
    };

    public enum TripleOSType
    {
        UnknownOS  = LLVMTripleOSType.LlvmTripleOSType_UnknownOS,
        CloudABI   = LLVMTripleOSType.LlvmTripleOSType_CloudABI,
        Darwin     = LLVMTripleOSType.LlvmTripleOSType_Darwin,
        DragonFly  = LLVMTripleOSType.LlvmTripleOSType_DragonFly,
        FreeBSD    = LLVMTripleOSType.LlvmTripleOSType_FreeBSD,
        IOS        = LLVMTripleOSType.LlvmTripleOSType_IOS,
        KFreeBSD   = LLVMTripleOSType.LlvmTripleOSType_KFreeBSD,
        Linux      = LLVMTripleOSType.LlvmTripleOSType_Linux,
        Lv2        = LLVMTripleOSType.LlvmTripleOSType_Lv2,
        MacOSX     = LLVMTripleOSType.LlvmTripleOSType_MacOSX,
        NetBSD     = LLVMTripleOSType.LlvmTripleOSType_NetBSD,
        OpenBSD    = LLVMTripleOSType.LlvmTripleOSType_OpenBSD,
        Solaris    = LLVMTripleOSType.LlvmTripleOSType_Solaris,
        Win32      = LLVMTripleOSType.LlvmTripleOSType_Win32,
        Haiku      = LLVMTripleOSType.LlvmTripleOSType_Haiku,
        Minix      = LLVMTripleOSType.LlvmTripleOSType_Minix,
        RTEMS      = LLVMTripleOSType.LlvmTripleOSType_RTEMS,
        NaCl       = LLVMTripleOSType.LlvmTripleOSType_NaCl,
        CNK        = LLVMTripleOSType.LlvmTripleOSType_CNK,
        Bitrig     = LLVMTripleOSType.LlvmTripleOSType_Bitrig,
        AIX        = LLVMTripleOSType.LlvmTripleOSType_AIX,
        CUDA       = LLVMTripleOSType.LlvmTripleOSType_CUDA,
        NVCL       = LLVMTripleOSType.LlvmTripleOSType_NVCL,
        AMDHSA     = LLVMTripleOSType.LlvmTripleOSType_AMDHSA,
        PS4        = LLVMTripleOSType.LlvmTripleOSType_PS4,
        ELFIAMCU   = LLVMTripleOSType.LlvmTripleOSType_ELFIAMCU,
        TvOS       = LLVMTripleOSType.LlvmTripleOSType_TvOS,
        WatchOS    = LLVMTripleOSType.LlvmTripleOSType_WatchOS,
        Mesa3D     = LLVMTripleOSType.LlvmTripleOSType_Mesa3D,
    };

    public enum TripleEnvironmentType
    {
        UnknownEnvironment  = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_UnknownEnvironment,
        GNU                 = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNU,
        GNUABI64            = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUABI64,
        GNUEABI             = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUEABI,
        GNUEABIHF           = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUEABIHF,
        GNUX32              = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUX32,
        CODE16              = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_CODE16,
        EABI                = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_EABI,
        EABIHF              = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_EABIHF,
        Android             = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Android,
        Musl                = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Musl,
        MuslEABI            = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_MuslEABI,
        MuslEABIHF          = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_MuslEABIHF,
        MSVC                = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_MSVC,
        Itanium             = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Itanium,
        Cygnus              = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Cygnus,
        AMDOpenCL           = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_AMDOpenCL,
        CoreCLR             = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_CoreCLR,
    };

    public enum TripleObjectFormatType
    {
        UnknownObjectFormat  = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_UnknownObjectFormat,
        COFF                 = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_COFF,
        ELF                  = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_ELF,
        MachO                = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_MachO,
    };
}