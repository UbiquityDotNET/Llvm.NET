// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMDiagnosticHandler = delegate* unmanaged[Cdecl]</*LLVMDiagnosticInfoRef*/ nint /*_0*/, void* /*_1*/, void /*retVal*/ >;
    using unsafe LLVMYieldCallback = delegate* unmanaged[Cdecl]< nint /*LLVMContextRefAlias*/ /*_0*/, void* /*_1*/, void /*retVal*/ >;
#pragma warning restore  IDE0065, SA1200

    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "Not flags, tooling detection of such is broken" )]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI" )]
    public enum LLVMOpcode
        : Int32
    {
        LLVMRet = 1,
        LLVMBr = 2,
        LLVMSwitch = 3,
        LLVMIndirectBr = 4,
        LLVMInvoke = 5,
        LLVMUnreachable = 7,
        LLVMCallBr = 67,
        LLVMFNeg = 66,
        LLVMAdd = 8,
        LLVMFAdd = 9,
        LLVMSub = 10,
        LLVMFSub = 11,
        LLVMMul = 12,
        LLVMFMul = 13,
        LLVMUDiv = 14,
        LLVMSDiv = 15,
        LLVMFDiv = 16,
        LLVMURem = 17,
        LLVMSRem = 18,
        LLVMFRem = 19,
        LLVMShl = 20,
        LLVMLShr = 21,
        LLVMAShr = 22,
        LLVMAnd = 23,
        LLVMOr = 24,
        LLVMXor = 25,
        LLVMAlloca = 26,
        LLVMLoad = 27,
        LLVMStore = 28,
        LLVMGetElementPtr = 29,
        LLVMTrunc = 30,
        LLVMZExt = 31,
        LLVMSExt = 32,
        LLVMFPToUI = 33,
        LLVMFPToSI = 34,
        LLVMUIToFP = 35,
        LLVMSIToFP = 36,
        LLVMFPTrunc = 37,
        LLVMFPExt = 38,
        LLVMPtrToInt = 39,
        LLVMIntToPtr = 40,
        LLVMBitCast = 41,
        LLVMAddrSpaceCast = 60,
        LLVMICmp = 42,
        LLVMFCmp = 43,
        LLVMPHI = 44,
        LLVMCall = 45,
        LLVMSelect = 46,
        LLVMUserOp1 = 47,
        LLVMUserOp2 = 48,
        LLVMVAArg = 49,
        LLVMExtractElement = 50,
        LLVMInsertElement = 51,
        LLVMShuffleVector = 52,
        LLVMExtractValue = 53,
        LLVMInsertValue = 54,
        LLVMFreeze = 68,
        LLVMFence = 55,
        LLVMAtomicCmpXchg = 56,
        LLVMAtomicRMW = 57,
        LLVMResume = 58,
        LLVMLandingPad = 59,
        LLVMCleanupRet = 61,
        LLVMCatchRet = 62,
        LLVMCatchPad = 63,
        LLVMCleanupPad = 64,
        LLVMCatchSwitch = 65,
    }

    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "Not flags, tool heuristics are too stupid to deal with it" )]
    public enum LLVMTypeKind
        : Int32
    {
        LLVMVoidTypeKind = 0,
        LLVMHalfTypeKind = 1,
        LLVMFloatTypeKind = 2,
        LLVMDoubleTypeKind = 3,
        LLVMX86_FP80TypeKind = 4,
        LLVMFP128TypeKind = 5,
        LLVMPPC_FP128TypeKind = 6,
        LLVMLabelTypeKind = 7,
        LLVMIntegerTypeKind = 8,
        LLVMFunctionTypeKind = 9,
        LLVMStructTypeKind = 10,
        LLVMArrayTypeKind = 11,
        LLVMPointerTypeKind = 12,
        LLVMVectorTypeKind = 13,
        LLVMMetadataTypeKind = 14,
        LLVMTokenTypeKind = 16,
        LLVMScalableVectorTypeKind = 17,
        LLVMBFloatTypeKind = 18,
        LLVMX86_AMXTypeKind = 19,
        LLVMTargetExtTypeKind = 20,
    }

    public enum LLVMLinkage
        : Int32
    {
        LLVMExternalLinkage = 0,
        LLVMAvailableExternallyLinkage = 1,
        LLVMLinkOnceAnyLinkage = 2,
        LLVMLinkOnceODRLinkage = 3,
        LLVMLinkOnceODRAutoHideLinkage = 4,
        LLVMWeakAnyLinkage = 5,
        LLVMWeakODRLinkage = 6,
        LLVMAppendingLinkage = 7,
        LLVMInternalLinkage = 8,
        LLVMPrivateLinkage = 9,
        LLVMDLLImportLinkage = 10,
        LLVMDLLExportLinkage = 11,
        LLVMExternalWeakLinkage = 12,
        LLVMGhostLinkage = 13,
        LLVMCommonLinkage = 14,
        LLVMLinkerPrivateLinkage = 15,
        LLVMLinkerPrivateWeakLinkage = 16,
    }

    public enum LLVMVisibility
        : Int32
    {
        LLVMDefaultVisibility = 0,
        LLVMHiddenVisibility = 1,
        LLVMProtectedVisibility = 2,
    }

    public enum LLVMUnnamedAddr
        : Int32
    {
        LLVMNoUnnamedAddr = 0,
        LLVMLocalUnnamedAddr = 1,
        LLVMGlobalUnnamedAddr = 2,
    }

    public enum LLVMDLLStorageClass
        : Int32
    {
        LLVMDefaultStorageClass = 0,
        LLVMDLLImportStorageClass = 1,
        LLVMDLLExportStorageClass = 2,
    }

    public enum LLVMCallConv
        : Int32
    {
        LLVMCCallConv = 0,
        LLVMFastCallConv = 8,
        LLVMColdCallConv = 9,
        LLVMGHCCallConv = 10,
        LLVMHiPECallConv = 11,
        LLVMAnyRegCallConv = 13,
        LLVMPreserveMostCallConv = 14,
        LLVMPreserveAllCallConv = 15,
        LLVMSwiftCallConv = 16,
        LLVMCXXFASTTLSCallConv = 17,
        LLVMX86StdcallCallConv = 64,
        LLVMX86FastcallCallConv = 65,
        LLVMARMAPCSCallConv = 66,
        LLVMARMAAPCSCallConv = 67,
        LLVMARMAAPCSVFPCallConv = 68,
        LLVMMSP430INTRCallConv = 69,
        LLVMX86ThisCallCallConv = 70,
        LLVMPTXKernelCallConv = 71,
        LLVMPTXDeviceCallConv = 72,
        LLVMSPIRFUNCCallConv = 75,
        LLVMSPIRKERNELCallConv = 76,
        LLVMIntelOCLBICallConv = 77,
        LLVMX8664SysVCallConv = 78,
        LLVMWin64CallConv = 79,
        LLVMX86VectorCallCallConv = 80,
        LLVMHHVMCallConv = 81,
        LLVMHHVMCCallConv = 82,
        LLVMX86INTRCallConv = 83,
        LLVMAVRINTRCallConv = 84,
        LLVMAVRSIGNALCallConv = 85,
        LLVMAVRBUILTINCallConv = 86,
        LLVMAMDGPUVSCallConv = 87,
        LLVMAMDGPUGSCallConv = 88,
        LLVMAMDGPUPSCallConv = 89,
        LLVMAMDGPUCSCallConv = 90,
        LLVMAMDGPUKERNELCallConv = 91,
        LLVMX86RegCallCallConv = 92,
        LLVMAMDGPUHSCallConv = 93,
        LLVMMSP430BUILTINCallConv = 94,
        LLVMAMDGPULSCallConv = 95,
        LLVMAMDGPUESCallConv = 96,
    }

    public enum LLVMValueKind
        : Int32
    {
        LLVMArgumentValueKind = 0,
        LLVMBasicBlockValueKind = 1,
        LLVMMemoryUseValueKind = 2,
        LLVMMemoryDefValueKind = 3,
        LLVMMemoryPhiValueKind = 4,
        LLVMFunctionValueKind = 5,
        LLVMGlobalAliasValueKind = 6,
        LLVMGlobalIFuncValueKind = 7,
        LLVMGlobalVariableValueKind = 8,
        LLVMBlockAddressValueKind = 9,
        LLVMConstantExprValueKind = 10,
        LLVMConstantArrayValueKind = 11,
        LLVMConstantStructValueKind = 12,
        LLVMConstantVectorValueKind = 13,
        LLVMUndefValueValueKind = 14,
        LLVMConstantAggregateZeroValueKind = 15,
        LLVMConstantDataArrayValueKind = 16,
        LLVMConstantDataVectorValueKind = 17,
        LLVMConstantIntValueKind = 18,
        LLVMConstantFPValueKind = 19,
        LLVMConstantPointerNullValueKind = 20,
        LLVMConstantTokenNoneValueKind = 21,
        LLVMMetadataAsValueValueKind = 22,
        LLVMInlineAsmValueKind = 23,
        LLVMInstructionValueKind = 24,
        LLVMPoisonValueValueKind = 25,
        LLVMConstantTargetNoneValueKind = 26,
        LLVMConstantPtrAuthValueKind = 27,
    }

    public enum LLVMIntPredicate
        : Int32
    {
        None = 0,

        LLVMIntEQ = 32,
        LLVMIntNE = 33,
        LLVMIntUGT = 34,
        LLVMIntUGE = 35,
        LLVMIntULT = 36,
        LLVMIntULE = 37,
        LLVMIntSGT = 38,
        LLVMIntSGE = 39,
        LLVMIntSLT = 40,
        LLVMIntSLE = 41,
    }

    public enum LLVMRealPredicate
        : Int32
    {
        LLVMRealPredicateFalse = 0,
        LLVMRealOEQ = 1,
        LLVMRealOGT = 2,
        LLVMRealOGE = 3,
        LLVMRealOLT = 4,
        LLVMRealOLE = 5,
        LLVMRealONE = 6,
        LLVMRealORD = 7,
        LLVMRealUNO = 8,
        LLVMRealUEQ = 9,
        LLVMRealUGT = 10,
        LLVMRealUGE = 11,
        LLVMRealULT = 12,
        LLVMRealULE = 13,
        LLVMRealUNE = 14,
        LLVMRealPredicateTrue = 15,
    }

    public enum LLVMLandingPadClauseTy
        : Int32
    {
        LLVMLandingPadCatch = 0,
        LLVMLandingPadFilter = 1,
    }

    public enum LLVMThreadLocalMode
        : Int32
    {
        LLVMNotThreadLocal = 0,
        LLVMGeneralDynamicTLSModel = 1,
        LLVMLocalDynamicTLSModel = 2,
        LLVMInitialExecTLSModel = 3,
        LLVMLocalExecTLSModel = 4,
    }

    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "NOT flags; tooling is too simplistic" )]
    public enum LLVMAtomicOrdering
        : Int32
    {
        LLVMAtomicOrderingNotAtomic = 0,
        LLVMAtomicOrderingUnordered = 1,
        LLVMAtomicOrderingMonotonic = 2,
        LLVMAtomicOrderingAcquire = 4,
        LLVMAtomicOrderingRelease = 5,
        LLVMAtomicOrderingAcquireRelease = 6,
        LLVMAtomicOrderingSequentiallyConsistent = 7,
    }

    public enum LLVMAtomicRMWBinOp
        : Int32
    {
        LLVMAtomicRMWBinOpXchg = 0,
        LLVMAtomicRMWBinOpAdd = 1,
        LLVMAtomicRMWBinOpSub = 2,
        LLVMAtomicRMWBinOpAnd = 3,
        LLVMAtomicRMWBinOpNand = 4,
        LLVMAtomicRMWBinOpOr = 5,
        LLVMAtomicRMWBinOpXor = 6,
        LLVMAtomicRMWBinOpMax = 7,
        LLVMAtomicRMWBinOpMin = 8,
        LLVMAtomicRMWBinOpUMax = 9,
        LLVMAtomicRMWBinOpUMin = 10,
        LLVMAtomicRMWBinOpFAdd = 11,
        LLVMAtomicRMWBinOpFSub = 12,
        LLVMAtomicRMWBinOpFMax = 13,
        LLVMAtomicRMWBinOpFMin = 14,
        LLVMAtomicRMWBinOpUIncWrap = 15,
        LLVMAtomicRMWBinOpUDecWrap = 16,
        LLVMAtomicRMWBinOpUSubCond = 17,
        LLVMAtomicRMWBinOpUSubSat = 18,
    }

    public enum LLVMDiagnosticSeverity
        : Int32
    {
        LLVMDSError = 0,
        LLVMDSWarning = 1,
        LLVMDSRemark = 2,
        LLVMDSNote = 3,
    }

    public enum LLVMInlineAsmDialect
        : Int32
    {
        LLVMInlineAsmDialectATT = 0,
        LLVMInlineAsmDialectIntel = 1,
    }

    public enum LLVMModuleFlagBehavior
        : Int32
    {
        LLVMModuleFlagBehaviorError = 0,
        LLVMModuleFlagBehaviorWarning = 1,
        LLVMModuleFlagBehaviorRequire = 2,
        LLVMModuleFlagBehaviorOverride = 3,
        LLVMModuleFlagBehaviorAppend = 4,
        LLVMModuleFlagBehaviorAppendUnique = 5,
    }

    public enum LLVMAttributeIndex
        : Int32
    {
        LLVMAttributeReturnIndex = 0,
        LLVMAttributeFunctionIndex = -1,
    }

    public enum LLVMTailCallKind
        : Int32
    {
        LLVMTailCallKindNone = 0,
        LLVMTailCallKindTail = 1,
        LLVMTailCallKindMustTail = 2,
        LLVMTailCallKindNoTail = 3,
    }

    [Flags]
    [SuppressMessage( "Design", "CA1028:Enum Storage should be Int32", Justification = "Matches ABI" )]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "It has one, name matches ABI" )]
    public enum LLVMFastMathFlags
        : UInt32
    {
        LLVMFastMathAllowReassoc = 1,
        LLVMFastMathNoNaNs = 2,
        LLVMFastMathNoInfs = 4,
        LLVMFastMathNoSignedZeros = 8,
        LLVMFastMathAllowReciprocal = 16,
        LLVMFastMathAllowContract = 32,
        LLVMFastMathApproxFunc = 64,
        LLVMFastMathNone = 0,
        LLVMFastMathAll = 127,
    }

    [Flags]
    [SuppressMessage( "Design", "CA1028:Enum Storage should be Int32", Justification = "Matches ABI" )]
    public enum LLVMGEPNoWrapFlags
        : UInt32
    {
        None = 0,
        LLVMGEPFlagInBounds = 1,
        LLVMGEPFlagNUSW = 2,
        LLVMGEPFlagNUW = 4,
    }

    public static partial class Core
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMShutdown( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetVersion( out uint Major, out uint Minor, out uint Patch );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRef LLVMContextCreate( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMGetGlobalContext( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMContextSetDiagnosticHandler( LLVMContextRefAlias C, LLVMDiagnosticHandler Handler, nint DiagnosticContext );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDiagnosticHandler LLVMContextGetDiagnosticHandler( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMContextGetDiagnosticContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMContextSetYieldCallback( LLVMContextRefAlias C, LLVMYieldCallback Callback, nint OpaqueHandle );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMContextShouldDiscardValueNames( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMContextSetDiscardValueNames( LLVMContextRefAlias C, [MarshalAs( UnmanagedType.Bool )] bool Discard );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LLVMGetDiagInfoDescription( LLVMDiagnosticInfoRef DI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDiagnosticSeverity LLVMGetDiagInfoSeverity( LLVMDiagnosticInfoRef DI );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static uint LLVMGetMDKindIDInContext( LLVMContextRefAlias C, LazyEncodedString Name )
        {
            return LLVMGetMDKindIDInContext( C, Name, Name.NativeStrLen.ToUInt32() );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial uint LLVMGetMDKindIDInContext( LLVMContextRefAlias C, LazyEncodedString Name, uint SLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetMDKindID( LazyEncodedString Name, uint SLen );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static uint LLVMGetSyncScopeID( LLVMContextRefAlias C, LazyEncodedString Name )
        {
            return LLVMGetSyncScopeID( C, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial uint LLVMGetSyncScopeID( LLVMContextRefAlias C, LazyEncodedString Name, nuint SLen );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static uint LLVMGetEnumAttributeKindForName( LazyEncodedString Name )
        {
            return LLVMGetEnumAttributeKindForName( Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial uint LLVMGetEnumAttributeKindForName( LazyEncodedString Name, nuint SLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetLastEnumAttributeKind( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMCreateEnumAttribute( LLVMContextRefAlias C, uint KindID, UInt64 Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetEnumAttributeKind( LLVMAttributeRef A );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetEnumAttributeValue( LLVMAttributeRef A );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMCreateTypeAttribute( LLVMContextRefAlias C, uint KindID, LLVMTypeRef type_ref );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTypeAttributeValue( LLVMAttributeRef A );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMAttributeRef LLVMCreateConstantRangeAttribute(
            LLVMContextRefAlias C,
            uint KindID,
            uint NumBits,
            [In] UInt64[] LowerWords,
            [In] UInt64[] UpperWords
            )
        {
#if DEBUG
            long requiredLen = (NumBits / 64) + ((NumBits % 64) == 0 ? 0 : 1);
            if(LowerWords.LongLength < requiredLen)
            {
                // TODO: Intern and localize this error message format
                throw new ArgumentException( $"Array length ({LowerWords.LongLength}) does not support specified bit width: {NumBits}", nameof( LowerWords ) );
            }

            if(UpperWords.LongLength < requiredLen)
            {
                // TODO: Intern and localize this error message format
                throw new ArgumentException( $"Array length ({UpperWords.LongLength}) does not support specified bit width: {NumBits}", nameof( UpperWords ) );
            }
#endif
            return NativeLLVMCreateConstantRangeAttribute( C, KindID, NumBits, LowerWords, UpperWords );
        }

        [LibraryImport( LibraryName, EntryPoint = "LLVMCreateConstantRangeAttribute" )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMAttributeRef NativeLLVMCreateConstantRangeAttribute(
            LLVMContextRefAlias C,
            uint KindID,
            uint NumBits,
            [In] UInt64[] LowerWords,
            [In] UInt64[] UpperWords
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMAttributeRef LLVMCreateStringAttribute(
            LLVMContextRefAlias C,
            LazyEncodedString K,
            LazyEncodedString V
            )
        {
            return LLVMCreateStringAttribute( C, K, K.NativeStrLen.ToUInt32(), V, V.NativeStrLen.ToUInt32() );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMAttributeRef LLVMCreateStringAttribute(
            LLVMContextRefAlias C,
            LazyEncodedString K,
            uint KLength,
            LazyEncodedString V,
            uint VLength
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetStringAttributeKind( LLVMAttributeRef A )
        {
            unsafe
            {
                byte* p = LLVMGetStringAttributeKind(A, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetStringAttributeKind( LLVMAttributeRef A, out uint Length );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetStringAttributeValue( LLVMAttributeRef A )
        {
            unsafe
            {
                byte* p = LLVMGetStringAttributeValue(A, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetStringAttributeValue( LLVMAttributeRef A, out uint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsEnumAttribute( LLVMAttributeRef A );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsStringAttribute( LLVMAttributeRef A );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsTypeAttribute( LLVMAttributeRef A );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTypeByName2( LLVMContextRefAlias C, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRef LLVMModuleCreateWithName( LazyEncodedString ModuleID );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRef LLVMModuleCreateWithNameInContext( LazyEncodedString ModuleID, LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRef LLVMCloneModule( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsNewDbgInfoFormat( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetIsNewDbgInfoFormat( LLVMModuleRefAlias M, [MarshalAs( UnmanagedType.Bool )] bool UseNewFormat );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetModuleIdentifier( LLVMModuleRefAlias M )
        {
            unsafe
            {
                byte* p = LLVMGetModuleIdentifier(M, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetModuleIdentifier( LLVMModuleRefAlias M, out nuint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void LLVMSetModuleIdentifier( LLVMModuleRefAlias M, LazyEncodedString Ident )
        {
            LLVMSetModuleIdentifier( M, Ident, Ident.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMSetModuleIdentifier( LLVMModuleRefAlias M, LazyEncodedString Ident, nuint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetSourceFileName( LLVMModuleRefAlias M )
        {
            unsafe
            {
                byte* p = LLVMGetSourceFileName(M, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetSourceFileName( LLVMModuleRefAlias M, out nuint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void LLVMSetSourceFileName( LLVMModuleRefAlias M, LazyEncodedString Name )
        {
            LLVMSetSourceFileName( M, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMSetSourceFileName( LLVMModuleRefAlias M, LazyEncodedString Name, nuint Len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetDataLayoutStr( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetDataLayout( LLVMModuleRefAlias M, LazyEncodedString DataLayoutStr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetTarget( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTarget( LLVMModuleRefAlias M, LazyEncodedString Triple );

        // Return is technically an array of opaque pointers that is allocated and owned by the native code.
        // It is treated like a handle for disposal. The metadata for elements is accessed via
        // LLVMDisposeModuleFlagsMetadata.
        // Caller must free the returned array via LLVMDisposeModuleFlagsMetadata which is handled by
        // the Dispose for the global handle. NOTE: The C++ type of Len is size_t BUT the type
        // of index is `unsigned` for `LLVMModuleFlagEntriesGetFlagBehavior` and `LLVMModuleFlagEntriesGetKey`
        // so the types are problematic in full verification. In reality, len will NOT exceed the uint.MaxValue
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( CountElementName = nameof( Len ) )]
        public static unsafe partial LLVMModuleFlagEntry LLVMCopyModuleFlagsMetadata( LLVMModuleRefAlias M, out nuint Len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleFlagBehavior LLVMModuleFlagEntriesGetFlagBehavior( LLVMModuleFlagEntry Entries, uint Index );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMModuleFlagEntriesGetKey( LLVMModuleFlagEntry Entries, uint Index )
        {
            unsafe
            {
                byte* p = LLVMModuleFlagEntriesGetKey(Entries, Index, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMModuleFlagEntriesGetKey( LLVMModuleFlagEntry Entries, uint Index, out nuint Len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMModuleFlagEntriesGetMetadata( LLVMModuleFlagEntry Entries, uint Index );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMGetModuleFlag( LLVMModuleRefAlias M, LazyEncodedString Key )
        {
            return LLVMGetModuleFlag( M, Key, Key.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMGetModuleFlag( LLVMModuleRefAlias M, LazyEncodedString Key, nuint KeyLen );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void LLVMAddModuleFlag(
            LLVMModuleRefAlias M,
            LLVMModuleFlagBehavior Behavior,
            LazyEncodedString Key,
            LLVMMetadataRef Val
            )
        {
            LLVMAddModuleFlag( M, Behavior, Key, Key.NativeStrLen, Val );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMAddModuleFlag(
            LLVMModuleRefAlias M,
            LLVMModuleFlagBehavior Behavior,
            LazyEncodedString Key,
            nuint KeyLen,
            LLVMMetadataRef Val
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDumpModule( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMPrintModuleToFile(
            LLVMModuleRefAlias M,
            LazyEncodedString Filename,
            [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage
            );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LLVMPrintModuleToString( LLVMModuleRefAlias M );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetModuleInlineAsm( LLVMModuleRefAlias M )
        {
            unsafe
            {
                byte* p = LLVMGetModuleInlineAsm(M, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetModuleInlineAsm( LLVMModuleRefAlias M, out nuint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void LLVMSetModuleInlineAsm2( LLVMModuleRefAlias M, LazyEncodedString Asm )
        {
            LLVMSetModuleInlineAsm2( M, Asm, Asm.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMSetModuleInlineAsm2( LLVMModuleRefAlias M, LazyEncodedString Asm, nuint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void LLVMAppendModuleInlineAsm( LLVMModuleRefAlias M, LazyEncodedString? Asm )
        {
            LLVMAppendModuleInlineAsm( M, Asm, Asm?.NativeStrLen ?? 0 );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMAppendModuleInlineAsm( LLVMModuleRefAlias M, LazyEncodedString? Asm, nuint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMGetInlineAsm(
            LLVMTypeRef Ty,
            LazyEncodedString AsmString,
            LazyEncodedString Constraints,
            bool HasSideEffects,
            bool IsAlignStack,
            LLVMInlineAsmDialect Dialect,
            bool CanThrow
            )
        {
            return LLVMGetInlineAsm(
                Ty,
                AsmString,
                AsmString.NativeStrLen,
                Constraints,
                Constraints.NativeStrLen,
                HasSideEffects,
                IsAlignStack,
                Dialect,
                CanThrow
                );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMGetInlineAsm(
            LLVMTypeRef Ty,
            LazyEncodedString AsmString,
            nuint AsmStringSize,
            LazyEncodedString Constraints,
            nuint ConstraintsSize,
            [MarshalAs( UnmanagedType.Bool )] bool HasSideEffects,
            [MarshalAs( UnmanagedType.Bool )] bool IsAlignStack,
            LLVMInlineAsmDialect Dialect,
            [MarshalAs( UnmanagedType.Bool )] bool CanThrow
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetInlineAsmAsmString( LLVMValueRef InlineAsmVal )
        {
            unsafe
            {
                byte* p = LLVMGetInlineAsmAsmString(InlineAsmVal, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetInlineAsmAsmString( LLVMValueRef InlineAsmVal, out nuint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetInlineAsmConstraintString( LLVMValueRef InlineAsmVal )
        {
            unsafe
            {
                byte* p = LLVMGetInlineAsmConstraintString(InlineAsmVal, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetInlineAsmConstraintString( LLVMValueRef InlineAsmVal, out nuint Len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMInlineAsmDialect LLVMGetInlineAsmDialect( LLVMValueRef InlineAsmVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetInlineAsmFunctionType( LLVMValueRef InlineAsmVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetInlineAsmHasSideEffects( LLVMValueRef InlineAsmVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetInlineAsmNeedsAlignedStack( LLVMValueRef InlineAsmVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetInlineAsmCanUnwind( LLVMValueRef InlineAsmVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMGetModuleContext( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTypeByName( LLVMModuleRefAlias M, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetFirstNamedMetadata( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetLastNamedMetadata( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetNextNamedMetadata( LLVMNamedMDNodeRef NamedMDNode );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMNamedMDNodeRef LLVMGetPreviousNamedMetadata( LLVMNamedMDNodeRef NamedMDNode );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMNamedMDNodeRef LLVMGetNamedMetadata( LLVMModuleRefAlias M, LazyEncodedString Name )
        {
            return LLVMGetNamedMetadata( M, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMNamedMDNodeRef LLVMGetNamedMetadata( LLVMModuleRefAlias M, LazyEncodedString Name, nuint NameLen );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMNamedMDNodeRef LLVMGetOrInsertNamedMetadata( LLVMModuleRefAlias M, LazyEncodedString Name )
        {
            return LLVMGetOrInsertNamedMetadata( M, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMNamedMDNodeRef LLVMGetOrInsertNamedMetadata( LLVMModuleRefAlias M, LazyEncodedString Name, nuint NameLen );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetNamedMetadataName( LLVMNamedMDNodeRef NamedMD )
        {
            unsafe
            {
                byte* p = LLVMGetNamedMetadataName(NamedMD, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetNamedMetadataName( LLVMNamedMDNodeRef NamedMD, out nuint NameLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNamedMetadataNumOperands( LLVMModuleRefAlias M, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetNamedMetadataOperands( LLVMModuleRefAlias M, LazyEncodedString Name, [Out] LLVMValueRef[] Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddNamedMetadataOperand( LLVMModuleRefAlias M, LazyEncodedString Name, LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial byte* LLVMGetDebugLocDirectory( LLVMValueRef Val, out uint Length );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetDebugLocFileName( LLVMValueRef Val )
        {
            unsafe
            {
                byte* p = LLVMGetDebugLocFilename(Val, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetDebugLocFilename( LLVMValueRef Val, out uint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetDebugLocLine( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetDebugLocColumn( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddFunction( LLVMModuleRefAlias M, LazyEncodedString Name, LLVMTypeRef FunctionTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNamedFunction( LLVMModuleRefAlias M, LazyEncodedString Name );

        public static LLVMValueRef LLVMGetNamedFunctionWithLength( LLVMModuleRefAlias M, LazyEncodedString Name )
        {
            return LLVMGetNamedFunctionWithLength( M, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMGetNamedFunctionWithLength( LLVMModuleRefAlias M, LazyEncodedString Name, nuint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstFunction( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastFunction( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextFunction( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousFunction( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetModuleInlineAsm( LLVMModuleRefAlias M, LazyEncodedString Asm );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeKind LLVMGetTypeKind( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMTypeIsSized( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMGetTypeContext( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDumpType( LLVMTypeRef Val );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LLVMPrintTypeToString( LLVMTypeRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt1TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt8TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt16TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt32TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt64TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt128TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntTypeInContext( LLVMContextRefAlias C, uint NumBits );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt1Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt8Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt16Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt32Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt64Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMInt128Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntType( uint NumBits );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetIntTypeWidth( LLVMTypeRef IntegerTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMHalfTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMBFloatTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFloatTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMDoubleTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86FP80TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFP128TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPPCFP128TypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMHalfType( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMBFloatType( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFloatType( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMDoubleType( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86FP80Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFP128Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPPCFP128Type( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMFunctionType(
            LLVMTypeRef ReturnType,
            [In] LLVMTypeRef[] ParamTypes,
            uint ParamCount,
            [MarshalAs( UnmanagedType.Bool )] bool IsVarArg
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsFunctionVarArg( LLVMTypeRef FunctionTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetReturnType( LLVMTypeRef FunctionTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountParamTypes( LLVMTypeRef FunctionTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetParamTypes( LLVMTypeRef FunctionTy, [Out] LLVMTypeRef[] Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructTypeInContext(
            LLVMContextRefAlias C,
            [In] LLVMTypeRef[] ElementTypes,
            uint ElementCount,
            [MarshalAs( UnmanagedType.Bool )] bool Packed
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructType( out LLVMTypeRef ElementTypes, uint ElementCount, [MarshalAs( UnmanagedType.Bool )] bool Packed );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructCreateNamed( LLVMContextRefAlias C, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetStructName( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMStructSetBody(
            LLVMTypeRef StructTy,
            [In] LLVMTypeRef[] ElementTypes,
            uint ElementCount,
            [MarshalAs( UnmanagedType.Bool )] bool Packed
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountStructElementTypes( LLVMTypeRef StructTy );

        /// <summary>Gets structure element types</summary>
        /// <param name="StructTy">Structure to get types for</param>
        /// <param name="Dest">Pointer to destination array</param>
        /// <remarks>
        /// <note type="important">
        /// The array <paramref name="Dest"/> MUST contain at least enough space
        /// for LLVMCountStructElementTypes(<paramref name="StructTy"/>) elements.
        /// or memory corruptions will occur. The impacts of such corruption are
        /// usually found in unrelated areas making these types of errors VERY difficult
        /// to find.</note>
        /// </remarks>
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetStructElementTypes( LLVMTypeRef StructTy, [Out] LLVMTypeRef[] Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMStructGetTypeAtIndex( LLVMTypeRef StructTy, uint i );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsPackedStruct( LLVMTypeRef StructTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsOpaqueStruct( LLVMTypeRef StructTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsLiteralStruct( LLVMTypeRef StructTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetElementType( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetSubtypes( LLVMTypeRef Tp, [Out] LLVMTypeRef[] Arr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumContainedTypes( LLVMTypeRef Tp );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMArrayType( LLVMTypeRef ElementType, uint ElementCount );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMArrayType2( LLVMTypeRef ElementType, UInt64 ElementCount );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetArrayLength( LLVMTypeRef ArrayTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetArrayLength2( LLVMTypeRef ArrayTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPointerType( LLVMTypeRef ElementType, uint AddressSpace );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMPointerTypeIsOpaque( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMPointerTypeInContext( LLVMContextRefAlias C, uint AddressSpace );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetPointerAddressSpace( LLVMTypeRef PointerTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMVectorType( LLVMTypeRef ElementType, uint ElementCount );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMScalableVectorType( LLVMTypeRef ElementType, uint ElementCount );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetVectorSize( LLVMTypeRef VectorTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthPointer( LLVMValueRef PtrAuth );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthKey( LLVMValueRef PtrAuth );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthDiscriminator( LLVMValueRef PtrAuth );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetConstantPtrAuthAddrDiscriminator( LLVMValueRef PtrAuth );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMVoidTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMLabelTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86AMXTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMTokenTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMMetadataTypeInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMVoidType( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMLabelType( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMX86AMXType( );

        /// <summary>Create a target specific type in context</summary>
        /// <param name="C">Context to create the type in</param>
        /// <param name="Name">Name of the type</param>
        /// <param name="TypeParams">Array of Type parameters</param>
        /// <param name="TypeParamCount">Count of elements in the <paramref name="TypeParams"/> array</param>
        /// <param name="IntParams">Array of integer parameters</param>
        /// <param name="IntParamCount">Count of elements in the <paramref name="IntParams"/> array</param>
        /// <returns>Handle for newly created type</returns>
        /// <remarks>
        /// <note type="important">It is important to note that the <paramref name="TypeParamCount"/>
        /// and <paramref name="IntParamCount"/> **MUST** contain accurate counts. The Managed to native marshalling doesn't
        /// need or use the CountElementName for "in" parameters and therefore it is up to the caller to ensure these
        /// counts are accurate or memory corruption and crashes are essentially guaranteed, and usually not at the point
        /// of the call!</note>
        /// </remarks>
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMTargetExtTypeInContext(
            LLVMContextRefAlias C,
            LazyEncodedString Name,
            [In] LLVMTypeRef[] TypeParams,
            uint TypeParamCount,
            [In] uint[] IntParams,
            uint IntParamCount
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetTargetExtTypeName( LLVMTypeRef TargetExtTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetTargetExtTypeNumTypeParams( LLVMTypeRef TargetExtTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetTargetExtTypeTypeParam( LLVMTypeRef TargetExtTy, uint Idx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetTargetExtTypeNumIntParams( LLVMTypeRef TargetExtTy );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetTargetExtTypeIntParam( LLVMTypeRef TargetExtTy, uint Idx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMTypeOf( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueKind LLVMGetValueKind( LLVMValueRef Val );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetValueName2( LLVMValueRef Val )
        {
            unsafe
            {
                byte* p = LLVMGetValueName2(Val, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetValueName2( LLVMValueRef Val, out nuint Length );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void LLVMSetValueName2( LLVMValueRef Val, LazyEncodedString Name )
        {
            LLVMSetValueName2( Val, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMSetValueName2( LLVMValueRef Val, LazyEncodedString Name, nuint NameLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDumpValue( LLVMValueRef Val );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LLVMPrintValueToString( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMGetValueContext( LLVMValueRef Val );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LLVMPrintDbgRecordToString( LLVMDbgRecordRef Record );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMReplaceAllUsesWith( LLVMValueRef OldVal, LLVMValueRef NewVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsConstant( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsUndef( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsPoison( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAArgument( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABasicBlock( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInlineAsm( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUser( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstant( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABlockAddress( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantAggregateZero( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantArray( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantDataSequential( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantDataArray( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantDataVector( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantExpr( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantFP( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantInt( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantPointerNull( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantStruct( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantTokenNone( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantVector( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAConstantPtrAuth( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalValue( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalAlias( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalObject( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFunction( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalVariable( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGlobalIFunc( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUndefValue( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAPoisonValue( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInstruction( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUnaryOperator( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABinaryOperator( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACallInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAIntrinsicInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgInfoIntrinsic( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgVariableIntrinsic( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgDeclareInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsADbgLabelInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemIntrinsic( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemCpyInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemMoveInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMemSetInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACmpInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFCmpInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAICmpInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAExtractElementInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAGetElementPtrInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInsertElementInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInsertValueInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsALandingPadInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAPHINode( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASelectInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAShuffleVectorInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAStoreInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABranchInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAIndirectBrInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAInvokeInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAReturnInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASwitchInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUnreachableInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAResumeInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACleanupReturnInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACatchReturnInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACatchSwitchInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACallBrInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFuncletPadInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACatchPadInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACleanupPadInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUnaryInstruction( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAllocaInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsACastInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAddrSpaceCastInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsABitCastInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPExtInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPToSIInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPToUIInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFPTruncInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAIntToPtrInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAPtrToIntInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASExtInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsASIToFPInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsATruncInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAUIToFPInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAZExtInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAExtractValueInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsALoadInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAVAArgInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFreezeInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAtomicCmpXchgInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAAtomicRMWInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAFenceInst( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMDNode( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAValueAsMetadata( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsAMDString( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUseRef LLVMGetFirstUse( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUseRef LLVMGetNextUse( LLVMUseRef U );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetUser( LLVMUseRef U );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetUsedValue( LLVMUseRef U );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetOperand( LLVMValueRef Val, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUseRef LLVMGetOperandUse( LLVMValueRef Val, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetOperand( LLVMValueRef User, uint Index, LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMGetNumOperands( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNull( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstAllOnes( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetUndef( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPoison( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsNull( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstPointerNull( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInt( LLVMTypeRef IntTy, ulong N, [MarshalAs( UnmanagedType.Bool )] bool SignExtend );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntOfArbitraryPrecision( LLVMTypeRef IntTy, uint NumWords, [In] UInt64[] Words );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntOfString( LLVMTypeRef IntTy, LazyEncodedString Text, byte Radix );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntOfStringAndSize( LLVMTypeRef IntTy, LazyEncodedString Text, uint SLen, byte Radix );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstReal( LLVMTypeRef RealTy, double N );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstRealOfString( LLVMTypeRef RealTy, LazyEncodedString Text );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstRealOfStringAndSize( LLVMTypeRef RealTy, LazyEncodedString Text, uint SLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMConstIntGetZExtValue( LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial long LLVMConstIntGetSExtValue( LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial double LLVMConstRealGetDouble( LLVMValueRef ConstantVal, [MarshalAs( UnmanagedType.Bool )] out bool losesInfo );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMConstStringInContext(
            LLVMContextRefAlias C,
            LazyEncodedString Str,
            [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate
            )
        {
            return LLVMConstStringInContext( C, Str, checked((uint)Str.NativeStrLen), DontNullTerminate );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMConstStringInContext(
            LLVMContextRefAlias C,
            LazyEncodedString Str,
            uint Length,
            [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMConstStringInContext2(
            LLVMContextRefAlias C,
            LazyEncodedString Str,
            [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate
            )
        {
            return LLVMConstStringInContext2( C, Str, Str.NativeStrLen, DontNullTerminate );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMConstStringInContext2(
            LLVMContextRefAlias C,
            LazyEncodedString Str,
            nuint Length,
            [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMConstString( LazyEncodedString Str, [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate )
        {
            return LLVMConstString( Str, checked((uint)Str.NativeStrLen), DontNullTerminate );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMConstString( LazyEncodedString Str, uint Length, [MarshalAs( UnmanagedType.Bool )] bool DontNullTerminate );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsConstantString( LLVMValueRef c );

        public static LazyEncodedString? LLVMGetAsString( LLVMValueRef c )
        {
            unsafe
            {
                byte* p = LLVMGetAsString(c, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetAsString( LLVMValueRef c, out nuint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstStructInContext(
            LLVMContextRefAlias C,
            [In] LLVMValueRef[] ConstantVals,
            uint Count,
            [MarshalAs( UnmanagedType.Bool )] bool Packed
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstStruct(
            [In] LLVMValueRef[] ConstantVals, uint Count,
            [MarshalAs( UnmanagedType.Bool )] bool Packed
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstArray( LLVMTypeRef ElementTy, [In] LLVMValueRef[] ConstantVals, uint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstArray2( LLVMTypeRef ElementTy, [In] LLVMValueRef[] ConstantVals, UInt64 Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNamedStruct( LLVMTypeRef StructTy, [In] LLVMValueRef[] ConstantVals, uint Count );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetAggregateElement( LLVMValueRef C, uint Idx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetElementAsConstant( LLVMValueRef C, uint idx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstVector( [In] LLVMValueRef[] ScalarConstantVals, uint Size );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstantPtrAuth( LLVMValueRef Ptr, LLVMValueRef Key, LLVMValueRef Disc, LLVMValueRef AddrDisc );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOpcode LLVMGetConstOpcode( LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAlignOf( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMSizeOf( LLVMTypeRef Ty );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNeg( LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWNeg( LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWNeg( LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNot( LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstAdd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWAdd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWAdd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstSub( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWSub( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWSub( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstMul( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNSWMul( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstNUWMul( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstXor( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstGEP2(
            LLVMTypeRef Ty,
            LLVMValueRef ConstantVal,
            [In] LLVMValueRef[] ConstantIndices,
            uint NumIndices
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInBoundsGEP2(
            LLVMTypeRef Ty,
            LLVMValueRef ConstantVal,
            [In] LLVMValueRef[] ConstantIndices,
            uint NumIndices
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstGEPWithNoWrapFlags(
            LLVMTypeRef Ty,
            LLVMValueRef ConstantVal,
            [In] LLVMValueRef[] ConstantIndices,
            uint NumIndices,
            LLVMGEPNoWrapFlags NoWrapFlags
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstTrunc( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstPtrToInt( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstIntToPtr( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstBitCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstAddrSpaceCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstTruncOrBitCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstPointerCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstExtractElement( LLVMValueRef VectorConstant, LLVMValueRef IndexConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInsertElement(
            LLVMValueRef VectorConstant,
            LLVMValueRef ElementValueConstant,
            LLVMValueRef IndexConstant
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstShuffleVector(
            LLVMValueRef VectorAConstant,
            LLVMValueRef VectorBConstant,
            LLVMValueRef MaskConstant
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBlockAddress( LLVMValueRef F, LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetBlockAddressFunction( LLVMValueRef BlockAddr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetBlockAddressBasicBlock( LLVMValueRef BlockAddr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMConstInlineAsm(
            LLVMTypeRef Ty,
            LazyEncodedString AsmString,
            LazyEncodedString Constraints,
            [MarshalAs( UnmanagedType.Bool )] bool HasSideEffects,
            [MarshalAs( UnmanagedType.Bool )] bool IsAlignStack
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRefAlias LLVMGetGlobalParent( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsDeclaration( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMLinkage LLVMGetLinkage( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetLinkage( LLVMValueRef Global, LLVMLinkage Linkage );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetSection( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSection( LLVMValueRef Global, LazyEncodedString Section );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMVisibility LLVMGetVisibility( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetVisibility( LLVMValueRef Global, LLVMVisibility Viz );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDLLStorageClass LLVMGetDLLStorageClass( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetDLLStorageClass( LLVMValueRef Global, LLVMDLLStorageClass Class );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMUnnamedAddr LLVMGetUnnamedAddress( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetUnnamedAddress( LLVMValueRef Global, LLVMUnnamedAddr UnnamedAddr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGlobalGetValueType( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasUnnamedAddr( LLVMValueRef Global );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetUnnamedAddr( LLVMValueRef Global, [MarshalAs( UnmanagedType.Bool )] bool HasUnnamedAddr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetAlignment( LLVMValueRef V );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAlignment( LLVMValueRef V, uint Bytes );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGlobalSetMetadata( LLVMValueRef Global, uint Kind, LLVMMetadataRef MD );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGlobalEraseMetadata( LLVMValueRef Global, uint Kind );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGlobalClearMetadata( LLVMValueRef Global );

        // Return is technically an array of opaque pointers that is allocated and owned by the native code.
        // It is treated like a handle for disposal. The metadata for elements is accessed via
        // LLVMValueMetadataEntriesGetMetadata.
        // Caller must free the returned array via LLVMDisposeValueMetadataEntries which is handled by
        // the Dispose for the global handle. NOTE: The C++ type of NumEntries is size_t BUT the type
        // of index is `unsigned` for `LLVMValueMetadataEntriesGetKind` and `LLVMValueMetadataEntriesGetMetadata`
        // so the types are problematic in full verification. In reality, NumEntries will NOT exceed the uint.MaxValue
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( CountElementName = nameof( NumEntries ) )]
        public static unsafe partial LLVMValueMetadataEntry LLVMGlobalCopyAllMetadata( LLVMValueRef Value, out nuint NumEntries );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMValueMetadataEntriesGetKind( LLVMValueMetadataEntry Entries, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMValueMetadataEntriesGetMetadata( LLVMValueMetadataEntry Entries, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddGlobal( LLVMModuleRefAlias M, LLVMTypeRef Ty, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddGlobalInAddressSpace(
            LLVMModuleRefAlias M,
            LLVMTypeRef Ty,
            LazyEncodedString Name,
            uint AddressSpace
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMGetNamedGlobal( LLVMModuleRefAlias M, LazyEncodedString Name )
        {
            return LLVMGetNamedGlobalWithLength( M, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMGetNamedGlobalWithLength( LLVMModuleRefAlias M, LazyEncodedString Name, nuint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstGlobal( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastGlobal( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextGlobal( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousGlobal( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteGlobal( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetInitializer( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInitializer( LLVMValueRef GlobalVar, LLVMValueRef ConstantVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsThreadLocal( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetThreadLocal( LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )] bool IsThreadLocal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsGlobalConstant( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetGlobalConstant( LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )] bool IsConstant );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMThreadLocalMode LLVMGetThreadLocalMode( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetThreadLocalMode( LLVMValueRef GlobalVar, LLVMThreadLocalMode Mode );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsExternallyInitialized( LLVMValueRef GlobalVar );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetExternallyInitialized( LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )] bool IsExtInit );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAddAlias2(
            LLVMModuleRefAlias M,
            LLVMTypeRef ValueTy,
            uint AddrSpace,
            LLVMValueRef Aliasee,
            LazyEncodedString Name
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMGetNamedGlobalAlias( LLVMModuleRefAlias M, LazyEncodedString Name )
        {
            return LLVMGetNamedGlobalAlias( M, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMGetNamedGlobalAlias( LLVMModuleRefAlias M, LazyEncodedString Name, nuint NameLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstGlobalAlias( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastGlobalAlias( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextGlobalAlias( LLVMValueRef GA );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousGlobalAlias( LLVMValueRef GA );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMAliasGetAliasee( LLVMValueRef Alias );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAliasSetAliasee( LLVMValueRef Alias, LLVMValueRef Aliasee );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteFunction( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasPersonalityFn( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPersonalityFn( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetPersonalityFn( LLVMValueRef Fn, LLVMValueRef PersonalityFn );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static uint LLVMLookupIntrinsicID( LazyEncodedString Name )
        {
            return LLVMLookupIntrinsicID( Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial uint LLVMLookupIntrinsicID( LazyEncodedString Name, nuint NameLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetIntrinsicID( LLVMValueRef Fn );

        public static LLVMValueRef LLVMGetIntrinsicDeclaration(
            LLVMModuleRefAlias Mod,
            uint ID,
            [In] LLVMTypeRef[] ParamTypes
            )
        {
            return LLVMGetIntrinsicDeclaration( Mod, ID, ParamTypes, checked((nuint)ParamTypes.LongLength) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMGetIntrinsicDeclaration(
            LLVMModuleRefAlias Mod,
            uint ID,
            [In] LLVMTypeRef[] ParamTypes,
            nuint ParamCount
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntrinsicGetType(
            LLVMContextRefAlias Ctx,
            uint ID,
            [In] LLVMTypeRef[] ParamTypes,
            nuint ParamCount
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMIntrinsicGetName( uint ID )
        {
            unsafe
            {
                byte* p = LLVMIntrinsicGetName(ID, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMIntrinsicGetName( uint ID, out nuint NameLength );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMIntrinsicCopyOverloadedName2(
            LLVMModuleRefAlias Mod,
            uint ID,
            [In] LLVMTypeRef[] ParamTypes
            )
        {
            unsafe
            {
                byte* p = LLVMIntrinsicCopyOverloadedName2(Mod, ID, ParamTypes, checked((nuint)ParamTypes.LongLength), out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMIntrinsicCopyOverloadedName2(
            LLVMModuleRefAlias Mod,
            uint ID,
            [In] LLVMTypeRef[] ParamTypes,
            nuint ParamCount,
            out nuint NameLength );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIntrinsicIsOverloaded( uint ID );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetFunctionCallConv( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetFunctionCallConv( LLVMValueRef Fn, uint CC );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetGC( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetGC( LLVMValueRef Fn, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPrefixData( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasPrefixData( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetPrefixData( LLVMValueRef Fn, LLVMValueRef prefixData );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPrologueData( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasPrologueData( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetPrologueData( LLVMValueRef Fn, LLVMValueRef prologueData );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, LLVMAttributeRef A );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetAttributeCountAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetAttributesAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, [Out] LLVMAttributeRef[] Attrs );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetEnumAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetStringAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, byte* K, uint KLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveEnumAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveStringAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, byte* K, uint KLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddTargetDependentFunctionAttr( LLVMValueRef Fn, LazyEncodedString A, LazyEncodedString V );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountParams( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetParams( LLVMValueRef Fn, out LLVMValueRef Params );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetParam( LLVMValueRef Fn, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetParamParent( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstParam( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastParam( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextParam( LLVMValueRef Arg );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousParam( LLVMValueRef Arg );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetParamAlignment( LLVMValueRef Arg, uint Align );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMAddGlobalIFunc(
            LLVMModuleRefAlias M,
            LazyEncodedString Name,
            LLVMTypeRef Ty,
            uint AddrSpace,
            LLVMValueRef Resolver
            )
        {
            return LLVMAddGlobalIFunc( M, Name, Name.NativeStrLen, Ty, AddrSpace, Resolver );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMAddGlobalIFunc(
            LLVMModuleRefAlias M,
            LazyEncodedString Name,
            nuint NameLen,
            LLVMTypeRef Ty,
            uint AddrSpace,
            LLVMValueRef Resolver
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMGetNamedGlobalIFunc( LLVMModuleRefAlias M, LazyEncodedString Name )
        {
            return LLVMGetNamedGlobalIFunc( M, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMGetNamedGlobalIFunc( LLVMModuleRefAlias M, LazyEncodedString Name, nuint NameLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstGlobalIFunc( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastGlobalIFunc( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextGlobalIFunc( LLVMValueRef IFunc );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousGlobalIFunc( LLVMValueRef IFunc );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetGlobalIFuncResolver( LLVMValueRef IFunc );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetGlobalIFuncResolver( LLVMValueRef IFunc, LLVMValueRef Resolver );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMEraseGlobalIFunc( LLVMValueRef IFunc );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveGlobalIFunc( LLVMValueRef IFunc );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMMDStringInContext2( LLVMContextRefAlias C, LazyEncodedString Str )
        {
            return LLVMMDStringInContext2( C, Str, Str.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMMDStringInContext2( LLVMContextRefAlias C, LazyEncodedString Str, nuint SLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMMDNodeInContext2( LLVMContextRefAlias C, [In] LLVMMetadataRef[] MDs, nuint Count );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMMetadataAsValue( LLVMContextRefAlias C, LLVMMetadataRef MD );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMValueAsMetadata( LLVMValueRef Val );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetMDString( LLVMValueRef V )
        {
            unsafe
            {
                byte* p = LLVMGetMDString(V, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetMDString( LLVMValueRef V, out uint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetMDNodeNumOperands( LLVMValueRef V );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetMDNodeOperands( LLVMValueRef V, [Out] LLVMValueRef[] Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMReplaceMDNodeOperandWith( LLVMValueRef V, uint Index, LLVMMetadataRef Replacement );

        public static LLVMValueRef LLVMMDStringInContext( LLVMContextRefAlias C, LazyEncodedString Str )
        {
            return LLVMMDStringInContext( C, Str, checked((uint)Str.NativeStrLen) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMMDStringInContext( LLVMContextRefAlias C, LazyEncodedString Str, uint SLen );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMValueRef LLVMMDString( LazyEncodedString Str )
        {
            return LLVMMDString( Str, checked((uint)Str.NativeStrLen) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMMDString( LazyEncodedString Str, uint SLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMMDNode( [In] LLVMValueRef[] Vals, uint Count );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMOperandBundleRef LLVMCreateOperandBundle( LazyEncodedString Tag, params LLVMValueRef[] Args )
        {
            return LLVMCreateOperandBundle( Tag, Tag.NativeStrLen, Args, checked((uint)Args.Length) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMOperandBundleRef LLVMCreateOperandBundle(
            LazyEncodedString Tag,
            nuint TagLen,
            [In] LLVMValueRef[] Args,
            uint NumArgs
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeOperandBundle( LLVMOperandBundleRef Bundle );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMGetOperandBundleTag( LLVMOperandBundleRef Bundle )
        {
            unsafe
            {
                byte* p = LLVMGetOperandBundleTag(Bundle, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMGetOperandBundleTag( LLVMOperandBundleRef Bundle, out uint Len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumOperandBundleArgs( LLVMOperandBundleRef Bundle );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetOperandBundleArgAtIndex( LLVMOperandBundleRef Bundle, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBasicBlockAsValue( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMValueIsBasicBlock( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMValueAsBasicBlock( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetBasicBlockName( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetBasicBlockParent( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetBasicBlockTerminator( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountBasicBlocks( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetBasicBlocks( LLVMValueRef Fn, [In] LLVMBasicBlockRef[] BasicBlocks );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetFirstBasicBlock( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetLastBasicBlock( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetNextBasicBlock( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetPreviousBasicBlock( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetEntryBasicBlock( LLVMValueRef Fn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInsertExistingBasicBlockAfterInsertBlock( LLVMBuilderRef Builder, LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAppendExistingBasicBlock( LLVMValueRef Fn, LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMCreateBasicBlockInContext( LLVMContextRefAlias C, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMAppendBasicBlockInContext( LLVMContextRefAlias C, LLVMValueRef Fn, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMAppendBasicBlock( LLVMValueRef Fn, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMInsertBasicBlockInContext( LLVMContextRefAlias C, LLVMBasicBlockRef BB, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMInsertBasicBlock( LLVMBasicBlockRef InsertBeforeBB, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteBasicBlock( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveBasicBlockFromParent( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveBasicBlockBefore( LLVMBasicBlockRef BB, LLVMBasicBlockRef MovePos );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveBasicBlockAfter( LLVMBasicBlockRef BB, LLVMBasicBlockRef MovePos );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetFirstInstruction( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetLastInstruction( LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMHasMetadata( LLVMValueRef Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetMetadata( LLVMValueRef Val, uint KindID );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetMetadata( LLVMValueRef Val, uint KindID, LLVMValueRef Node );

        // NOTE: Return type is technically a pointer to an array, but is considered an OPAQUE handle in LLVM-C API
        //       To get elements use LLVMValueMetadataEntriesGetMetadata().
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueMetadataEntry LLVMInstructionGetAllMetadataOtherThanDebugLoc( LLVMValueRef Instr, out nuint NumEntries );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetInstructionParent( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetNextInstruction( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetPreviousInstruction( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstructionRemoveFromParent( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstructionEraseFromParent( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDeleteInstruction( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOpcode LLVMGetInstructionOpcode( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMIntPredicate LLVMGetICmpPredicate( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMRealPredicate LLVMGetFCmpPredicate( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMInstructionClone( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMIsATerminatorInst( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetFirstDbgRecord( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetLastDbgRecord( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetNextDbgRecord( LLVMDbgRecordRef DbgRecord );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMGetPreviousDbgRecord( LLVMDbgRecordRef DbgRecord );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumArgOperands( LLVMValueRef Instr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInstructionCallConv( LLVMValueRef Instr, uint CC );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetInstructionCallConv( LLVMValueRef Instr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInstrParamAlignment( LLVMValueRef Instr, LLVMAttributeIndex Idx, uint Align );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddCallSiteAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, LLVMAttributeRef A );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetCallSiteAttributeCount( LLVMValueRef C, LLVMAttributeIndex Idx );

        // size of Attrs must contain enough room for LLVMGetCallSiteAttributeCount() elements or memory corruption
        // will occur. The native code only deals with a pointer and assumes it points to a region big enough to
        // hold the correct amount.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGetCallSiteAttributes(
            LLVMValueRef C,
            LLVMAttributeIndex Idx,
            /*LLVMAttributeRef[LLVMGetCallSiteAttributeCount(C, Idx)]*/LLVMAttributeRef* Attrs
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetCallSiteEnumAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, uint KindID );

        public static LLVMAttributeRef LLVMGetCallSiteStringAttribute(
            LLVMValueRef C,
            LLVMAttributeIndex Idx,
            LazyEncodedString K
            )
        {
            return LLVMGetCallSiteStringAttribute( C, Idx, K, checked((uint)K.NativeStrLen) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMAttributeRef LLVMGetCallSiteStringAttribute(
            LLVMValueRef C,
            LLVMAttributeIndex Idx,
            LazyEncodedString K, uint KLen
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAttributeRef LLVMGetCallSiteStringAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, byte* K, uint KLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveCallSiteEnumAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, uint KindID );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMRemoveCallSiteStringAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, byte* K, uint KLen );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetCalledFunctionType( LLVMValueRef C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetCalledValue( LLVMValueRef Instr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumOperandBundles( LLVMValueRef C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOperandBundleRef LLVMGetOperandBundleAtIndex( LLVMValueRef C, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsTailCall( LLVMValueRef CallInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTailCall( LLVMValueRef CallInst, [MarshalAs( UnmanagedType.Bool )] bool IsTailCall );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTailCallKind LLVMGetTailCallKind( LLVMValueRef CallInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetTailCallKind( LLVMValueRef CallInst, LLVMTailCallKind kind );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetNormalDest( LLVMValueRef InvokeInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetUnwindDest( LLVMValueRef InvokeInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNormalDest( LLVMValueRef InvokeInst, LLVMBasicBlockRef B );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetUnwindDest( LLVMValueRef InvokeInst, LLVMBasicBlockRef B );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetCallBrDefaultDest( LLVMValueRef CallBr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetCallBrNumIndirectDests( LLVMValueRef CallBr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetCallBrIndirectDest( LLVMValueRef CallBr, uint Idx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumSuccessors( LLVMValueRef Term );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetSuccessor( LLVMValueRef Term, uint i );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSuccessor( LLVMValueRef Term, uint i, LLVMBasicBlockRef block );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsConditional( LLVMValueRef Branch );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetCondition( LLVMValueRef Branch );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCondition( LLVMValueRef Branch, LLVMValueRef Cond );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetSwitchDefaultDest( LLVMValueRef SwitchInstr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetAllocatedType( LLVMValueRef Alloca );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsInBounds( LLVMValueRef GEP );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetIsInBounds( LLVMValueRef GEP, [MarshalAs( UnmanagedType.Bool )] bool InBounds );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMGetGEPSourceElementType( LLVMValueRef GEP );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGEPNoWrapFlags LLVMGEPGetNoWrapFlags( LLVMValueRef GEP );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMGEPSetNoWrapFlags( LLVMValueRef GEP, LLVMGEPNoWrapFlags NoWrapFlags );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddIncoming(
            LLVMValueRef PhiNode,
            [In] LLVMValueRef[] IncomingValues,
            [In] LLVMBasicBlockRef[] IncomingBlocks,
            uint Count
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCountIncoming( LLVMValueRef PhiNode );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetIncomingValue( LLVMValueRef PhiNode, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetIncomingBlock( LLVMValueRef PhiNode, uint Index );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumIndices( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint* LLVMGetIndices( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBuilderRef LLVMCreateBuilderInContext( LLVMContextRefAlias C );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBuilderRef LLVMCreateBuilder( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilder( LLVMBuilderRef Builder, LLVMBasicBlockRef Block, LLVMValueRef Instr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderBeforeDbgRecords( LLVMBuilderRef Builder, LLVMBasicBlockRef Block, LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderBefore( LLVMBuilderRef Builder, LLVMValueRef Instr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderBeforeInstrAndDbgRecords( LLVMBuilderRef Builder, LLVMValueRef Instr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMPositionBuilderAtEnd( LLVMBuilderRef Builder, LLVMBasicBlockRef Block );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBasicBlockRef LLVMGetInsertBlock( LLVMBuilderRef Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMClearInsertionPosition( LLVMBuilderRef Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInsertIntoBuilder( LLVMBuilderRef Builder, LLVMValueRef Instr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInsertIntoBuilderWithName( LLVMBuilderRef Builder, LLVMValueRef Instr, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMGetCurrentDebugLocation2( LLVMBuilderRef Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCurrentDebugLocation2( LLVMBuilderRef Builder, LLVMMetadataRef Loc );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetInstDebugLocation( LLVMBuilderRef Builder, LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddMetadataToInst( LLVMBuilderRef Builder, LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMBuilderGetDefaultFPMathTag( LLVMBuilderRef Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMBuilderSetDefaultFPMathTag( LLVMBuilderRef Builder, LLVMMetadataRef FPMathTag );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMGetBuilderContext( LLVMBuilderRef Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCurrentDebugLocation( LLVMBuilderRef Builder, LLVMValueRef L );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetCurrentDebugLocation( LLVMBuilderRef Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildRetVoid( LLVMBuilderRef B );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildRet( LLVMBuilderRef B, LLVMValueRef V );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAggregateRet( LLVMBuilderRef B, out LLVMValueRef RetVals, uint N );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildBr( LLVMBuilderRef B, LLVMBasicBlockRef Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCondBr( LLVMBuilderRef B, LLVMValueRef If, LLVMBasicBlockRef Then, LLVMBasicBlockRef Else );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSwitch( LLVMBuilderRef B, LLVMValueRef V, LLVMBasicBlockRef Else, uint NumCases );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIndirectBr( LLVMBuilderRef B, LLVMValueRef Addr, uint NumDests );

        public static LLVMValueRef LLVMBuildCallBr(
            LLVMBuilderRef builder,
            LLVMTypeRef typeRef,
            LLVMValueRef functionValue,
            LLVMBasicBlockRef defaultDest,
            LLVMBasicBlockRef[] indirectDest,
            LLVMValueRef[] args,
            LLVMOperandBundleRef[] bundles,
            LazyEncodedString Name
            )
        {
            unsafe
            {
                return RefHandleMarshaller.WithNativePointer(
                            bundles,
                            ( p, size ) => LLVMBuildCallBr(
                                               builder,
                                               typeRef,
                                               functionValue,
                                               defaultDest,
                                               indirectDest,
                                               checked((uint)indirectDest.Length),
                                               args,
                                               checked((uint)args.Length),
                                               p,
                                               checked((uint)size),
                                               Name
                                               )
                       );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMBuildCallBr(
            LLVMBuilderRef B,
            LLVMTypeRef Ty,
            LLVMValueRef Fn,
            LLVMBasicBlockRef DefaultDest,
            [In] LLVMBasicBlockRef[] IndirectDests, // The length of this array MUST be at least NumIndirectDests
            uint NumIndirectDests,
            [In] LLVMValueRef[] Args, // the length of this array MUST be at least NumArgs
            uint NumArgs,
            /* No marshalling attribute exists to set the length, the length of this array MUST be at least NumBundles
               Marshalling of safe handle arrays is not supported by generated interop. Use RefHandleMarshaller for these */
            /*[In] LLVMOperandBundleRef[]*/ nint* Bundles,
            uint NumBundles,
            LazyEncodedString Name
            );

        public static LLVMValueRef LLVMBuildInvoke2(
            LLVMBuilderRef builder,
            LLVMTypeRef typeRef,
            LLVMValueRef functionValue,
            LLVMValueRef[] args,
            LLVMBasicBlockRef @then,
            LLVMBasicBlockRef @catch,
            LazyEncodedString name )
        {
            return LLVMBuildInvoke2(
                    builder,
                    typeRef,
                    functionValue,
                    args,
                    checked((uint)args.Length),
                    @then,
                    @catch,
                    name
                  );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMBuildInvoke2(
            LLVMBuilderRef B,
            LLVMTypeRef Ty,
            LLVMValueRef Fn,
            [In] LLVMValueRef[] Args, // the length of this array MUST be at least NumArgs
            uint NumArgs,
            LLVMBasicBlockRef Then,
            LLVMBasicBlockRef Catch,
            LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInvokeWithOperandBundles(
            LLVMBuilderRef B,
            LLVMTypeRef Ty,
            LLVMValueRef Fn,

            // No marshalling attribute exists to set the length, the length of this array MUST be at least NumArgs
            [In] LLVMValueRef[] Args,
            uint NumArgs,
            LLVMBasicBlockRef Then,
            LLVMBasicBlockRef Catch,

            // No marshalling attribute exists to set the length, the length of this array MUST be at least NumBundles
            // Marshalling of safe handle arrays is not supported by generated interop. Use RefHandleMarshaller for these
            /*[In] LLVMOperandBundleRef[]*/ nint* Bundles,
            uint NumBundles,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildUnreachable( LLVMBuilderRef B );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildResume( LLVMBuilderRef B, LLVMValueRef Exn );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildLandingPad(
            LLVMBuilderRef B,
            LLVMTypeRef Ty,
            LLVMValueRef PersFn,
            uint NumClauses,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCleanupRet( LLVMBuilderRef B, LLVMValueRef CatchPad, LLVMBasicBlockRef BB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCatchRet( LLVMBuilderRef B, LLVMValueRef CatchPad, LLVMBasicBlockRef BB );

        public static LLVMValueRef LLVMBuildCatchPad(
            LLVMBuilderRef B,
            LLVMValueRef ParentPad,
            LLVMValueRef[]? Args,
            LazyEncodedString? Name
            )
        {
            Args ??= [];
            return LLVMBuildCatchPad( B, ParentPad, Args, checked((uint)Args.Length), Name ?? LazyEncodedString.Empty );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMValueRef LLVMBuildCatchPad(
            LLVMBuilderRef B,
            LLVMValueRef ParentPad,

            // No marshalling attribute exists to set the length, the length of this array MUST be at least NumArgs
            [In] LLVMValueRef[] Args,
            uint NumArgs,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCleanupPad(
            LLVMBuilderRef B,
            LLVMValueRef ParentPad,

            // No marshalling attribute exists to set the length, the length of this array MUST be at least NumArgs
            [In] LLVMValueRef[] Args,
            uint NumArgs,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCatchSwitch(
            LLVMBuilderRef B,
            LLVMValueRef ParentPad,
            LLVMBasicBlockRef UnwindBB,
            uint NumHandlers,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddCase( LLVMValueRef Switch, LLVMValueRef OnVal, LLVMBasicBlockRef Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddDestination( LLVMValueRef IndirectBr, LLVMBasicBlockRef Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumClauses( LLVMValueRef LandingPad );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetClause( LLVMValueRef LandingPad, uint Idx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddClause( LLVMValueRef LandingPad, LLVMValueRef ClauseVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsCleanup( LLVMValueRef LandingPad );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCleanup( LLVMValueRef LandingPad, [MarshalAs( UnmanagedType.Bool )] bool Val );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddHandler( LLVMValueRef CatchSwitch, LLVMBasicBlockRef Dest );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumHandlers( LLVMValueRef CatchSwitch );

        public static LLVMBasicBlockRef[] LLVMGetHandlers( LLVMValueRef CatchSwitch )
        {
            // Custom marshalling for the return "out" array
            // while ensuring the size is correct. This takes advantage of the fact
            // that LLVMBasicBlockRef is a value type that wraps an nint (effectively a stronger
            // typedef for nint) but is re-interpret cast compatible with nint. So this will
            // allocate the  required space, pin it, then ask the native code to fill it and
            // ultimately return it. This is the most efficient means of filling an array,
            // given the LLVM-C API.
            unsafe
            {
                uint size = LLVMGetNumHandlers(CatchSwitch);
                var retVal = new LLVMBasicBlockRef[size];
                fixed(LLVMBasicBlockRef* p = retVal)
                {
                    LLVMGetHandlers( CatchSwitch.DangerousGetHandle(), (nint*)p );
                }

                return retVal;
            }
        }

        // Array provided for 'Handlers' must be at least (LLVMGetNumHandlers()) large
        // Use of generated marshalling is VERY inefficient as it doesn't know
        // that a LLVMBasicBlockRef is reinterpret_cast<nint> compatible.
        // and tries to use a buffer array to `marshal` between forms even though
        // they have the exact same bit pattern
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial void LLVMGetHandlers(
            /*LLVMValueRef*/ nint CatchSwitch,
            /*[Out] LLVMBasicBlockRef[]*/ nint* Handlers
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetArgOperand( LLVMValueRef Funclet, uint i );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetArgOperand( LLVMValueRef Funclet, uint i, LLVMValueRef value );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMGetParentCatchSwitch( LLVMValueRef CatchPad );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetParentCatchSwitch( LLVMValueRef CatchPad, LLVMValueRef CatchSwitch );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAdd( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWAdd( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWAdd( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFAdd( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSub( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWSub( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWSub( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFSub( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMul( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWMul( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWMul( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFMul( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildUDiv( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExactUDiv( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSDiv( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExactSDiv( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFDiv( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildURem( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSRem( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFRem( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildShl( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildLShr( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAShr( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAnd( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildOr( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildXor( LLVMBuilderRef B, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildBinOp(
            LLVMBuilderRef B,
            LLVMOpcode Op,
            LLVMValueRef LHS,
            LLVMValueRef RHS,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNeg( LLVMBuilderRef B, LLVMValueRef V, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNSWNeg( LLVMBuilderRef B, LLVMValueRef V, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNUWNeg( LLVMBuilderRef B, LLVMValueRef V, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFNeg( LLVMBuilderRef B, LLVMValueRef V, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildNot( LLVMBuilderRef B, LLVMValueRef V, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetNUW( LLVMValueRef ArithInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNUW( LLVMValueRef ArithInst, [MarshalAs( UnmanagedType.Bool )] bool HasNUW );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetNSW( LLVMValueRef ArithInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNSW( LLVMValueRef ArithInst, [MarshalAs( UnmanagedType.Bool )] bool HasNSW );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetExact( LLVMValueRef DivOrShrInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetExact( LLVMValueRef DivOrShrInst, [MarshalAs( UnmanagedType.Bool )] bool IsExact );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetNNeg( LLVMValueRef NonNegInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetNNeg( LLVMValueRef NonNegInst, [MarshalAs( UnmanagedType.Bool )] bool IsNonNeg );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMFastMathFlags LLVMGetFastMathFlags( LLVMValueRef FPMathInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetFastMathFlags( LLVMValueRef FPMathInst, LLVMFastMathFlags FMF );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMCanValueUseFastMathFlags( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetIsDisjoint( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetIsDisjoint( LLVMValueRef Inst, [MarshalAs( UnmanagedType.Bool )] bool IsDisjoint );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMalloc( LLVMBuilderRef B, LLVMTypeRef Ty, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildArrayMalloc( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Val, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMemSet( LLVMBuilderRef B, LLVMValueRef Ptr, LLVMValueRef Val, LLVMValueRef Len, uint Align );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMemCpy( LLVMBuilderRef B, LLVMValueRef Dst, uint DstAlign, LLVMValueRef Src, uint SrcAlign, LLVMValueRef Size );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildMemMove( LLVMBuilderRef B, LLVMValueRef Dst, uint DstAlign, LLVMValueRef Src, uint SrcAlign, LLVMValueRef Size );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAlloca( LLVMBuilderRef B, LLVMTypeRef Ty, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildArrayAlloca( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Val, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFree( LLVMBuilderRef B, LLVMValueRef PointerVal );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildLoad2( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef PointerVal, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildStore( LLVMBuilderRef B, LLVMValueRef Val, LLVMValueRef Ptr );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGEP2( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, [In] LLVMValueRef[] Indices, uint NumIndices, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInBoundsGEP2( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, [In] LLVMValueRef[] Indices, uint NumIndices, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGEPWithNoWrapFlags( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, out LLVMValueRef Indices, uint NumIndices, LazyEncodedString Name, LLVMGEPNoWrapFlags NoWrapFlags );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildStructGEP2( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef Pointer, uint Idx, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGlobalString( LLVMBuilderRef B, LazyEncodedString Str, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildGlobalStringPtr( LLVMBuilderRef B, LazyEncodedString Str, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetVolatile( LLVMValueRef MemoryAccessInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetVolatile( LLVMValueRef MemoryAccessInst, [MarshalAs( UnmanagedType.Bool )] bool IsVolatile );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetWeak( LLVMValueRef CmpXchgInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetWeak( LLVMValueRef CmpXchgInst, [MarshalAs( UnmanagedType.Bool )] bool IsWeak );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicOrdering LLVMGetOrdering( LLVMValueRef MemoryAccessInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetOrdering( LLVMValueRef MemoryAccessInst, LLVMAtomicOrdering Ordering );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicRMWBinOp LLVMGetAtomicRMWBinOp( LLVMValueRef AtomicRMWInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAtomicRMWBinOp( LLVMValueRef AtomicRMWInst, LLVMAtomicRMWBinOp BinOp );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildTrunc( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildZExt( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSExt( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPToUI( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPToSI( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildUIToFP( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSIToFP( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPTrunc( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPExt( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPtrToInt( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIntToPtr( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildBitCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAddrSpaceCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildZExtOrBitCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSExtOrBitCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildTruncOrBitCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCast( LLVMBuilderRef B, LLVMOpcode Op, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPointerCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIntCast2( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.Bool )] bool IsSigned, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFPCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIntCast( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOpcode LLVMGetCastOpcode( LLVMValueRef Src, [MarshalAs( UnmanagedType.Bool )] bool SrcIsSigned, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.Bool )] bool DestIsSigned );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildICmp( LLVMBuilderRef B, LLVMIntPredicate Op, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFCmp( LLVMBuilderRef B, LLVMRealPredicate Op, LLVMValueRef LHS, LLVMValueRef RHS, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPhi( LLVMBuilderRef B, LLVMTypeRef Ty, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCall2( LLVMBuilderRef B, LLVMTypeRef _1, LLVMValueRef Fn, [In] LLVMValueRef[] Args, uint NumArgs, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildCallWithOperandBundles(
            LLVMBuilderRef B,
            LLVMTypeRef _1,
            LLVMValueRef Fn,

            // No marshalling attribute exists to set the length, the length of this array MUST be at least NumArgs
            [In] LLVMValueRef[] Args,
            uint NumArgs,

            // No marshalling attribute exists to set the length, the length of this array MUST be at least NumBundles
            // Marshalling of safe handle arrays is not supported by generated interop. Use RefHandleMarshaller for these
            /*[In] LLVMOperandBundleRef[]*/ nint* Bundles,
            uint NumBundles,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildSelect(
            LLVMBuilderRef B,
            LLVMValueRef If,
            LLVMValueRef Then,
            LLVMValueRef Else,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildVAArg(
            LLVMBuilderRef B,
            LLVMValueRef List,
            LLVMTypeRef Ty,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExtractElement(
            LLVMBuilderRef B,
            LLVMValueRef VecVal,
            LLVMValueRef Index,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInsertElement(
            LLVMBuilderRef B,
            LLVMValueRef VecVal,
            LLVMValueRef EltVal,
            LLVMValueRef Index,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildShuffleVector(
            LLVMBuilderRef B,
            LLVMValueRef V1,
            LLVMValueRef V2,
            LLVMValueRef Mask,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildExtractValue( LLVMBuilderRef B, LLVMValueRef AggVal, uint Index, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildInsertValue(
            LLVMBuilderRef B,
            LLVMValueRef AggVal,
            LLVMValueRef EltVal,
            uint Index,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFreeze( LLVMBuilderRef B, LLVMValueRef Val, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIsNull( LLVMBuilderRef B, LLVMValueRef Val, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildIsNotNull( LLVMBuilderRef B, LLVMValueRef Val, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildPtrDiff2(
            LLVMBuilderRef B,
            LLVMTypeRef ElemTy,
            LLVMValueRef LHS,
            LLVMValueRef RHS,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFence(
            LLVMBuilderRef B,
            LLVMAtomicOrdering ordering,
            [MarshalAs( UnmanagedType.Bool )] bool singleThread,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildFenceSyncScope(
            LLVMBuilderRef B,
            LLVMAtomicOrdering ordering,
            uint SSID,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicRMW(
            LLVMBuilderRef B,
            LLVMAtomicRMWBinOp op,
            LLVMValueRef PTR,
            LLVMValueRef Val,
            LLVMAtomicOrdering ordering,
            [MarshalAs( UnmanagedType.Bool )] bool singleThread
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicRMWSyncScope(
            LLVMBuilderRef B,
            LLVMAtomicRMWBinOp op,
            LLVMValueRef PTR,
            LLVMValueRef Val,
            LLVMAtomicOrdering ordering,
            uint SSID
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicCmpXchg(
            LLVMBuilderRef B,
            LLVMValueRef Ptr,
            LLVMValueRef Cmp,
            LLVMValueRef New,
            LLVMAtomicOrdering SuccessOrdering,
            LLVMAtomicOrdering FailureOrdering,
            [MarshalAs( UnmanagedType.Bool )] bool SingleThread
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMValueRef LLVMBuildAtomicCmpXchgSyncScope(
            LLVMBuilderRef B,
            LLVMValueRef Ptr,
            LLVMValueRef Cmp,
            LLVMValueRef New,
            LLVMAtomicOrdering SuccessOrdering,
            LLVMAtomicOrdering FailureOrdering,
            uint SSID
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetNumMaskElements( LLVMValueRef ShuffleVectorInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMGetUndefMaskElem( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial int LLVMGetMaskValue( LLVMValueRef ShuffleVectorInst, uint Elt );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsAtomicSingleThread( LLVMValueRef AtomicInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAtomicSingleThread( LLVMValueRef AtomicInst, [MarshalAs( UnmanagedType.Bool )] bool SingleThread );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsAtomic( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetAtomicSyncScopeID( LLVMValueRef AtomicInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetAtomicSyncScopeID( LLVMValueRef AtomicInst, uint SSID );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicOrdering LLVMGetCmpXchgSuccessOrdering( LLVMValueRef CmpXchgInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCmpXchgSuccessOrdering( LLVMValueRef CmpXchgInst, LLVMAtomicOrdering Ordering );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMAtomicOrdering LLVMGetCmpXchgFailureOrdering( LLVMValueRef CmpXchgInst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetCmpXchgFailureOrdering( LLVMValueRef CmpXchgInst, LLVMAtomicOrdering Ordering );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateMemoryBufferWithContentsOfFile(
            LazyEncodedString Path,
            out LLVMMemoryBufferRef OutMemBuf,
            [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string OutMessage
            );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMCreateMemoryBufferWithSTDIN( out LLVMMemoryBufferRef OutMemBuf, out string OutMessage );

        // NOTE: This does NOT use an array as a param as it MUST remain valid and fixed for the lifetime of this
        // buffer ref. That is it builds a reference to the data - NOT a copy.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRange(
            byte* InputData, nuint InputDataLength, // Intentionally not an array.
            LazyEncodedString BufferName,
            [MarshalAs( UnmanagedType.Bool )] bool RequiresNullTerminator
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRangeCopy( [In] byte[] InputData, LazyEncodedString BufferName )
        {
            return LLVMCreateMemoryBufferWithMemoryRangeCopy( InputData, checked((nuint)InputData.LongLength), BufferName );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRangeCopy(
            [In] byte[] InputData,
            nuint InputDataLength, // InputDataLength must be <= InputData.Length
            LazyEncodedString BufferName
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGetBufferStart( LLVMMemoryBufferRef MemBuf );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nuint LLVMGetBufferSize( LLVMMemoryBufferRef MemBuf );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassManagerRef LLVMCreatePassManager( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMPassManagerRef LLVMCreateFunctionPassManagerForModule( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMRunPassManager( LLVMPassManagerRef PM, LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMInitializeFunctionPassManager( LLVMPassManagerRef FPM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMRunFunctionPassManager( LLVMPassManagerRef FPM, LLVMValueRef F );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMFinalizeFunctionPassManager( LLVMPassManagerRef FPM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMStartMultithreaded( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMStopMultithreaded( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsMultithreaded( );
    }
}
