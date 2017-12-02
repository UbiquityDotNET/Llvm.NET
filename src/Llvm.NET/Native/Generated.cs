// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

/* NOTE: While this code was originally generated from Clang based tool parsing the LLVM headers
// it was modified extensively since then to use more correct marshaling attributes as well as
// custom marshaling for the specially allocated strings used by LLVM. This is not auto generated.
// This represents a low level interop P/Invoke of the standard LLVM-C API. Additional C-APIs are
// added by the LibLlvm project and those are declared in the CustomGenerated. (Which is also
// maintained manually now)
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

// warning CS0649: Field 'xxx' is never assigned to, and will always have its default value 0
#pragma warning disable 649

// Mostly generated code, but all pure interop, documentation is in native code
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1649 // Filename must match first type name
#pragma warning disable SA1124 // Do not use regions - using regions to group functions for eventual split to distinct source files

namespace Llvm.NET.Native
{
    internal struct LLVMOpInfoSymbol1
    {
        internal int @Present;
        [MarshalAs( UnmanagedType.LPStr )]
        internal string @Name;
        internal int @Value;
    }

    internal struct LLVMOpInfo1
    {
        internal LLVMOpInfoSymbol1 @AddSymbol;
        internal LLVMOpInfoSymbol1 @SubtractSymbol;
        internal int @Value;
        internal int @VariantKind;
    }

    internal struct LLVMMCJITCompilerOptions
    {
        internal uint @OptLevel;
        internal LLVMCodeModel @CodeModel;
        internal int @NoFramePointerElim;
        internal int @EnableFastISel;
        internal LLVMMCJITMemoryManagerRef @MCJMM;
    }

    // maps to LLVMBool in LLVM-C for methods that return
    // 0 on success. This was hand added to help clarify use
    // when a return value is not really a bool but a status
    // where (0==SUCCESS)
    internal struct LLVMStatus
    {
        public LLVMStatus( int value )
        {
            ErrorCode = value;
        }

        public bool Succeeded => ErrorCode == 0;

        public bool Failed => !Succeeded;

        public int ErrorCode { get; }
    }

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMFatalErrorHandler( [MarshalAs( UnmanagedType.LPStr )] string reason );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMDiagnosticHandler( LLVMDiagnosticInfoRef @param0, IntPtr @param1 );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMYieldCallback( LLVMContextRef @param0, IntPtr @param1 );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate int LLVMOpInfoCallback( IntPtr disInfo, int pC, int offset, int size, int tagType, IntPtr tagBuf );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate string LLVMSymbolLookupCallback( IntPtr disInfo, int referenceValue, out int referenceType, int referencePC, out IntPtr referenceName );

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
    internal struct llvm_lto_t
    {
        internal llvm_lto_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal struct lto_bool_t
    {
        internal lto_bool_t( bool value )
        {
            Value = value;
        }

        internal bool Value;
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal struct lto_module_t
    {
        internal lto_module_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal struct lto_code_gen_t
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

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate ulong LLVMOrcLazyCompileCallbackFn( LLVMOrcJITStackRef jITStack, IntPtr callbackCtx );

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

        #region Misc...
        [DllImport( LibraryPath, EntryPoint = "LLVMInstallFatalErrorHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInstallFatalErrorHandler( LLVMFatalErrorHandler @Handler );

        [DllImport( LibraryPath, EntryPoint = "LLVMResetFatalErrorHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMResetFatalErrorHandler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMEnablePrettyStackTrace", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMEnablePrettyStackTrace( );

        [DllImport( LibraryPath, EntryPoint = "LLVMShutdown", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMShutdown( );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseCommandLineOptions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMParseCommandLineOptions( int @argc, string[ ] @argv, [MarshalAs( UnmanagedType.LPStr )] string @Overview );

        [DllImport( LibraryPath, EntryPoint = "LLVMSearchForAddressOfSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern IntPtr LLVMSearchForAddressOfSymbol( [MarshalAs( UnmanagedType.LPStr )] string @symbolName );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMAddSymbol( [MarshalAs( UnmanagedType.LPStr )] string @symbolName, IntPtr @symbolValue );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDiagInfoDescription", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMGetDiagInfoDescription( LLVMDiagnosticInfoRef @DI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDiagInfoSeverity", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDiagnosticSeverity LLVMGetDiagInfoSeverity( LLVMDiagnosticInfoRef @DI );
        #endregion

        #region Context
        [DllImport( LibraryPath, EntryPoint = "LLVMContextCreate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextRef LLVMContextCreate( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextAlias LLVMGetGlobalContext( );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextSetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMContextSetDiagnosticHandler( LLVMContextRef @C, IntPtr @Handler, IntPtr @DiagnosticContext );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextGetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDiagnosticHandler LLVMContextGetDiagnosticHandler( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextGetDiagnosticContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMContextGetDiagnosticContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMContextSetYieldCallback", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMContextSetYieldCallback( LLVMContextRef @C, LLVMYieldCallback @Callback, IntPtr @OpaqueHandle );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMDKindIDInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint LLVMGetMDKindIDInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseIRInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMParseIRInContext( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM, out IntPtr @OutMessage );
        #endregion

        #region Attributes
        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKindForName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint LLVMGetEnumAttributeKindForName( [MarshalAs( UnmanagedType.LPStr )] string @Name, size_t @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetLastEnumAttributeKind( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef LLVMCreateEnumAttribute( LLVMContextRef @C, uint @KindID, ulong @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetEnumAttributeKind( LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMGetEnumAttributeValue( LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef LLVMCreateStringAttribute( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLength, [MarshalAs( UnmanagedType.LPStr )] string @V, uint @VLength );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetStringAttributeKind( LLVMAttributeRef @A, out uint @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetStringAttributeValue( LLVMAttributeRef @A, out uint @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsEnumAttribute( LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsStringAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsStringAttribute( LLVMAttributeRef @A );
        #endregion

        #region Module
        [DllImport( LibraryPath, EntryPoint = "LLVMLinkModules2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMLinkModules2( LLVMModuleRef @Dest, LLVMModuleRef @Src );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleCreateWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMModuleRef LLVMModuleCreateWithName( [MarshalAs( UnmanagedType.LPStr )] string @ModuleID );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleCreateWithNameInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMModuleRef LLVMModuleCreateWithNameInContext( [MarshalAs( UnmanagedType.LPStr )] string @ModuleID, LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMCloneModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleRef LLVMCloneModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleIdentifier", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetModuleIdentifier( LLVMModuleRef @M, out size_t @Len );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleIdentifier", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetModuleIdentifier( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Ident, size_t @Len );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDataLayoutStr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetDataLayoutStr( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetDataLayout( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDataLayout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetDataLayout( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @DataLayoutStr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTarget", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetTarget( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetTarget", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetTarget( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMDumpModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDumpModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintModuleToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMPrintModuleToFile( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Filename, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintModuleToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMPrintModuleToString( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetModuleInlineAsm( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Asm );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMContextAlias LLVMGetModuleContext( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTypeByName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTypeRef LLVMGetTypeByName( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedMetadataNumOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint LLVMGetNamedMetadataNumOperands( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedMetadataOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMGetNamedMetadataOperands( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, out LLVMValueRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddNamedMetadataOperand", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMAddNamedMetadataOperand( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMAddFunction( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMGetNamedFunction( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetFirstFunction( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetLastFunction( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetNextFunction( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetPreviousFunction( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMWriteBitcodeToFile( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Path );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFD", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMWriteBitcodeToFD( LLVMModuleRef @M, int @FD, int @ShouldClose, int @Unbuffered );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFileHandle", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMWriteBitcodeToFileHandle( LLVMModuleRef @M, int @Handle );

        [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMemoryBufferRef LLVMWriteBitcodeToMemoryBuffer( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateModuleProviderForExistingModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleProviderRef LLVMCreateModuleProviderForExistingModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeModuleProvider", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeModuleProvider( LLVMModuleProviderRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcode2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMParseBitcode2( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModule2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMGetBitcodeModule2( LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcodeInContext2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMParseBitcodeInContext2( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutModule );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModuleInContext2", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMGetBitcodeModuleInContext2( LLVMContextRef @ContextRef, LLVMMemoryBufferRef @MemBuf, out LLVMModuleRef @OutM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMVerifyModule( LLVMModuleRef @M, LLVMVerifierFailureAction @Action, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutMessage );
        #endregion

        #region Types
        [DllImport( LibraryPath, EntryPoint = "LLVMGetTypeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeKind LLVMGetTypeKind( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMTypeIsSized", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTypeIsSized( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMDumpType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDumpType( LLVMTypeRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintTypeToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMPrintTypeToString( LLVMTypeRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt1TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt1TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt8TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt8TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt16TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt16TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt32TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt32TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt64TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt64TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt128TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMIntTypeInContext( LLVMContextRef @C, uint @NumBits );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt1Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt1ype( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt8Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt8ype( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt16Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt16Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt32Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt32Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt64Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt64Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInt128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMInt128Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMIntType( uint @NumBits );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIntTypeWidth", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetIntTypeWidth( LLVMTypeRef @IntegerTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMHalfTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMHalfTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMFloatTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMFloatTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMDoubleTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMDoubleTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86FP80TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMX8FP80TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMFP18TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMPPCFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMPPCFP18TypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMHalfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMHalfType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMFloatType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMFloatType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMDoubleType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMDoubleType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86FP80Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMX8FP80Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMFP128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMFP18Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMPPCFP128Type", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMPPCFP18Type( );

        [DllImport( LibraryPath, EntryPoint = "LLVMFunctionType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMFunctionType( LLVMTypeRef @ReturnType, out LLVMTypeRef @ParamTypes, uint @ParamCount, [MarshalAs( UnmanagedType.Bool )]bool @IsVarArg );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsFunctionVarArg", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsFunctionVarArg( LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetReturnType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMGetReturnType( LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountParamTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMCountParamTypes( LLVMTypeRef @FunctionTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParamTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetParamTypes( LLVMTypeRef @FunctionTy, out LLVMTypeRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMStructTypeInContext( LLVMContextRef @C, out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMStructType( out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructCreateNamed", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTypeRef LLVMStructCreateNamed( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStructName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetStructName( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructSetBody", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMStructSetBody( LLVMTypeRef @StructTy, out LLVMTypeRef @ElementTypes, uint @ElementCount, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMCountStructElementTypes( LLVMTypeRef @StructTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetStructElementTypes( LLVMTypeRef @StructTy, out LLVMTypeRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMStructGetTypeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMStructGetTypeAtIndex( LLVMTypeRef @StructTy, uint @i );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsPackedStruct", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsPackedStruct( LLVMTypeRef @StructTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsOpaqueStruct", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsOpaqueStruct( LLVMTypeRef @StructTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetElementType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMGetElementType( LLVMTypeRef @Ty );

        // Added to LLVM-C APIs in 5.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMGetSubtypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetSubtypes( LLVMTypeRef Tp, out LLVMTypeRef Arr );

        // Added to LLVM-C APIs in 5.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumContainedTypes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetNumContainedTypes( LLVMTypeRef Tp );

        [DllImport( LibraryPath, EntryPoint = "LLVMArrayType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMArrayType( LLVMTypeRef @ElementType, uint @ElementCount );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetArrayLength", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetArrayLength( LLVMTypeRef @ArrayTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMPointerType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMPointerType( LLVMTypeRef @ElementType, uint @AddressSpace );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPointerAddressSpace", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetPointerAddressSpace( LLVMTypeRef @PointerTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMVectorType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMVectorType( LLVMTypeRef @ElementType, uint @ElementCount );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetVectorSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetVectorSize( LLVMTypeRef @VectorTy );

        [DllImport( LibraryPath, EntryPoint = "LLVMVoidTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMVoidTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMLabelTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMLabelTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86MMXTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMX8MMXTypeInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMVoidType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMVoidType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMLabelType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMLabelType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMX86MMXType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMX8MMXType( );

        [DllImport( LibraryPath, EntryPoint = "LLVMTypeOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMTypeOf( LLVMValueRef @Val );
        #endregion

        #region Value
        /* excluded in favor of custom version that redirects to GetValueId
        // [DllImport(libraryPath, EntryPoint = "LLVMGetValueKind", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern LLVMValueKind LLVMGetValueKindLLVMValueRef( @Val);
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMSetParamAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetParamAlignment( LLVMValueRef @Arg, uint @Align );

        [DllImport( LibraryPath, EntryPoint = "LLVMHasMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMHasMetadata( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetMetadata( LLVMValueRef @Val, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetMetadata", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetMetadata( LLVMValueRef @Val, uint @KindID, LLVMValueRef @Node );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAtomicSingleThread", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsAtomicSingleThread( LLVMValueRef @AtomicInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetAtomicSingleThread", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetAtomicSingleThread( LLVMValueRef @AtomicInst, [MarshalAs( UnmanagedType.Bool )]bool @SingleThread );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCmpXchgSuccessOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering LLVMGetCmpXchgSuccessOrdering( LLVMValueRef @CmpXchgInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCmpXchgSuccessOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetCmpXchgSuccessOrdering( LLVMValueRef @CmpXchgInst, LLVMAtomicOrdering @Ordering );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCmpXchgFailureOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering LLVMGetCmpXchgFailureOrdering( LLVMValueRef @CmpXchgInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCmpXchgFailureOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetCmpXchgFailureOrdering( LLVMValueRef @CmpXchgInst, LLVMAtomicOrdering @Ordering );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetValueName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetValueName( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetValueName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetValueName( LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMDumpValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDumpValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMPrintValueToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMPrintValueToString( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMReplaceAllUsesWith", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMReplaceAllUsesWith( LLVMValueRef @OldVal, LLVMValueRef @NewVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsConstant", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsConstant( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsUndef", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsUndef( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAArgument", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAArgument( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsABasicBlock( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInlineAsm", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAInlineAsm( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAUser( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstant( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABlockAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsABlockAddress( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantAggregateZero", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantAggregateZero( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantArray( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataSequential", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantDataSequential( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantDataArray( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantDataVector( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantExpr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantExpr( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantFP( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantInt( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantPointerNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantPointerNull( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantStruct( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantTokenNone", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantTokenNone( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAConstantVector( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAGlobalValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalAlias", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAGlobalAlias( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalObject", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAGlobalObject( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAFunction( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalVariable", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAGlobalVariable( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUndefValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAUndefValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAInstruction( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABinaryOperator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsABinaryOperator( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACallInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsACallInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAIntrinsicInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAIntrinsicInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsADbgInfoIntrinsic", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsADbgInfoIntrinsic( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsADbgDeclareInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsADbgDeclareInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemIntrinsic", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAMemIntrinsic( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemCpyInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAMemCpyInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemMoveInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAMemMoveInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemSetInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAMemSetInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsACmpInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFCmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAFCmpInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAICmpInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAICmpInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAExtractElementInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAExtractElementInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAGetElementPtrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAGetElementPtrInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInsertElementInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAInsertElementInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInsertValueInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAInsertValueInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsALandingPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsALandingPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAPHINode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAPHINode( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASelectInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsASelectInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAShuffleVectorInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAShuffleVectorInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAStoreInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAStoreInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsATerminatorInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsATerminatorInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABranchInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsABranchInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAIndirectBrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAIndirectBrInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAInvokeInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAInvokeInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAReturnInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASwitchInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsASwitchInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUnreachableInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAUnreachableInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAResumeInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAResumeInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACleanupReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsACleanupReturnInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACatchReturnInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsACatchReturnInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFuncletPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAFuncletPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACatchPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsACatchPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACleanupPadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsACleanupPadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUnaryInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAUnaryInstruction( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAAllocaInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAAllocaInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsACastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsACastInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAAddrSpaceCastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAAddrSpaceCastInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsABitCastInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsABitCastInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAFPExtInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPToSIInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAFPToSIInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPToUIInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAFPToUIInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPTruncInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAFPTruncInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAIntToPtrInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAIntToPtrInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAPtrToIntInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAPtrToIntInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsASExtInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsASIToFPInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsASIToFPInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsATruncInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsATruncInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAUIToFPInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAUIToFPInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAZExtInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAZExtInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAExtractValueInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAExtractValueInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsALoadInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsALoadInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAVAArgInst", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAVAArgInst( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMDNode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAMDNode( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsAMDString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMIsAMDString( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetUser( LLVMUseRef @U );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUsedValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetUsedValue( LLVMUseRef @U );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetOperandUse", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMUseRef LLVMGetOperandUse( LLVMValueRef @Val, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNull( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAllOnes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstAllOnes( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUndef", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetUndef( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAsString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetAsString( LLVMValueRef @c, out size_t @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsNull", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsNull( LLVMValueRef @Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LVMVerifyFunction( LLVMValueRef @Fn, LLVMVerifierFailureAction @Action );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMViewFunctionCFG( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMViewFunctionCFGOnly", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMViewFunctionCFGOnly( LLVMValueRef @Fn );
        #endregion

        #region Constants
        [DllImport( LibraryPath, EntryPoint = "LLVMConstPointerNull", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstPointerNull( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstInt( LLVMTypeRef @IntTy, ulong @N, [MarshalAs( UnmanagedType.Bool )]bool @SignExtend );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfArbitraryPrecision", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstIntOfArbitraryPrecision( LLVMTypeRef @IntTy, uint @NumWords, int[ ] @Words );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMConstIntOfString( LLVMTypeRef @IntTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, byte @Radix );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMConstIntOfStringAndSize( LLVMTypeRef @IntTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, uint @SLen, byte @Radix );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstReal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstReal( LLVMTypeRef @RealTy, double @N );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstRealOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMConstRealOfString( LLVMTypeRef @RealTy, [MarshalAs( UnmanagedType.LPStr )] string @Text );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstRealOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMConstRealOfStringAndSize( LLVMTypeRef @RealTy, [MarshalAs( UnmanagedType.LPStr )] string @Text, uint @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntGetZExtValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMConstIntGetZExtValue( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntGetSExtValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern long LLVMConstIntGetSExtValue( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstRealGetDouble", CallingConvention = CallingConvention.Cdecl )]
        internal static extern double LLVMConstRealGetDouble( LLVMValueRef @ConstantVal, [MarshalAs( UnmanagedType.Bool )]out bool @losesInfo );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstStringInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMConstStringInContext( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @Length, [MarshalAs( UnmanagedType.Bool )]bool @DontNullTerminate );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMConstString( [MarshalAs( UnmanagedType.LPStr )] string @Str, uint @Length, [MarshalAs( UnmanagedType.Bool )]bool @DontNullTerminate );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsConstantString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsConstantString( LLVMValueRef @c );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstStructInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstStructInContext( LLVMContextRef @C, out LLVMValueRef @ConstantVals, uint @Count, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstStruct( out LLVMValueRef @ConstantVals, uint @Count, [MarshalAs( UnmanagedType.Bool )]bool @Packed );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstArray", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstArray( LLVMTypeRef @ElementTy, out LLVMValueRef @ConstantVals, uint @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNamedStruct", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNamedStruct( LLVMTypeRef @StructTy, out LLVMValueRef @ConstantVals, uint @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetElementAsConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetElementAsConstant( LLVMValueRef @C, uint @idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstVector( out LLVMValueRef @ScalarConstantVals, uint @Size );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetConstOpcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOpcode LLVMGetConstOpcode( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMAlignOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMAlignOf( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMSizeOf", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMSizeOf( LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNSWNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNUWNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFNeg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFNeg( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNot", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNot( LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNSWAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNUWAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFAdd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFAdd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNSWSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNUWSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFSub", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFSub( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNSWMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstNUWMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFMul", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFMul( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstUDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstUDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        // Added to LLVM-C APIs in LLVM 4.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMConstExactUDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstExactUDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstSDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstExactSDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstExactSDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFDiv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFDiv( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstURem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstURem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSRem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstSRem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFRem", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFRem( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAnd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstAnd( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstOr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstOr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstXor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstXor( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstICmp", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstICmp( LLVMIntPredicate @Predicate, LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFCmp", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFCmp( LLVMRealPredicate @Predicate, LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstShl", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstShl( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstLShr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstLShr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAShr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstAShr( LLVMValueRef @LHSConstant, LLVMValueRef @RHSConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstGEP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstGEP( LLVMValueRef @ConstantVal, out LLVMValueRef @ConstantIndices, uint @NumIndices );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInBoundsGEP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstInBoundsGEP( LLVMValueRef @ConstantVal, out LLVMValueRef @ConstantIndices, uint @NumIndices );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstTrunc", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstTrunc( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstSExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstZExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstZExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPTrunc", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFPTrunc( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPExt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFPExt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstUIToFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstUIToFP( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSIToFP", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstSIToFP( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPToUI", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFPToUI( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPToSI", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFPToSI( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstPtrToInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstPtrToInt( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntToPtr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstIntToPtr( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstAddrSpaceCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstAddrSpaceCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstZExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstZExtOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstSExtOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstTruncOrBitCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstTruncOrBitCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstPointerCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstPointerCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstIntCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstIntCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType, [MarshalAs( UnmanagedType.Bool )]bool @isSigned );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstFPCast", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstFPCast( LLVMValueRef @ConstantVal, LLVMTypeRef @ToType );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstSelect", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstSelect( LLVMValueRef @ConstantCondition, LLVMValueRef @ConstantIfTrue, LLVMValueRef @ConstantIfFalse );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstExtractElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstExtractElement( LLVMValueRef @VectorConstant, LLVMValueRef @IndexConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInsertElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstInsertElement( LLVMValueRef @VectorConstant, LLVMValueRef @ElementValueConstant, LLVMValueRef @IndexConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstShuffleVector", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstShuffleVector( LLVMValueRef @VectorAConstant, LLVMValueRef @VectorBConstant, LLVMValueRef @MaskConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstExtractValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstExtractValue( LLVMValueRef @AggConstant, out uint @IdxList, uint @NumIdx );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInsertValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMConstInsertValue( LLVMValueRef @AggConstant, LLVMValueRef @ElementValueConstant, out uint @IdxList, uint @NumIdx );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMConstInlineAsm( LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @AsmString, [MarshalAs( UnmanagedType.LPStr )] string @Constraints, [MarshalAs( UnmanagedType.Bool )]bool @HasSideEffects, [MarshalAs( UnmanagedType.Bool )]bool @IsAlignStack );

        [DllImport( LibraryPath, EntryPoint = "LLVMBlockAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBlockAddress( LLVMValueRef @F, LLVMBasicBlockRef @BB );
        #endregion

        #region Globals
        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMModuleRef LLVMGetGlobalParent( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsDeclaration", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsDeclaration( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLinkage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMLinkage LLVMGetLinkage( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetLinkage", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetLinkage( LLVMValueRef @Global, LLVMLinkage @Linkage );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSection", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetSection( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetSection", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetSection( LLVMValueRef @Global, [MarshalAs( UnmanagedType.LPStr )] string @Section );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetVisibility", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMVisibility LLVMGetVisibility( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetVisibility", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetVisibility( LLVMValueRef @Global, LLVMVisibility @Viz );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDLLStorageClass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMDLLStorageClass LLVMGetDLLStorageClass( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDLLStorageClass", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetDLLStorageClass( LLVMValueRef @Global, LLVMDLLStorageClass @Class );

        [DllImport( LibraryPath, EntryPoint = "LLVMHasUnnamedAddr", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMHasUnnamedAddr( LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetUnnamedAddr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetUnnamedAddr( LLVMValueRef @Global, [MarshalAs( UnmanagedType.Bool )]bool hasUnnamedAddr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetAlignment( LLVMValueRef @V );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetAlignment( LLVMValueRef @V, uint @Bytes );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMAddGlobal( LLVMModuleRef @M, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobalInAddressSpace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMAddGlobalInAddressSpace( LLVMModuleRef @M, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name, uint @AddressSpace );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMGetNamedGlobal( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetFirstGlobal( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetLastGlobal( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetNextGlobal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetPreviousGlobal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMDeleteGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDeleteGlobal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInitializer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetInitializer( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInitializer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetInitializer( LLVMValueRef @GlobalVar, LLVMValueRef @ConstantVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsThreadLocal", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsThreadLocal( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetThreadLocal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetThreadLocal( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isThreadLocal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsGlobalConstant", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsGlobalConstant( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetGlobalConstant", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetGlobalConstant( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isConstant );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetThreadLocalMode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMThreadLocalMode LLVMGetThreadLocalMode( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetThreadLocalMode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetThreadLocalMode( LLVMValueRef @GlobalVar, LLVMThreadLocalMode @Mode );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsExternallyInitialized", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsExternallyInitialized( LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetExternallyInitialized", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetExternallyInitialized( LLVMValueRef @GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool @IsExtInit );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAlias", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMAddAlias( LLVMModuleRef @M, LLVMTypeRef @Ty, LLVMValueRef @Aliasee, [MarshalAs( UnmanagedType.LPStr )] string @Name );
        #endregion

        #region Functions
        [DllImport( LibraryPath, EntryPoint = "LLVMDeleteFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDeleteFunction( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMHasPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMHasPersonalityFn( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetPersonalityFn( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetPersonalityFn", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetPersonalityFn( LLVMValueRef @Fn, LLVMValueRef @PersonalityFn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIntrinsicID", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetIntrinsicID( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFunctionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetFunctionCallConv( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetFunctionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetFunctionCallConv( LLVMValueRef @Fn, uint @CC );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountParams", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMCountParams( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParams", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetParams( LLVMValueRef @Fn, out LLVMValueRef @Params );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetParam( LLVMValueRef @Fn, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetParamParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetParamParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetFirstParam( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetLastParam( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetNextParam( LLVMValueRef @Arg );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousParam", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetPreviousParam( LLVMValueRef @Arg );
        #endregion

        #region Attributes
        [DllImport( LibraryPath, EntryPoint = "LLVMGetGC", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetGC( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetGC", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetGC( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributeCountAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetAttributeCountAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributesAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetAttributesAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, out LLVMAttributeRef Attrs );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef LLVMGetEnumAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef LLVMGetStringAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMRemoveEnumAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMRemoveStringAttributeAtIndex( LLVMValueRef @F, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );
        #endregion

        #region Basic Blocks
        [DllImport( LibraryPath, EntryPoint = "LLVMBasicBlockAsValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBasicBlockAsValue( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMValueIsBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMValueIsBasicBlock( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMValueAsBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMValueAsBasicBlock( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetBasicBlockName( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetBasicBlockParent( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockTerminator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetBasicBlockTerminator( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountBasicBlocks", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMCountBasicBlocks( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlocks", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetBasicBlocks( LLVMValueRef @Fn, out LLVMBasicBlockRef @BasicBlocks );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetFirstBasicBlock( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetLastBasicBlock( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetNextBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetPreviousBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEntryBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetEntryBasicBlock( LLVMValueRef @Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMAppendBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef LLVMAppendBasicBlockInContext( LLVMContextRef @C, LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMAppendBasicBlock", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef LLVMAppendBasicBlock( LLVMValueRef @Fn, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef LLVMInsertBasicBlockInContext( LLVMContextRef @C, LLVMBasicBlockRef @BB, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertBasicBlock", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMBasicBlockRef LLVMInsertBasicBlock( LLVMBasicBlockRef @InsertBeforeBB, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMDeleteBasicBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDeleteBasicBlock( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveBasicBlockFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMRemoveBasicBlockFromParent( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveBasicBlockBefore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveBasicBlockBefore( LLVMBasicBlockRef @BB, LLVMBasicBlockRef @MovePos );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveBasicBlockAfter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveBasicBlockAfter( LLVMBasicBlockRef @BB, LLVMBasicBlockRef @MovePos );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetFirstInstruction( LLVMBasicBlockRef @BB );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetLastInstruction( LLVMBasicBlockRef @BB );
        #endregion

        #region Instructions
        [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetInstructionParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetNextInstruction( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousInstruction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetPreviousInstruction( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMInstructionRemoveFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInstructionRemoveFromParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMInstructionEraseFromParent", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInstructionEraseFromParent( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionOpcode", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMOpcode LLVMGetInstructionOpcode( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetICmpPredicate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMIntPredicate LLVMGetICmpPredicate( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFCmpPredicate", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMRealPredicate LLVMGetFCmpPredicate( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMInstructionClone", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMInstructionClone( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumArgOperands", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetNumArgOperands( LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetInstructionCallConv( LLVMValueRef @Instr, uint @CC );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetInstructionCallConv( LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInstrParamAlignment", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetInstrParamAlignment( LLVMValueRef @Instr, uint @index, uint @Align );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCallSiteAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddCallSiteAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, LLVMAttributeRef @A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteAttributeCount", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetCallSiteAttributeCount( LLVMValueRef C, LLVMAttributeIndex Idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteAttributes", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetCallSiteAttributes( LLVMValueRef C, LLVMAttributeIndex Idx, out LLVMAttributeRef attributes );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef LLVMGetCallSiteEnumAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef LLVMGetCallSiteStringAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMRemoveCallSiteEnumAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, uint @KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMRemoveCallSiteStringAttribute( LLVMValueRef @C, LLVMAttributeIndex @Idx, [MarshalAs( UnmanagedType.LPStr )] string @K, uint @KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCalledValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetCalledValue( LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsTailCall", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsTailCall( LLVMValueRef @CallInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetTailCall", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetTailCall( LLVMValueRef @CallInst, [MarshalAs( UnmanagedType.Bool )]bool isTailCall );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNormalDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetNormalDest( LLVMValueRef @InvokeInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetUnwindDest( LLVMValueRef @InvokeInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetNormalDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetNormalDest( LLVMValueRef @InvokeInst, LLVMBasicBlockRef @B );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetUnwindDest( LLVMValueRef @InvokeInst, LLVMBasicBlockRef @B );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumSuccessors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetNumSuccessors( LLVMValueRef @Term );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSuccessor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetSuccessor( LLVMValueRef @Term, uint @i );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetSuccessor", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetSuccessor( LLVMValueRef @Term, uint @i, LLVMBasicBlockRef @block );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsConditional", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsConditional( LLVMValueRef @Branch );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCondition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetCondition( LLVMValueRef @Branch );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCondition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetCondition( LLVMValueRef @Branch, LLVMValueRef @Cond );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSwitchDefaultDest", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetSwitchDefaultDest( LLVMValueRef @SwitchInstr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAllocatedType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMGetAllocatedType( LLVMValueRef @Alloca );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsInBounds", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsInBounds( LLVMValueRef @GEP );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetIsInBounds", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetIsInBounds( LLVMValueRef @GEP, [MarshalAs( UnmanagedType.Bool )]bool @InBounds );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddIncoming", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddIncoming( LLVMValueRef @PhiNode, out LLVMValueRef @IncomingValues, out LLVMBasicBlockRef @IncomingBlocks, uint @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMCountIncoming", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMCountIncoming( LLVMValueRef @PhiNode );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIncomingValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetIncomingValue( LLVMValueRef @PhiNode, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIncomingBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetIncomingBlock( LLVMValueRef @PhiNode, uint @Index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumIndices", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetNumIndices( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetIndices", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetIndices( LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateBuilderInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBuilderRef LLVMCreateBuilderInContext( LLVMContextRef @C );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBuilderRef LLVMCreateBuilder( );

        [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMPositionBuilder( LLVMBuilderRef @Builder, LLVMBasicBlockRef @Block, LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilderBefore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMPositionBuilderBefore( LLVMBuilderRef @Builder, LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilderAtEnd", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMPositionBuilderAtEnd( LLVMBuilderRef @Builder, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetInsertBlock", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMBasicBlockRef LLVMGetInsertBlock( LLVMBuilderRef @Builder );

        [DllImport( LibraryPath, EntryPoint = "LLVMClearInsertionPosition", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMClearInsertionPosition( LLVMBuilderRef @Builder );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertIntoBuilder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInsertIntoBuilder( LLVMBuilderRef @Builder, LLVMValueRef @Instr );

        [DllImport( LibraryPath, EntryPoint = "LLVMInsertIntoBuilderWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMInsertIntoBuilderWithName( LLVMBuilderRef @Builder, LLVMValueRef @Instr, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetCurrentDebugLocation( LLVMBuilderRef @Builder, LLVMValueRef @L );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetCurrentDebugLocation( LLVMBuilderRef @Builder );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetInstDebugLocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetInstDebugLocation( LLVMBuilderRef @Builder, LLVMValueRef @Inst );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildRetVoid", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildRetVoid( LLVMBuilderRef @param0 );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildRet", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildRet( LLVMBuilderRef @param0, LLVMValueRef @V );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAggregateRet", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildAggregateRet( LLVMBuilderRef @param0, out LLVMValueRef @RetVals, uint @N );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildBr( LLVMBuilderRef @param0, LLVMBasicBlockRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildCondBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildCondBr( LLVMBuilderRef @param0, LLVMValueRef @If, LLVMBasicBlockRef @Then, LLVMBasicBlockRef @Else );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSwitch", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildSwitch( LLVMBuilderRef @param0, LLVMValueRef @V, LLVMBasicBlockRef @Else, uint @NumCases );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIndirectBr", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildIndirectBr( LLVMBuilderRef @B, LLVMValueRef @Addr, uint @NumDests );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInvoke", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildInvoke( LLVMBuilderRef @param0, LLVMValueRef @Fn, out LLVMValueRef @Args, uint @NumArgs, LLVMBasicBlockRef @Then, LLVMBasicBlockRef @Catch, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildLandingPad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildLandingPad( LLVMBuilderRef @B, LLVMTypeRef @Ty, LLVMValueRef @PersFn, uint @NumClauses, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildResume", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildResume( LLVMBuilderRef @B, LLVMValueRef @Exn );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildUnreachable", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildUnreachable( LLVMBuilderRef @param0 );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCase", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddCase( LLVMValueRef @Switch, LLVMValueRef @OnVal, LLVMBasicBlockRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDestination", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddDestination( LLVMValueRef @IndirectBr, LLVMBasicBlockRef @Dest );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumClauses", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetNumClauses( LLVMValueRef @LandingPad );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetClause", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMGetClause( LLVMValueRef @LandingPad, uint @Idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddClause", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddClause( LLVMValueRef @LandingPad, LLVMValueRef @ClauseVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsCleanup", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsCleanup( LLVMValueRef @LandingPad );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCleanup", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetCleanup( LLVMValueRef @LandingPad, [MarshalAs( UnmanagedType.Bool )]bool @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNSWAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNUWAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFAdd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNSWSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNUWSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFSub( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNSWMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNUWMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFMul( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildUDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        // Added to LLVM-C API in LLVM 4.0.0
        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExactUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildExactUDiv( LLVMBuilderRef @param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildSDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExactSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildExactSDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFDiv( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildURem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildURem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildSRem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFRem( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildShl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildShl( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildLShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildLShr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildAShr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildAnd( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildOr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildOr( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildXor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildXor( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildBinOp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildBinOp( LLVMBuilderRef @B, LLVMOpcode @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNeg( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNSWNeg( LLVMBuilderRef @B, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNUWNeg( LLVMBuilderRef @B, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFNeg( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildNot", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildNot( LLVMBuilderRef @param0, LLVMValueRef @V, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildMalloc( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildArrayMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildArrayMalloc( LLVMBuilderRef @param0, LLVMTypeRef @Ty, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildAlloca( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildArrayAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildArrayAlloca( LLVMBuilderRef @param0, LLVMTypeRef @Ty, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFree", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildFree( LLVMBuilderRef @param0, LLVMValueRef @PointerVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildLoad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildLoad( LLVMBuilderRef @param0, LLVMValueRef @PointerVal, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildStore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildStore( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMValueRef @Ptr );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, out LLVMValueRef @Indices, uint @NumIndices, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInBoundsGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildInBoundsGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, out LLVMValueRef @Indices, uint @NumIndices, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildStructGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildStructGEP( LLVMBuilderRef @B, LLVMValueRef @Pointer, uint @Idx, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildGlobalString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildGlobalString( LLVMBuilderRef @B, [MarshalAs( UnmanagedType.LPStr )] string @Str, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildGlobalStringPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildGlobalStringPtr( LLVMBuilderRef @B, [MarshalAs( UnmanagedType.LPStr )] string @Str, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetVolatile", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMGetVolatile( LLVMValueRef @MemoryAccessInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetVolatile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetVolatile( LLVMValueRef @MemoryAccessInst, [MarshalAs( UnmanagedType.Bool )]bool @IsVolatile );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAtomicOrdering LLVMGetOrdering( LLVMValueRef @MemoryAccessInst );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetOrdering", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetOrdering( LLVMValueRef @MemoryAccessInst, LLVMAtomicOrdering @Ordering );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildTrunc( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildZExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildZExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildSExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPToUI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFPToUI( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPToSI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFPToSI( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildUIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildUIToFP( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildSIToFP( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFPTrunc( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFPExt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPtrToInt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildPtrToInt( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIntToPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildIntToPtr( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAddrSpaceCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildAddrSpaceCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildZExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildZExtOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildSExtOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildTruncOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildTruncOrBitCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildCast( LLVMBuilderRef @B, LLVMOpcode @Op, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPointerCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildPointerCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIntCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildIntCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFPCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildICmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildICmp( LLVMBuilderRef @param0, LLVMIntPredicate @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFCmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFCmp( LLVMBuilderRef @param0, LLVMRealPredicate @Op, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPhi", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildPhi( LLVMBuilderRef @param0, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildCall", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildCall( LLVMBuilderRef @param0, LLVMValueRef @Fn, out LLVMValueRef @Args, uint @NumArgs, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildSelect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildSelect( LLVMBuilderRef @param0, LLVMValueRef @If, LLVMValueRef @Then, LLVMValueRef @Else, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildVAArg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildVAArg( LLVMBuilderRef @param0, LLVMValueRef @List, LLVMTypeRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExtractElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildExtractElement( LLVMBuilderRef @param0, LLVMValueRef @VecVal, LLVMValueRef @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInsertElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildInsertElement( LLVMBuilderRef @param0, LLVMValueRef @VecVal, LLVMValueRef @EltVal, LLVMValueRef @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildShuffleVector", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildShuffleVector( LLVMBuilderRef @param0, LLVMValueRef @V1, LLVMValueRef @V2, LLVMValueRef @Mask, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildExtractValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildExtractValue( LLVMBuilderRef @param0, LLVMValueRef @AggVal, uint @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildInsertValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildInsertValue( LLVMBuilderRef @param0, LLVMValueRef @AggVal, LLVMValueRef @EltVal, uint @Index, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIsNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildIsNull( LLVMBuilderRef @param0, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIsNotNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildIsNotNull( LLVMBuilderRef @param0, LLVMValueRef @Val, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildPtrDiff", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildPtrDiff( LLVMBuilderRef @param0, LLVMValueRef @LHS, LLVMValueRef @RHS, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildFence", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMValueRef LLVMBuildFence( LLVMBuilderRef @B, LLVMAtomicOrdering @ordering, [MarshalAs( UnmanagedType.Bool )]bool @singleThread, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAtomicRMW", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildAtomicRMW( LLVMBuilderRef @B, LLVMAtomicRMWBinOp @op, LLVMValueRef @PTR, LLVMValueRef @Val, LLVMAtomicOrdering @ordering, [MarshalAs( UnmanagedType.Bool )]bool @singleThread );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildAtomicCmpXchg", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMValueRef LLVMBuildAtomicCmpXchg( LLVMBuilderRef @B, LLVMValueRef @Ptr, LLVMValueRef @Cmp, LLVMValueRef @New, LLVMAtomicOrdering @SuccessOrdering, LLVMAtomicOrdering @FailureOrdering, [MarshalAs( UnmanagedType.Bool )]bool @SingleThread );
        #endregion

        #region Memory Buffer
        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithContentsOfFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMCreateMemoryBufferWithContentsOfFile( [MarshalAs( UnmanagedType.LPStr )] string @Path, out LLVMMemoryBufferRef @OutMemBuf, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]out string @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithSTDIN", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMCreateMemoryBufferWithSTDIN( out LLVMMemoryBufferRef @OutMemBuf, out IntPtr @OutMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithMemoryRange", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRange( [MarshalAs( UnmanagedType.LPStr )] string @InputData, size_t @InputDataLength, [MarshalAs( UnmanagedType.LPStr )] string @BufferName, [MarshalAs( UnmanagedType.Bool )]bool @RequiresNullTerminator );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMemoryBufferWithMemoryRangeCopy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRangeCopy( [MarshalAs( UnmanagedType.LPStr )] string @InputData, size_t @InputDataLength, [MarshalAs( UnmanagedType.LPStr )] string @BufferName );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBufferStart", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetBufferStart( LLVMMemoryBufferRef @MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetBufferSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern size_t LLVMGetBufferSize( LLVMMemoryBufferRef @MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeMemoryBuffer( LLVMMemoryBufferRef @MemBuf );
        #endregion

        #region Pass Manager
        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalPassRegistry", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassRegistryRef LLVMGetGlobalPassRegistry( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreatePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef LLVMCreatePassManager( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManagerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef LLVMCreateFunctionPassManagerForModule( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef LLVMCreateFunctionPassManager( LLVMModuleProviderRef @MP );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMRunPassManager( LLVMPassManagerRef @PM, LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMInitializeFunctionPassManager( LLVMPassManagerRef @FPM );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMRunFunctionPassManager( LLVMPassManagerRef @FPM, LLVMValueRef @F );

        [DllImport( LibraryPath, EntryPoint = "LLVMFinalizeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMFinalizeFunctionPassManager( LLVMPassManagerRef @FPM );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposePassManager( IntPtr @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeCore( LLVMPassRegistryRef @R );
        #endregion

        #region Disassembler
        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef LLVMCreateDisasm( [MarshalAs( UnmanagedType.LPStr )] string @TripleName, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasmCPU", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef LLVMCreateDisasmCPU( [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasmCPUFeatures", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef LLVMCreateDisasmCPUFeatures( [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, [MarshalAs( UnmanagedType.LPStr )] string @Features, IntPtr @DisInfo, int @TagType, LLVMOpInfoCallback @GetOpInfo, LLVMSymbolLookupCallback @SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDisasmOptions", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMSetDisasmOptions( LLVMDisasmContextRef @DC, int @Options );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisasmDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisasmDispose( LLVMDisasmContextRef @DC );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisasmInstruction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern ulong LLVMDisasmInstruction( LLVMDisasmContextRef @DC, IntPtr @Bytes, long @BytesSize, long @PC, IntPtr @OutString, size_t @OutStringSize );
        #endregion

        #region Targets
        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAMDGPUTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSystemZTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeHexagonTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeNVPTXTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMSP430TargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeXCoreTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMipsTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAArch64TargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeARMTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializePowerPCTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSparcTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86TargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeX86TargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTargetInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeBPFTargetInfo( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAMDGPUTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSystemZTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeHexagonTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeNVPTXTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMSP430Target( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeXCoreTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMipsTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAArch64Target( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeARMTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializePowerPCTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSparcTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86Target", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeX86Target( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeBPFTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAMDGPUTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSystemZTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeHexagonTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeNVPTXTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMSP430TargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeXCoreTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMipsTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAArch64TargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeARMTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializePowerPCTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSparcTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86TargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeX86TargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTargetMC", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeBPFTargetMC( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAMDGPUAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSystemZAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeHexagonAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeNVPTXAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMSP430AsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeXCoreAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMipsAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAArch64AsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeARMAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializePowerPCAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSparcAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeX86AsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeBPFAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAMDGPUAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSystemZAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMipsAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64AsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAArch64AsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeARMAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializePowerPCAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSparcAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86AsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeX86AsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSystemZDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeHexagonDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeXCoreDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMipsDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64Disassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAArch64Disassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeARMDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializePowerPCDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeSparcDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86Disassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeX86Disassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargetInfos", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAllTargetInfos( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargets", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAllTargets( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargetMCs", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAllTargetMCs( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllAsmPrinters", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAllAsmPrinters( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllAsmParsers", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAllAsmParsers( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllDisassemblers", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAllDisassemblers( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMInitializeNativeTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeAsmParser", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMInitializeNativeAsmParser( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMInitializeNativeAsmPrinter( );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeDisassembler", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMInitializeNativeDisassembler( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef LLVMGetModuleDataLayout( LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetModuleDataLayout( LLVMModuleRef @M, LLVMTargetDataRef @DL );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetDataRef LLVMCreateTargetData( [MarshalAs( UnmanagedType.LPStr )] string @StringRep );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTargetLibraryInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddTargetLibraryInfo( LLVMTargetLibraryInfoRef @TLI, LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMCopyStringRepOfTargetData", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMCopyStringRepOfTargetData( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMByteOrder", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMByteOrdering LLVMByteOrder( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMPointerSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMPointerSize( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMPointerSizeForAS", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMPointerSizeForAS( LLVMTargetDataRef @TD, uint @AS );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMIntPtrType( LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeForAS", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMIntPtrTypeForAS( LLVMTargetDataRef @TD, uint @AS );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMIntPtrTypeInContext( LLVMContextRef @C, LLVMTargetDataRef @TD );

        [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeForASInContext", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTypeRef LLVMIntPtrTypeForASInContext( LLVMContextRef @C, LLVMTargetDataRef @TD, uint @AS );

        [DllImport( LibraryPath, EntryPoint = "LLVMSizeOfTypeInBits", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMSizeOfTypeInBits( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMStoreSizeOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMStoreSizeOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMABISizeOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMABISizeOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMABIAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMABIAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMCallFrameAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMCallFrameAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMPreferredAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMPreferredAlignmentOfType( LLVMTargetDataRef @TD, LLVMTypeRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMPreferredAlignmentOfGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMPreferredAlignmentOfGlobal( LLVMTargetDataRef @TD, LLVMValueRef @GlobalVar );

        [DllImport( LibraryPath, EntryPoint = "LLVMElementAtOffset", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMElementAtOffset( LLVMTargetDataRef @TD, LLVMTypeRef @StructTy, ulong @Offset );

        [DllImport( LibraryPath, EntryPoint = "LLVMOffsetOfElement", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMOffsetOfElement( LLVMTargetDataRef @TD, LLVMTypeRef @StructTy, uint @Element );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef LLVMGetFirstTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef LLVMGetNextTarget( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetRef LLVMGetTargetFromName( [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromTriple", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMGetTargetFromTriple( [MarshalAs( UnmanagedType.LPStr )] string @Triple, out LLVMTargetRef @T, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetTargetName( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetDescription", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetTargetDescription( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasJIT", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTargetHasJIT( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTargetHasTargetMachine( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasAsmBackend", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTargetHasAsmBackend( LLVMTargetRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetMachine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetMachineRef LLVMCreateTargetMachine( LLVMTargetRef @T, [MarshalAs( UnmanagedType.LPStr )] string @Triple, [MarshalAs( UnmanagedType.LPStr )] string @CPU, [MarshalAs( UnmanagedType.LPStr )] string @Features, LLVMCodeGenOptLevel @Level, LLVMRelocMode @Reloc, LLVMCodeModel @CodeModel );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeTargetMachine( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef LLVMGetTargetMachineTarget( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTriple", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMGetTargetMachineTriple( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineCPU", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMGetTargetMachineCPU( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineFeatureString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMGetTargetMachineFeatureString( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef LLVMCreateTargetDataLayout( LLVMTargetMachineRef @T );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetTargetMachineAsmVerbosity", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetTargetMachineAsmVerbosity( LLVMTargetMachineRef @T, [MarshalAs( UnmanagedType.Bool )]bool @VerboseAsm );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMTargetMachineEmitToFile( LLVMTargetMachineRef @T, LLVMModuleRef @M, string @Filename, LLVMCodeGenFileType @codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMTargetMachineEmitToMemoryBuffer( LLVMTargetMachineRef @T, LLVMModuleRef @M, LLVMCodeGenFileType @codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @ErrorMessage, out LLVMMemoryBufferRef @OutMemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDefaultTargetTriple", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetDefaultTargetTriple( );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAnalysisPasses", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddAnalysisPasses( LLVMTargetMachineRef @T, LLVMPassManagerRef @PM );
        #endregion

        #region ExecutionEngine/JIT
        /*[DllImport( LibraryPath, EntryPoint = "LLVMLinkInMCJIT", CallingConvention = CallingConvention.Cdecl )]
        //internal static extern void LLVMLinkInMCJIT( );

        //[DllImport( LibraryPath, EntryPoint = "LLVMLinkInInterpreter", CallingConvention = CallingConvention.Cdecl )]
        //internal static extern void LLVMLinkInInterpreter( );
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateGenericValueOfInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef LLVMCreateGenericValueOfInt( LLVMTypeRef @Ty, ulong @N, [MarshalAs( UnmanagedType.Bool )]bool @IsSigned );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateGenericValueOfPointer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef LLVMCreateGenericValueOfPointer( IntPtr @P );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateGenericValueOfFloat", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef LLVMCreateGenericValueOfFloat( LLVMTypeRef @Ty, double @N );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueIntWidth", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGenericValueIntWidth( LLVMGenericValueRef @GenValRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueToInt", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMGenericValueToInt( LLVMGenericValueRef @GenVal, [MarshalAs( UnmanagedType.Bool )]bool @IsSigned );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueToPointer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGenericValueToPointer( LLVMGenericValueRef @GenVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMGenericValueToFloat", CallingConvention = CallingConvention.Cdecl )]
        internal static extern double LLVMGenericValueToFloat( LLVMTypeRef @TyRef, LLVMGenericValueRef @GenVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeGenericValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeGenericValue( IntPtr @GenVal );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateExecutionEngineForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMCreateExecutionEngineForModule( out LLVMExecutionEngineRef @OutEE, LLVMModuleRef @M, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateInterpreterForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMCreateInterpreterForModule( out LLVMExecutionEngineRef @OutInterp, LLVMModuleRef @M, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateJITCompilerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMCreateJITCompilerForModule( out LLVMExecutionEngineRef @OutJIT, LLVMModuleRef @M, uint @OptLevel, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMCJITCompilerOptions", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeMCJITCompilerOptions( out LLVMMCJITCompilerOptions @Options, size_t @SizeOfOptions );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateMCJITCompilerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMCreateMCJITCompilerForModule( out LLVMExecutionEngineRef @OutJIT, LLVMModuleRef @M, out LLVMMCJITCompilerOptions @Options, size_t @SizeOfOptions, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeExecutionEngine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeExecutionEngine( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunStaticConstructors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMRunStaticConstructors( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunStaticDestructors", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMRunStaticDestructors( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunFunctionAsMain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern int LLVMRunFunctionAsMain( LLVMExecutionEngineRef @EE, LLVMValueRef @F, uint @ArgC, string[ ] @ArgV, string[ ] @EnvP );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunFunction", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMGenericValueRef LLVMRunFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @F, uint @NumArgs, out LLVMGenericValueRef @Args );

        /* As of LLVM 5, at least this is an empty function in the LLVM-C API
        //[DllImport( LibraryPath, EntryPoint = "LLVMFreeMachineCodeForFunction", CallingConvention = CallingConvention.Cdecl )]
        //internal static extern void LLVMFreeMachineCodeForFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @F );
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMAddModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddModule( LLVMExecutionEngineRef @EE, LLVMModuleRef @M );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMRemoveModule( LLVMExecutionEngineRef @EE, LLVMModuleRef @M, out LLVMModuleRef @OutMod, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string @OutError );

        [DllImport( LibraryPath, EntryPoint = "LLVMFindFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMFindFunction( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name, out LLVMValueRef @OutFn );

        /* As of at least LLVM 4.0.1 this just returns null
        //[DllImport( libraryPath, EntryPoint = "LLVMRecompileAndRelinkFunction", CallingConvention = CallingConvention.Cdecl )]
        //internal static extern IntPtr LLVMRecompileAndRelinkFunction( LLVMExecutionEngineRef @EE, LLVMValueRef @Fn );
        */

        [DllImport( LibraryPath, EntryPoint = "LLVMGetExecutionEngineTargetData", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataAlias LLVMGetExecutionEngineTargetData( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetExecutionEngineTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetMachineAlias LLVMGetExecutionEngineTargetMachine( LLVMExecutionEngineRef @EE );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobalMapping", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddGlobalMapping( LLVMExecutionEngineRef @EE, LLVMValueRef @Global, IntPtr @Addr );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetPointerToGlobal", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetPointerToGlobal( LLVMExecutionEngineRef @EE, LLVMValueRef @Global );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalValueAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern UInt64 LLVMGetGlobalValueAddress( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFunctionAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern UInt64 LLVMGetFunctionAddress( LLVMExecutionEngineRef @EE, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateSimpleMCJITMemoryManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMCJITMemoryManagerRef LLVMCreateSimpleMCJITMemoryManager( IntPtr @Opaque, LLVMMemoryManagerAllocateCodeSectionCallback @AllocateCodeSection, LLVMMemoryManagerAllocateDataSectionCallback @AllocateDataSection, LLVMMemoryManagerFinalizeMemoryCallback @FinalizeMemory, LLVMMemoryManagerDestroyCallback @Destroy );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeMCJITMemoryManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeMCJITMemoryManager( IntPtr @MM );
        #endregion

        #region Optimization/Passes
        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTransformUtils", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeTransformUtils( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeScalarOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeScalarOpts( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeObjCARCOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeObjCARCOpts( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeVectorization", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeVectorization( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstCombine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeInstCombine( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPO", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeIPO( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstrumentation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeInstrumentation( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAnalysis", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAnalysis( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPA", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeIPA( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCodeGen", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeCodeGen( LLVMPassRegistryRef @R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeTarget( LLVMPassRegistryRef @R );
        #endregion

        #region Object file Manipulation support
        [DllImport( LibraryPath, EntryPoint = "LLVMCreateObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMObjectFileRef LLVMCreateObjectFile( LLVMMemoryBufferRef @MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeObjectFile( LLVMObjectFileRef @ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSections", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSectionIteratorRef LLVMGetSections( LLVMObjectFileRef @ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSectionIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeSectionIterator( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsSectionIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsSectionIteratorAtEnd( LLVMObjectFileRef @ObjectFile, LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToNextSection( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToContainingSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToContainingSection( LLVMSectionIteratorRef @Sect, LLVMSymbolIteratorRef @Sym );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbols", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef LLVMGetSymbols( LLVMObjectFileRef @ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSymbolIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeSymbolIterator( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsSymbolIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsSymbolIteratorAtEnd( LLVMObjectFileRef @ObjectFile, LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToNextSymbol( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetSectionName( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSectionSize( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContents", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetSectionContents( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSectionAddress( LLVMSectionIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContainsSymbol", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMGetSectionContainsSymbol( LLVMSectionIteratorRef @SI, LLVMSymbolIteratorRef @Sym );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocations", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMRelocationIteratorRef LLVMGetRelocations( LLVMSectionIteratorRef @Section );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeRelocationIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeRelocationIterator( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsRelocationIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsRelocationIteratorAtEnd( LLVMSectionIteratorRef @Section, LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextRelocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToNextRelocation( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetSymbolName( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSymbolAddress( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSymbolSize( LLVMSymbolIteratorRef @SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationOffset", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetRelocationOffset( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef LLVMGetRelocationSymbol( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetRelocationType( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationTypeName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetRelocationTypeName( LLVMRelocationIteratorRef @RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationValueString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetRelocationValueString( LLVMRelocationIteratorRef @RI );
        #endregion
    }
}

#pragma warning restore SA1600 // Elements must be documented
