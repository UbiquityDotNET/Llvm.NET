using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

// warning CS0649: Field 'xxx' is never assigned to, and will always have its default value 0
#pragma warning disable 649

namespace Llvm.NET.Native
{
    internal partial struct LLVMOpaqueMemoryBuffer
    {
    }

    internal partial struct LLVMOpaqueContext
    {
    }

    internal partial struct LLVMOpaqueModule
    {
    }

    internal partial struct LLVMOpaqueType
    {
    }

    internal partial struct LLVMOpaqueValue
    {
    }

    internal partial struct LLVMOpaqueBasicBlock
    {
    }

    internal partial struct LLVMOpaqueBuilder
    {
    }

    internal partial struct LLVMOpaqueModuleProvider
    {
    }

    internal partial struct LLVMOpaquePassManager
    {
    }

    internal partial struct LLVMOpaquePassRegistry
    {
    }

    internal partial struct LLVMOpaqueUse
    {
    }

    internal partial struct LLVMOpaqueDiagnosticInfo
    {
    }

    internal partial struct LLVMOpInfoSymbol1
    {
        internal int @Present;
        [MarshalAs( UnmanagedType.LPStr )]
        internal string @Name;
        internal int @Value;
    }

    internal partial struct LLVMOpInfo1
    {
        internal LLVMOpInfoSymbol1 @AddSymbol;
        internal LLVMOpInfoSymbol1 @SubtractSymbol;
        internal int @Value;
        internal int @VariantKind;
    }

    internal partial struct LLVMOpaqueTargetData
    {
    }

    internal partial struct LLVMOpaqueTargetLibraryInfotData
    {
    }

    internal partial struct LLVMOpaqueTargetMachine
    {
    }

    internal partial struct LLVMTarget
    {
    }

    internal partial struct LLVMOpaqueGenericValue
    {
    }

    internal partial struct LLVMOpaqueExecutionEngine
    {
    }

    internal partial struct LLVMOpaqueMCJITMemoryManager
    {
    }

    internal partial struct LLVMMCJITCompilerOptions
    {
        internal uint @OptLevel;
        internal LLVMCodeModel @CodeModel;
        internal int @NoFramePointerElim;
        internal int @EnableFastISel;
        internal LLVMMCJITMemoryManagerRef @MCJMM;
    }

    internal partial struct LLVMOpaqueLTOModule
    {
    }

    internal partial struct LLVMOpaqueLTOCodeGenerator
    {
    }

    internal partial struct LLVMOpaqueObjectFile
    {
    }

    internal partial struct LLVMOpaqueSectionIterator
    {
    }

    internal partial struct LLVMOpaqueSymbolIterator
    {
    }

    internal partial struct LLVMOpaqueRelocationIterator
    {
    }

    internal partial struct LLVMOrcOpaqueJITStack
    {
    }

    internal partial struct LLVMOpaquePassManagerBuilder
    {
    }

    // hand added to help clarify use when the value
    // is not really a bool but a status where (0==SUCCESS)
    internal partial struct LLVMStatus
    {
        public LLVMStatus( int value )
        {
            ErrorCode = value;
        }

        public bool Succeeded => ErrorCode == 0;

        public bool Failed => !Succeeded;

        public static implicit operator bool( LLVMStatus value ) => value.Succeeded;

        public int ErrorCode { get; }
    }

    internal partial struct LLVMMemoryBufferRef
    {
        internal LLVMMemoryBufferRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMContextRef
    {
        internal LLVMContextRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMModuleRef
    {
        internal LLVMModuleRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMTypeRef
    {
        internal LLVMTypeRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMValueRef
    {
        internal LLVMValueRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMBasicBlockRef
    {
        internal LLVMBasicBlockRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMMetadataRef
    {
        internal LLVMMetadataRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal readonly IntPtr Pointer;
    }

    internal partial struct LLVMDIBuilderRef
    {
        internal LLVMDIBuilderRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal readonly IntPtr Pointer;
    }

    /* replaced with SafeHandle Variant to ensure release
    internal partial struct LLVMBuilderRef
    {
        internal LLVMBuilderRef(IntPtr pointer)
        {
            this.Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }
    */

    internal partial struct LLVMModuleProviderRef
    {
        internal LLVMModuleProviderRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMPassManagerRef
    {
        internal LLVMPassManagerRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    /* replaced with SafeHandle Variant to ensure release
    internal partial struct LLVMPassRegistryRef
    {
        internal LLVMPassRegistryRef(IntPtr pointer)
        {
            this.Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }
    */

    internal partial struct LLVMUseRef
    {
        internal LLVMUseRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMAttributeRef
    {
        internal LLVMAttributeRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMDiagnosticInfoRef
    {
        internal LLVMDiagnosticInfoRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMFatalErrorHandler( [MarshalAs( UnmanagedType.LPStr )] string reason );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMDiagnosticHandler( LLVMDiagnosticInfoRef @param0, IntPtr @param1 );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMYieldCallback( LLVMContextRef @param0, IntPtr @param1 );

    internal partial struct LLVMDisasmContextRef
    {
        internal LLVMDisasmContextRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate int LLVMOpInfoCallback( IntPtr disInfo, int pC, int offset, int size, int tagType, IntPtr tagBuf );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate string LLVMSymbolLookupCallback( IntPtr disInfo, int referenceValue, out int referenceType, int referencePC, out IntPtr referenceName );

    internal partial struct LLVMTargetDataRef
    {
        internal LLVMTargetDataRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMTargetLibraryInfoRef
    {
        internal LLVMTargetLibraryInfoRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMTargetMachineRef
    {
        internal LLVMTargetMachineRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMTargetRef
    {
        internal LLVMTargetRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMGenericValueRef
    {
        internal LLVMGenericValueRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMExecutionEngineRef
    {
        internal LLVMExecutionEngineRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMMCJITMemoryManagerRef
    {
        internal LLVMMCJITMemoryManagerRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate IntPtr LLVMMemoryManagerAllocateCodeSectionCallback( IntPtr opaque, int size, uint alignment, uint sectionID, [MarshalAs( UnmanagedType.LPStr )] string sectionName );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate IntPtr LLVMMemoryManagerAllocateDataSectionCallback( IntPtr opaque, int size, uint alignment, uint sectionID, [MarshalAs( UnmanagedType.LPStr )] string sectionName, int isReadOnly );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate int LLVMMemoryManagerFinalizeMemoryCallback( IntPtr opaque, out IntPtr errMsg );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMMemoryManagerDestroyCallback( IntPtr opaque );

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type")]
    internal partial struct llvm_lto_t
    {
        internal llvm_lto_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal partial struct lto_bool_t
    {
        internal lto_bool_t( bool value )
        {
            Value = value;
        }

        internal bool Value;
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal partial struct lto_module_t
    {
        internal lto_module_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal partial struct lto_code_gen_t
    {
        internal lto_code_gen_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal delegate void lto_diagnostic_handler_t( lto_codegen_diagnostic_severity_t @severity, [MarshalAs( UnmanagedType.LPStr )] string @diag, IntPtr @ctxt );

    internal partial struct LLVMObjectFileRef
    {
        internal LLVMObjectFileRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMSectionIteratorRef
    {
        internal LLVMSectionIteratorRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMSymbolIteratorRef
    {
        internal LLVMSymbolIteratorRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMRelocationIteratorRef
    {
        internal LLVMRelocationIteratorRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMSharedModuleRef
    {
        internal LLVMSharedModuleRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMSharedObjectBufferRef
    {
        internal LLVMSharedObjectBufferRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMOrcJITStackRef
    {
        internal LLVMOrcJITStackRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMOrcModuleHandle
    {
        internal LLVMOrcModuleHandle( int value )
        {
            Value = value;
        }

        internal int Value { get; }
    }

    internal partial struct LLVMOrcTargetAddress
    {
        internal LLVMOrcTargetAddress( ulong value )
        {
            Value = value;
        }

        internal ulong Value { get; }
    }

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate ulong LLVMOrcSymbolResolverFn( [MarshalAs( UnmanagedType.LPStr )] string name, IntPtr lookupCtx );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate ulong LLVMOrcLazyCompileCallbackFn( LLVMOrcJITStackRef jITStack, IntPtr callbackCtx );

    internal partial struct LLVMPassManagerBuilderRef
    {
        internal LLVMPassManagerBuilderRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal enum LLVMVerifierFailureAction
    {
        @LLVMAbortProcessAction = 0,
        @LLVMPrintMessageAction = 1,
        @LLVMReturnStatusAction = 2,
    }

#pragma warning disable CA1008 // Enums should have zero value.
    internal enum LLVMAttribute
    {
        @LLVMZExtAttribute = 1,
        @LLVMSExtAttribute = 2,
        @LLVMNoReturnAttribute = 4,
        @LLVMInRegAttribute = 8,
        @LLVMStructRetAttribute = 16,
        @LLVMNoUnwindAttribute = 32,
        @LLVMNoAliasAttribute = 64,
        @LLVMByValAttribute = 128,
        @LLVMNestAttribute = 256,
        @LLVMReadNoneAttribute = 512,
        @LLVMReadOnlyAttribute = 1024,
        @LLVMNoInlineAttribute = 2048,
        @LLVMAlwaysInlineAttribute = 4096,
        @LLVMOptimizeForSizeAttribute = 8192,
        @LLVMStackProtectAttribute = 16384,
        @LLVMStackProtectReqAttribute = 32768,
        @LLVMAlignment = 2031616,
        @LLVMNoCaptureAttribute = 2097152,
        @LLVMNoRedZoneAttribute = 4194304,
        @LLVMNoImplicitFloatAttribute = 8388608,
        @LLVMNakedAttribute = 16777216,
        @LLVMInlineHintAttribute = 33554432,
        @LLVMStackAlignment = 469762048,
        @LLVMReturnsTwice = 536870912,
        @LLVMUWTable = 1073741824,
        @LLVMNonLazyBind = -2147483648,
    }

    internal enum LLVMOpcode
    {
        @LLVMRet = 1,
        @LLVMBr = 2,
        @LLVMSwitch = 3,
        @LLVMIndirectBr = 4,
        @LLVMInvoke = 5,
        @LLVMUnreachable = 7,
        @LLVMAdd = 8,
        @LLVMFAdd = 9,
        @LLVMSub = 10,
        @LLVMFSub = 11,
        @LLVMMul = 12,
        @LLVMFMul = 13,
        @LLVMUDiv = 14,
        @LLVMSDiv = 15,
        @LLVMFDiv = 16,
        @LLVMURem = 17,
        @LLVMSRem = 18,
        @LLVMFRem = 19,
        @LLVMShl = 20,
        @LLVMLShr = 21,
        @LLVMAShr = 22,
        @LLVMAnd = 23,
        @LLVMOr = 24,
        @LLVMXor = 25,
        @LLVMAlloca = 26,
        @LLVMLoad = 27,
        @LLVMStore = 28,
        @LLVMGetElementPtr = 29,
        @LLVMTrunc = 30,
        @LLVMZExt = 31,
        @LLVMSExt = 32,
        @LLVMFPToUI = 33,
        @LLVMFPToSI = 34,
        @LLVMUIToFP = 35,
        @LLVMSIToFP = 36,
        @LLVMFPTrunc = 37,
        @LLVMFPExt = 38,
        @LLVMPtrToInt = 39,
        @LLVMIntToPtr = 40,
        @LLVMBitCast = 41,
        @LLVMAddrSpaceCast = 60,
        @LLVMICmp = 42,
        @LLVMFCmp = 43,
        @LLVMPHI = 44,
        @LLVMCall = 45,
        @LLVMSelect = 46,
        @LLVMUserOp1 = 47,
        @LLVMUserOp2 = 48,
        @LLVMVAArg = 49,
        @LLVMExtractElement = 50,
        @LLVMInsertElement = 51,
        @LLVMShuffleVector = 52,
        @LLVMExtractValue = 53,
        @LLVMInsertValue = 54,
        @LLVMFence = 55,
        @LLVMAtomicCmpXchg = 56,
        @LLVMAtomicRMW = 57,
        @LLVMResume = 58,
        @LLVMLandingPad = 59,
        @LLVMCleanupRet = 61,
        @LLVMCatchRet = 62,
        @LLVMCatchPad = 63,
        @LLVMCleanupPad = 64,
        @LLVMCatchSwitch = 65,
    }
#pragma warning restore CA1008 // Enums should have zero value.

    internal enum LLVMTypeKind
    {
        @LLVMVoidTypeKind = 0,
        @LLVMHalfTypeKind = 1,
        @LLVMFloatTypeKind = 2,
        @LLVMDoubleTypeKind = 3,
        @LLVMX86_FP80TypeKind = 4,
        @LLVMFP128TypeKind = 5,
        @LLVMPPC_FP128TypeKind = 6,
        @LLVMLabelTypeKind = 7,
        @LLVMIntegerTypeKind = 8,
        @LLVMFunctionTypeKind = 9,
        @LLVMStructTypeKind = 10,
        @LLVMArrayTypeKind = 11,
        @LLVMPointerTypeKind = 12,
        @LLVMVectorTypeKind = 13,
        @LLVMMetadataTypeKind = 14,
        @LLVMX86_MMXTypeKind = 15,
        @LLVMTokenTypeKind = 16,
    }

    internal enum LLVMLinkage
    {
        @LLVMExternalLinkage = 0,
        @LLVMAvailableExternallyLinkage = 1,
        @LLVMLinkOnceAnyLinkage = 2,
        @LLVMLinkOnceODRLinkage = 3,
        @LLVMLinkOnceODRAutoHideLinkage = 4,
        @LLVMWeakAnyLinkage = 5,
        @LLVMWeakODRLinkage = 6,
        @LLVMAppendingLinkage = 7,
        @LLVMInternalLinkage = 8,
        @LLVMPrivateLinkage = 9,
        @LLVMDLLImportLinkage = 10,
        @LLVMDLLExportLinkage = 11,
        @LLVMExternalWeakLinkage = 12,
        @LLVMGhostLinkage = 13,
        @LLVMCommonLinkage = 14,
        @LLVMLinkerPrivateLinkage = 15,
        @LLVMLinkerPrivateWeakLinkage = 16,
    }

    internal enum LLVMVisibility
    {
        @LLVMDefaultVisibility = 0,
        @LLVMHiddenVisibility = 1,
        @LLVMProtectedVisibility = 2,
    }

    internal enum LLVMDLLStorageClass
    {
        @LLVMDefaultStorageClass = 0,
        @LLVMDLLImportStorageClass = 1,
        @LLVMDLLExportStorageClass = 2,
    }

    internal enum LLVMCallConv
    {
        @LLVMCCallConv = 0,
        @LLVMFastCallConv = 8,
        @LLVMColdCallConv = 9,
        @LLVMWebKitJSCallConv = 12,
        @LLVMAnyRegCallConv = 13,
        @LLVMX86StdcallCallConv = 64,
        @LLVMX86FastcallCallConv = 65,
    }

    internal enum LLVMValueKind
    {
        @LLVMArgumentValueKind = 0,
        @LLVMBasicBlockValueKind = 1,
        @LLVMMemoryUseValueKind = 2,
        @LLVMMemoryDefValueKind = 3,
        @LLVMMemoryPhiValueKind = 4,
        @LLVMFunctionValueKind = 5,
        @LLVMGlobalAliasValueKind = 6,
        @LLVMGlobalIFuncValueKind = 7,
        @LLVMGlobalVariableValueKind = 8,
        @LLVMBlockAddressValueKind = 9,
        @LLVMConstantExprValueKind = 10,
        @LLVMConstantArrayValueKind = 11,
        @LLVMConstantStructValueKind = 12,
        @LLVMConstantVectorValueKind = 13,
        @LLVMUndefValueValueKind = 14,
        @LLVMConstantAggregateZeroValueKind = 15,
        @LLVMConstantDataArrayValueKind = 16,
        @LLVMConstantDataVectorValueKind = 17,
        @LLVMConstantIntValueKind = 18,
        @LLVMConstantFPValueKind = 19,
        @LLVMConstantPointerNullValueKind = 20,
        @LLVMConstantTokenNoneValueKind = 21,
        @LLVMMetadataAsValueValueKind = 22,
        @LLVMInlineAsmValueKind = 23,
        @LLVMInstructionValueKind = 24,
    }

#pragma warning disable CA1008 // Enums should have zero value.
    internal enum LLVMIntPredicate
    {
        @LLVMIntEQ = 32,
        @LLVMIntNE = 33,
        @LLVMIntUGT = 34,
        @LLVMIntUGE = 35,
        @LLVMIntULT = 36,
        @LLVMIntULE = 37,
        @LLVMIntSGT = 38,
        @LLVMIntSGE = 39,
        @LLVMIntSLT = 40,
        @LLVMIntSLE = 41,
    }
#pragma warning restore CA1008 // Enums should have zero value.

    internal enum LLVMRealPredicate
    {
        @LLVMRealPredicateFalse = 0,
        @LLVMRealOEQ = 1,
        @LLVMRealOGT = 2,
        @LLVMRealOGE = 3,
        @LLVMRealOLT = 4,
        @LLVMRealOLE = 5,
        @LLVMRealONE = 6,
        @LLVMRealORD = 7,
        @LLVMRealUNO = 8,
        @LLVMRealUEQ = 9,
        @LLVMRealUGT = 10,
        @LLVMRealUGE = 11,
        @LLVMRealULT = 12,
        @LLVMRealULE = 13,
        @LLVMRealUNE = 14,
        @LLVMRealPredicateTrue = 15,
    }

    internal enum LLVMLandingPadClauseTy
    {
        @LLVMLandingPadCatch = 0,
        @LLVMLandingPadFilter = 1,
    }

    internal enum LLVMThreadLocalMode
    {
        @LLVMNotThreadLocal = 0,
        @LLVMGeneralDynamicTLSModel = 1,
        @LLVMLocalDynamicTLSModel = 2,
        @LLVMInitialExecTLSModel = 3,
        @LLVMLocalExecTLSModel = 4,
    }

    internal enum LLVMAtomicOrdering
    {
        @LLVMAtomicOrderingNotAtomic = 0,
        @LLVMAtomicOrderingUnordered = 1,
        @LLVMAtomicOrderingMonotonic = 2,
        @LLVMAtomicOrderingAcquire = 4,
        @LLVMAtomicOrderingRelease = 5,
        @LLVMAtomicOrderingAcquireRelease = 6,
        @LLVMAtomicOrderingSequentiallyConsistent = 7,
    }

    internal enum LLVMAtomicRMWBinOp
    {
        @LLVMAtomicRMWBinOpXchg = 0,
        @LLVMAtomicRMWBinOpAdd = 1,
        @LLVMAtomicRMWBinOpSub = 2,
        @LLVMAtomicRMWBinOpAnd = 3,
        @LLVMAtomicRMWBinOpNand = 4,
        @LLVMAtomicRMWBinOpOr = 5,
        @LLVMAtomicRMWBinOpXor = 6,
        @LLVMAtomicRMWBinOpMax = 7,
        @LLVMAtomicRMWBinOpMin = 8,
        @LLVMAtomicRMWBinOpUMax = 9,
        @LLVMAtomicRMWBinOpUMin = 10,
    }

    internal enum LLVMDiagnosticSeverity
    {
        @LLVMDSError = 0,
        @LLVMDSWarning = 1,
        @LLVMDSRemark = 2,
        @LLVMDSNote = 3,
    }

    internal enum LLVMAttributeIndex
    {
        @LLVMAttributeReturnIndex = 0,
        @LLVMAttributeFunctionIndex = -1,
    }

    internal enum LLVMByteOrdering
    {
        @LLVMBigEndian = 0,
        @LLVMLittleEndian = 1,
    }

    internal enum LLVMCodeGenOptLevel
    {
        @LLVMCodeGenLevelNone = 0,
        @LLVMCodeGenLevelLess = 1,
        @LLVMCodeGenLevelDefault = 2,
        @LLVMCodeGenLevelAggressive = 3,
    }

    internal enum LLVMRelocMode
    {
        @LLVMRelocDefault = 0,
        @LLVMRelocStatic = 1,
        @LLVMRelocPIC = 2,
        @LLVMRelocDynamicNoPic = 3,
    }

    internal enum LLVMCodeModel
    {
        @LLVMCodeModelDefault = 0,
        @LLVMCodeModelJITDefault = 1,
        @LLVMCodeModelSmall = 2,
        @LLVMCodeModelKernel = 3,
        @LLVMCodeModelMedium = 4,
        @LLVMCodeModelLarge = 5,
    }

    internal enum LLVMCodeGenFileType
    {
        @LLVMAssemblyFile = 0,
        @LLVMObjectFile = 1,
    }

    internal enum LLVMLinkerMode
    {
        @LLVMLinkerDestroySource = 0,
        @LLVMLinkerPreserveSource_Removed = 1,
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum llvm_lto_status
    {
        @LLVM_LTO_UNKNOWN = 0,
        @LLVM_LTO_OPT_SUCCESS = 1,
        @LLVM_LTO_READ_SUCCESS = 2,
        @LLVM_LTO_READ_FAILURE = 3,
        @LLVM_LTO_WRITE_FAILURE = 4,
        @LLVM_LTO_NO_TARGET = 5,
        @LLVM_LTO_NO_WORK = 6,
        @LLVM_LTO_MODULE_MERGE_FAILURE = 7,
        @LLVM_LTO_ASM_FAILURE = 8,
        @LLVM_LTO_NULL_OBJECT = 9,
    }

#pragma warning disable CA1008 // Enums should have zero value.
    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_symbol_attributes
    {
        @LTO_SYMBOL_ALIGNMENT_MASK = 31,
        @LTO_SYMBOL_PERMISSIONS_MASK = 224,
        @LTO_SYMBOL_PERMISSIONS_CODE = 160,
        @LTO_SYMBOL_PERMISSIONS_DATA = 192,
        @LTO_SYMBOL_PERMISSIONS_RODATA = 128,
        @LTO_SYMBOL_DEFINITION_MASK = 1792,
        @LTO_SYMBOL_DEFINITION_REGULAR = 256,
        @LTO_SYMBOL_DEFINITION_TENTATIVE = 512,
        @LTO_SYMBOL_DEFINITION_WEAK = 768,
        @LTO_SYMBOL_DEFINITION_UNDEFINED = 1024,
        @LTO_SYMBOL_DEFINITION_WEAKUNDEF = 1280,
        @LTO_SYMBOL_SCOPE_MASK = 14336,
        @LTO_SYMBOL_SCOPE_INTERNAL = 2048,
        @LTO_SYMBOL_SCOPE_HIDDEN = 4096,
        @LTO_SYMBOL_SCOPE_PROTECTED = 8192,
        @LTO_SYMBOL_SCOPE_DEFAULT = 6144,
        @LTO_SYMBOL_SCOPE_DEFAULT_CAN_BE_HIDDEN = 10240,
        @LTO_SYMBOL_COMDAT = 16384,
        @LTO_SYMBOL_ALIAS = 32768,
    }
#pragma warning restore CA1008 // Enums should have zero value.

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_debug_model
    {
        @LTO_DEBUG_MODEL_NONE = 0,
        @LTO_DEBUG_MODEL_DWARF = 1,
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_codegen_model
    {
        @LTO_CODEGEN_PIC_MODEL_STATIC = 0,
        @LTO_CODEGEN_PIC_MODEL_DYNAMIC = 1,
        @LTO_CODEGEN_PIC_MODEL_DYNAMIC_NO_PIC = 2,
        @LTO_CODEGEN_PIC_MODEL_DEFAULT = 3,
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_codegen_diagnostic_severity_t
    {
        @LTO_DS_ERROR = 0,
        @LTO_DS_WARNING = 1,
        @LTO_DS_REMARK = 3,
        @LTO_DS_NOTE = 2,
    }

    internal enum LLVMOrcErrorCode
    {
        @LLVMOrcErrSuccess = 0,
        @LLVMOrcErrGeneric = 1,
    }

    internal static partial class NativeMethods
    {
        private const string libraryPath = "libLLVM";

        [DllImport( libraryPath, EntryPoint = "LLVMVerifyModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus VerifyModule( LLVMModuleRef @M, LLVMVerifierFailureAction @Action, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMVerifyFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus VerifyFunction( LLVMValueRef @Fn, LLVMVerifierFailureAction @Action );

        [DllImport( libraryPath, EntryPoint = "LLVMViewFunctionCFG", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ViewFunctionCFG( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMViewFunctionCFGOnly", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ViewFunctionCFGOnly( LLVMValueRef @Fn );

        [Obsolete( "Use LLVMParseBitcode2 instead" )]
        [DllImport( libraryPath, EntryPoint = "LLVMParseBitcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcode( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule, out IntPtr @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMParseBitcode2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcode2( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule );

        [DllImport( libraryPath, EntryPoint = "LLVMParseBitcodeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcodeInContext( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMParseBitcodeInContext2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcodeInContext2( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBitcodeModuleInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModuleInContext( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM, out IntPtr @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBitcodeModuleInContext2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModuleInContext2( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBitcodeModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModule( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM, out IntPtr @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBitcodeModule2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModule2( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM );

        [DllImport( libraryPath, EntryPoint = "LLVMWriteBitcodeToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus WriteBitcodeToFile( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Path );

        [DllImport( libraryPath, EntryPoint = "LLVMWriteBitcodeToFD", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus WriteBitcodeToFD( LLVMModuleRef @M, int @FD, int @ShouldClose, int @Unbuffered );

        [DllImport( libraryPath, EntryPoint = "LLVMWriteBitcodeToFileHandle", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus WriteBitcodeToFileHandle( LLVMModuleRef @M, int @Handle );

        [DllImport( libraryPath, EntryPoint = "LLVMWriteBitcodeToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMemoryBufferRef WriteBitcodeToMemoryBuffer( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMInstallFatalErrorHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InstallFatalErrorHandler( LLVMFatalErrorHandler @Handler );

        [DllImport( libraryPath, EntryPoint = "LLVMResetFatalErrorHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ResetFatalErrorHandler( );

        [DllImport( libraryPath, EntryPoint = "LLVMEnablePrettyStackTrace", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void EnablePrettyStackTrace( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeCore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeCore( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMShutdown", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void Shutdown( );

        /* CreateMessage should never be called by managed code
        //[DllImport( libraryPath, EntryPoint = "LLVMCreateMessage", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        //internal static extern IntPtr CreateMessage( [MarshalAs( UnmanagedType.LPStr )] string @Message );
        */

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeMessage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeMessage( IntPtr @Message );

        [DllImport( libraryPath, EntryPoint = "LLVMContextCreate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef ContextCreate( );

        [DllImport( libraryPath, EntryPoint = "LLVMGetGlobalContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef GetGlobalContext( );

        [DllImport( libraryPath, EntryPoint = "LLVMContextSetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ContextSetDiagnosticHandler( LLVMContextRef @C, IntPtr @Handler, IntPtr @DiagnosticContext );

        [DllImport( libraryPath, EntryPoint = "LLVMContextGetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDiagnosticHandler ContextGetDiagnosticHandler( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMContextGetDiagnosticContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr ContextGetDiagnosticContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMContextSetYieldCallback", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ContextSetYieldCallback( LLVMContextRef @C, LLVMYieldCallback @Callback, IntPtr @OpaqueHandle );

        [DllImport( libraryPath, EntryPoint = "LLVMContextDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ContextDispose( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMGetDiagInfoDescription", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetDiagInfoDescription( LLVMDiagnosticInfoRef @DI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetDiagInfoSeverity", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDiagnosticSeverity GetDiagInfoSeverity( LLVMDiagnosticInfoRef @DI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetMDKindIDInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetMDKindIDInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @SLen );

        [DllImport( libraryPath, EntryPoint = "LLVMGetMDKindID", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetMDKindID( [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @SLen );

        [DllImport( libraryPath, EntryPoint = "LLVMGetEnumAttributeKindForName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetEnumAttributeKindForName( [MarshalAs( UnmanagedType.LPStr )] string @Name, size_t @SLen );

        [DllImport( libraryPath, EntryPoint = "LLVMGetLastEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetLastEnumAttributeKind( );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef CreateEnumAttribute( LLVMContextRef @C, uint @KindID, ulong @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetEnumAttributeKind( LLVMAttributeRef @A );

        [DllImport( libraryPath, EntryPoint = "LLVMGetEnumAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong GetEnumAttributeValue( LLVMAttributeRef @A );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef CreateStringAttribute( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLength, [MarshalAs( UnmanagedType.LPStr )] string @V, uint @VLength );

        [DllImport( libraryPath, EntryPoint = "LLVMGetStringAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetStringAttributeKind( LLVMAttributeRef @A, out uint @Length );

        [DllImport( libraryPath, EntryPoint = "LLVMGetStringAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetStringAttributeValue( LLVMAttributeRef @A, out uint @Length );

        [DllImport( libraryPath, EntryPoint = "LLVMIsEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsEnumAttribute( LLVMAttributeRef @A );

        [DllImport( libraryPath, EntryPoint = "LLVMIsStringAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsStringAttribute( LLVMAttributeRef @A );

        [DllImport( libraryPath, EntryPoint = "LLVMModuleCreateWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMModuleRef ModuleCreateWithName( [MarshalAs( UnmanagedType.LPStr )] string @ModuleID );

        [DllImport( libraryPath, EntryPoint = "LLVMModuleCreateWithNameInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMModuleRef ModuleCreateWithNameInContext( [MarshalAs( UnmanagedType.LPStr )] string @ModuleID, LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMCloneModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleRef CloneModule( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeModule( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMGetModuleIdentifier", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetModuleIdentifier( LLVMModuleRef @M, out size_t @Len );

        [DllImport( libraryPath, EntryPoint = "LLVMSetModuleIdentifier", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetModuleIdentifier( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Ident, size_t @Len );

        [DllImport( libraryPath, EntryPoint = "LLVMGetDataLayoutStr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetDataLayoutStr( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMGetDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetDataLayout( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMSetDataLayout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetDataLayout( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @DataLayoutStr );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTarget", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetTarget( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMSetTarget", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetTarget( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Triple );

        [DllImport( libraryPath, EntryPoint = "LLVMDumpModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DumpModule( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMPrintModuleToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus PrintModuleToFile( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Filename, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMPrintModuleToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string PrintModuleToString( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMSetModuleInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetModuleInlineAsm( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Asm );

        [DllImport( libraryPath, EntryPoint = "LLVMGetModuleContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef GetModuleContext( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTypeByName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTypeRef GetTypeByName( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNamedMetadataNumOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetNamedMetadataNumOperands( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNamedMetadataOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void GetNamedMetadataOperands( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, out LLVMValueRef @Dest );

        [DllImport( libraryPath, EntryPoint = "LLVMAddNamedMetadataOperand", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void AddNamedMetadataOperand( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMAddFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddFunction( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMTypeRef @FunctionTy );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNamedFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef GetNamedFunction( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFirstFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstFunction( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMGetLastFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastFunction( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNextFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextFunction( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPreviousFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousFunction( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTypeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeKind GetTypeKind( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMTypeIsSized", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TypeIsSized( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTypeContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef GetTypeContext( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMDumpType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DumpType( LLVMTypeRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMPrintTypeToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string PrintTypeToString( LLVMTypeRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMInt1TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int1TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMInt8TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int8TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMInt16TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int16TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMInt32TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int32TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMInt64TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int64TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMInt128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int128TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMIntTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntTypeInContext( LLVMContextRef @C, uint @NumBits );

        [DllImport( libraryPath, EntryPoint = "LLVMInt1Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int1Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMInt8Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int8Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMInt16Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int16Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMInt32Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int32Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMInt64Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int64Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMInt128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int128Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMIntType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntType( uint @NumBits );

        [DllImport( libraryPath, EntryPoint = "LLVMGetIntTypeWidth", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetIntTypeWidth( LLVMTypeRef @IntegerTy );

        [DllImport( libraryPath, EntryPoint = "LLVMHalfTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef HalfTypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMFloatTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FloatTypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMDoubleTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef DoubleTypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMX86FP80TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86FP80TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FP128TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMPPCFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef PPCFP128TypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMHalfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef HalfType( );

        [DllImport( libraryPath, EntryPoint = "LLVMFloatType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FloatType( );

        [DllImport( libraryPath, EntryPoint = "LLVMDoubleType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef DoubleType( );

        [DllImport( libraryPath, EntryPoint = "LLVMX86FP80Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86FP80Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMFP128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FP128Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMPPCFP128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef PPCFP128Type( );

        [DllImport( libraryPath, EntryPoint = "LLVMFunctionType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FunctionType( LLVMTypeRef @ReturnType, out LLVMTypeRef @ParamTypes, uint @ParamCount, [MarshalAs( UnmanagedType.Bool )]bool @IsVarArg );

        [DllImport( libraryPath, EntryPoint = "LLVMIsFunctionVarArg", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsFunctionVarArg( LLVMTypeRef @FunctionTy );

        [DllImport( libraryPath, EntryPoint = "LLVMGetReturnType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef GetReturnType( LLVMTypeRef @FunctionTy );

        [DllImport( libraryPath, EntryPoint = "LLVMCountParamTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountParamTypes( LLVMTypeRef @FunctionTy );

        [DllImport( libraryPath, EntryPoint = "LLVMGetParamTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetParamTypes( LLVMTypeRef @FunctionTy, out LLVMTypeRef @Dest );

        [DllImport( libraryPath, EntryPoint = "LLVMStructTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef StructTypeInContext( LLVMContextRef @C, out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( libraryPath, EntryPoint = "LLVMStructType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef StructType( out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( libraryPath, EntryPoint = "LLVMStructCreateNamed", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTypeRef StructCreateNamed( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetStructName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetStructName( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMStructSetBody", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void StructSetBody( LLVMTypeRef @StructTy, out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( libraryPath, EntryPoint = "LLVMCountStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountStructElementTypes( LLVMTypeRef @StructTy );

        [DllImport( libraryPath, EntryPoint = "LLVMGetStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetStructElementTypes( LLVMTypeRef @StructTy, out LLVMTypeRef @Dest );

        [DllImport( libraryPath, EntryPoint = "LLVMStructGetTypeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef StructGetTypeAtIndex( LLVMTypeRef @StructTy, uint @i );

        [DllImport( libraryPath, EntryPoint = "LLVMIsPackedStruct", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsPackedStruct( LLVMTypeRef @StructTy );

        [DllImport( libraryPath, EntryPoint = "LLVMIsOpaqueStruct", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsOpaqueStruct( LLVMTypeRef @StructTy );

        [DllImport( libraryPath, EntryPoint = "LLVMGetElementType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef GetElementType( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMArrayType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef ArrayType( LLVMTypeRef @ElementType, uint @ElementCount );

        [DllImport( libraryPath, EntryPoint = "LLVMGetArrayLength", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetArrayLength( LLVMTypeRef @ArrayTy );

        [DllImport( libraryPath, EntryPoint = "LLVMPointerType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef PointerType( LLVMTypeRef @ElementType, uint @AddressSpace );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPointerAddressSpace", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetPointerAddressSpace( LLVMTypeRef @PointerTy );

        [DllImport( libraryPath, EntryPoint = "LLVMVectorType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef VectorType( LLVMTypeRef @ElementType, uint @ElementCount );

        [DllImport( libraryPath, EntryPoint = "LLVMGetVectorSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetVectorSize( LLVMTypeRef @VectorTy );

        [DllImport( libraryPath, EntryPoint = "LLVMVoidTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef VoidTypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMLabelTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LabelTypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMX86MMXTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86MMXTypeInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMVoidType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef VoidType( );

        [DllImport( libraryPath, EntryPoint = "LLVMLabelType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LabelType( );

        [DllImport( libraryPath, EntryPoint = "LLVMX86MMXType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86MMXType( );

        [DllImport( libraryPath, EntryPoint = "LLVMTypeOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef TypeOf( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetValueKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueKind GetValueKind( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetValueName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetValueName( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMSetValueName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetValueName( LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMDumpValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DumpValue( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMPrintValueToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string PrintValueToString( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMReplaceAllUsesWith", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ReplaceAllUsesWith( LLVMValueRef @OldVal, LLVMValueRef @NewVal );

        [DllImport( libraryPath, EntryPoint = "LLVMIsConstant", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsConstant( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsUndef", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsUndef( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAArgument", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAArgument( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsABasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABasicBlock( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAInlineAsm", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInlineAsm( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAUser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUser( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstant( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsABlockAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABlockAddress( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantAggregateZero", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantAggregateZero( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantArray( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantDataSequential", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantDataSequential( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantDataArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantDataArray( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantDataVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantDataVector( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantExpr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantExpr( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantFP( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantInt( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantPointerNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantPointerNull( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantStruct( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantTokenNone", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantTokenNone( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAConstantVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantVector( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAGlobalValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalValue( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAGlobalAlias", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalAlias( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAGlobalObject", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalObject( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFunction( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAGlobalVariable", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalVariable( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAUndefValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUndefValue( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInstruction( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsABinaryOperator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABinaryOperator( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsACallInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACallInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAIntrinsicInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAIntrinsicInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsADbgInfoIntrinsic", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsADbgInfoIntrinsic( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsADbgDeclareInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsADbgDeclareInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAMemIntrinsic", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemIntrinsic( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAMemCpyInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemCpyInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAMemMoveInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemMoveInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAMemSetInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemSetInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsACmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACmpInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAFCmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFCmpInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAICmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAICmpInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAExtractElementInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAExtractElementInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAGetElementPtrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGetElementPtrInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAInsertElementInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInsertElementInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAInsertValueInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInsertValueInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsALandingPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsALandingPadInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAPHINode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAPHINode( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsASelectInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASelectInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAShuffleVectorInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAShuffleVectorInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAStoreInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAStoreInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsATerminatorInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsATerminatorInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsABranchInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABranchInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAIndirectBrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAIndirectBrInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAInvokeInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInvokeInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAReturnInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsASwitchInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASwitchInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAUnreachableInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUnreachableInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAResumeInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAResumeInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsACleanupReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACleanupReturnInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsACatchReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACatchReturnInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAFuncletPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFuncletPadInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsACatchPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACatchPadInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsACleanupPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACleanupPadInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAUnaryInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUnaryInstruction( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAAllocaInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAAllocaInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsACastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACastInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAAddrSpaceCastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAAddrSpaceCastInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsABitCastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABitCastInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAFPExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPExtInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAFPToSIInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPToSIInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAFPToUIInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPToUIInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAFPTruncInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPTruncInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAIntToPtrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAIntToPtrInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAPtrToIntInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAPtrToIntInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsASExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASExtInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsASIToFPInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASIToFPInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsATruncInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsATruncInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAUIToFPInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUIToFPInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAZExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAZExtInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAExtractValueInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAExtractValueInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsALoadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsALoadInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAVAArgInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAVAArgInst( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAMDNode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMDNode( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAMDString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMDString( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFirstUse", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMUseRef GetFirstUse( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNextUse", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMUseRef GetNextUse( LLVMUseRef @U );

        [DllImport( libraryPath, EntryPoint = "LLVMGetUser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetUser( LLVMUseRef @U );

        [DllImport( libraryPath, EntryPoint = "LLVMGetUsedValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetUsedValue( LLVMUseRef @U );

        [DllImport( libraryPath, EntryPoint = "LLVMGetOperand", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetOperand( LLVMValueRef @Val, uint @Index );

        [DllImport( libraryPath, EntryPoint = "LLVMGetOperandUse", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMUseRef GetOperandUse( LLVMValueRef @Val, uint @Index );

        [DllImport( libraryPath, EntryPoint = "LLVMSetOperand", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetOperand( LLVMValueRef @User, uint @Index, LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNumOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetNumOperands( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNull( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMConstAllOnes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAllOnes( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMGetUndef", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetUndef( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMIsNull", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsNull( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMConstPointerNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstPointerNull( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMConstInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInt( LLVMTypeRef @IntTy, ulong @N, [MarshalAs( UnmanagedType.Bool )]bool @SignExtend );

        [DllImport( libraryPath, EntryPoint = "LLVMConstIntOfArbitraryPrecision", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstIntOfArbitraryPrecision( LLVMTypeRef @IntTy, uint @NumWords, int[ ] @Words );

        [DllImport( libraryPath, EntryPoint = "LLVMConstIntOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstIntOfString( LLVMTypeRef @IntTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, byte @Radix );

        [DllImport( libraryPath, EntryPoint = "LLVMConstIntOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstIntOfStringAndSize( LLVMTypeRef @IntTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, uint @SLen, byte @Radix );

        [DllImport( libraryPath, EntryPoint = "LLVMConstReal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstReal( LLVMTypeRef @RealTy, double @N );

        [DllImport( libraryPath, EntryPoint = "LLVMConstRealOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstRealOfString( LLVMTypeRef @RealTy, [MarshalAs( UnmanagedType.LPStr )] string @Text );

        [DllImport( libraryPath, EntryPoint = "LLVMConstRealOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstRealOfStringAndSize( LLVMTypeRef @RealTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, uint @SLen );

        [DllImport( libraryPath, EntryPoint = "LLVMConstIntGetZExtValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong ConstIntGetZExtValue( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMConstIntGetSExtValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern long ConstIntGetSExtValue( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMConstRealGetDouble", CallingConvention = CallingConvention.Cdecl )]
        internal static extern double ConstRealGetDouble( LLVMValueRef @ConstantVal, [MarshalAs( UnmanagedType.Bool )]out bool @losesInfo );

        [DllImport( libraryPath, EntryPoint = "LLVMConstStringInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstStringInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @Length, [MarshalAs( UnmanagedType.Bool )]bool @DontNullTerminate );

        [DllImport( libraryPath, EntryPoint = "LLVMConstString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstString( [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @Length, [MarshalAs( UnmanagedType.Bool )]bool @DontNullTerminate );

        [DllImport( libraryPath, EntryPoint = "LLVMIsConstantString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsConstantString( LLVMValueRef @c );

        [DllImport( libraryPath, EntryPoint = "LLVMGetAsString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetAsString( LLVMValueRef @c, out size_t @Length );

        [DllImport( libraryPath, EntryPoint = "LLVMConstStructInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstStructInContext( LLVMContextRef @C, out LLVMValueRef @ConstantVals, uint @Count, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( libraryPath, EntryPoint = "LLVMConstStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstStruct( out LLVMValueRef @ConstantVals, uint @Count, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( libraryPath, EntryPoint = "LLVMConstArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstArray( LLVMTypeRef @ElementTy, out LLVMValueRef @ConstantVals, uint @Length );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNamedStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNamedStruct( LLVMTypeRef @StructTy, out LLVMValueRef @ConstantVals, uint @Count );

        [DllImport( libraryPath, EntryPoint = "LLVMGetElementAsConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetElementAsConstant( LLVMValueRef @C, uint @idx );

        [DllImport( libraryPath, EntryPoint = "LLVMConstVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstVector( out LLVMValueRef @ScalarConstantVals, uint @Size );

        [DllImport( libraryPath, EntryPoint = "LLVMGetConstOpcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOpcode GetConstOpcode( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMAlignOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef AlignOf( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMSizeOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef SizeOf( LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNeg( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNSWNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWNeg( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNUWNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWNeg( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFNeg( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNot", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNot( LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMConstAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNSWAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNUWAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNSWSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNUWSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNSWMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstNUWMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstUDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstUDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        // Added to LLVM-C APIs in LLVM 4.0.0
        [DllImport( libraryPath, EntryPoint = "LLVMConstExactUDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExactUDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstSDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstExactSDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExactSDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstURem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstURem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstSRem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSRem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFRem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFRem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstAnd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAnd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstOr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstOr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstXor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstXor( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstICmp", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstICmp( LLVMIntPredicate @Predicate, LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFCmp", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFCmp( LLVMRealPredicate @Predicate, LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstShl", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstShl( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstLShr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstLShr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstAShr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAShr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstGEP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstGEP( LLVMValueRef @ConstantVal, out LLVMValueRef @ConstantIndices, uint @NumIndices );

        [DllImport( libraryPath, EntryPoint = "LLVMConstInBoundsGEP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInBoundsGEP( LLVMValueRef @ConstantVal, out LLVMValueRef @ConstantIndices, uint @NumIndices );

        [DllImport( libraryPath, EntryPoint = "LLVMConstTrunc", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstTrunc( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstSExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstZExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstZExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFPTrunc", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPTrunc( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFPExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstUIToFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstUIToFP( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstSIToFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSIToFP( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFPToUI", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPToUI( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFPToSI", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPToSI( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstPtrToInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstPtrToInt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstIntToPtr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstIntToPtr( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstAddrSpaceCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAddrSpaceCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstZExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstZExtOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstSExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSExtOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstTruncOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstTruncOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstPointerCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstPointerCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstIntCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstIntCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType, [MarshalAs( UnmanagedType.Bool )]bool @isSigned );

        [DllImport( libraryPath, EntryPoint = "LLVMConstFPCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( libraryPath, EntryPoint = "LLVMConstSelect", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSelect( LLVMValueRef @ConstantCondition, LLVMValueRef @ConstantIfTrue, LLVMValueRef @ConstantIfFalse );

        [DllImport( libraryPath, EntryPoint = "LLVMConstExtractElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExtractElement( LLVMValueRef @VectorConstant, LLVMValueRef @IndexConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstInsertElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInsertElement( LLVMValueRef @VectorConstant, LLVMValueRef @ElementValueConstant, LLVMValueRef @IndexConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstShuffleVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstShuffleVector( LLVMValueRef @VectorAConstant, LLVMValueRef @VectorBConstant, LLVMValueRef @MaskConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMConstExtractValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExtractValue( LLVMValueRef @AggConstant, out uint @IdxList, uint @NumIdx );

        [DllImport( libraryPath, EntryPoint = "LLVMConstInsertValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInsertValue( LLVMValueRef @AggConstant, LLVMValueRef @ElementValueConstant, out uint @IdxList, uint @NumIdx );

        [DllImport( libraryPath, EntryPoint = "LLVMConstInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstInlineAsm( LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @AsmString, [MarshalAs( UnmanagedType.LPStr )] string @Constraints, [MarshalAs( UnmanagedType.Bool )]bool @HasSideEffects, [MarshalAs( UnmanagedType.Bool )]bool @IsAlignStack );

        [DllImport( libraryPath, EntryPoint = "LLVMBlockAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BlockAddress( LLVMValueRef @F, LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMGetGlobalParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleRef GetGlobalParent( LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMIsDeclaration", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsDeclaration( LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMGetLinkage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMLinkage GetLinkage( LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMSetLinkage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetLinkage( LLVMValueRef @Global, LLVMLinkage @Linkage );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSection", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetSection( LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMSetSection", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetSection( LLVMValueRef @Global, [MarshalAs( UnmanagedType.LPStr )] string @Section );

        [DllImport( libraryPath, EntryPoint = "LLVMGetVisibility", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMVisibility GetVisibility( LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMSetVisibility", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetVisibility( LLVMValueRef @Global, LLVMVisibility @Viz );

        [DllImport( libraryPath, EntryPoint = "LLVMGetDLLStorageClass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDLLStorageClass GetDLLStorageClass( LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMSetDLLStorageClass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetDLLStorageClass( LLVMValueRef @Global, LLVMDLLStorageClass @Class );

        [DllImport( libraryPath, EntryPoint = "LLVMHasUnnamedAddr", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool HasUnnamedAddr( LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMSetUnnamedAddr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetUnnamedAddr( LLVMValueRef @Global, [MarshalAs( UnmanagedType.Bool )]bool hasUnnamedAddr );

        [DllImport( libraryPath, EntryPoint = "LLVMGetAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetAlignment( LLVMValueRef @V );

        [DllImport( libraryPath, EntryPoint = "LLVMSetAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetAlignment( LLVMValueRef @V, uint @Bytes );

        [DllImport( libraryPath, EntryPoint = "LLVMAddGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddGlobal( LLVMModuleRef @M, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMAddGlobalInAddressSpace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddGlobalInAddressSpace( LLVMModuleRef @M, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @AddressSpace );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNamedGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef GetNamedGlobal( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFirstGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstGlobal( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMGetLastGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastGlobal( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNextGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextGlobal( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPreviousGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousGlobal( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMDeleteGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DeleteGlobal( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMGetInitializer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetInitializer( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMSetInitializer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInitializer( LLVMValueRef @GlobalVar, LLVMValueRef @ConstantVal );

        [DllImport( libraryPath, EntryPoint = "LLVMIsThreadLocal", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsThreadLocal( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMSetThreadLocal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetThreadLocal( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isThreadLocal );

        [DllImport( libraryPath, EntryPoint = "LLVMIsGlobalConstant", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsGlobalConstant( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMSetGlobalConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetGlobalConstant( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isConstant );

        [DllImport( libraryPath, EntryPoint = "LLVMGetThreadLocalMode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMThreadLocalMode GetThreadLocalMode( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMSetThreadLocalMode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetThreadLocalMode( LLVMValueRef @GlobalVar, LLVMThreadLocalMode @Mode );

        [DllImport( libraryPath, EntryPoint = "LLVMIsExternallyInitialized", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsExternallyInitialized( LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMSetExternallyInitialized", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetExternallyInitialized( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool @IsExtInit );

        [DllImport( libraryPath, EntryPoint = "LLVMAddAlias", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddAlias( LLVMModuleRef @M, LLVMTypeRef @Ty, LLVMValueRef @Aliasee, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMDeleteFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DeleteFunction( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMHasPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool HasPersonalityFn( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPersonalityFn( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMSetPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetPersonalityFn( LLVMValueRef @Fn, LLVMValueRef @PersonalityFn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetIntrinsicID", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetIntrinsicID( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFunctionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetFunctionCallConv( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMSetFunctionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetFunctionCallConv( LLVMValueRef @Fn, uint @CC );

        [DllImport( libraryPath, EntryPoint = "LLVMGetGC", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetGC( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMSetGC", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetGC( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMAddAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, LLVMAttributeRef @A );

        [DllImport( libraryPath, EntryPoint = "LLVMGetAttributeCountAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetAttributeCountAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx );

        [DllImport( libraryPath, EntryPoint = "LLVMGetAttributesAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetAttributesAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, out LLVMAttributeRef Attrs );

        [DllImport( libraryPath, EntryPoint = "LLVMGetEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef GetEnumAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( libraryPath, EntryPoint = "LLVMGetStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef GetStringAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( libraryPath, EntryPoint = "LLVMRemoveEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RemoveEnumAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( libraryPath, EntryPoint = "LLVMRemoveStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void RemoveStringAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( libraryPath, EntryPoint = "LLVMAddTargetDependentFunctionAttr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void AddTargetDependentFunctionAttr( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @A, [MarshalAs( UnmanagedType.LPStr )] string @V );

        [DllImport( libraryPath, EntryPoint = "LLVMCountParams", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountParams( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetParams", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetParams( LLVMValueRef @Fn, out LLVMValueRef @Params );

        [DllImport( libraryPath, EntryPoint = "LLVMGetParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetParam( LLVMValueRef @Fn, uint @Index );

        [DllImport( libraryPath, EntryPoint = "LLVMGetParamParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetParamParent( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFirstParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstParam( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetLastParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastParam( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNextParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextParam( LLVMValueRef @Arg );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPreviousParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousParam( LLVMValueRef @Arg );

        [DllImport( libraryPath, EntryPoint = "LLVMSetParamAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetParamAlignment( LLVMValueRef @Arg, uint @Align );

        [DllImport( libraryPath, EntryPoint = "LLVMMDStringInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef MDStringInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @SLen );

        [DllImport( libraryPath, EntryPoint = "LLVMMDString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef MDString( [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @SLen );

        [DllImport( libraryPath, EntryPoint = "LLVMMDNodeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef MDNodeInContext( LLVMContextRef @C, out LLVMValueRef @Vals, uint @Count );

        [DllImport( libraryPath, EntryPoint = "LLVMMDNode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef MDNode( out LLVMValueRef @Vals, uint @Count );

        [DllImport( libraryPath, EntryPoint = "LLVMGetMDString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetMDString( LLVMValueRef @V, out uint @Length );

        [DllImport( libraryPath, EntryPoint = "LLVMGetMDNodeNumOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetMDNodeNumOperands( LLVMValueRef @V );

        [DllImport( libraryPath, EntryPoint = "LLVMGetMDNodeOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetMDNodeOperands( LLVMValueRef @V, out LLVMValueRef @Dest );

        [DllImport( libraryPath, EntryPoint = "LLVMBasicBlockAsValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BasicBlockAsValue( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMValueIsBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool ValueIsBasicBlock( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMValueAsBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef ValueAsBasicBlock( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBasicBlockName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetBasicBlockName( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBasicBlockParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetBasicBlockParent( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBasicBlockTerminator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetBasicBlockTerminator( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMCountBasicBlocks", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountBasicBlocks( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBasicBlocks", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetBasicBlocks( LLVMValueRef @Fn, out LLVMBasicBlockRef @BasicBlocks );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFirstBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetFirstBasicBlock( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetLastBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetLastBasicBlock( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNextBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetNextBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPreviousBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetPreviousBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMGetEntryBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetEntryBasicBlock( LLVMValueRef @Fn );

        [DllImport( libraryPath, EntryPoint = "LLVMAppendBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef AppendBasicBlockInContext( LLVMContextRef @C, LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMAppendBasicBlock", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef AppendBasicBlock( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMInsertBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef InsertBasicBlockInContext( LLVMContextRef @C, LLVMBasicBlockRef @BB, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMInsertBasicBlock", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef InsertBasicBlock( LLVMBasicBlockRef @InsertBeforeBB, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMDeleteBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DeleteBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMRemoveBasicBlockFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RemoveBasicBlockFromParent( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMMoveBasicBlockBefore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveBasicBlockBefore( LLVMBasicBlockRef @BB, LLVMBasicBlockRef @MovePos );

        [DllImport( libraryPath, EntryPoint = "LLVMMoveBasicBlockAfter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveBasicBlockAfter( LLVMBasicBlockRef @BB, LLVMBasicBlockRef @MovePos );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFirstInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstInstruction( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMGetLastInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastInstruction( LLVMBasicBlockRef @BB );

        [DllImport( libraryPath, EntryPoint = "LLVMHasMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int HasMetadata( LLVMValueRef @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMGetMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetMetadata( LLVMValueRef @Val, uint @KindID );

        [DllImport( libraryPath, EntryPoint = "LLVMSetMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetMetadata( LLVMValueRef @Val, uint @KindID, LLVMValueRef @Node );

        [DllImport( libraryPath, EntryPoint = "LLVMGetInstructionParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetInstructionParent( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNextInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextInstruction( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPreviousInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousInstruction( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMInstructionRemoveFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InstructionRemoveFromParent( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMInstructionEraseFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InstructionEraseFromParent( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetInstructionOpcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOpcode GetInstructionOpcode( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetICmpPredicate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMIntPredicate GetICmpPredicate( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFCmpPredicate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMRealPredicate GetFCmpPredicate( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMInstructionClone", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef InstructionClone( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNumArgOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumArgOperands( LLVMValueRef @Instr );

        [DllImport( libraryPath, EntryPoint = "LLVMSetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInstructionCallConv( LLVMValueRef @Instr, uint @CC );

        [DllImport( libraryPath, EntryPoint = "LLVMGetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetInstructionCallConv( LLVMValueRef @Instr );

        [DllImport( libraryPath, EntryPoint = "LLVMSetInstrParamAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInstrParamAlignment( LLVMValueRef @Instr, uint @index, uint @Align );

        [DllImport( libraryPath, EntryPoint = "LLVMAddCallSiteAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCallSiteAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, LLVMAttributeRef @A );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCallSiteAttributeCount", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetCallSiteAttributeCount( LLVMValueRef C, LLVMAttributeIndex Idx );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCallSiteAttributes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetCallSiteAttributes( LLVMValueRef C, LLVMAttributeIndex Idx, out LLVMAttributeRef attributes );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef GetCallSiteEnumAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef GetCallSiteStringAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( libraryPath, EntryPoint = "LLVMRemoveCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RemoveCallSiteEnumAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( libraryPath, EntryPoint = "LLVMRemoveCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void RemoveCallSiteStringAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCalledValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetCalledValue( LLVMValueRef @Instr );

        [DllImport( libraryPath, EntryPoint = "LLVMIsTailCall", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsTailCall( LLVMValueRef @CallInst );

        [DllImport( libraryPath, EntryPoint = "LLVMSetTailCall", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetTailCall( LLVMValueRef @CallInst, [MarshalAs( UnmanagedType.Bool )]bool isTailCall );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNormalDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetNormalDest( LLVMValueRef @InvokeInst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetUnwindDest( LLVMValueRef @InvokeInst );

        [DllImport( libraryPath, EntryPoint = "LLVMSetNormalDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetNormalDest( LLVMValueRef @InvokeInst, LLVMBasicBlockRef @B );

        [DllImport( libraryPath, EntryPoint = "LLVMSetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetUnwindDest( LLVMValueRef @InvokeInst, LLVMBasicBlockRef @B );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNumSuccessors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumSuccessors( LLVMValueRef @Term );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSuccessor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetSuccessor( LLVMValueRef @Term, uint @i );

        [DllImport( libraryPath, EntryPoint = "LLVMSetSuccessor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetSuccessor( LLVMValueRef @Term, uint @i, LLVMBasicBlockRef @block );

        [DllImport( libraryPath, EntryPoint = "LLVMIsConditional", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsConditional( LLVMValueRef @Branch );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCondition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetCondition( LLVMValueRef @Branch );

        [DllImport( libraryPath, EntryPoint = "LLVMSetCondition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCondition( LLVMValueRef @Branch, LLVMValueRef @Cond );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSwitchDefaultDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetSwitchDefaultDest( LLVMValueRef @SwitchInstr );

        [DllImport( libraryPath, EntryPoint = "LLVMGetAllocatedType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef GetAllocatedType( LLVMValueRef @Alloca );

        [DllImport( libraryPath, EntryPoint = "LLVMIsInBounds", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsInBounds( LLVMValueRef @GEP );

        [DllImport( libraryPath, EntryPoint = "LLVMSetIsInBounds", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetIsInBounds( LLVMValueRef @GEP, [MarshalAs( UnmanagedType.Bool )]bool @InBounds );

        [DllImport( libraryPath, EntryPoint = "LLVMAddIncoming", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIncoming( LLVMValueRef @PhiNode, out LLVMValueRef @IncomingValues, out LLVMBasicBlockRef @IncomingBlocks, uint @Count );

        [DllImport( libraryPath, EntryPoint = "LLVMCountIncoming", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountIncoming( LLVMValueRef @PhiNode );

        [DllImport( libraryPath, EntryPoint = "LLVMGetIncomingValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetIncomingValue( LLVMValueRef @PhiNode, uint @Index );

        [DllImport( libraryPath, EntryPoint = "LLVMGetIncomingBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetIncomingBlock( LLVMValueRef @PhiNode, uint @Index );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNumIndices", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumIndices( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMGetIndices", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetIndices( LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateBuilderInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBuilderRef CreateBuilderInContext( LLVMContextRef @C );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBuilderRef CreateBuilder( );

        [DllImport( libraryPath, EntryPoint = "LLVMPositionBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PositionBuilder( LLVMBuilderRef @Builder, LLVMBasicBlockRef @Block, LLVMValueRef @Instr );

        [DllImport( libraryPath, EntryPoint = "LLVMPositionBuilderBefore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PositionBuilderBefore( LLVMBuilderRef @Builder, LLVMValueRef @Instr );

        [DllImport( libraryPath, EntryPoint = "LLVMPositionBuilderAtEnd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PositionBuilderAtEnd( LLVMBuilderRef @Builder, LLVMBasicBlockRef @Block );

        [DllImport( libraryPath, EntryPoint = "LLVMGetInsertBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetInsertBlock( LLVMBuilderRef @Builder );

        [DllImport( libraryPath, EntryPoint = "LLVMClearInsertionPosition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ClearInsertionPosition( LLVMBuilderRef @Builder );

        [DllImport( libraryPath, EntryPoint = "LLVMInsertIntoBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InsertIntoBuilder( LLVMBuilderRef @Builder, LLVMValueRef @Instr );

        [DllImport( libraryPath, EntryPoint = "LLVMInsertIntoBuilderWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void InsertIntoBuilderWithName( LLVMBuilderRef @Builder, LLVMValueRef @Instr, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeBuilder( IntPtr @Builder );

        [DllImport( libraryPath, EntryPoint = "LLVMSetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCurrentDebugLocation( LLVMBuilderRef @Builder, LLVMValueRef @L );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetCurrentDebugLocation( LLVMBuilderRef @Builder );

        [DllImport( libraryPath, EntryPoint = "LLVMSetInstDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInstDebugLocation( LLVMBuilderRef @Builder, LLVMValueRef @Inst );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildRetVoid", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildRetVoid( LLVMBuilderRef @param0 );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildRet", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildRet( LLVMBuilderRef @param0, LLVMValueRef @V );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAggregateRet", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildAggregateRet( LLVMBuilderRef @param0, out LLVMValueRef @RetVals, uint @N );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildBr( LLVMBuilderRef @param0, LLVMBasicBlockRef @Dest );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildCondBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildCondBr( LLVMBuilderRef @param0, LLVMValueRef @If, LLVMBasicBlockRef @Then, LLVMBasicBlockRef @Else );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSwitch", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildSwitch( LLVMBuilderRef @param0, LLVMValueRef @V, LLVMBasicBlockRef @Else, uint @NumCases );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildIndirectBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildIndirectBr( LLVMBuilderRef @B, LLVMValueRef @Addr, uint @NumDests );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildInvoke", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInvoke( LLVMBuilderRef @param0, LLVMValueRef @Fn, out LLVMValueRef @Args, uint @NumArgs, LLVMBasicBlockRef @Then, LLVMBasicBlockRef @Catch, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildLandingPad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildLandingPad( LLVMBuilderRef @B, LLVMTypeRef @Ty, LLVMValueRef @PersFn, uint @NumClauses, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildResume", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildResume( LLVMBuilderRef @B, LLVMValueRef @Exn );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildUnreachable", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildUnreachable( LLVMBuilderRef @param0 );

        [DllImport( libraryPath, EntryPoint = "LLVMAddCase", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCase( LLVMValueRef @Switch, LLVMValueRef @OnVal, LLVMBasicBlockRef @Dest );

        [DllImport( libraryPath, EntryPoint = "LLVMAddDestination", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDestination( LLVMValueRef @IndirectBr, LLVMBasicBlockRef @Dest );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNumClauses", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumClauses( LLVMValueRef @LandingPad );

        [DllImport( libraryPath, EntryPoint = "LLVMGetClause", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetClause( LLVMValueRef @LandingPad, uint @Idx );

        [DllImport( libraryPath, EntryPoint = "LLVMAddClause", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddClause( LLVMValueRef @LandingPad, LLVMValueRef @ClauseVal );

        [DllImport( libraryPath, EntryPoint = "LLVMIsCleanup", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsCleanup( LLVMValueRef @LandingPad );

        [DllImport( libraryPath, EntryPoint = "LLVMSetCleanup", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCleanup( LLVMValueRef @LandingPad, [MarshalAs( UnmanagedType.Bool )]bool @Val );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNSWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNUWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNSWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNUWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNSWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNUWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildUDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        // Added to LLVM-C API in LLVM 4.0.0
        [DllImport( libraryPath, EntryPoint = "LLVMBuildExactUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExactUDiv( LLVMBuilderRef @param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildExactSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExactSDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildURem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildURem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSRem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFRem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildShl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildShl( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildLShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildLShr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAShr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAnd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildOr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildOr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildXor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildXor( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildBinOp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildBinOp( LLVMBuilderRef @B, LLVMOpcode @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNeg( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNSWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWNeg( LLVMBuilderRef @B, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNUWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWNeg( LLVMBuilderRef @B, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFNeg( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildNot", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNot( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildMalloc( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildArrayMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildArrayMalloc( LLVMBuilderRef @param0, LLVMTypeRef @Ty, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAlloca( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildArrayAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildArrayAlloca( LLVMBuilderRef @param0, LLVMTypeRef @Ty, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFree", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildFree( LLVMBuilderRef @param0, LLVMValueRef @PointerVal );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildLoad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildLoad( LLVMBuilderRef @param0, LLVMValueRef @PointerVal, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildStore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildStore( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMValueRef @Ptr );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, out LLVMValueRef @Indices, uint @NumIndices, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildInBoundsGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInBoundsGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, out LLVMValueRef @Indices, uint @NumIndices, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildStructGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildStructGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, uint @Idx, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildGlobalString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildGlobalString( LLVMBuilderRef @B, [MarshalAs( UnmanagedType.LPStr )] string @Str, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildGlobalStringPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildGlobalStringPtr( LLVMBuilderRef @B, [MarshalAs( UnmanagedType.LPStr )] string @Str, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetVolatile", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool GetVolatile( LLVMValueRef @MemoryAccessInst );

        [DllImport( libraryPath, EntryPoint = "LLVMSetVolatile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetVolatile( LLVMValueRef @MemoryAccessInst, [MarshalAs( UnmanagedType.Bool )]bool @IsVolatile );

        [DllImport( libraryPath, EntryPoint = "LLVMGetOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering GetOrdering( LLVMValueRef @MemoryAccessInst );

        [DllImport( libraryPath, EntryPoint = "LLVMSetOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetOrdering( LLVMValueRef @MemoryAccessInst, LLVMAtomicOrdering @Ordering );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildTrunc( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildZExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildZExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFPToUI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPToUI( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFPToSI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPToSI( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildUIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildUIToFP( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSIToFP( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFPTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPTrunc( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFPExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildPtrToInt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPtrToInt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildIntToPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIntToPtr( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAddrSpaceCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAddrSpaceCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildZExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildZExtOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSExtOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildTruncOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildTruncOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildCast( LLVMBuilderRef @B, LLVMOpcode @Op, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildPointerCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPointerCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildIntCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIntCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFPCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildICmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildICmp( LLVMBuilderRef @param0, LLVMIntPredicate @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFCmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFCmp( LLVMBuilderRef @param0, LLVMRealPredicate @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildPhi", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPhi( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildCall", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildCall( LLVMBuilderRef @param0, LLVMValueRef @Fn, out LLVMValueRef @Args, uint @NumArgs, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildSelect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSelect( LLVMBuilderRef @param0, LLVMValueRef @If, LLVMValueRef @Then, LLVMValueRef @Else, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildVAArg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildVAArg( LLVMBuilderRef @param0, LLVMValueRef @List, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildExtractElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExtractElement( LLVMBuilderRef @param0, LLVMValueRef @VecVal, LLVMValueRef @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildInsertElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInsertElement( LLVMBuilderRef @param0, LLVMValueRef @VecVal, LLVMValueRef @EltVal, LLVMValueRef @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildShuffleVector", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildShuffleVector( LLVMBuilderRef @param0, LLVMValueRef @V1, LLVMValueRef @V2, LLVMValueRef @Mask, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildExtractValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExtractValue( LLVMBuilderRef @param0, LLVMValueRef @AggVal, uint @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildInsertValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInsertValue( LLVMBuilderRef @param0, LLVMValueRef @AggVal, LLVMValueRef @EltVal, uint @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildIsNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIsNull( LLVMBuilderRef @param0, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildIsNotNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIsNotNull( LLVMBuilderRef @param0, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildPtrDiff", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPtrDiff( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildFence", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFence( LLVMBuilderRef @B, LLVMAtomicOrdering @ordering, [MarshalAs( UnmanagedType.Bool )]bool @singleThread, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAtomicRMW", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildAtomicRMW( LLVMBuilderRef @B, LLVMAtomicRMWBinOp @op, LLVMValueRef @PTR, LLVMValueRef @Val, LLVMAtomicOrdering @ordering, [MarshalAs( UnmanagedType.Bool )]bool @singleThread );

        [DllImport( libraryPath, EntryPoint = "LLVMBuildAtomicCmpXchg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildAtomicCmpXchg( LLVMBuilderRef @B, LLVMValueRef @Ptr, LLVMValueRef @Cmp, LLVMValueRef @New, LLVMAtomicOrdering @SuccessOrdering, LLVMAtomicOrdering @FailureOrdering, [MarshalAs( UnmanagedType.Bool )]bool @SingleThread );

        [DllImport( libraryPath, EntryPoint = "LLVMIsAtomicSingleThread", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsAtomicSingleThread( LLVMValueRef @AtomicInst );

        [DllImport( libraryPath, EntryPoint = "LLVMSetAtomicSingleThread", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetAtomicSingleThread( LLVMValueRef @AtomicInst, [MarshalAs( UnmanagedType.Bool )]bool @SingleThread );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCmpXchgSuccessOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering GetCmpXchgSuccessOrdering( LLVMValueRef @CmpXchgInst );

        [DllImport( libraryPath, EntryPoint = "LLVMSetCmpXchgSuccessOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCmpXchgSuccessOrdering( LLVMValueRef @CmpXchgInst, LLVMAtomicOrdering @Ordering );

        [DllImport( libraryPath, EntryPoint = "LLVMGetCmpXchgFailureOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering GetCmpXchgFailureOrdering( LLVMValueRef @CmpXchgInst );

        [DllImport( libraryPath, EntryPoint = "LLVMSetCmpXchgFailureOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCmpXchgFailureOrdering( LLVMValueRef @CmpXchgInst, LLVMAtomicOrdering @Ordering );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateModuleProviderForExistingModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleProviderRef CreateModuleProviderForExistingModule( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeModuleProvider", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeModuleProvider( LLVMModuleProviderRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateMemoryBufferWithContentsOfFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus CreateMemoryBufferWithContentsOfFile( [MarshalAs( UnmanagedType.LPStr )] string @Path, out LLVMMemoryBufferRef @OutMemBuf, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]out string @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateMemoryBufferWithSTDIN", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateMemoryBufferWithSTDIN( out LLVMMemoryBufferRef @OutMemBuf, out IntPtr @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateMemoryBufferWithMemoryRange", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMMemoryBufferRef CreateMemoryBufferWithMemoryRange( [MarshalAs( UnmanagedType.LPStr )] string @InputData, size_t @InputDataLength, [MarshalAs( UnmanagedType.LPStr )] string @BufferName, [MarshalAs( UnmanagedType.Bool )]bool @RequiresNullTerminator );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateMemoryBufferWithMemoryRangeCopy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMMemoryBufferRef CreateMemoryBufferWithMemoryRangeCopy( [MarshalAs( UnmanagedType.LPStr )] string @InputData, size_t @InputDataLength, [MarshalAs( UnmanagedType.LPStr )] string @BufferName );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBufferStart", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetBufferStart( LLVMMemoryBufferRef @MemBuf );

        [DllImport( libraryPath, EntryPoint = "LLVMGetBufferSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern size_t GetBufferSize( LLVMMemoryBufferRef @MemBuf );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeMemoryBuffer( LLVMMemoryBufferRef @MemBuf );

        [DllImport( libraryPath, EntryPoint = "LLVMGetGlobalPassRegistry", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassRegistryRef GetGlobalPassRegistry( );

        [DllImport( libraryPath, EntryPoint = "LLVMCreatePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef CreatePassManager( );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateFunctionPassManagerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef CreateFunctionPassManagerForModule( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef CreateFunctionPassManager( LLVMModuleProviderRef @MP );

        [DllImport( libraryPath, EntryPoint = "LLVMRunPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool RunPassManager( LLVMPassManagerRef @PM, LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool InitializeFunctionPassManager( LLVMPassManagerRef @FPM );

        [DllImport( libraryPath, EntryPoint = "LLVMRunFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool RunFunctionPassManager( LLVMPassManagerRef @FPM, LLVMValueRef @F );

        [DllImport( libraryPath, EntryPoint = "LLVMFinalizeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool FinalizeFunctionPassManager( LLVMPassManagerRef @FPM );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposePassManager( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMIsMultithreaded", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsMultithreaded( );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateDisasm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef CreateDisasm( [MarshalAs( UnmanagedType.LPStr )] string @TripleName, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateDisasmCPU", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef CreateDisasmCPU( [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateDisasmCPUFeatures", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef CreateDisasmCPUFeatures( [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, [MarshalAs( UnmanagedType.LPStr )] string @Features, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( libraryPath, EntryPoint = "LLVMSetDisasmOptions", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int SetDisasmOptions( LLVMDisasmContextRef @DC, int @Options );

        [DllImport( libraryPath, EntryPoint = "LLVMDisasmDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisasmDispose( LLVMDisasmContextRef @DC );

        [DllImport( libraryPath, EntryPoint = "LLVMDisasmInstruction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern ulong DisasmInstruction( LLVMDisasmContextRef @DC, IntPtr @Bytes, long @BytesSize, long @PC, IntPtr @OutString, size_t @OutStringSize );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSystemZTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeHexagonTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNVPTXTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMSP430TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430TargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeXCoreTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMipsTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAArch64TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64TargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeARMTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializePowerPCTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSparcTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeX86TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86TargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeBPFTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFTargetInfo( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAMDGPUTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSystemZTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeHexagonTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNVPTXTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMSP430Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430Target( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeXCoreTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMipsTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAArch64Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64Target( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeARMTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializePowerPCTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSparcTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeX86Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86Target( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeBPFTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSystemZTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeHexagonTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNVPTXTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMSP430TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430TargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeXCoreTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMipsTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAArch64TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64TargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeARMTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializePowerPCTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSparcTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeX86TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86TargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeBPFTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFTargetMC( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSystemZAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeHexagonAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNVPTXAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMSP430AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430AsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeXCoreAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMipsAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAArch64AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64AsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeARMAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializePowerPCAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSparcAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeX86AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86AsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeBPFAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUAsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSystemZAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZAsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMipsAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsAsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAArch64AsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64AsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeARMAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMAsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializePowerPCAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCAsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSparcAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcAsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeX86AsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86AsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSystemZDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeHexagonDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeXCoreDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMipsDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAArch64Disassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64Disassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeARMDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializePowerPCDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeSparcDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeX86Disassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86Disassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAllTargetInfos", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllTargetInfos( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAllTargets", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllTargets( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAllTargetMCs", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllTargetMCs( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAllAsmPrinters", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllAsmPrinters( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAllAsmParsers", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllAsmParsers( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAllDisassemblers", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllDisassemblers( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNativeTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNativeAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeAsmParser( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNativeAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeAsmPrinter( );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeNativeDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeDisassembler( );

        [DllImport( libraryPath, EntryPoint = "LLVMGetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef GetModuleDataLayout( LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMSetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetModuleDataLayout( LLVMModuleRef @M, LLVMTargetDataRef @DL );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateTargetData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetDataRef CreateTargetData( [MarshalAs( UnmanagedType.LPStr )] string @StringRep );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeTargetData", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeTargetData( LLVMTargetDataRef @TD );

        [DllImport( libraryPath, EntryPoint = "LLVMAddTargetLibraryInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddTargetLibraryInfo( LLVMTargetLibraryInfoRef @TLI, LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMCopyStringRepOfTargetData", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string CopyStringRepOfTargetData( LLVMTargetDataRef @TD );

        [DllImport( libraryPath, EntryPoint = "LLVMByteOrder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMByteOrdering ByteOrder( LLVMTargetDataRef @TD );

        [DllImport( libraryPath, EntryPoint = "LLVMPointerSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PointerSize( LLVMTargetDataRef @TD );

        [DllImport( libraryPath, EntryPoint = "LLVMPointerSizeForAS", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PointerSizeForAS( LLVMTargetDataRef @TD, uint @AS );

        [DllImport( libraryPath, EntryPoint = "LLVMIntPtrType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrType( LLVMTargetDataRef @TD );

        [DllImport( libraryPath, EntryPoint = "LLVMIntPtrTypeForAS", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrTypeForAS( LLVMTargetDataRef @TD, uint @AS );

        [DllImport( libraryPath, EntryPoint = "LLVMIntPtrTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrTypeInContext( LLVMContextRef @C, LLVMTargetDataRef @TD );

        [DllImport( libraryPath, EntryPoint = "LLVMIntPtrTypeForASInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrTypeForASInContext( LLVMContextRef @C, LLVMTargetDataRef @TD, uint @AS );

        [DllImport( libraryPath, EntryPoint = "LLVMSizeOfTypeInBits", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong SizeOfTypeInBits( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMStoreSizeOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong StoreSizeOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMABISizeOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong ABISizeOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMABIAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint ABIAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMCallFrameAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CallFrameAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMPreferredAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PreferredAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( libraryPath, EntryPoint = "LLVMPreferredAlignmentOfGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PreferredAlignmentOfGlobal( LLVMTargetDataRef @TD, LLVMValueRef @GlobalVar );

        [DllImport( libraryPath, EntryPoint = "LLVMElementAtOffset", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint ElementAtOffset( LLVMTargetDataRef @TD, LLVMTypeRef @StructTy, ulong @Offset );

        [DllImport( libraryPath, EntryPoint = "LLVMOffsetOfElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong OffsetOfElement( LLVMTargetDataRef @TD, LLVMTypeRef @StructTy, uint @Element );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFirstTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef GetFirstTarget( );

        [DllImport( libraryPath, EntryPoint = "LLVMGetNextTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef GetNextTarget( LLVMTargetRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetFromName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetRef GetTargetFromName( [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetFromTriple", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus GetTargetFromTriple( [MarshalAs( UnmanagedType.LPStr )] string @Triple, out LLVMTargetRef @T, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetTargetName( LLVMTargetRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetDescription", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetTargetDescription( LLVMTargetRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMTargetHasJIT", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TargetHasJIT( LLVMTargetRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMTargetHasTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TargetHasTargetMachine( LLVMTargetRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMTargetHasAsmBackend", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TargetHasAsmBackend( LLVMTargetRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateTargetMachine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetMachineRef CreateTargetMachine( LLVMTargetRef @T, [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, [MarshalAs( UnmanagedType.LPStr )] string @Features, LLVMCodeGenOptLevel @Level, LLVMRelocMode @Reloc, LLVMCodeModel @CodeModel );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeTargetMachine( LLVMTargetMachineRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetMachineTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef GetTargetMachineTarget( LLVMTargetMachineRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetMachineTriple", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetTargetMachineTriple( LLVMTargetMachineRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetMachineCPU", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetTargetMachineCPU( LLVMTargetMachineRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMGetTargetMachineFeatureString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetTargetMachineFeatureString( LLVMTargetMachineRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateTargetDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef CreateTargetDataLayout( LLVMTargetMachineRef @T );

        [DllImport( libraryPath, EntryPoint = "LLVMSetTargetMachineAsmVerbosity", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetTargetMachineAsmVerbosity( LLVMTargetMachineRef @T, [MarshalAs( UnmanagedType.Bool )]bool @VerboseAsm );

        [DllImport( libraryPath, EntryPoint = "LLVMTargetMachineEmitToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus TargetMachineEmitToFile( LLVMTargetMachineRef @T, LLVMModuleRef @M, string @Filename, LLVMCodeGenFileType @codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMTargetMachineEmitToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus TargetMachineEmitToMemoryBuffer( LLVMTargetMachineRef @T, LLVMModuleRef @M, LLVMCodeGenFileType @codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage, out LLVMMemoryBufferRef @OutMemBuf );

        [DllImport( libraryPath, EntryPoint = "LLVMGetDefaultTargetTriple", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetDefaultTargetTriple( );

        [DllImport( libraryPath, EntryPoint = "LLVMAddAnalysisPasses", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAnalysisPasses( LLVMTargetMachineRef @T, LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMLinkInMCJIT", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LinkInMCJIT( );

        [DllImport( libraryPath, EntryPoint = "LLVMLinkInInterpreter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LinkInInterpreter( );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateGenericValueOfInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef CreateGenericValueOfInt( LLVMTypeRef @Ty, ulong @N, [MarshalAs( UnmanagedType.Bool )]bool @IsSigned );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateGenericValueOfPointer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef CreateGenericValueOfPointer( IntPtr @P );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateGenericValueOfFloat", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef CreateGenericValueOfFloat( LLVMTypeRef @Ty, double @N );

        [DllImport( libraryPath, EntryPoint = "LLVMGenericValueIntWidth", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GenericValueIntWidth( LLVMGenericValueRef @GenValRef );

        [DllImport( libraryPath, EntryPoint = "LLVMGenericValueToInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong GenericValueToInt( LLVMGenericValueRef @GenVal, [MarshalAs( UnmanagedType.Bool )]bool @IsSigned );

        [DllImport( libraryPath, EntryPoint = "LLVMGenericValueToPointer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GenericValueToPointer( LLVMGenericValueRef @GenVal );

        [DllImport( libraryPath, EntryPoint = "LLVMGenericValueToFloat", CallingConvention = CallingConvention.Cdecl )]
        internal static extern double GenericValueToFloat( LLVMTypeRef @TyRef, LLVMGenericValueRef @GenVal );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeGenericValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeGenericValue( LLVMGenericValueRef @GenVal );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateExecutionEngineForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateExecutionEngineForModule( out LLVMExecutionEngineRef @OutEE, LLVMModuleRef @M, out IntPtr @OutError );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateInterpreterForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateInterpreterForModule( out LLVMExecutionEngineRef @OutInterp, LLVMModuleRef @M, out IntPtr @OutError );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateJITCompilerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateJITCompilerForModule( out LLVMExecutionEngineRef @OutJIT, LLVMModuleRef @M, uint @OptLevel, out IntPtr @OutError );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeMCJITCompilerOptions", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMCJITCompilerOptions( out LLVMMCJITCompilerOptions @Options, size_t @SizeOfOptions );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateMCJITCompilerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateMCJITCompilerForModule( out LLVMExecutionEngineRef @OutJIT, LLVMModuleRef @M, out LLVMMCJITCompilerOptions @Options, size_t @SizeOfOptions, out IntPtr @OutError );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeExecutionEngine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeExecutionEngine( LLVMExecutionEngineRef @EE );

        [DllImport( libraryPath, EntryPoint = "LLVMRunStaticConstructors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RunStaticConstructors( LLVMExecutionEngineRef @EE );

        [DllImport( libraryPath, EntryPoint = "LLVMRunStaticDestructors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RunStaticDestructors( LLVMExecutionEngineRef @EE );

        [DllImport( libraryPath, EntryPoint = "LLVMRunFunctionAsMain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern int RunFunctionAsMain( LLVMExecutionEngineRef @EE, LLVMValueRef @F, uint @ArgC, string[ ] @ArgV, string[ ] @EnvP );

        [DllImport( libraryPath, EntryPoint = "LLVMRunFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef RunFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @F, uint @NumArgs, out LLVMGenericValueRef @Args );

        [DllImport( libraryPath, EntryPoint = "LLVMFreeMachineCodeForFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void FreeMachineCodeForFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @F );

        [DllImport( libraryPath, EntryPoint = "LLVMAddModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddModule( LLVMExecutionEngineRef @EE, LLVMModuleRef @M );

        [DllImport( libraryPath, EntryPoint = "LLVMRemoveModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus RemoveModule( LLVMExecutionEngineRef @EE, LLVMModuleRef @M, out LLVMModuleRef @OutMod, out IntPtr @OutError );

        [DllImport( libraryPath, EntryPoint = "LLVMFindFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus FindFunction( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name, out LLVMValueRef @OutFn );

        /* As of at least LLVM 4.0.1 this just returns null
        //[DllImport( libraryPath, EntryPoint = "LLVMRecompileAndRelinkFunction", CallingConvention = CallingConvention.Cdecl )]
        //internal static extern IntPtr RecompileAndRelinkFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @Fn );
        */

        [DllImport( libraryPath, EntryPoint = "LLVMGetExecutionEngineTargetData", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef GetExecutionEngineTargetData( LLVMExecutionEngineRef @EE );

        [DllImport( libraryPath, EntryPoint = "LLVMGetExecutionEngineTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetMachineRef GetExecutionEngineTargetMachine( LLVMExecutionEngineRef @EE );

        [DllImport( libraryPath, EntryPoint = "LLVMAddGlobalMapping", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGlobalMapping( LLVMExecutionEngineRef @EE, LLVMValueRef @Global, IntPtr @Addr );

        [DllImport( libraryPath, EntryPoint = "LLVMGetPointerToGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetPointerToGlobal( LLVMExecutionEngineRef @EE, LLVMValueRef @Global );

        [DllImport( libraryPath, EntryPoint = "LLVMGetGlobalValueAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern int GetGlobalValueAddress( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMGetFunctionAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern int GetFunctionAddress( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateSimpleMCJITMemoryManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMCJITMemoryManagerRef CreateSimpleMCJITMemoryManager( IntPtr @Opaque, LLVMMemoryManagerAllocateCodeSectionCallback @AllocateCodeSection, LLVMMemoryManagerAllocateDataSectionCallback @AllocateDataSection, LLVMMemoryManagerFinalizeMemoryCallback @FinalizeMemory, LLVMMemoryManagerDestroyCallback @Destroy );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeMCJITMemoryManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeMCJITMemoryManager( LLVMMCJITMemoryManagerRef @MM );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeTransformUtils", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeTransformUtils( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeScalarOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeScalarOpts( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeObjCARCOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeObjCARCOpts( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeVectorization", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeVectorization( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeInstCombine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeInstCombine( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeIPO", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeIPO( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeInstrumentation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeInstrumentation( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeAnalysis", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAnalysis( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeIPA", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeIPA( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeCodeGen", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeCodeGen( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMInitializeTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeTarget( LLVMPassRegistryRef @R );

        [DllImport( libraryPath, EntryPoint = "LLVMParseIRInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseIRInContext( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM, out IntPtr @OutMessage );

        [DllImport( libraryPath, EntryPoint = "LLVMLinkModules2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LinkModules2( LLVMModuleRef @Dest, LLVMModuleRef @Src );

        [DllImport( libraryPath, EntryPoint = "LLVMCreateObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMObjectFileRef CreateObjectFile( LLVMMemoryBufferRef @MemBuf );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeObjectFile( LLVMObjectFileRef @ObjectFile );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSections", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSectionIteratorRef GetSections( LLVMObjectFileRef @ObjectFile );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeSectionIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeSectionIterator( LLVMSectionIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMIsSectionIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsSectionIteratorAtEnd( LLVMObjectFileRef @ObjectFile, LLVMSectionIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMMoveToNextSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToNextSection( LLVMSectionIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMMoveToContainingSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToContainingSection( LLVMSectionIteratorRef @Sect, LLVMSymbolIteratorRef @Sym );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSymbols", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef GetSymbols( LLVMObjectFileRef @ObjectFile );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeSymbolIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeSymbolIterator( LLVMSymbolIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMIsSymbolIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsSymbolIteratorAtEnd( LLVMObjectFileRef @ObjectFile, LLVMSymbolIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMMoveToNextSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToNextSymbol( LLVMSymbolIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSectionName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetSectionName( LLVMSectionIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSectionSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSectionSize( LLVMSectionIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSectionContents", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetSectionContents( LLVMSectionIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSectionAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSectionAddress( LLVMSectionIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSectionContainsSymbol", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool GetSectionContainsSymbol( LLVMSectionIteratorRef @SI, LLVMSymbolIteratorRef @Sym );

        [DllImport( libraryPath, EntryPoint = "LLVMGetRelocations", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMRelocationIteratorRef GetRelocations( LLVMSectionIteratorRef @Section );

        [DllImport( libraryPath, EntryPoint = "LLVMDisposeRelocationIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeRelocationIterator( LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMIsRelocationIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsRelocationIteratorAtEnd( LLVMSectionIteratorRef @Section, LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMMoveToNextRelocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToNextRelocation( LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSymbolName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetSymbolName( LLVMSymbolIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSymbolAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSymbolAddress( LLVMSymbolIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetSymbolSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSymbolSize( LLVMSymbolIteratorRef @SI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetRelocationOffset", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetRelocationOffset( LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetRelocationSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef GetRelocationSymbol( LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetRelocationType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetRelocationType( LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetRelocationTypeName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetRelocationTypeName( LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMGetRelocationValueString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetRelocationValueString( LLVMRelocationIteratorRef @RI );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcCreateInstance", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcJITStackRef OrcCreateInstance( LLVMTargetMachineRef @TM );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcGetErrorMsg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr OrcGetErrorMsg( LLVMOrcJITStackRef @JITStack );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcGetMangledSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void OrcGetMangledSymbol( LLVMOrcJITStackRef @JITStack, out IntPtr @MangledSymbol, [MarshalAs( UnmanagedType.LPStr )] string @Symbol );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcDisposeMangledSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcDisposeMangledSymbol( IntPtr @MangledSymbol );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcCreateLazyCompileCallback", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcTargetAddress OrcCreateLazyCompileCallback( LLVMOrcJITStackRef @JITStack, LLVMOrcLazyCompileCallbackFn @Callback, IntPtr @CallbackCtx );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcCreateIndirectStub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMOrcErrorCode OrcCreateIndirectStub( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @InitAddr );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcSetIndirectStubPointer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMOrcErrorCode OrcSetIndirectStubPointer( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @NewAddr );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcAddEagerlyCompiledIR", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcModuleHandle OrcAddEagerlyCompiledIR( LLVMOrcJITStackRef @JITStack, LLVMSharedModuleRef @Mod, LLVMOrcSymbolResolverFn @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcAddLazilyCompiledIR", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcModuleHandle OrcAddLazilyCompiledIR( LLVMOrcJITStackRef @JITStack, LLVMSharedModuleRef @Mod, LLVMOrcSymbolResolverFn @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcRemoveModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcRemoveModule( LLVMOrcJITStackRef @JITStack, LLVMOrcModuleHandle @H );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcGetSymbolAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMOrcTargetAddress OrcGetSymbolAddress( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @SymbolName );

        [DllImport( libraryPath, EntryPoint = "LLVMOrcDisposeInstance", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcDisposeInstance( LLVMOrcJITStackRef @JITStack );

        [DllImport( libraryPath, EntryPoint = "LLVMLoadLibraryPermanently", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LoadLibraryPermanently( [MarshalAs( UnmanagedType.LPStr )] string @Filename );

        [DllImport( libraryPath, EntryPoint = "LLVMParseCommandLineOptions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void ParseCommandLineOptions( int @argc, string[ ] @argv, [MarshalAs( UnmanagedType.LPStr )] string @Overview );

        [DllImport( libraryPath, EntryPoint = "LLVMSearchForAddressOfSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern IntPtr SearchForAddressOfSymbol( [MarshalAs( UnmanagedType.LPStr )] string @symbolName );

        [DllImport( libraryPath, EntryPoint = "LLVMAddSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void AddSymbol( [MarshalAs( UnmanagedType.LPStr )] string @symbolName, IntPtr @symbolValue );

        [DllImport( libraryPath, EntryPoint = "LLVMAddArgumentPromotionPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddArgumentPromotionPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddConstantMergePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddConstantMergePass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddDeadArgEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDeadArgEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddFunctionAttrsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddFunctionAttrsPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddFunctionInliningPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddFunctionInliningPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddAlwaysInlinerPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAlwaysInlinerPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddGlobalDCEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGlobalDCEPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddGlobalOptimizerPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGlobalOptimizerPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddIPConstantPropagationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIPConstantPropagationPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddPruneEHPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddPruneEHPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddIPSCCPPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIPSCCPPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddInternalizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddInternalizePass( LLVMPassManagerRef @param0, uint @AllButMain );

        [DllImport( libraryPath, EntryPoint = "LLVMAddStripDeadPrototypesPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddStripDeadPrototypesPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddStripSymbolsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddStripSymbolsPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderCreate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerBuilderRef PassManagerBuilderCreate( );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderDispose( LLVMPassManagerBuilderRef @PMB );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderSetOptLevel", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetOptLevel( LLVMPassManagerBuilderRef @PMB, uint @OptLevel );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderSetSizeLevel", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetSizeLevel( LLVMPassManagerBuilderRef @PMB, uint @SizeLevel );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnitAtATime", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetDisableUnitAtATime( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnrollLoops", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetDisableUnrollLoops( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableSimplifyLibCalls", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetDisableSimplifyLibCalls( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderUseInlinerWithThreshold", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderUseInlinerWithThreshold( LLVMPassManagerBuilderRef @PMB, uint @Threshold );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderPopulateFunctionPassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateModulePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderPopulateModulePassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateLTOPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderPopulateLTOPassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM, [MarshalAs( UnmanagedType.Bool )]bool @Internalize, [MarshalAs( UnmanagedType.Bool )]bool @RunInliner );

        [DllImport( libraryPath, EntryPoint = "LLVMAddAggressiveDCEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAggressiveDCEPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddBitTrackingDCEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddBitTrackingDCEPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddAlignmentFromAssumptionsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAlignmentFromAssumptionsPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddCFGSimplificationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCFGSimplificationPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddDeadStoreEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDeadStoreEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddScalarizerPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarizerPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddMergedLoadStoreMotionPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddMergedLoadStoreMotionPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddGVNPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGVNPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddNewGVNPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddNewGVNPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddIndVarSimplifyPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIndVarSimplifyPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddInstructionCombiningPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddInstructionCombiningPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddJumpThreadingPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddJumpThreadingPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLICMPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLICMPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLoopDeletionPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopDeletionPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLoopIdiomPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopIdiomPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLoopRotatePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopRotatePass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLoopRerollPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopRerollPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLoopUnrollPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopUnrollPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLoopUnswitchPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopUnswitchPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddMemCpyOptPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddMemCpyOptPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddPartiallyInlineLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddPartiallyInlineLibCallsPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLowerSwitchPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLowerSwitchPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddPromoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddPromoteMemoryToRegisterPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddReassociatePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddReassociatePass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddSCCPPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddSCCPPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarReplAggregatesPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassSSA", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarReplAggregatesPassSSA( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassWithThreshold", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarReplAggregatesPassWithThreshold( LLVMPassManagerRef @PM, int @Threshold );

        [DllImport( libraryPath, EntryPoint = "LLVMAddSimplifyLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddSimplifyLibCallsPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddTailCallEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddTailCallEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddConstantPropagationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddConstantPropagationPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddDemoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDemoteMemoryToRegisterPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddVerifierPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddVerifierPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddCorrelatedValuePropagationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCorrelatedValuePropagationPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddEarlyCSEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddEarlyCSEPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddEarlyCSEMemSSAPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddEarlyCSEMemSSAPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLowerExpectIntrinsicPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLowerExpectIntrinsicPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddTypeBasedAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddTypeBasedAliasAnalysisPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddScopedNoAliasAAPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScopedNoAliasAAPass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddBasicAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddBasicAliasAnalysisPass( LLVMPassManagerRef @PM );

        [Obsolete( "Use AddSLPVectorizePass instead" )]
        [DllImport( libraryPath, EntryPoint = "LLVMAddBBVectorizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddBBVectorizePass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddLoopVectorizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopVectorizePass( LLVMPassManagerRef @PM );

        [DllImport( libraryPath, EntryPoint = "LLVMAddSLPVectorizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddSLPVectorizePass( LLVMPassManagerRef @PM );
    }
}