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
        internal const string LibraryPath = "libLLVM";

        [DllImport( LibraryPath, EntryPoint = "LLVMVerifyModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus VerifyModule( LLVMModuleRef @M, LLVMVerifierFailureAction @Action, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMVerifyFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus VerifyFunction( LLVMValueRef @Fn, LLVMVerifierFailureAction @Action );

        [DllImport( LibraryPath, EntryPoint = "LLVMViewFunctionCFG", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ViewFunctionCFG( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMViewFunctionCFGOnly", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ViewFunctionCFGOnly( LLVMValueRef @Fn );

        [Obsolete( "Use LLVMParseBitcode2 instead" )]
        [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcode( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule, out IntPtr @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcode2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcode2( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcodeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcodeInContext( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcodeInContext2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseBitcodeInContext2( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModuleInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModuleInContext( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM, out IntPtr @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModuleInContext2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModuleInContext2( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModule( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM, out IntPtr @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModule2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus GetBitcodeModule2( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus WriteBitcodeToFile( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Path );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFD", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus WriteBitcodeToFD( LLVMModuleRef @M, int @FD, int @ShouldClose, int @Unbuffered );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFileHandle", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus WriteBitcodeToFileHandle( LLVMModuleRef @M, int @Handle );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMemoryBufferRef WriteBitcodeToMemoryBuffer( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMInstallFatalErrorHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InstallFatalErrorHandler( LLVMFatalErrorHandler @Handler );

        [DllImport( LibraryPath, EntryPoint = "LLVMResetFatalErrorHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ResetFatalErrorHandler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMEnablePrettyStackTrace", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void EnablePrettyStackTrace( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeCore( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMShutdown", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void Shutdown( );

        /* CreateMessage should never be called by managed code
        //[DllImport( libraryPath, EntryPoint = "LLVMCreateMessage", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        //internal static extern IntPtr CreateMessage( [MarshalAs( UnmanagedType.LPStr )] string @Message );
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeMessage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeMessage( IntPtr @Message );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextCreate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef ContextCreate( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef GetGlobalContext( );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextSetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ContextSetDiagnosticHandler( LLVMContextRef @C, IntPtr @Handler, IntPtr @DiagnosticContext );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextGetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDiagnosticHandler ContextGetDiagnosticHandler( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextGetDiagnosticContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr ContextGetDiagnosticContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextSetYieldCallback", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ContextSetYieldCallback( LLVMContextRef @C, LLVMYieldCallback @Callback, IntPtr @OpaqueHandle );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ContextDispose( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDiagInfoDescription", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetDiagInfoDescription( LLVMDiagnosticInfoRef @DI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDiagInfoSeverity", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDiagnosticSeverity GetDiagInfoSeverity( LLVMDiagnosticInfoRef @DI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMDKindIDInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetMDKindIDInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMDKindID", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetMDKindID( [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKindForName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetEnumAttributeKindForName( [MarshalAs( UnmanagedType.LPStr )] string @Name, size_t @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetLastEnumAttributeKind( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef CreateEnumAttribute( LLVMContextRef @C, uint @KindID, ulong @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetEnumAttributeKind( LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong GetEnumAttributeValue( LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef CreateStringAttribute( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLength, [MarshalAs( UnmanagedType.LPStr )] string @V, uint @VLength );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetStringAttributeKind( LLVMAttributeRef @A, out uint @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetStringAttributeValue( LLVMAttributeRef @A, out uint @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsEnumAttribute( LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsStringAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsStringAttribute( LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleCreateWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMModuleRef ModuleCreateWithName( [MarshalAs( UnmanagedType.LPStr )] string @ModuleID );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleCreateWithNameInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMModuleRef ModuleCreateWithNameInContext( [MarshalAs( UnmanagedType.LPStr )] string @ModuleID, LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMCloneModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleRef CloneModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleIdentifier", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetModuleIdentifier( LLVMModuleRef @M, out size_t @Len );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleIdentifier", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetModuleIdentifier( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Ident, size_t @Len );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDataLayoutStr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetDataLayoutStr( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetDataLayout( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDataLayout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetDataLayout( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @DataLayoutStr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTarget", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetTarget( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetTarget", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetTarget( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMDumpModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DumpModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintModuleToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus PrintModuleToFile( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Filename, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintModuleToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string PrintModuleToString( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetModuleInlineAsm( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Asm );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef GetModuleContext( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTypeByName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTypeRef GetTypeByName( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedMetadataNumOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint GetNamedMetadataNumOperands( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedMetadataOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void GetNamedMetadataOperands( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, out LLVMValueRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddNamedMetadataOperand", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void AddNamedMetadataOperand( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddFunction( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef GetNamedFunction( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstFunction( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastFunction( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextFunction( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousFunction( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTypeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeKind GetTypeKind( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMTypeIsSized", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TypeIsSized( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTypeContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef GetTypeContext( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMDumpType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DumpType( LLVMTypeRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintTypeToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string PrintTypeToString( LLVMTypeRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt1TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int1TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt8TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int8TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt16TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int16TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt32TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int32TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt64TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int64TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int128TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntTypeInContext( LLVMContextRef @C, uint @NumBits );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt1Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int1Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt8Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int8Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt16Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int16Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt32Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int32Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt64Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int64Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef Int128Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntType( uint @NumBits );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIntTypeWidth", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetIntTypeWidth( LLVMTypeRef @IntegerTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMHalfTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef HalfTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMFloatTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FloatTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMDoubleTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef DoubleTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86FP80TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86FP80TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FP128TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMPPCFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef PPCFP128TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMHalfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef HalfType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMFloatType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FloatType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMDoubleType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef DoubleType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86FP80Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86FP80Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMFP128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FP128Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMPPCFP128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef PPCFP128Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMFunctionType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef FunctionType( LLVMTypeRef @ReturnType, out LLVMTypeRef @ParamTypes, uint @ParamCount, [MarshalAs( UnmanagedType.Bool )]bool @IsVarArg );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsFunctionVarArg", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsFunctionVarArg( LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetReturnType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef GetReturnType( LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountParamTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountParamTypes( LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParamTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetParamTypes( LLVMTypeRef @FunctionTy, out LLVMTypeRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef StructTypeInContext( LLVMContextRef @C, out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef StructType( out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructCreateNamed", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTypeRef StructCreateNamed( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStructName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetStructName( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructSetBody", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void StructSetBody( LLVMTypeRef @StructTy, out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountStructElementTypes( LLVMTypeRef @StructTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetStructElementTypes( LLVMTypeRef @StructTy, out LLVMTypeRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructGetTypeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef StructGetTypeAtIndex( LLVMTypeRef @StructTy, uint @i );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsPackedStruct", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsPackedStruct( LLVMTypeRef @StructTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsOpaqueStruct", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsOpaqueStruct( LLVMTypeRef @StructTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetElementType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef GetElementType( LLVMTypeRef @Ty );

        // Added to LLVM-C APIs in 5.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMGetSubtypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetSubtypes( LLVMTypeRef Tp, out LLVMTypeRef Arr );

        // Added to LLVM-C APIs in 5.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumContainedTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumContainedTypes( LLVMTypeRef Tp );

        [DllImport( LibraryPath, EntryPoint = "LLVMArrayType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef ArrayType( LLVMTypeRef @ElementType, uint @ElementCount );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetArrayLength", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetArrayLength( LLVMTypeRef @ArrayTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMPointerType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef PointerType( LLVMTypeRef @ElementType, uint @AddressSpace );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPointerAddressSpace", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetPointerAddressSpace( LLVMTypeRef @PointerTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMVectorType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef VectorType( LLVMTypeRef @ElementType, uint @ElementCount );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetVectorSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetVectorSize( LLVMTypeRef @VectorTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMVoidTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef VoidTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMLabelTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LabelTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86MMXTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86MMXTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMVoidType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef VoidType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMLabelType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LabelType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86MMXType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef X86MMXType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMTypeOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef TypeOf( LLVMValueRef @Val );

        /* excluded in favor of custom version that redirects to GetValueId
        // [DllImport(libraryPath, EntryPoint = "LLVMGetValueKind", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern LLVMValueKind GetValueKind(LLVMValueRef @Val);
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMGetValueName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetValueName( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetValueName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetValueName( LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMDumpValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DumpValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintValueToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string PrintValueToString( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMReplaceAllUsesWith", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ReplaceAllUsesWith( LLVMValueRef @OldVal, LLVMValueRef @NewVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsConstant", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsConstant( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsUndef", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsUndef( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAArgument", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAArgument( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABasicBlock( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInlineAsm", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInlineAsm( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUser( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstant( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABlockAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABlockAddress( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantAggregateZero", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantAggregateZero( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantArray( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataSequential", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantDataSequential( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantDataArray( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantDataVector( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantExpr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantExpr( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantFP( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantInt( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantPointerNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantPointerNull( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantStruct( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantTokenNone", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantTokenNone( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAConstantVector( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalAlias", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalAlias( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalObject", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalObject( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFunction( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalVariable", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGlobalVariable( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUndefValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUndefValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInstruction( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABinaryOperator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABinaryOperator( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACallInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACallInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAIntrinsicInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAIntrinsicInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsADbgInfoIntrinsic", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsADbgInfoIntrinsic( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsADbgDeclareInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsADbgDeclareInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemIntrinsic", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemIntrinsic( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemCpyInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemCpyInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemMoveInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemMoveInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemSetInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMemSetInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACmpInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFCmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFCmpInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAICmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAICmpInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAExtractElementInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAExtractElementInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGetElementPtrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAGetElementPtrInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInsertElementInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInsertElementInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInsertValueInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInsertValueInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsALandingPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsALandingPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAPHINode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAPHINode( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASelectInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASelectInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAShuffleVectorInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAShuffleVectorInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAStoreInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAStoreInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsATerminatorInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsATerminatorInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABranchInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABranchInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAIndirectBrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAIndirectBrInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInvokeInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAInvokeInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAReturnInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASwitchInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASwitchInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUnreachableInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUnreachableInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAResumeInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAResumeInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACleanupReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACleanupReturnInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACatchReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACatchReturnInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFuncletPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFuncletPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACatchPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACatchPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACleanupPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACleanupPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUnaryInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUnaryInstruction( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAAllocaInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAAllocaInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsACastInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAAddrSpaceCastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAAddrSpaceCastInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABitCastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsABitCastInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPExtInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPToSIInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPToSIInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPToUIInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPToUIInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPTruncInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAFPTruncInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAIntToPtrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAIntToPtrInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAPtrToIntInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAPtrToIntInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASExtInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASIToFPInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsASIToFPInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsATruncInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsATruncInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUIToFPInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAUIToFPInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAZExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAZExtInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAExtractValueInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAExtractValueInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsALoadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsALoadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAVAArgInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAVAArgInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMDNode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMDNode( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMDString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef IsAMDString( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstUse", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMUseRef GetFirstUse( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextUse", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMUseRef GetNextUse( LLVMUseRef @U );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetUser( LLVMUseRef @U );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUsedValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetUsedValue( LLVMUseRef @U );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetOperand", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetOperand( LLVMValueRef @Val, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetOperandUse", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMUseRef GetOperandUse( LLVMValueRef @Val, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetOperand", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetOperand( LLVMValueRef @User, uint @Index, LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetNumOperands( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNull( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAllOnes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAllOnes( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUndef", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetUndef( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsNull", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsNull( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstPointerNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstPointerNull( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInt( LLVMTypeRef @IntTy, ulong @N, [MarshalAs( UnmanagedType.Bool )]bool @SignExtend );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfArbitraryPrecision", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstIntOfArbitraryPrecision( LLVMTypeRef @IntTy, uint @NumWords, int[ ] @Words );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstIntOfString( LLVMTypeRef @IntTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, byte @Radix );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstIntOfStringAndSize( LLVMTypeRef @IntTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, uint @SLen, byte @Radix );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstReal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstReal( LLVMTypeRef @RealTy, double @N );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstRealOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstRealOfString( LLVMTypeRef @RealTy, [MarshalAs( UnmanagedType.LPStr )] string @Text );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstRealOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstRealOfStringAndSize( LLVMTypeRef @RealTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, uint @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntGetZExtValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong ConstIntGetZExtValue( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntGetSExtValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern long ConstIntGetSExtValue( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstRealGetDouble", CallingConvention = CallingConvention.Cdecl )]
        internal static extern double ConstRealGetDouble( LLVMValueRef @ConstantVal, [MarshalAs( UnmanagedType.Bool )]out bool @losesInfo );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstStringInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstStringInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @Length, [MarshalAs( UnmanagedType.Bool )]bool @DontNullTerminate );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstString( [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @Length, [MarshalAs( UnmanagedType.Bool )]bool @DontNullTerminate );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsConstantString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsConstantString( LLVMValueRef @c );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAsString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetAsString( LLVMValueRef @c, out size_t @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstStructInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstStructInContext( LLVMContextRef @C, out LLVMValueRef @ConstantVals, uint @Count, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstStruct( out LLVMValueRef @ConstantVals, uint @Count, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstArray( LLVMTypeRef @ElementTy, out LLVMValueRef @ConstantVals, uint @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNamedStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNamedStruct( LLVMTypeRef @StructTy, out LLVMValueRef @ConstantVals, uint @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetElementAsConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetElementAsConstant( LLVMValueRef @C, uint @idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstVector( out LLVMValueRef @ScalarConstantVals, uint @Size );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetConstOpcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOpcode GetConstOpcode( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMAlignOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef AlignOf( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMSizeOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef SizeOf( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNot", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNot( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNSWMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstNUWMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstUDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstUDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        // Added to LLVM-C APIs in LLVM 4.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMConstExactUDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExactUDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstExactSDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExactSDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstURem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstURem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSRem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSRem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFRem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFRem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAnd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAnd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstOr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstOr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstXor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstXor( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstICmp", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstICmp( LLVMIntPredicate @Predicate, LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFCmp", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFCmp( LLVMRealPredicate @Predicate, LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstShl", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstShl( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstLShr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstLShr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAShr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAShr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstGEP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstGEP( LLVMValueRef @ConstantVal, out LLVMValueRef @ConstantIndices, uint @NumIndices );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInBoundsGEP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInBoundsGEP( LLVMValueRef @ConstantVal, out LLVMValueRef @ConstantIndices, uint @NumIndices );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstTrunc", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstTrunc( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstZExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstZExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPTrunc", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPTrunc( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstUIToFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstUIToFP( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSIToFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSIToFP( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPToUI", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPToUI( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPToSI", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPToSI( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstPtrToInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstPtrToInt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntToPtr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstIntToPtr( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAddrSpaceCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstAddrSpaceCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstZExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstZExtOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSExtOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstTruncOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstTruncOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstPointerCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstPointerCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstIntCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType, [MarshalAs( UnmanagedType.Bool )]bool @isSigned );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstFPCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSelect", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstSelect( LLVMValueRef @ConstantCondition, LLVMValueRef @ConstantIfTrue, LLVMValueRef @ConstantIfFalse );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstExtractElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExtractElement( LLVMValueRef @VectorConstant, LLVMValueRef @IndexConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInsertElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInsertElement( LLVMValueRef @VectorConstant, LLVMValueRef @ElementValueConstant, LLVMValueRef @IndexConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstShuffleVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstShuffleVector( LLVMValueRef @VectorAConstant, LLVMValueRef @VectorBConstant, LLVMValueRef @MaskConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstExtractValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstExtractValue( LLVMValueRef @AggConstant, out uint @IdxList, uint @NumIdx );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInsertValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef ConstInsertValue( LLVMValueRef @AggConstant, LLVMValueRef @ElementValueConstant, out uint @IdxList, uint @NumIdx );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef ConstInlineAsm( LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @AsmString, [MarshalAs( UnmanagedType.LPStr )] string @Constraints, [MarshalAs( UnmanagedType.Bool )]bool @HasSideEffects, [MarshalAs( UnmanagedType.Bool )]bool @IsAlignStack );

        [DllImport( LibraryPath, EntryPoint = "LLVMBlockAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BlockAddress( LLVMValueRef @F, LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleRef GetGlobalParent( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsDeclaration", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsDeclaration( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLinkage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMLinkage GetLinkage( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetLinkage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetLinkage( LLVMValueRef @Global, LLVMLinkage @Linkage );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSection", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetSection( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetSection", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetSection( LLVMValueRef @Global, [MarshalAs( UnmanagedType.LPStr )] string @Section );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetVisibility", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMVisibility GetVisibility( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetVisibility", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetVisibility( LLVMValueRef @Global, LLVMVisibility @Viz );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDLLStorageClass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDLLStorageClass GetDLLStorageClass( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDLLStorageClass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetDLLStorageClass( LLVMValueRef @Global, LLVMDLLStorageClass @Class );

        [DllImport( LibraryPath, EntryPoint = "LLVMHasUnnamedAddr", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool HasUnnamedAddr( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetUnnamedAddr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetUnnamedAddr( LLVMValueRef @Global, [MarshalAs( UnmanagedType.Bool )]bool hasUnnamedAddr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetAlignment( LLVMValueRef @V );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetAlignment( LLVMValueRef @V, uint @Bytes );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddGlobal( LLVMModuleRef @M, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobalInAddressSpace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddGlobalInAddressSpace( LLVMModuleRef @M, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @AddressSpace );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef GetNamedGlobal( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstGlobal( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastGlobal( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextGlobal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousGlobal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMDeleteGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DeleteGlobal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInitializer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetInitializer( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInitializer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInitializer( LLVMValueRef @GlobalVar, LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsThreadLocal", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsThreadLocal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetThreadLocal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetThreadLocal( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isThreadLocal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsGlobalConstant", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsGlobalConstant( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetGlobalConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetGlobalConstant( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetThreadLocalMode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMThreadLocalMode GetThreadLocalMode( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetThreadLocalMode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetThreadLocalMode( LLVMValueRef @GlobalVar, LLVMThreadLocalMode @Mode );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsExternallyInitialized", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsExternallyInitialized( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetExternallyInitialized", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetExternallyInitialized( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool @IsExtInit );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAlias", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef AddAlias( LLVMModuleRef @M, LLVMTypeRef @Ty, LLVMValueRef @Aliasee, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMDeleteFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DeleteFunction( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMHasPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool HasPersonalityFn( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPersonalityFn( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetPersonalityFn( LLVMValueRef @Fn, LLVMValueRef @PersonalityFn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIntrinsicID", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetIntrinsicID( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFunctionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetFunctionCallConv( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetFunctionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetFunctionCallConv( LLVMValueRef @Fn, uint @CC );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGC", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetGC( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetGC", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void SetGC( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        /* Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMAddFunctionAttr", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern void AddFunctionAttr(LLVMValueRef @Fn, LLVMAttribute @PA);
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributeCountAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetAttributeCountAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributesAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetAttributesAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, out LLVMAttributeRef Attrs );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef GetEnumAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef GetStringAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RemoveEnumAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void RemoveStringAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTargetDependentFunctionAttr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void AddTargetDependentFunctionAttr( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @A, [MarshalAs( UnmanagedType.LPStr )] string @V );

        /* Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMGetFunctionAttr", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern LLVMAttribute GetFunctionAttr(LLVMValueRef @Fn);
        */

        /* Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMRemoveFunctionAttr", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern void RemoveFunctionAttr(LLVMValueRef @Fn, LLVMAttribute @PA);
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMCountParams", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountParams( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParams", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetParams( LLVMValueRef @Fn, out LLVMValueRef @Params );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetParam( LLVMValueRef @Fn, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParamParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetParamParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstParam( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastParam( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextParam( LLVMValueRef @Arg );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousParam( LLVMValueRef @Arg );

        /* Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMAddAttribute", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern void AddAttribute(LLVMValueRef @Arg, LLVMAttribute @PA);

        // Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMRemoveAttribute", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern void RemoveAttribute(LLVMValueRef @Arg, LLVMAttribute @PA);

        // Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMGetAttribute", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern LLVMAttribute GetAttribute(LLVMValueRef @Arg);
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMSetParamAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetParamAlignment( LLVMValueRef @Arg, uint @Align );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDStringInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef MDStringInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef MDString( [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDNodeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef MDNodeInContext( LLVMContextRef @C, out LLVMValueRef @Vals, uint @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDNode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef MDNode( out LLVMValueRef @Vals, uint @Count );

        // Added to LLVM-C API in LLVM 5.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMMetadataAsValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef MetadataAsValue( LLVMContextRef context, LLVMMetadataRef metadataRef );

        // Added to LLVM-C API in LLVM 5.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMValueAsMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMetadataRef ValueAsMetadata( LLVMValueRef Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMDString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetMDString( LLVMValueRef @V, out uint @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMDNodeNumOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetMDNodeNumOperands( LLVMValueRef @V );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMDNodeOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetMDNodeOperands( LLVMValueRef @V, out LLVMValueRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMBasicBlockAsValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BasicBlockAsValue( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMValueIsBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool ValueIsBasicBlock( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMValueAsBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef ValueAsBasicBlock( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetBasicBlockName( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetBasicBlockParent( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockTerminator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetBasicBlockTerminator( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountBasicBlocks", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountBasicBlocks( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlocks", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetBasicBlocks( LLVMValueRef @Fn, out LLVMBasicBlockRef @BasicBlocks );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetFirstBasicBlock( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetLastBasicBlock( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetNextBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetPreviousBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEntryBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetEntryBasicBlock( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMAppendBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef AppendBasicBlockInContext( LLVMContextRef @C, LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMAppendBasicBlock", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef AppendBasicBlock( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef InsertBasicBlockInContext( LLVMContextRef @C, LLVMBasicBlockRef @BB, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertBasicBlock", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef InsertBasicBlock( LLVMBasicBlockRef @InsertBeforeBB, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMDeleteBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DeleteBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveBasicBlockFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RemoveBasicBlockFromParent( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveBasicBlockBefore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveBasicBlockBefore( LLVMBasicBlockRef @BB, LLVMBasicBlockRef @MovePos );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveBasicBlockAfter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveBasicBlockAfter( LLVMBasicBlockRef @BB, LLVMBasicBlockRef @MovePos );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetFirstInstruction( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetLastInstruction( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMHasMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int HasMetadata( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetMetadata( LLVMValueRef @Val, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetMetadata( LLVMValueRef @Val, uint @KindID, LLVMValueRef @Node );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetInstructionParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetNextInstruction( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetPreviousInstruction( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMInstructionRemoveFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InstructionRemoveFromParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMInstructionEraseFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InstructionEraseFromParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionOpcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOpcode GetInstructionOpcode( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetICmpPredicate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMIntPredicate GetICmpPredicate( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFCmpPredicate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMRealPredicate GetFCmpPredicate( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMInstructionClone", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef InstructionClone( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumArgOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumArgOperands( LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInstructionCallConv( LLVMValueRef @Instr, uint @CC );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetInstructionCallConv( LLVMValueRef @Instr );

        /* Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMAddInstrAttribute", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern void AddInstrAttribute(LLVMValueRef @Instr, uint @index, LLVMAttribute @param2);

        // Removed from LLVM-C APIs in LLVM 4.0.0
        // [DllImport(libraryPath, EntryPoint = "LLVMRemoveInstrAttribute", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern void RemoveInstrAttribute(LLVMValueRef @Instr, uint @index, LLVMAttribute @param2);
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInstrParamAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInstrParamAlignment( LLVMValueRef @Instr, uint @index, uint @Align );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCallSiteAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCallSiteAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteAttributeCount", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetCallSiteAttributeCount( LLVMValueRef C, LLVMAttributeIndex Idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteAttributes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GetCallSiteAttributes( LLVMValueRef C, LLVMAttributeIndex Idx, out LLVMAttributeRef attributes );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef GetCallSiteEnumAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef GetCallSiteStringAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RemoveCallSiteEnumAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void RemoveCallSiteStringAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCalledValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetCalledValue( LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsTailCall", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsTailCall( LLVMValueRef @CallInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetTailCall", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetTailCall( LLVMValueRef @CallInst, [MarshalAs( UnmanagedType.Bool )]bool isTailCall );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNormalDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetNormalDest( LLVMValueRef @InvokeInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetUnwindDest( LLVMValueRef @InvokeInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetNormalDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetNormalDest( LLVMValueRef @InvokeInst, LLVMBasicBlockRef @B );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetUnwindDest( LLVMValueRef @InvokeInst, LLVMBasicBlockRef @B );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumSuccessors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumSuccessors( LLVMValueRef @Term );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSuccessor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetSuccessor( LLVMValueRef @Term, uint @i );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetSuccessor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetSuccessor( LLVMValueRef @Term, uint @i, LLVMBasicBlockRef @block );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsConditional", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsConditional( LLVMValueRef @Branch );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCondition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetCondition( LLVMValueRef @Branch );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCondition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCondition( LLVMValueRef @Branch, LLVMValueRef @Cond );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSwitchDefaultDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetSwitchDefaultDest( LLVMValueRef @SwitchInstr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAllocatedType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef GetAllocatedType( LLVMValueRef @Alloca );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsInBounds", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsInBounds( LLVMValueRef @GEP );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetIsInBounds", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetIsInBounds( LLVMValueRef @GEP, [MarshalAs( UnmanagedType.Bool )]bool @InBounds );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddIncoming", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIncoming( LLVMValueRef @PhiNode, out LLVMValueRef @IncomingValues, out LLVMBasicBlockRef @IncomingBlocks, uint @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountIncoming", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CountIncoming( LLVMValueRef @PhiNode );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIncomingValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetIncomingValue( LLVMValueRef @PhiNode, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIncomingBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetIncomingBlock( LLVMValueRef @PhiNode, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumIndices", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumIndices( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIndices", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetIndices( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateBuilderInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBuilderRef CreateBuilderInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBuilderRef CreateBuilder( );

        [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PositionBuilder( LLVMBuilderRef @Builder, LLVMBasicBlockRef @Block, LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilderBefore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PositionBuilderBefore( LLVMBuilderRef @Builder, LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilderAtEnd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PositionBuilderAtEnd( LLVMBuilderRef @Builder, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInsertBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef GetInsertBlock( LLVMBuilderRef @Builder );

        [DllImport( LibraryPath, EntryPoint = "LLVMClearInsertionPosition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void ClearInsertionPosition( LLVMBuilderRef @Builder );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertIntoBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InsertIntoBuilder( LLVMBuilderRef @Builder, LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertIntoBuilderWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void InsertIntoBuilderWithName( LLVMBuilderRef @Builder, LLVMValueRef @Instr, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeBuilder( IntPtr @Builder );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCurrentDebugLocation( LLVMBuilderRef @Builder, LLVMValueRef @L );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetCurrentDebugLocation( LLVMBuilderRef @Builder );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInstDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetInstDebugLocation( LLVMBuilderRef @Builder, LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildRetVoid", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildRetVoid( LLVMBuilderRef @param0 );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildRet", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildRet( LLVMBuilderRef @param0, LLVMValueRef @V );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAggregateRet", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildAggregateRet( LLVMBuilderRef @param0, out LLVMValueRef @RetVals, uint @N );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildBr( LLVMBuilderRef @param0, LLVMBasicBlockRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildCondBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildCondBr( LLVMBuilderRef @param0, LLVMValueRef @If, LLVMBasicBlockRef @Then, LLVMBasicBlockRef @Else );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSwitch", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildSwitch( LLVMBuilderRef @param0, LLVMValueRef @V, LLVMBasicBlockRef @Else, uint @NumCases );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIndirectBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildIndirectBr( LLVMBuilderRef @B, LLVMValueRef @Addr, uint @NumDests );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInvoke", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInvoke( LLVMBuilderRef @param0, LLVMValueRef @Fn, out LLVMValueRef @Args, uint @NumArgs, LLVMBasicBlockRef @Then, LLVMBasicBlockRef @Catch, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildLandingPad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildLandingPad( LLVMBuilderRef @B, LLVMTypeRef @Ty, LLVMValueRef @PersFn, uint @NumClauses, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildResume", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildResume( LLVMBuilderRef @B, LLVMValueRef @Exn );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildUnreachable", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildUnreachable( LLVMBuilderRef @param0 );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCase", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCase( LLVMValueRef @Switch, LLVMValueRef @OnVal, LLVMBasicBlockRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDestination", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDestination( LLVMValueRef @IndirectBr, LLVMBasicBlockRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumClauses", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GetNumClauses( LLVMValueRef @LandingPad );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetClause", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef GetClause( LLVMValueRef @LandingPad, uint @Idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddClause", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddClause( LLVMValueRef @LandingPad, LLVMValueRef @ClauseVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsCleanup", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsCleanup( LLVMValueRef @LandingPad );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCleanup", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCleanup( LLVMValueRef @LandingPad, [MarshalAs( UnmanagedType.Bool )]bool @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildUDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        // Added to LLVM-C API in LLVM 4.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExactUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExactUDiv( LLVMBuilderRef @param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExactSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExactSDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildURem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildURem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSRem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFRem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildShl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildShl( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildLShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildLShr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAShr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAnd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildOr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildOr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildXor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildXor( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildBinOp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildBinOp( LLVMBuilderRef @B, LLVMOpcode @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNeg( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNSWNeg( LLVMBuilderRef @B, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNUWNeg( LLVMBuilderRef @B, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFNeg( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNot", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildNot( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildMalloc( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildArrayMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildArrayMalloc( LLVMBuilderRef @param0, LLVMTypeRef @Ty, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAlloca( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildArrayAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildArrayAlloca( LLVMBuilderRef @param0, LLVMTypeRef @Ty, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFree", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildFree( LLVMBuilderRef @param0, LLVMValueRef @PointerVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildLoad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildLoad( LLVMBuilderRef @param0, LLVMValueRef @PointerVal, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildStore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildStore( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMValueRef @Ptr );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, out LLVMValueRef @Indices, uint @NumIndices, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInBoundsGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInBoundsGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, out LLVMValueRef @Indices, uint @NumIndices, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildStructGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildStructGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, uint @Idx, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildGlobalString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildGlobalString( LLVMBuilderRef @B, [MarshalAs( UnmanagedType.LPStr )] string @Str, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildGlobalStringPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildGlobalStringPtr( LLVMBuilderRef @B, [MarshalAs( UnmanagedType.LPStr )] string @Str, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetVolatile", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool GetVolatile( LLVMValueRef @MemoryAccessInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetVolatile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetVolatile( LLVMValueRef @MemoryAccessInst, [MarshalAs( UnmanagedType.Bool )]bool @IsVolatile );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering GetOrdering( LLVMValueRef @MemoryAccessInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetOrdering( LLVMValueRef @MemoryAccessInst, LLVMAtomicOrdering @Ordering );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildTrunc( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildZExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildZExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPToUI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPToUI( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPToSI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPToSI( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildUIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildUIToFP( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSIToFP( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPTrunc( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPtrToInt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPtrToInt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIntToPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIntToPtr( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAddrSpaceCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildAddrSpaceCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildZExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildZExtOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSExtOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildTruncOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildTruncOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildCast( LLVMBuilderRef @B, LLVMOpcode @Op, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPointerCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPointerCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIntCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIntCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFPCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildICmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildICmp( LLVMBuilderRef @param0, LLVMIntPredicate @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFCmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFCmp( LLVMBuilderRef @param0, LLVMRealPredicate @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPhi", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPhi( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildCall", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildCall( LLVMBuilderRef @param0, LLVMValueRef @Fn, out LLVMValueRef @Args, uint @NumArgs, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSelect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildSelect( LLVMBuilderRef @param0, LLVMValueRef @If, LLVMValueRef @Then, LLVMValueRef @Else, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildVAArg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildVAArg( LLVMBuilderRef @param0, LLVMValueRef @List, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExtractElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExtractElement( LLVMBuilderRef @param0, LLVMValueRef @VecVal, LLVMValueRef @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInsertElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInsertElement( LLVMBuilderRef @param0, LLVMValueRef @VecVal, LLVMValueRef @EltVal, LLVMValueRef @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildShuffleVector", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildShuffleVector( LLVMBuilderRef @param0, LLVMValueRef @V1, LLVMValueRef @V2, LLVMValueRef @Mask, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExtractValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildExtractValue( LLVMBuilderRef @param0, LLVMValueRef @AggVal, uint @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInsertValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildInsertValue( LLVMBuilderRef @param0, LLVMValueRef @AggVal, LLVMValueRef @EltVal, uint @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIsNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIsNull( LLVMBuilderRef @param0, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIsNotNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildIsNotNull( LLVMBuilderRef @param0, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPtrDiff", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildPtrDiff( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFence", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef BuildFence( LLVMBuilderRef @B, LLVMAtomicOrdering @ordering, [MarshalAs( UnmanagedType.Bool )]bool @singleThread, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAtomicRMW", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildAtomicRMW( LLVMBuilderRef @B, LLVMAtomicRMWBinOp @op, LLVMValueRef @PTR, LLVMValueRef @Val, LLVMAtomicOrdering @ordering, [MarshalAs( UnmanagedType.Bool )]bool @singleThread );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAtomicCmpXchg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef BuildAtomicCmpXchg( LLVMBuilderRef @B, LLVMValueRef @Ptr, LLVMValueRef @Cmp, LLVMValueRef @New, LLVMAtomicOrdering @SuccessOrdering, LLVMAtomicOrdering @FailureOrdering, [MarshalAs( UnmanagedType.Bool )]bool @SingleThread );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAtomicSingleThread", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsAtomicSingleThread( LLVMValueRef @AtomicInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetAtomicSingleThread", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetAtomicSingleThread( LLVMValueRef @AtomicInst, [MarshalAs( UnmanagedType.Bool )]bool @SingleThread );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCmpXchgSuccessOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering GetCmpXchgSuccessOrdering( LLVMValueRef @CmpXchgInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCmpXchgSuccessOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCmpXchgSuccessOrdering( LLVMValueRef @CmpXchgInst, LLVMAtomicOrdering @Ordering );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCmpXchgFailureOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering GetCmpXchgFailureOrdering( LLVMValueRef @CmpXchgInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCmpXchgFailureOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetCmpXchgFailureOrdering( LLVMValueRef @CmpXchgInst, LLVMAtomicOrdering @Ordering );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateModuleProviderForExistingModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleProviderRef CreateModuleProviderForExistingModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeModuleProvider", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeModuleProvider( LLVMModuleProviderRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithContentsOfFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus CreateMemoryBufferWithContentsOfFile( [MarshalAs( UnmanagedType.LPStr )] string @Path, out LLVMMemoryBufferRef @OutMemBuf, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]out string @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithSTDIN", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateMemoryBufferWithSTDIN( out LLVMMemoryBufferRef @OutMemBuf, out IntPtr @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithMemoryRange", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMMemoryBufferRef CreateMemoryBufferWithMemoryRange( [MarshalAs( UnmanagedType.LPStr )] string @InputData, size_t @InputDataLength, [MarshalAs( UnmanagedType.LPStr )] string @BufferName, [MarshalAs( UnmanagedType.Bool )]bool @RequiresNullTerminator );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithMemoryRangeCopy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMMemoryBufferRef CreateMemoryBufferWithMemoryRangeCopy( [MarshalAs( UnmanagedType.LPStr )] string @InputData, size_t @InputDataLength, [MarshalAs( UnmanagedType.LPStr )] string @BufferName );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBufferStart", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetBufferStart( LLVMMemoryBufferRef @MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBufferSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern size_t GetBufferSize( LLVMMemoryBufferRef @MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeMemoryBuffer( LLVMMemoryBufferRef @MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalPassRegistry", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassRegistryRef GetGlobalPassRegistry( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreatePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef CreatePassManager( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManagerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef CreateFunctionPassManagerForModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef CreateFunctionPassManager( LLVMModuleProviderRef @MP );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool RunPassManager( LLVMPassManagerRef @PM, LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool InitializeFunctionPassManager( LLVMPassManagerRef @FPM );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool RunFunctionPassManager( LLVMPassManagerRef @FPM, LLVMValueRef @F );

        [DllImport( LibraryPath, EntryPoint = "LLVMFinalizeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool FinalizeFunctionPassManager( LLVMPassManagerRef @FPM );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposePassManager( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsMultithreaded", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsMultithreaded( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef CreateDisasm( [MarshalAs( UnmanagedType.LPStr )] string @TripleName, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasmCPU", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef CreateDisasmCPU( [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasmCPUFeatures", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef CreateDisasmCPUFeatures( [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, [MarshalAs( UnmanagedType.LPStr )] string @Features, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDisasmOptions", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int SetDisasmOptions( LLVMDisasmContextRef @DC, int @Options );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisasmDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisasmDispose( LLVMDisasmContextRef @DC );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisasmInstruction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern ulong DisasmInstruction( LLVMDisasmContextRef @DC, IntPtr @Bytes, long @BytesSize, long @PC, IntPtr @OutString, size_t @OutStringSize );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430TargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64TargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86TargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430Target( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64Target( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86Target( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430TargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64TargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86TargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeNVPTXAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMSP430AsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64AsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86AsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeBPFAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAMDGPUAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64AsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64AsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86AsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86AsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSystemZDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeHexagonDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeXCoreDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMipsDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64Disassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAArch64Disassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeARMDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializePowerPCDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeSparcDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86Disassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeX86Disassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargetInfos", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllTargetInfos( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargets", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllTargets( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargetMCs", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllTargetMCs( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllAsmPrinters", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllAsmPrinters( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllAsmParsers", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllAsmParsers( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllDisassemblers", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAllDisassemblers( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus InitializeNativeDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef GetModuleDataLayout( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetModuleDataLayout( LLVMModuleRef @M, LLVMTargetDataRef @DL );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetDataRef CreateTargetData( [MarshalAs( UnmanagedType.LPStr )] string @StringRep );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeTargetData", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeTargetData( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTargetLibraryInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddTargetLibraryInfo( LLVMTargetLibraryInfoRef @TLI, LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMCopyStringRepOfTargetData", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string CopyStringRepOfTargetData( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMByteOrder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMByteOrdering ByteOrder( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMPointerSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PointerSize( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMPointerSizeForAS", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PointerSizeForAS( LLVMTargetDataRef @TD, uint @AS );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrType( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeForAS", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrTypeForAS( LLVMTargetDataRef @TD, uint @AS );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrTypeInContext( LLVMContextRef @C, LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeForASInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef IntPtrTypeForASInContext( LLVMContextRef @C, LLVMTargetDataRef @TD, uint @AS );

        [DllImport( LibraryPath, EntryPoint = "LLVMSizeOfTypeInBits", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong SizeOfTypeInBits( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMStoreSizeOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong StoreSizeOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMABISizeOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong ABISizeOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMABIAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint ABIAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMCallFrameAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint CallFrameAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMPreferredAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PreferredAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMPreferredAlignmentOfGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint PreferredAlignmentOfGlobal( LLVMTargetDataRef @TD, LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMElementAtOffset", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint ElementAtOffset( LLVMTargetDataRef @TD, LLVMTypeRef @StructTy, ulong @Offset );

        [DllImport( LibraryPath, EntryPoint = "LLVMOffsetOfElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong OffsetOfElement( LLVMTargetDataRef @TD, LLVMTypeRef @StructTy, uint @Element );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef GetFirstTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef GetNextTarget( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetRef GetTargetFromName( [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromTriple", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus GetTargetFromTriple( [MarshalAs( UnmanagedType.LPStr )] string @Triple, out LLVMTargetRef @T, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetTargetName( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetDescription", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetTargetDescription( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasJIT", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TargetHasJIT( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TargetHasTargetMachine( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasAsmBackend", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TargetHasAsmBackend( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetMachine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetMachineRef CreateTargetMachine( LLVMTargetRef @T, [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, [MarshalAs( UnmanagedType.LPStr )] string @Features, LLVMCodeGenOptLevel @Level, LLVMRelocMode @Reloc, LLVMCodeModel @CodeModel );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeTargetMachine( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef GetTargetMachineTarget( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTriple", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetTargetMachineTriple( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineCPU", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetTargetMachineCPU( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineFeatureString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string GetTargetMachineFeatureString( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef CreateTargetDataLayout( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetTargetMachineAsmVerbosity", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void SetTargetMachineAsmVerbosity( LLVMTargetMachineRef @T, [MarshalAs( UnmanagedType.Bool )]bool @VerboseAsm );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus TargetMachineEmitToFile( LLVMTargetMachineRef @T, LLVMModuleRef @M, string @Filename, LLVMCodeGenFileType @codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus TargetMachineEmitToMemoryBuffer( LLVMTargetMachineRef @T, LLVMModuleRef @M, LLVMCodeGenFileType @codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage, out LLVMMemoryBufferRef @OutMemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDefaultTargetTriple", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetDefaultTargetTriple( );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAnalysisPasses", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAnalysisPasses( LLVMTargetMachineRef @T, LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMLinkInMCJIT", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LinkInMCJIT( );

        [DllImport( LibraryPath, EntryPoint = "LLVMLinkInInterpreter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LinkInInterpreter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateGenericValueOfInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef CreateGenericValueOfInt( LLVMTypeRef @Ty, ulong @N, [MarshalAs( UnmanagedType.Bool )]bool @IsSigned );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateGenericValueOfPointer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef CreateGenericValueOfPointer( IntPtr @P );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateGenericValueOfFloat", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef CreateGenericValueOfFloat( LLVMTypeRef @Ty, double @N );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueIntWidth", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint GenericValueIntWidth( LLVMGenericValueRef @GenValRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueToInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong GenericValueToInt( LLVMGenericValueRef @GenVal, [MarshalAs( UnmanagedType.Bool )]bool @IsSigned );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueToPointer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GenericValueToPointer( LLVMGenericValueRef @GenVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueToFloat", CallingConvention = CallingConvention.Cdecl )]
        internal static extern double GenericValueToFloat( LLVMTypeRef @TyRef, LLVMGenericValueRef @GenVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeGenericValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeGenericValue( LLVMGenericValueRef @GenVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateExecutionEngineForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateExecutionEngineForModule( out LLVMExecutionEngineRef @OutEE, LLVMModuleRef @M, out IntPtr @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateInterpreterForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateInterpreterForModule( out LLVMExecutionEngineRef @OutInterp, LLVMModuleRef @M, out IntPtr @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateJITCompilerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateJITCompilerForModule( out LLVMExecutionEngineRef @OutJIT, LLVMModuleRef @M, uint @OptLevel, out IntPtr @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMCJITCompilerOptions", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeMCJITCompilerOptions( out LLVMMCJITCompilerOptions @Options, size_t @SizeOfOptions );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMCJITCompilerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus CreateMCJITCompilerForModule( out LLVMExecutionEngineRef @OutJIT, LLVMModuleRef @M, out LLVMMCJITCompilerOptions @Options, size_t @SizeOfOptions, out IntPtr @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeExecutionEngine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeExecutionEngine( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunStaticConstructors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RunStaticConstructors( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunStaticDestructors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void RunStaticDestructors( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunFunctionAsMain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern int RunFunctionAsMain( LLVMExecutionEngineRef @EE, LLVMValueRef @F, uint @ArgC, string[ ] @ArgV, string[ ] @EnvP );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef RunFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @F, uint @NumArgs, out LLVMGenericValueRef @Args );

        [DllImport( LibraryPath, EntryPoint = "LLVMFreeMachineCodeForFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void FreeMachineCodeForFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @F );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddModule( LLVMExecutionEngineRef @EE, LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus RemoveModule( LLVMExecutionEngineRef @EE, LLVMModuleRef @M, out LLVMModuleRef @OutMod, out IntPtr @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMFindFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus FindFunction( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name, out LLVMValueRef @OutFn );

        /* As of at least LLVM 4.0.1 this just returns null
        //[DllImport( libraryPath, EntryPoint = "LLVMRecompileAndRelinkFunction", CallingConvention = CallingConvention.Cdecl )]
        //internal static extern IntPtr RecompileAndRelinkFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @Fn );
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMGetExecutionEngineTargetData", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef GetExecutionEngineTargetData( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetExecutionEngineTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetMachineRef GetExecutionEngineTargetMachine( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobalMapping", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGlobalMapping( LLVMExecutionEngineRef @EE, LLVMValueRef @Global, IntPtr @Addr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPointerToGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetPointerToGlobal( LLVMExecutionEngineRef @EE, LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalValueAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern int GetGlobalValueAddress( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFunctionAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern int GetFunctionAddress( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateSimpleMCJITMemoryManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMCJITMemoryManagerRef CreateSimpleMCJITMemoryManager( IntPtr @Opaque, LLVMMemoryManagerAllocateCodeSectionCallback @AllocateCodeSection, LLVMMemoryManagerAllocateDataSectionCallback @AllocateDataSection, LLVMMemoryManagerFinalizeMemoryCallback @FinalizeMemory, LLVMMemoryManagerDestroyCallback @Destroy );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeMCJITMemoryManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeMCJITMemoryManager( LLVMMCJITMemoryManagerRef @MM );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTransformUtils", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeTransformUtils( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeScalarOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeScalarOpts( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeObjCARCOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeObjCARCOpts( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeVectorization", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeVectorization( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstCombine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeInstCombine( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPO", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeIPO( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstrumentation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeInstrumentation( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAnalysis", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeAnalysis( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPA", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeIPA( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCodeGen", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeCodeGen( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void InitializeTarget( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseIRInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus ParseIRInContext( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM, out IntPtr @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMLinkModules2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LinkModules2( LLVMModuleRef @Dest, LLVMModuleRef @Src );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMObjectFileRef CreateObjectFile( LLVMMemoryBufferRef @MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeObjectFile( LLVMObjectFileRef @ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSections", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSectionIteratorRef GetSections( LLVMObjectFileRef @ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSectionIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeSectionIterator( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsSectionIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsSectionIteratorAtEnd( LLVMObjectFileRef @ObjectFile, LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToNextSection( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToContainingSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToContainingSection( LLVMSectionIteratorRef @Sect, LLVMSymbolIteratorRef @Sym );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbols", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef GetSymbols( LLVMObjectFileRef @ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSymbolIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeSymbolIterator( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsSymbolIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsSymbolIteratorAtEnd( LLVMObjectFileRef @ObjectFile, LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToNextSymbol( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetSectionName( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSectionSize( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContents", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetSectionContents( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSectionAddress( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContainsSymbol", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool GetSectionContainsSymbol( LLVMSectionIteratorRef @SI, LLVMSymbolIteratorRef @Sym );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocations", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMRelocationIteratorRef GetRelocations( LLVMSectionIteratorRef @Section );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeRelocationIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void DisposeRelocationIterator( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsRelocationIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsRelocationIteratorAtEnd( LLVMSectionIteratorRef @Section, LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextRelocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void MoveToNextRelocation( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetSymbolName( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSymbolAddress( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetSymbolSize( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationOffset", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetRelocationOffset( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef GetRelocationSymbol( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int GetRelocationType( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationTypeName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetRelocationTypeName( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationValueString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr GetRelocationValueString( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcMakeSharedModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSharedModuleRef OrcMakeSharedModule( LLVMModuleRef Mod );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcDisposeSharedModuleRef", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcDisposeSharedModuleRef( LLVMSharedModuleRef SharedMod );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcMakeSharedObjectBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSharedObjectBufferRef OrcMakeSharedObjectBuffer( LLVMMemoryBufferRef ObjBuffer );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcDisposeSharedObjectBufferRef", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcDisposeSharedObjectBufferRef( LLVMSharedObjectBufferRef SharedObjBuffer );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcCreateInstance", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcJITStackRef OrcCreateInstance( LLVMTargetMachineRef @TM );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcGetErrorMsg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr OrcGetErrorMsg( LLVMOrcJITStackRef @JITStack );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcGetMangledSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void OrcGetMangledSymbol( LLVMOrcJITStackRef @JITStack, out IntPtr @MangledSymbol, [MarshalAs( UnmanagedType.LPStr )] string @Symbol );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcDisposeMangledSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcDisposeMangledSymbol( IntPtr @MangledSymbol );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcCreateLazyCompileCallback", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcTargetAddress OrcCreateLazyCompileCallback( LLVMOrcJITStackRef @JITStack, LLVMOrcLazyCompileCallbackFn @Callback, IntPtr @CallbackCtx );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcCreateIndirectStub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMOrcErrorCode OrcCreateIndirectStub( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @InitAddr );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcSetIndirectStubPointer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMOrcErrorCode OrcSetIndirectStubPointer( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @StubName, LLVMOrcTargetAddress @NewAddr );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcAddEagerlyCompiledIR", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcModuleHandle OrcAddEagerlyCompiledIR( LLVMOrcJITStackRef @JITStack, LLVMSharedModuleRef @Mod, LLVMOrcSymbolResolverFn @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcAddLazilyCompiledIR", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcModuleHandle OrcAddLazilyCompiledIR( LLVMOrcJITStackRef @JITStack, LLVMSharedModuleRef @Mod, LLVMOrcSymbolResolverFn @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcAddObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOrcModuleHandle OrcAddObjectFile( LLVMOrcJITStackRef @JITStack, LLVMSharedObjectBufferRef @Obj, LLVMOrcSymbolResolverFn @SymbolResolver, IntPtr @SymbolResolverCtx );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcRemoveModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcRemoveModule( LLVMOrcJITStackRef @JITStack, LLVMOrcModuleHandle @H );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcGetSymbolAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMOrcTargetAddress OrcGetSymbolAddress( LLVMOrcJITStackRef @JITStack, [MarshalAs( UnmanagedType.LPStr )] string @SymbolName );

        [DllImport( LibraryPath, EntryPoint = "LLVMOrcDisposeInstance", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void OrcDisposeInstance( LLVMOrcJITStackRef @JITStack );

        [DllImport( LibraryPath, EntryPoint = "LLVMLoadLibraryPermanently", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LoadLibraryPermanently( [MarshalAs( UnmanagedType.LPStr )] string @Filename );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseCommandLineOptions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void ParseCommandLineOptions( int @argc, string[ ] @argv, [MarshalAs( UnmanagedType.LPStr )] string @Overview );

        [DllImport( LibraryPath, EntryPoint = "LLVMSearchForAddressOfSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern IntPtr SearchForAddressOfSymbol( [MarshalAs( UnmanagedType.LPStr )] string @symbolName );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void AddSymbol( [MarshalAs( UnmanagedType.LPStr )] string @symbolName, IntPtr @symbolValue );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddArgumentPromotionPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddArgumentPromotionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddConstantMergePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddConstantMergePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDeadArgEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDeadArgEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddFunctionAttrsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddFunctionAttrsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddFunctionInliningPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddFunctionInliningPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAlwaysInlinerPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAlwaysInlinerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobalDCEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGlobalDCEPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobalOptimizerPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGlobalOptimizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddIPConstantPropagationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIPConstantPropagationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddPruneEHPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddPruneEHPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddIPSCCPPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIPSCCPPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddInternalizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddInternalizePass( LLVMPassManagerRef @param0, uint @AllButMain );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddStripDeadPrototypesPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddStripDeadPrototypesPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddStripSymbolsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddStripSymbolsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderCreate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerBuilderRef PassManagerBuilderCreate( );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderDispose( LLVMPassManagerBuilderRef @PMB );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetOptLevel", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetOptLevel( LLVMPassManagerBuilderRef @PMB, uint @OptLevel );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetSizeLevel", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetSizeLevel( LLVMPassManagerBuilderRef @PMB, uint @SizeLevel );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnitAtATime", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetDisableUnitAtATime( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnrollLoops", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetDisableUnrollLoops( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableSimplifyLibCalls", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderSetDisableSimplifyLibCalls( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderUseInlinerWithThreshold", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderUseInlinerWithThreshold( LLVMPassManagerBuilderRef @PMB, uint @Threshold );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderPopulateFunctionPassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateModulePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderPopulateModulePassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateLTOPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void PassManagerBuilderPopulateLTOPassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM, [MarshalAs( UnmanagedType.Bool )]bool @Internalize, [MarshalAs( UnmanagedType.Bool )]bool @RunInliner );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAggressiveDCEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAggressiveDCEPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddBitTrackingDCEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddBitTrackingDCEPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAlignmentFromAssumptionsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddAlignmentFromAssumptionsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCFGSimplificationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCFGSimplificationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLateCFGSimplificationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLateCFGSimplificationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDeadStoreEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDeadStoreEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarizerPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddMergedLoadStoreMotionPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddMergedLoadStoreMotionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGVNPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddGVNPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddNewGVNPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddNewGVNPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddIndVarSimplifyPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddIndVarSimplifyPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddInstructionCombiningPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddInstructionCombiningPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddJumpThreadingPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddJumpThreadingPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLICMPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLICMPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopDeletionPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopDeletionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopIdiomPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopIdiomPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopRotatePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopRotatePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopRerollPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopRerollPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopUnrollPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopUnrollPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopUnswitchPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopUnswitchPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddMemCpyOptPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddMemCpyOptPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddPartiallyInlineLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddPartiallyInlineLibCallsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLowerSwitchPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLowerSwitchPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddPromoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddPromoteMemoryToRegisterPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddReassociatePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddReassociatePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSCCPPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddSCCPPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarReplAggregatesPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassSSA", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarReplAggregatesPassSSA( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassWithThreshold", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScalarReplAggregatesPassWithThreshold( LLVMPassManagerRef @PM, int @Threshold );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSimplifyLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddSimplifyLibCallsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTailCallEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddTailCallEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddConstantPropagationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddConstantPropagationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDemoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddDemoteMemoryToRegisterPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddVerifierPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddVerifierPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCorrelatedValuePropagationPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddCorrelatedValuePropagationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddEarlyCSEPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddEarlyCSEPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddEarlyCSEMemSSAPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddEarlyCSEMemSSAPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLowerExpectIntrinsicPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLowerExpectIntrinsicPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTypeBasedAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddTypeBasedAliasAnalysisPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScopedNoAliasAAPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddScopedNoAliasAAPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddBasicAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddBasicAliasAnalysisPass( LLVMPassManagerRef @PM );

        [Obsolete( "Use AddSLPVectorizePass instead" )]
        [DllImport( LibraryPath, EntryPoint = "LLVMAddBBVectorizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddBBVectorizePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopVectorizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddLoopVectorizePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSLPVectorizePass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void AddSLPVectorizePass( LLVMPassManagerRef @PM );
    }
}
