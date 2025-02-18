// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMDiagnosticHandler = delegate* unmanaged[Cdecl]<LLVMDiagnosticInfoRef /*_0*/, void* /*_1*/, void /*retVal*/ >;
    using unsafe LLVMYieldCallback = delegate* unmanaged[Cdecl]<nint /*LLVMContextRef*/ /*_0*/, void* /*_1*/, void /*retVal*/ >;
#pragma warning restore  IDE0065, SA1200

    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "Not flags, tooling detection of such is broken" )]
    public enum LLVMOpcode
        : Int32
    {
        None = 0,
        Ret = 1,
        Br = 2,
        Switch = 3,
        IndirectBr = 4,
        Invoke = 5,
        Unreachable = 7,
        CallBr = 67,
        FNeg = 66,
        Add = 8,
        FAdd = 9,
        Sub = 10,
        FSub = 11,
        Mul = 12,
        FMul = 13,
        UDiv = 14,
        SDiv = 15,
        FDiv = 16,
        URem = 17,
        SRem = 18,
        FRem = 19,
        Shl = 20,
        LShr = 21,
        AShr = 22,
        And = 23,
        Or = 24,
        Xor = 25,
        Alloca = 26,
        Load = 27,
        Store = 28,
        GetElementPtr = 29,
        Trunc = 30,
        ZExt = 31,
        SExt = 32,
        FPToUI = 33,
        FPToSI = 34,
        UIToFP = 35,
        SIToFP = 36,
        FPTrunc = 37,
        FPExt = 38,
        PtrToInt = 39,
        IntToPtr = 40,
        BitCast = 41,
        AddrSpaceCast = 60,
        ICmp = 42,
        FCmp = 43,
        PHI = 44,
        Call = 45,
        Select = 46,
        UserOp1 = 47,
        UserOp2 = 48,
        VAArg = 49,
        ExtractElement = 50,
        InsertElement = 51,
        ShuffleVector = 52,
        ExtractValue = 53,
        InsertValue = 54,
        Freeze = 68,
        Fence = 55,
        AtomicCmpXchg = 56,
        AtomicRMW = 57,
        Resume = 58,
        LandingPad = 59,
        CleanupRet = 61,
        CatchRet = 62,
        CatchPad = 63,
        CleanupPad = 64,
        CatchSwitch = 65,
    }

    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "Not flags, tool heuristics are too stupid to deal with it" )]
    public enum LLVMTypeKind
        : Int32
    {
        Void = 0,
        Half = 1,
        Float = 2,
        Double = 3,
        X86_FP80 = 4,
        FP128 = 5,
        PPC_FP128 = 6,
        Label = 7,
        Integer = 8,
        Function = 9,
        Struct = 10,
        Array = 11,
        Pointer = 12,
        Vector = 13,
        Metadata = 14,
        Token = 16,
        ScalableVector = 17,
        BFloat = 18,
        X86_AMX = 19,
        TargetExt = 20,
    }

    public enum LLVMLinkage
        : Int32
    {
        External = 0,
        AvailableExternally = 1,
        LinkOnceAny = 2,
        LinkOnceODR = 3,
        LinkOnceODRAutoHide = 4,
        WeakAny = 5,
        WeakODR = 6,
        Appending = 7,
        Internal = 8,
        Private = 9,
        DLLImport = 10,
        DLLExport = 11,
        ExternalWeak = 12,
        Ghost = 13,
        Common = 14,
        LinkerPrivate = 15,
        LinkerPrivateWeak = 16,
    }

    public enum LLVMVisibility
        : Int32
    {
        Default = 0,
        Hidden = 1,
        Protected = 2,
    }

    public enum LLVMUnnamedAddr
        : Int32
    {
        None = 0,
        Local = 1,
        Global = 2,
    }

    public enum LLVMDLLStorageClass
        : Int32
    {
        Default = 0,
        DLLImport = 1,
        DLLExport = 2,
    }

    public enum LLVMCallConv
        : Int32
    {
        C = 0,
        Fast = 8,
        Cold = 9,
        GHC = 10,
        HiPE = 11,
        AnyReg = 13,
        PreserveMost = 14,
        PreserveAll = 15,
        Swift = 16,
        CXXFASTTLS = 17,
        X86Stdcall = 64,
        X86Fastcall = 65,
        ARMAPCS = 66,
        ARMAAPCS = 67,
        ARMAAPCSVFP = 68,
        MSP430INTR = 69,
        X86ThisCall = 70,
        PTXKernel = 71,
        PTXDevice = 72,
        SPIRFUNC = 75,
        SPIRKERNEL = 76,
        IntelOCLBI = 77,
        X8664SysV = 78,
        Win64 = 79,
        X86VectorCall = 80,
        HHVM = 81,
        HHVMC = 82,
        X86INTR = 83,
        AVRINTR = 84,
        AVRSIGNAL = 85,
        AVRBUILTIN = 86,
        AMDGPUVS = 87,
        AMDGPUGS = 88,
        AMDGPUPS = 89,
        AMDGPUCS = 90,
        AMDGPUKERNEL = 91,
        X86RegCall = 92,
        AMDGPUHS = 93,
        MSP430BUILTIN = 94,
        AMDGPULS = 95,
        AMDGPUES = 96,
    }

    public enum LLVMValueKind
        : Int32
    {
        Argument = 0,
        BasicBlock = 1,
        MemoryUse = 2,
        MemoryDef = 3,
        MemoryPhi = 4,
        Function = 5,
        GlobalAlias = 6,
        GlobalIFunc = 7,
        GlobalVariable = 8,
        BlockAddress = 9,
        ConstantExpr = 10,
        ConstantArray = 11,
        ConstantStruct = 12,
        ConstantVector = 13,
        UndefValue = 14,
        ConstantAggregateZero = 15,
        ConstantDataArray = 16,
        ConstantDataVector = 17,
        ConstantInt = 18,
        ConstantFP = 19,
        ConstantPointerNull = 20,
        ConstantTokenNone = 21,
        MetadataAsValue = 22,
        InlineAsm = 23,
        Instruction = 24,
        PoisonValue = 25,
        ConstantTargetNone = 26,
        ConstantPtrAuth = 27,
    }

    public enum LLVMIntPredicate
        : Int32
    {
        None = 0,

        EQ = 32,
        NE = 33,
        UGT = 34,
        UGE = 35,
        ULT = 36,
        ULE = 37,
        SGT = 38,
        SGE = 39,
        SLT = 40,
        SLE = 41,
    }

    public enum LLVMRealPredicate
        : Int32
    {
        PredicateFalse = 0,
        OEQ = 1,
        OGT = 2,
        OGE = 3,
        OLT = 4,
        OLE = 5,
        ONE = 6,
        ORD = 7,
        UNO = 8,
        UEQ = 9,
        UGT = 10,
        UGE = 11,
        ULT = 12,
        ULE = 13,
        UNE = 14,
        PredicateTrue = 15,
    }

    public enum LLVMLandingPadClauseTy
        : Int32
    {
        Catch = 0,
        Filter = 1,
    }

    public enum LLVMThreadLocalMode
        : Int32
    {
        NotThreadLocal = 0,
        GeneralDynamicTLSModel = 1,
        LocalDynamicTLSModel = 2,
        InitialExecTLSModel = 3,
        LocalExecTLSModel = 4,
    }

    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "NOT flags; tooling is too simplistic" )]
    public enum LLVMAtomicOrdering
        : Int32
    {
        NotAtomic = 0,
        Unordered = 1,
        Monotonic = 2,
        Acquire = 4,
        Release = 5,
        AcquireRelease = 6,
        SequentiallyConsistent = 7,
    }

    public enum LLVMAtomicRMWBinOp
        : Int32
    {
        Xchg = 0,
        Add = 1,
        Sub = 2,
        And = 3,
        Nand = 4,
        Or = 5,
        Xor = 6,
        Max = 7,
        Min = 8,
        UMax = 9,
        UMin = 10,
        FAdd = 11,
        FSub = 12,
        FMax = 13,
        FMin = 14,
        UIncWrap = 15,
        UDecWrap = 16,
        USubCond = 17,
        USubSat = 18,
    }

    public enum LLVMDiagnosticSeverity
        : Int32
    {
        Error = 0,
        Warning = 1,
        Remark = 2,
        Note = 3,
    }

    public enum LLVMInlineAsmDialect
        : Int32
    {
        ATT = 0,
        Intel = 1,
    }

    public enum LLVMModuleFlagBehavior
        : Int32
    {
        Error = 0,
        Warning = 1,
        Require = 2,
        Override = 3,
        Append = 4,
        AppendUnique = 5,
    }

    public enum LLVMAttributeIndex
        : Int32
    {
        Return = 0,
        Function = -1,
    }

    public enum LLVMTailCallKind
        : Int32
    {
        None = 0,
        Tail = 1,
        MustTail = 2,
        NoTail = 3,
    }

    [Flags]
    public enum LLVMFastMath
        : Int32
    {
        AllowReassoc = 1,
        NoNaNs = 2,
        NoInfs = 4,
        NoSignedZeros = 8,
        AllowReciprocal = 16,
        AllowContract = 32,
        ApproxFunc = 64,
        None = 0,
        All = 127,
    }

    [Flags]
    public enum LLVMGEPNoWrap
        : Int32
    {
        None = 0,
        InBounds = 1,
        NUSW = 2,
        NUW = 4,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMShutdown();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetVersion(out uint Major, out uint Minor, out uint Patch);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRef LLVMContextCreate();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRef LLVMGetGlobalContext();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMContextSetDiagnosticHandler(LLVMContextRef C, LLVMDiagnosticHandler Handler, void* DiagnosticContext);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDiagnosticHandler LLVMContextGetDiagnosticHandler(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMContextGetDiagnosticContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMContextSetYieldCallback(LLVMContextRef C, LLVMYieldCallback Callback, void* OpaqueHandle);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMContextShouldDiscardValueNames(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMContextSetDiscardValueNames(LLVMContextRef C, [MarshalAs( UnmanagedType.Bool )] bool Discard);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMGetDiagInfoDescription(LLVMDiagnosticInfoRef DI);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDiagnosticSeverity LLVMGetDiagInfoSeverity(LLVMDiagnosticInfoRef DI);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetMDKindIDInContext(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, uint SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetMDKindID([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, uint SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetSyncScopeID(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetEnumAttributeKindForName([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetLastEnumAttributeKind();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMCreateEnumAttribute(LLVMContextRef C, uint KindID, UInt64 Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetEnumAttributeKind(LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetEnumAttributeValue(LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMCreateTypeAttribute(LLVMContextRef C, uint KindID, LLVMTypeRef type_ref);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTypeAttributeValue(LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMCreateConstantRangeAttribute(LLVMContextRef C, uint KindID, uint NumBits, [In] UInt64[] LowerWords, [In] UInt64[] UpperWords);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMCreateStringAttribute(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string K, uint KLength, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string V, uint VLength);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetStringAttributeKind(LLVMAttributeRef A, out uint Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetStringAttributeValue(LLVMAttributeRef A, out uint Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsEnumAttribute(LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsStringAttribute(LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsTypeAttribute(LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTypeByName2(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRef LLVMModuleCreateWithName([MarshalUsing( typeof( AnsiStringMarshaller ) )] string ModuleID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRef LLVMModuleCreateWithNameInContext([MarshalUsing( typeof( AnsiStringMarshaller ) )] string ModuleID, LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRef LLVMCloneModule(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsNewDbgInfoFormat(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetIsNewDbgInfoFormat(LLVMModuleRef M, [MarshalAs( UnmanagedType.Bool )] bool UseNewFormat);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetModuleIdentifier(LLVMModuleRef M, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetModuleIdentifier(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Ident, size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetSourceFileName(LLVMModuleRef M, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSourceFileName(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetDataLayoutStr(LLVMModuleRef M);

        [Obsolete( "Use LLVMGetDataLayoutStr instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetDataLayout(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetDataLayout(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string DataLayoutStr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetTarget(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTarget(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleFlagEntry LLVMCopyModuleFlagsMetadata(LLVMModuleRef M, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleFlagBehavior LLVMModuleFlagEntriesGetFlagBehavior(LLVMModuleFlagEntry Entries, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMModuleFlagEntriesGetKey(LLVMModuleFlagEntry Entries, uint Index, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMModuleFlagEntriesGetMetadata(LLVMModuleFlagEntry Entries, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMGetModuleFlag(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Key, size_t KeyLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddModuleFlag(LLVMModuleRef M, LLVMModuleFlagBehavior Behavior, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Key, size_t KeyLen, LLVMMetadataRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDumpModule(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMPrintModuleToFile(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Filename, [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMPrintModuleToString(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetModuleInlineAsm(LLVMModuleRef M, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetModuleInlineAsm2(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Asm, size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAppendModuleInlineAsm(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Asm, size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetInlineAsm(LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string AsmString, size_t AsmStringSize, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Constraints, size_t ConstraintsSize, [MarshalAs( UnmanagedType.Bool )] bool HasSideEffects, [MarshalAs( UnmanagedType.Bool )] bool IsAlignStack, LLVMInlineAsmDialect Dialect, [MarshalAs( UnmanagedType.Bool )] bool CanThrow);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetInlineAsmAsmString(LLVMValueRef InlineAsmVal, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetInlineAsmConstraintString(LLVMValueRef InlineAsmVal, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMInlineAsmDialect LLVMGetInlineAsmDialect(LLVMValueRef InlineAsmVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetInlineAsmFunctionType(LLVMValueRef InlineAsmVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetInlineAsmHasSideEffects(LLVMValueRef InlineAsmVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetInlineAsmNeedsAlignedStack(LLVMValueRef InlineAsmVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetInlineAsmCanUnwind(LLVMValueRef InlineAsmVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMGetModuleContext(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTypeByName(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetFirstNamedMetadata(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetLastNamedMetadata(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetNextNamedMetadata(LLVMNamedMDNodeRef NamedMDNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetPreviousNamedMetadata(LLVMNamedMDNodeRef NamedMDNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetNamedMetadata(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetOrInsertNamedMetadata(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetNamedMetadataName(LLVMNamedMDNodeRef NamedMD, out size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNamedMetadataNumOperands(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetNamedMetadataOperands(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, out LLVMValueRef Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddNamedMetadataOperand(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetDebugLocDirectory(LLVMValueRef Val, out uint Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetDebugLocFilename(LLVMValueRef Val, out uint Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetDebugLocLine(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetDebugLocColumn(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddFunction(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, LLVMTypeRef FunctionTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNamedFunction(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNamedFunctionWithLength(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstFunction(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastFunction(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextFunction(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousFunction(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetModuleInlineAsm(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Asm);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeKind LLVMGetTypeKind(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTypeIsSized(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMGetTypeContext(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDumpType(LLVMTypeRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMPrintTypeToString(LLVMTypeRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt1TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt8TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt16TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt32TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt64TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt128TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntTypeInContext(LLVMContextRef C, uint NumBits);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt1Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt8Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt16Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt32Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt64Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt128Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntType(uint NumBits);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetIntTypeWidth(LLVMTypeRef IntegerTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMHalfTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMBFloatTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFloatTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMDoubleTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86FP80TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFP128TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPPCFP128TypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMHalfType();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMBFloatType();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFloatType();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMDoubleType();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86FP80Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFP128Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPPCFP128Type();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFunctionType(LLVMTypeRef ReturnType, [In] LLVMTypeRef[] ParamTypes, uint ParamCount, [MarshalAs( UnmanagedType.Bool )] bool IsVarArg);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsFunctionVarArg(LLVMTypeRef FunctionTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetReturnType(LLVMTypeRef FunctionTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountParamTypes(LLVMTypeRef FunctionTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetParamTypes(LLVMTypeRef FunctionTy, [In][Out] LLVMTypeRef[] Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructTypeInContext(LLVMContextRef C, [In] LLVMTypeRef[] ElementTypes, uint ElementCount, [MarshalAs( UnmanagedType.Bool )] bool Packed);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructType(out LLVMTypeRef ElementTypes, uint ElementCount, [MarshalAs( UnmanagedType.Bool )] bool Packed);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructCreateNamed(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetStructName(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMStructSetBody(LLVMTypeRef StructTy, [In] LLVMTypeRef[] ElementTypes, uint ElementCount, [MarshalAs( UnmanagedType.Bool )] bool Packed);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountStructElementTypes(LLVMTypeRef StructTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetStructElementTypes(LLVMTypeRef StructTy, out LLVMTypeRef Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructGetTypeAtIndex(LLVMTypeRef StructTy, uint i);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsPackedStruct(LLVMTypeRef StructTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsOpaqueStruct(LLVMTypeRef StructTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsLiteralStruct(LLVMTypeRef StructTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetElementType(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetSubtypes(LLVMTypeRef Tp, out LLVMTypeRef Arr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumContainedTypes(LLVMTypeRef Tp);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMArrayType(LLVMTypeRef ElementType, uint ElementCount);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMArrayType2(LLVMTypeRef ElementType, UInt64 ElementCount);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetArrayLength(LLVMTypeRef ArrayTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetArrayLength2(LLVMTypeRef ArrayTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPointerType(LLVMTypeRef ElementType, uint AddressSpace);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMPointerTypeIsOpaque(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPointerTypeInContext(LLVMContextRef C, uint AddressSpace);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetPointerAddressSpace(LLVMTypeRef PointerTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMVectorType(LLVMTypeRef ElementType, uint ElementCount);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMScalableVectorType(LLVMTypeRef ElementType, uint ElementCount);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetVectorSize(LLVMTypeRef VectorTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthPointer(LLVMValueRef PtrAuth);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthKey(LLVMValueRef PtrAuth);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthDiscriminator(LLVMValueRef PtrAuth);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthAddrDiscriminator(LLVMValueRef PtrAuth);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMVoidTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMLabelTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86AMXTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMTokenTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMMetadataTypeInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMVoidType();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMLabelType();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86AMXType();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMTargetExtTypeInContext(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, out LLVMTypeRef TypeParams, uint TypeParamCount, out uint IntParams, uint IntParamCount);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetTargetExtTypeName(LLVMTypeRef TargetExtTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetTargetExtTypeNumTypeParams(LLVMTypeRef TargetExtTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTargetExtTypeTypeParam(LLVMTypeRef TargetExtTy, uint Idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetTargetExtTypeNumIntParams(LLVMTypeRef TargetExtTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetTargetExtTypeIntParam(LLVMTypeRef TargetExtTy, uint Idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMTypeOf(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueKind LLVMGetValueKind(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetValueName2(LLVMValueRef Val, out size_t Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetValueName2(LLVMValueRef Val, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDumpValue(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMPrintValueToString(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRef LLVMGetValueContext(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMPrintDbgRecordToString(LLVMDbgRecordRef Record);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMReplaceAllUsesWith(LLVMValueRef OldVal, LLVMValueRef NewVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsConstant(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsUndef(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsPoison(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAArgument(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABasicBlock(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInlineAsm(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUser(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstant(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABlockAddress(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantAggregateZero(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantArray(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantDataSequential(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantDataArray(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantDataVector(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantExpr(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantFP(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantInt(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantPointerNull(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantStruct(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantTokenNone(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantVector(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantPtrAuth(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalValue(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalAlias(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalObject(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFunction(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalVariable(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalIFunc(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUndefValue(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAPoisonValue(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInstruction(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUnaryOperator(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABinaryOperator(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACallInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAIntrinsicInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgInfoIntrinsic(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgVariableIntrinsic(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgDeclareInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgLabelInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemIntrinsic(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemCpyInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemMoveInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemSetInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACmpInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFCmpInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAICmpInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAExtractElementInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGetElementPtrInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInsertElementInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInsertValueInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsALandingPadInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAPHINode(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASelectInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAShuffleVectorInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAStoreInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABranchInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAIndirectBrInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInvokeInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAReturnInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASwitchInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUnreachableInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAResumeInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACleanupReturnInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACatchReturnInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACatchSwitchInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACallBrInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFuncletPadInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACatchPadInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACleanupPadInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUnaryInstruction(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAllocaInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACastInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAddrSpaceCastInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABitCastInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPExtInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPToSIInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPToUIInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPTruncInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAIntToPtrInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAPtrToIntInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASExtInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASIToFPInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsATruncInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUIToFPInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAZExtInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAExtractValueInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsALoadInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAVAArgInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFreezeInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAtomicCmpXchgInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAtomicRMWInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFenceInst(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMDNode(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAValueAsMetadata(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMDString(LLVMValueRef Val);

        [Obsolete( "Use LLVMGetValueName2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetValueName(LLVMValueRef Val);

        [Obsolete( "Use LLVMSetValueName2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetValueName(LLVMValueRef Val, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUseRef LLVMGetFirstUse(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUseRef LLVMGetNextUse(LLVMUseRef U);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetUser(LLVMUseRef U);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetUsedValue(LLVMUseRef U);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetOperand(LLVMValueRef Val, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUseRef LLVMGetOperandUse(LLVMValueRef Val, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetOperand(LLVMValueRef User, uint Index, LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMGetNumOperands(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNull(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstAllOnes(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetUndef(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPoison(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsNull(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstPointerNull(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInt(LLVMTypeRef IntTy, ulong N, [MarshalAs( UnmanagedType.Bool )] bool SignExtend);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntOfArbitraryPrecision(LLVMTypeRef IntTy, uint NumWords, [In] UInt64[] Words);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntOfString(LLVMTypeRef IntTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Text, byte Radix);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntOfStringAndSize(LLVMTypeRef IntTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Text, uint SLen, byte Radix);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstReal(LLVMTypeRef RealTy, double N);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstRealOfString(LLVMTypeRef RealTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Text);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstRealOfStringAndSize(LLVMTypeRef RealTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Text, uint SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMConstIntGetZExtValue(LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial long LLVMConstIntGetSExtValue(LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial double LLVMConstRealGetDouble(LLVMValueRef ConstantVal, [MarshalAs( UnmanagedType.Bool )] out bool losesInfo);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstStringInContext(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, uint Length, [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstStringInContext2(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, size_t Length, [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstString([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, uint Length, [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsConstantString(LLVMValueRef c);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetAsString(LLVMValueRef c, out size_t Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstStructInContext(LLVMContextRef C, [In] LLVMValueRef[] ConstantVals, uint Count, [MarshalAs( UnmanagedType.Bool )] bool Packed);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstStruct(out LLVMValueRef ConstantVals, uint Count, [MarshalAs( UnmanagedType.Bool )] bool Packed);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstArray(LLVMTypeRef ElementTy, [In] LLVMValueRef[] ConstantVals, uint Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstArray2(LLVMTypeRef ElementTy, out LLVMValueRef ConstantVals, UInt64 Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNamedStruct(LLVMTypeRef StructTy, [In] LLVMValueRef[] ConstantVals, uint Count);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetAggregateElement(LLVMValueRef C, uint Idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetElementAsConstant(LLVMValueRef C, uint idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstVector(out LLVMValueRef ScalarConstantVals, uint Size);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstantPtrAuth(LLVMValueRef Ptr, LLVMValueRef Key, LLVMValueRef Disc, LLVMValueRef AddrDisc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOpcode LLVMGetConstOpcode(LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAlignOf(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMSizeOf(LLVMTypeRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNeg(LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWNeg(LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWNeg(LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNot(LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstAdd(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWAdd(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWAdd(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstSub(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWSub(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWSub(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstMul(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWMul(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWMul(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstXor(LLVMValueRef LHSConstant, LLVMValueRef RHSConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstGEP2(LLVMTypeRef Ty, LLVMValueRef ConstantVal, out LLVMValueRef ConstantIndices, uint NumIndices);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInBoundsGEP2(LLVMTypeRef Ty, LLVMValueRef ConstantVal, out LLVMValueRef ConstantIndices, uint NumIndices);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstGEPWithNoWrapFlags(LLVMTypeRef Ty, LLVMValueRef ConstantVal, out LLVMValueRef ConstantIndices, uint NumIndices, LLVMGEPNoWrap NoWrapFlags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstTrunc(LLVMValueRef ConstantVal, LLVMTypeRef ToType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstPtrToInt(LLVMValueRef ConstantVal, LLVMTypeRef ToType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntToPtr(LLVMValueRef ConstantVal, LLVMTypeRef ToType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstBitCast(LLVMValueRef ConstantVal, LLVMTypeRef ToType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstAddrSpaceCast(LLVMValueRef ConstantVal, LLVMTypeRef ToType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstTruncOrBitCast(LLVMValueRef ConstantVal, LLVMTypeRef ToType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstPointerCast(LLVMValueRef ConstantVal, LLVMTypeRef ToType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstExtractElement(LLVMValueRef VectorConstant, LLVMValueRef IndexConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInsertElement(LLVMValueRef VectorConstant, LLVMValueRef ElementValueConstant, LLVMValueRef IndexConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstShuffleVector(LLVMValueRef VectorAConstant, LLVMValueRef VectorBConstant, LLVMValueRef MaskConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBlockAddress(LLVMValueRef F, LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetBlockAddressFunction(LLVMValueRef BlockAddr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetBlockAddressBasicBlock(LLVMValueRef BlockAddr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInlineAsm(LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string AsmString, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Constraints, [MarshalAs( UnmanagedType.Bool )] bool HasSideEffects, [MarshalAs( UnmanagedType.Bool )] bool IsAlignStack);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRefAlias LLVMGetGlobalParent(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsDeclaration(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMLinkage LLVMGetLinkage(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetLinkage(LLVMValueRef Global, LLVMLinkage Linkage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetSection(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSection(LLVMValueRef Global, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Section);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMVisibility LLVMGetVisibility(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetVisibility(LLVMValueRef Global, LLVMVisibility Viz);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDLLStorageClass LLVMGetDLLStorageClass(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetDLLStorageClass(LLVMValueRef Global, LLVMDLLStorageClass Class);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUnnamedAddr LLVMGetUnnamedAddress(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetUnnamedAddress(LLVMValueRef Global, LLVMUnnamedAddr UnnamedAddr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGlobalGetValueType(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasUnnamedAddr(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetUnnamedAddr(LLVMValueRef Global, [MarshalAs( UnmanagedType.Bool )] bool HasUnnamedAddr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetAlignment(LLVMValueRef V);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAlignment(LLVMValueRef V, uint Bytes);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGlobalSetMetadata(LLVMValueRef Global, uint Kind, LLVMMetadataRef MD);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGlobalEraseMetadata(LLVMValueRef Global, uint Kind);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGlobalClearMetadata(LLVMValueRef Global);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueMetadataEntry LLVMGlobalCopyAllMetadata(LLVMValueRef Value, out size_t NumEntries);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMValueMetadataEntriesGetKind(LLVMValueMetadataEntry Entries, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMValueMetadataEntriesGetMetadata(LLVMValueMetadataEntry Entries, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddGlobal(LLVMModuleRef M, LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddGlobalInAddressSpace(LLVMModuleRef M, LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, uint AddressSpace);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNamedGlobal(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNamedGlobalWithLength(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstGlobal(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastGlobal(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextGlobal(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousGlobal(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteGlobal(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetInitializer(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInitializer(LLVMValueRef GlobalVar, LLVMValueRef ConstantVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsThreadLocal(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetThreadLocal(LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )] bool IsThreadLocal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsGlobalConstant(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetGlobalConstant(LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )] bool IsConstant);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMThreadLocalMode LLVMGetThreadLocalMode(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetThreadLocalMode(LLVMValueRef GlobalVar, LLVMThreadLocalMode Mode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsExternallyInitialized(LLVMValueRef GlobalVar);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetExternallyInitialized(LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )] bool IsExtInit);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddAlias2(LLVMModuleRef M, LLVMTypeRef ValueTy, uint AddrSpace, LLVMValueRef Aliasee, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNamedGlobalAlias(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstGlobalAlias(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastGlobalAlias(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextGlobalAlias(LLVMValueRef GA);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousGlobalAlias(LLVMValueRef GA);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAliasGetAliasee(LLVMValueRef Alias);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAliasSetAliasee(LLVMValueRef Alias, LLVMValueRef Aliasee);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteFunction(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasPersonalityFn(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPersonalityFn(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetPersonalityFn(LLVMValueRef Fn, LLVMValueRef PersonalityFn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMLookupIntrinsicID([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetIntrinsicID(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetIntrinsicDeclaration(LLVMModuleRef Mod, uint ID, [In] LLVMTypeRef[] ParamTypes, size_t ParamCount);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntrinsicGetType(LLVMContextRef Ctx, uint ID, [In] LLVMTypeRef[] ParamTypes, size_t ParamCount);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMIntrinsicGetName(uint ID, out size_t NameLength);

        [Obsolete( "Use  LLVMIntrinsicCopyOverloadedName2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMIntrinsicCopyOverloadedName(uint ID, [In] LLVMTypeRef[] ParamTypes, size_t ParamCount, out size_t NameLength);

        [Obsolete( "Use  LLVMIntrinsicCopyOverloadedName2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMIntrinsicCopyOverloadedName2(LLVMModuleRef Mod, uint ID, [In] LLVMTypeRef[] ParamTypes, size_t ParamCount, out size_t NameLength);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIntrinsicIsOverloaded(uint ID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetFunctionCallConv(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetFunctionCallConv(LLVMValueRef Fn, uint CC);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetGC(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetGC(LLVMValueRef Fn, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPrefixData(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasPrefixData(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetPrefixData(LLVMValueRef Fn, LLVMValueRef prefixData);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPrologueData(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasPrologueData(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetPrologueData(LLVMValueRef Fn, LLVMValueRef prologueData);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddAttributeAtIndex(LLVMValueRef F, LLVMAttributeIndex Idx, LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetAttributeCountAtIndex(LLVMValueRef F, LLVMAttributeIndex Idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetAttributesAtIndex(LLVMValueRef F, LLVMAttributeIndex Idx, [In][Out] LLVMAttributeRef[] Attrs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetEnumAttributeAtIndex(LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetStringAttributeAtIndex(LLVMValueRef F, LLVMAttributeIndex Idx, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string K, uint KLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveEnumAttributeAtIndex(LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveStringAttributeAtIndex(LLVMValueRef F, LLVMAttributeIndex Idx, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string K, uint KLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddTargetDependentFunctionAttr(LLVMValueRef Fn, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string A, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string V);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountParams(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetParams(LLVMValueRef Fn, out LLVMValueRef Params);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetParam(LLVMValueRef Fn, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetParamParent(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstParam(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastParam(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextParam(LLVMValueRef Arg);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousParam(LLVMValueRef Arg);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetParamAlignment(LLVMValueRef Arg, uint Align);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddGlobalIFunc(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMTypeRef Ty, uint AddrSpace, LLVMValueRef Resolver);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNamedGlobalIFunc(LLVMModuleRef M, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstGlobalIFunc(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastGlobalIFunc(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextGlobalIFunc(LLVMValueRef IFunc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousGlobalIFunc(LLVMValueRef IFunc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetGlobalIFuncResolver(LLVMValueRef IFunc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetGlobalIFuncResolver(LLVMValueRef IFunc, LLVMValueRef Resolver);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMEraseGlobalIFunc(LLVMValueRef IFunc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveGlobalIFunc(LLVMValueRef IFunc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMMDStringInContext2(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, size_t SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMMDNodeInContext2(LLVMContextRef C, [In] LLVMMetadataRef[] MDs, size_t Count);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMMetadataAsValue(LLVMContextRef C, LLVMMetadataRef MD);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMValueAsMetadata(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetMDString(LLVMValueRef V, out uint Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetMDNodeNumOperands(LLVMValueRef V);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetMDNodeOperands(LLVMValueRef V, out LLVMValueRef Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMReplaceMDNodeOperandWith(LLVMValueRef V, uint Index, LLVMMetadataRef Replacement);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMMDStringInContext(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, uint SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMMDString([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, uint SLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMMDNodeInContext(LLVMContextRef C, out LLVMValueRef Vals, uint Count);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMMDNode(out LLVMValueRef Vals, uint Count);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOperandBundleRef LLVMCreateOperandBundle([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Tag, size_t TagLen, out LLVMValueRef Args, uint NumArgs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeOperandBundle(LLVMOperandBundleRef Bundle);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetOperandBundleTag(LLVMOperandBundleRef Bundle, out size_t Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumOperandBundleArgs(LLVMOperandBundleRef Bundle);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetOperandBundleArgAtIndex(LLVMOperandBundleRef Bundle, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBasicBlockAsValue(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMValueIsBasicBlock(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMValueAsBasicBlock(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMGetBasicBlockName(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetBasicBlockParent(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetBasicBlockTerminator(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountBasicBlocks(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetBasicBlocks(LLVMValueRef Fn, [In] LLVMBasicBlockRef[] BasicBlocks);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetFirstBasicBlock(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetLastBasicBlock(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetNextBasicBlock(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetPreviousBasicBlock(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetEntryBasicBlock(LLVMValueRef Fn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInsertExistingBasicBlockAfterInsertBlock(LLVMBuilderRef Builder, LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAppendExistingBasicBlock(LLVMValueRef Fn, LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMCreateBasicBlockInContext(LLVMContextRef C, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMAppendBasicBlockInContext(LLVMContextRef C, LLVMValueRef Fn, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMAppendBasicBlock(LLVMValueRef Fn, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMInsertBasicBlockInContext(LLVMContextRef C, LLVMBasicBlockRef BB, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMInsertBasicBlock(LLVMBasicBlockRef InsertBeforeBB, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteBasicBlock(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveBasicBlockFromParent(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveBasicBlockBefore(LLVMBasicBlockRef BB, LLVMBasicBlockRef MovePos);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveBasicBlockAfter(LLVMBasicBlockRef BB, LLVMBasicBlockRef MovePos);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstInstruction(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastInstruction(LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasMetadata(LLVMValueRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetMetadata(LLVMValueRef Val, uint KindID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetMetadata(LLVMValueRef Val, uint KindID, LLVMValueRef Node);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueMetadataEntry LLVMInstructionGetAllMetadataOtherThanDebugLoc(LLVMValueRef Instr, out size_t NumEntries);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetInstructionParent(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextInstruction(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousInstruction(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstructionRemoveFromParent(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstructionEraseFromParent(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteInstruction(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOpcode LLVMGetInstructionOpcode(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMIntPredicate LLVMGetICmpPredicate(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMRealPredicate LLVMGetFCmpPredicate(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMInstructionClone(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsATerminatorInst(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetFirstDbgRecord(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetLastDbgRecord(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetNextDbgRecord(LLVMDbgRecordRef DbgRecord);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetPreviousDbgRecord(LLVMDbgRecordRef DbgRecord);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumArgOperands(LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInstructionCallConv(LLVMValueRef Instr, uint CC);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetInstructionCallConv(LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInstrParamAlignment(LLVMValueRef Instr, LLVMAttributeIndex Idx, uint Align);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddCallSiteAttribute(LLVMValueRef C, LLVMAttributeIndex Idx, LLVMAttributeRef A);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetCallSiteAttributeCount(LLVMValueRef C, LLVMAttributeIndex Idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetCallSiteAttributes(LLVMValueRef C, LLVMAttributeIndex Idx, [In][Out] LLVMAttributeRef[] Attrs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetCallSiteEnumAttribute(LLVMValueRef C, LLVMAttributeIndex Idx, uint KindID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetCallSiteStringAttribute(LLVMValueRef C, LLVMAttributeIndex Idx, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string K, uint KLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveCallSiteEnumAttribute(LLVMValueRef C, LLVMAttributeIndex Idx, uint KindID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveCallSiteStringAttribute(LLVMValueRef C, LLVMAttributeIndex Idx, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string K, uint KLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetCalledFunctionType(LLVMValueRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetCalledValue(LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumOperandBundles(LLVMValueRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOperandBundleRef LLVMGetOperandBundleAtIndex(LLVMValueRef C, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsTailCall(LLVMValueRef CallInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTailCall(LLVMValueRef CallInst, [MarshalAs( UnmanagedType.Bool )] bool IsTailCall);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTailCallKind LLVMGetTailCallKind(LLVMValueRef CallInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTailCallKind(LLVMValueRef CallInst, LLVMTailCallKind kind);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetNormalDest(LLVMValueRef InvokeInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetUnwindDest(LLVMValueRef InvokeInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNormalDest(LLVMValueRef InvokeInst, LLVMBasicBlockRef B);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetUnwindDest(LLVMValueRef InvokeInst, LLVMBasicBlockRef B);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetCallBrDefaultDest(LLVMValueRef CallBr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetCallBrNumIndirectDests(LLVMValueRef CallBr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetCallBrIndirectDest(LLVMValueRef CallBr, uint Idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumSuccessors(LLVMValueRef Term);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetSuccessor(LLVMValueRef Term, uint i);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSuccessor(LLVMValueRef Term, uint i, LLVMBasicBlockRef block);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsConditional(LLVMValueRef Branch);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetCondition(LLVMValueRef Branch);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCondition(LLVMValueRef Branch, LLVMValueRef Cond);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetSwitchDefaultDest(LLVMValueRef SwitchInstr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetAllocatedType(LLVMValueRef Alloca);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsInBounds(LLVMValueRef GEP);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetIsInBounds(LLVMValueRef GEP, [MarshalAs( UnmanagedType.Bool )] bool InBounds);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetGEPSourceElementType(LLVMValueRef GEP);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGEPNoWrap LLVMGEPGetNoWrapFlags(LLVMValueRef GEP);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGEPSetNoWrapFlags(LLVMValueRef GEP, LLVMGEPNoWrap NoWrapFlags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddIncoming(LLVMValueRef PhiNode, [In] LLVMValueRef[] IncomingValues, [In] LLVMBasicBlockRef[] IncomingBlocks, uint Count);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountIncoming(LLVMValueRef PhiNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetIncomingValue(LLVMValueRef PhiNode, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetIncomingBlock(LLVMValueRef PhiNode, uint Index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumIndices(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint* LLVMGetIndices(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBuilderRef LLVMCreateBuilderInContext(LLVMContextRef C);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBuilderRef LLVMCreateBuilder();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilder(LLVMBuilderRef Builder, LLVMBasicBlockRef Block, LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderBeforeDbgRecords(LLVMBuilderRef Builder, LLVMBasicBlockRef Block, LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderBefore(LLVMBuilderRef Builder, LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderBeforeInstrAndDbgRecords(LLVMBuilderRef Builder, LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderAtEnd(LLVMBuilderRef Builder, LLVMBasicBlockRef Block);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetInsertBlock(LLVMBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMClearInsertionPosition(LLVMBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInsertIntoBuilder(LLVMBuilderRef Builder, LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInsertIntoBuilderWithName(LLVMBuilderRef Builder, LLVMValueRef Instr, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMGetCurrentDebugLocation2(LLVMBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCurrentDebugLocation2(LLVMBuilderRef Builder, LLVMMetadataRef Loc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInstDebugLocation(LLVMBuilderRef Builder, LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddMetadataToInst(LLVMBuilderRef Builder, LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMBuilderGetDefaultFPMathTag(LLVMBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMBuilderSetDefaultFPMathTag(LLVMBuilderRef Builder, LLVMMetadataRef FPMathTag);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRef LLVMGetBuilderContext(LLVMBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCurrentDebugLocation(LLVMBuilderRef Builder, LLVMValueRef L);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetCurrentDebugLocation(LLVMBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildRetVoid(LLVMBuilderRef _0);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildRet(LLVMBuilderRef _0, LLVMValueRef V);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAggregateRet(LLVMBuilderRef _0, out LLVMValueRef RetVals, uint N);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildBr(LLVMBuilderRef _0, LLVMBasicBlockRef Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCondBr(LLVMBuilderRef _0, LLVMValueRef If, LLVMBasicBlockRef Then, LLVMBasicBlockRef Else);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSwitch(LLVMBuilderRef _0, LLVMValueRef V, LLVMBasicBlockRef Else, uint NumCases);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIndirectBr(LLVMBuilderRef B, LLVMValueRef Addr, uint NumDests);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCallBr(LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Fn, LLVMBasicBlockRef DefaultDest, out LLVMBasicBlockRef IndirectDests, uint NumIndirectDests, out LLVMValueRef Args, uint NumArgs, out LLVMOperandBundleRef Bundles, uint NumBundles, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInvoke2(LLVMBuilderRef _0, LLVMTypeRef Ty, LLVMValueRef Fn, [In] LLVMValueRef[] Args, uint NumArgs, LLVMBasicBlockRef Then, LLVMBasicBlockRef Catch, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInvokeWithOperandBundles(LLVMBuilderRef _0, LLVMTypeRef Ty, LLVMValueRef Fn, out LLVMValueRef Args, uint NumArgs, LLVMBasicBlockRef Then, LLVMBasicBlockRef Catch, out LLVMOperandBundleRef Bundles, uint NumBundles, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildUnreachable(LLVMBuilderRef _0);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildResume(LLVMBuilderRef B, LLVMValueRef Exn);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildLandingPad(LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef PersFn, uint NumClauses, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCleanupRet(LLVMBuilderRef B, LLVMValueRef CatchPad, LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCatchRet(LLVMBuilderRef B, LLVMValueRef CatchPad, LLVMBasicBlockRef BB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCatchPad(LLVMBuilderRef B, LLVMValueRef ParentPad, out LLVMValueRef Args, uint NumArgs, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCleanupPad(LLVMBuilderRef B, LLVMValueRef ParentPad, out LLVMValueRef Args, uint NumArgs, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCatchSwitch(LLVMBuilderRef B, LLVMValueRef ParentPad, LLVMBasicBlockRef UnwindBB, uint NumHandlers, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddCase(LLVMValueRef Switch, LLVMValueRef OnVal, LLVMBasicBlockRef Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddDestination(LLVMValueRef IndirectBr, LLVMBasicBlockRef Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumClauses(LLVMValueRef LandingPad);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetClause(LLVMValueRef LandingPad, uint Idx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddClause(LLVMValueRef LandingPad, LLVMValueRef ClauseVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsCleanup(LLVMValueRef LandingPad);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCleanup(LLVMValueRef LandingPad, [MarshalAs( UnmanagedType.Bool )] bool Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddHandler(LLVMValueRef CatchSwitch, LLVMBasicBlockRef Dest);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumHandlers(LLVMValueRef CatchSwitch);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetHandlers(LLVMValueRef CatchSwitch, out LLVMBasicBlockRef Handlers);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetArgOperand(LLVMValueRef Funclet, uint i);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetArgOperand(LLVMValueRef Funclet, uint i, LLVMValueRef value);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetParentCatchSwitch(LLVMValueRef CatchPad);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetParentCatchSwitch(LLVMValueRef CatchPad, LLVMValueRef CatchSwitch);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAdd(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWAdd(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWAdd(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFAdd(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSub(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWSub(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWSub(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFSub(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMul(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWMul(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWMul(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFMul(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildUDiv(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExactUDiv(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSDiv(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExactSDiv(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFDiv(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildURem(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSRem(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFRem(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildShl(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildLShr(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAShr(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAnd(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildOr(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildXor(LLVMBuilderRef _0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildBinOp(LLVMBuilderRef B, LLVMOpcode Op, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNeg(LLVMBuilderRef _0, LLVMValueRef V, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWNeg(LLVMBuilderRef B, LLVMValueRef V, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWNeg(LLVMBuilderRef B, LLVMValueRef V, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFNeg(LLVMBuilderRef _0, LLVMValueRef V, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNot(LLVMBuilderRef _0, LLVMValueRef V, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetNUW(LLVMValueRef ArithInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNUW(LLVMValueRef ArithInst, [MarshalAs( UnmanagedType.Bool )] bool HasNUW);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetNSW(LLVMValueRef ArithInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNSW(LLVMValueRef ArithInst, [MarshalAs( UnmanagedType.Bool )] bool HasNSW);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetExact(LLVMValueRef DivOrShrInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetExact(LLVMValueRef DivOrShrInst, [MarshalAs( UnmanagedType.Bool )] bool IsExact);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetNNeg(LLVMValueRef NonNegInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNNeg(LLVMValueRef NonNegInst, [MarshalAs( UnmanagedType.Bool )] bool IsNonNeg);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMFastMath LLVMGetFastMathFlags(LLVMValueRef FPMathInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetFastMathFlags(LLVMValueRef FPMathInst, LLVMFastMath FMF);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMCanValueUseFastMathFlags(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetIsDisjoint(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetIsDisjoint(LLVMValueRef Inst, [MarshalAs( UnmanagedType.Bool )] bool IsDisjoint);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMalloc(LLVMBuilderRef _0, LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildArrayMalloc(LLVMBuilderRef _0, LLVMTypeRef Ty, LLVMValueRef Val, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMemSet(LLVMBuilderRef B, LLVMValueRef Ptr, LLVMValueRef Val, LLVMValueRef Len, uint Align);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMemCpy(LLVMBuilderRef B, LLVMValueRef Dst, uint DstAlign, LLVMValueRef Src, uint SrcAlign, LLVMValueRef Size);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMemMove(LLVMBuilderRef B, LLVMValueRef Dst, uint DstAlign, LLVMValueRef Src, uint SrcAlign, LLVMValueRef Size);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAlloca(LLVMBuilderRef _0, LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildArrayAlloca(LLVMBuilderRef _0, LLVMTypeRef Ty, LLVMValueRef Val, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFree(LLVMBuilderRef _0, LLVMValueRef PointerVal);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildLoad2(LLVMBuilderRef _0, LLVMTypeRef Ty, LLVMValueRef PointerVal, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildStore(LLVMBuilderRef _0, LLVMValueRef Val, LLVMValueRef Ptr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGEP2(LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, [In] LLVMValueRef[] Indices, uint NumIndices, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInBoundsGEP2(LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, [In] LLVMValueRef[] Indices, uint NumIndices, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGEPWithNoWrapFlags(LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, out LLVMValueRef Indices, uint NumIndices, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, LLVMGEPNoWrap NoWrapFlags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildStructGEP2(LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, uint Idx, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGlobalString(LLVMBuilderRef B, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGlobalStringPtr(LLVMBuilderRef B, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Str, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetVolatile(LLVMValueRef MemoryAccessInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetVolatile(LLVMValueRef MemoryAccessInst, [MarshalAs( UnmanagedType.Bool )] bool IsVolatile);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetWeak(LLVMValueRef CmpXchgInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetWeak(LLVMValueRef CmpXchgInst, [MarshalAs( UnmanagedType.Bool )] bool IsWeak);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicOrdering LLVMGetOrdering(LLVMValueRef MemoryAccessInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetOrdering(LLVMValueRef MemoryAccessInst, LLVMAtomicOrdering Ordering);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicRMWBinOp LLVMGetAtomicRMWBinOp(LLVMValueRef AtomicRMWInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAtomicRMWBinOp(LLVMValueRef AtomicRMWInst, LLVMAtomicRMWBinOp BinOp);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildTrunc(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildZExt(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSExt(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPToUI(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPToSI(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildUIToFP(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSIToFP(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPTrunc(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPExt(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPtrToInt(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIntToPtr(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildBitCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAddrSpaceCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildZExtOrBitCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSExtOrBitCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildTruncOrBitCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCast(LLVMBuilderRef B, LLVMOpcode Op, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPointerCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIntCast2(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.Bool )] bool IsSigned, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIntCast(LLVMBuilderRef _0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOpcode LLVMGetCastOpcode(LLVMValueRef Src, [MarshalAs( UnmanagedType.Bool )] bool SrcIsSigned, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.Bool )] bool DestIsSigned);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildICmp(LLVMBuilderRef _0, LLVMIntPredicate Op, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFCmp(LLVMBuilderRef _0, LLVMRealPredicate Op, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPhi(LLVMBuilderRef _0, LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCall2(LLVMBuilderRef _0, LLVMTypeRef _1, LLVMValueRef Fn, [In] LLVMValueRef[] Args, uint NumArgs, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCallWithOperandBundles(LLVMBuilderRef _0, LLVMTypeRef _1, LLVMValueRef Fn, out LLVMValueRef Args, uint NumArgs, out LLVMOperandBundleRef Bundles, uint NumBundles, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSelect(LLVMBuilderRef _0, LLVMValueRef If, LLVMValueRef Then, LLVMValueRef Else, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildVAArg(LLVMBuilderRef _0, LLVMValueRef List, LLVMTypeRef Ty, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExtractElement(LLVMBuilderRef _0, LLVMValueRef VecVal, LLVMValueRef Index, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInsertElement(LLVMBuilderRef _0, LLVMValueRef VecVal, LLVMValueRef EltVal, LLVMValueRef Index, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildShuffleVector(LLVMBuilderRef _0, LLVMValueRef V1, LLVMValueRef V2, LLVMValueRef Mask, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExtractValue(LLVMBuilderRef _0, LLVMValueRef AggVal, uint Index, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInsertValue(LLVMBuilderRef _0, LLVMValueRef AggVal, LLVMValueRef EltVal, uint Index, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFreeze(LLVMBuilderRef _0, LLVMValueRef Val, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIsNull(LLVMBuilderRef _0, LLVMValueRef Val, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIsNotNull(LLVMBuilderRef _0, LLVMValueRef Val, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPtrDiff2(LLVMBuilderRef _0, LLVMTypeRef ElemTy, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFence(LLVMBuilderRef B, LLVMAtomicOrdering ordering, [MarshalAs( UnmanagedType.Bool )] bool singleThread, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFenceSyncScope(LLVMBuilderRef B, LLVMAtomicOrdering ordering, uint SSID, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicRMW(LLVMBuilderRef B, LLVMAtomicRMWBinOp op, LLVMValueRef PTR, LLVMValueRef Val, LLVMAtomicOrdering ordering, [MarshalAs( UnmanagedType.Bool )] bool singleThread);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicRMWSyncScope(LLVMBuilderRef B, LLVMAtomicRMWBinOp op, LLVMValueRef PTR, LLVMValueRef Val, LLVMAtomicOrdering ordering, uint SSID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicCmpXchg(LLVMBuilderRef B, LLVMValueRef Ptr, LLVMValueRef Cmp, LLVMValueRef New, LLVMAtomicOrdering SuccessOrdering, LLVMAtomicOrdering FailureOrdering, [MarshalAs( UnmanagedType.Bool )] bool SingleThread);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicCmpXchgSyncScope(LLVMBuilderRef B, LLVMValueRef Ptr, LLVMValueRef Cmp, LLVMValueRef New, LLVMAtomicOrdering SuccessOrdering, LLVMAtomicOrdering FailureOrdering, uint SSID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumMaskElements(LLVMValueRef ShuffleVectorInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMGetUndefMaskElem();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMGetMaskValue(LLVMValueRef ShuffleVectorInst, uint Elt);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsAtomicSingleThread(LLVMValueRef AtomicInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAtomicSingleThread(LLVMValueRef AtomicInst, [MarshalAs( UnmanagedType.Bool )] bool SingleThread);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsAtomic(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetAtomicSyncScopeID(LLVMValueRef AtomicInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAtomicSyncScopeID(LLVMValueRef AtomicInst, uint SSID);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicOrdering LLVMGetCmpXchgSuccessOrdering(LLVMValueRef CmpXchgInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCmpXchgSuccessOrdering(LLVMValueRef CmpXchgInst, LLVMAtomicOrdering Ordering);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicOrdering LLVMGetCmpXchgFailureOrdering(LLVMValueRef CmpXchgInst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCmpXchgFailureOrdering(LLVMValueRef CmpXchgInst, LLVMAtomicOrdering Ordering);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleProviderRef LLVMCreateModuleProviderForExistingModule(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateMemoryBufferWithContentsOfFile([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Path, out LLVMMemoryBufferRef OutMemBuf, [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string OutMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateMemoryBufferWithSTDIN(out LLVMMemoryBufferRef OutMemBuf, [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string OutMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRangeCopy([In] byte[] InputData, size_t InputDataLength, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string BufferName);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGetBufferStart(LLVMMemoryBufferRef MemBuf);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial size_t LLVMGetBufferSize(LLVMMemoryBufferRef MemBuf);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassManagerRef LLVMCreatePassManager();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassManagerRef LLVMCreateFunctionPassManagerForModule(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassManagerRef LLVMCreateFunctionPassManager(LLVMModuleProviderRef MP);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMRunPassManager(LLVMPassManagerRef PM, LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMInitializeFunctionPassManager(LLVMPassManagerRef FPM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMRunFunctionPassManager(LLVMPassManagerRef FPM, LLVMValueRef F);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMFinalizeFunctionPassManager(LLVMPassManagerRef FPM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMStartMultithreaded();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMStopMultithreaded();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsMultithreaded();
    }
}
