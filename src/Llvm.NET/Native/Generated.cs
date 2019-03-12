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
        internal int Present;
        [MarshalAs( UnmanagedType.LPStr )]
        internal string Name;
        internal int Value;
    }

    internal struct LLVMOpInfo1
    {
        internal LLVMOpInfoSymbol1 AddSymbol;
        internal LLVMOpInfoSymbol1 SubtractSymbol;
        internal int Value;
        internal int VariantKind;
    }

    internal struct LLVMMCJITCompilerOptions
    {
        internal uint OptLevel;
        internal LLVMCodeModel CodeModel;
        internal int NoFramePointerElim;
        internal int EnableFastISel;
        internal LLVMMCJITMemoryManagerRef MCJMM;
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
    internal delegate void LLVMDiagnosticHandler( LLVMDiagnosticInfoRef param0, IntPtr param1 );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMYieldCallback( LLVMContextRef param0, IntPtr param1 );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate int LLVMOpInfoCallback( IntPtr disInfo, int pc, int offset, int size, int tagType, IntPtr tagBuf );

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
    internal delegate void lto_diagnostic_handler_t( lto_codegen_diagnostic_severity_t severity, [MarshalAs( UnmanagedType.LPStr )] string diag, IntPtr ctxt );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate ulong LLVMOrcLazyCompileCallbackFn( LLVMOrcJITStackRef jitStack, IntPtr callbackCtx );

    internal enum LLVMVerifierFailureAction
    {
        LLVMAbortProcessAction = 0,
        LLVMPrintMessageAction = 1,
        LLVMReturnStatusAction = 2
    }

#pragma warning disable CA1008 // Enums should have zero value.
    internal enum LLVMAttribute
    {
        LLVMZExtAttribute = 1,
        LLVMSExtAttribute = 2,
        LLVMNoReturnAttribute = 4,
        LLVMInRegAttribute = 8,
        LLVMStructRetAttribute = 16,
        LLVMNoUnwindAttribute = 32,
        LLVMNoAliasAttribute = 64,
        LLVMByValAttribute = 128,
        LLVMNestAttribute = 256,
        LLVMReadNoneAttribute = 512,
        LLVMReadOnlyAttribute = 1024,
        LLVMNoInlineAttribute = 2048,
        LLVMAlwaysInlineAttribute = 4096,
        LLVMOptimizeForSizeAttribute = 8192,
        LLVMStackProtectAttribute = 16384,
        LLVMStackProtectReqAttribute = 32768,
        LLVMAlignment = 2031616,
        LLVMNoCaptureAttribute = 2097152,
        LLVMNoRedZoneAttribute = 4194304,
        LLVMNoImplicitFloatAttribute = 8388608,
        LLVMNakedAttribute = 16777216,
        LLVMInlineHintAttribute = 33554432,
        LLVMStackAlignment = 469762048,
        LLVMReturnsTwice = 536870912,
        LLVMUWTable = 1073741824,
        LLVMNonLazyBind = -2147483648
    }

    internal enum LLVMOpcode
    {
        LLVMRet = 1,
        LLVMBr = 2,
        LLVMSwitch = 3,
        LLVMIndirectBr = 4,
        LLVMInvoke = 5,
        LLVMUnreachable = 7,
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
        LLVMFence = 55,
        LLVMAtomicCmpXchg = 56,
        LLVMAtomicRMW = 57,
        LLVMResume = 58,
        LLVMLandingPad = 59,
        LLVMCleanupRet = 61,
        LLVMCatchRet = 62,
        LLVMCatchPad = 63,
        LLVMCleanupPad = 64,
        LLVMCatchSwitch = 65
    }
#pragma warning restore CA1008 // Enums should have zero value.

    internal enum LLVMTypeKind
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
        LLVMX86_MMXTypeKind = 15,
        LLVMTokenTypeKind = 16
    }

    internal enum LLVMLinkage
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
        LLVMLinkerPrivateWeakLinkage = 16
    }

    internal enum LLVMVisibility
    {
        LLVMDefaultVisibility = 0,
        LLVMHiddenVisibility = 1,
        LLVMProtectedVisibility = 2
    }

    internal enum LLVMDLLStorageClass
    {
        LLVMDefaultStorageClass = 0,
        LLVMDLLImportStorageClass = 1,
        LLVMDLLExportStorageClass = 2
    }

    internal enum LLVMCallConv
    {
        LLVMCCallConv = 0,
        LLVMFastCallConv = 8,
        LLVMColdCallConv = 9,
        LLVMWebKitJSCallConv = 12,
        LLVMAnyRegCallConv = 13,
        LLVMX86StdcallCallConv = 64,
        LLVMX86FastcallCallConv = 65
    }

    internal enum LLVMValueKind
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
        LLVMInstructionValueKind = 24
    }

#pragma warning disable CA1008 // Enums should have zero value.
    internal enum LLVMIntPredicate
    {
        LLVMIntEQ = 32,
        LLVMIntNE = 33,
        LLVMIntUGT = 34,
        LLVMIntUGE = 35,
        LLVMIntULT = 36,
        LLVMIntULE = 37,
        LLVMIntSGT = 38,
        LLVMIntSGE = 39,
        LLVMIntSLT = 40,
        LLVMIntSLE = 41
    }
#pragma warning restore CA1008 // Enums should have zero value.

    internal enum LLVMRealPredicate
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
        LLVMRealPredicateTrue = 15
    }

    internal enum LLVMLandingPadClauseTy
    {
        LLVMLandingPadCatch = 0,
        LLVMLandingPadFilter = 1
    }

    internal enum LLVMThreadLocalMode
    {
        LLVMNotThreadLocal = 0,
        LLVMGeneralDynamicTLSModel = 1,
        LLVMLocalDynamicTLSModel = 2,
        LLVMInitialExecTLSModel = 3,
        LLVMLocalExecTLSModel = 4
    }

    internal enum LLVMAtomicOrdering
    {
        LLVMAtomicOrderingNotAtomic = 0,
        LLVMAtomicOrderingUnordered = 1,
        LLVMAtomicOrderingMonotonic = 2,
        LLVMAtomicOrderingAcquire = 4,
        LLVMAtomicOrderingRelease = 5,
        LLVMAtomicOrderingAcquireRelease = 6,
        LLVMAtomicOrderingSequentiallyConsistent = 7
    }

    internal enum LLVMAtomicRMWBinOp
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
        LLVMAtomicRMWBinOpUMin = 10
    }

    internal enum LLVMDiagnosticSeverity
    {
        LLVMDSError = 0,
        LLVMDSWarning = 1,
        LLVMDSRemark = 2,
        LLVMDSNote = 3
    }

    internal enum LLVMAttributeIndex
    {
        LLVMAttributeReturnIndex = 0,
        LLVMAttributeFunctionIndex = -1
    }

    internal enum LLVMByteOrdering
    {
        LLVMBigEndian = 0,
        LLVMLittleEndian = 1
    }

    internal enum LLVMCodeGenOptLevel
    {
        LLVMCodeGenLevelNone = 0,
        LLVMCodeGenLevelLess = 1,
        LLVMCodeGenLevelDefault = 2,
        LLVMCodeGenLevelAggressive = 3
    }

    internal enum LLVMRelocMode
    {
        LLVMRelocDefault = 0,
        LLVMRelocStatic = 1,
        LLVMRelocPIC = 2,
        LLVMRelocDynamicNoPic = 3
    }

    internal enum LLVMCodeModel
    {
        LLVMCodeModelDefault = 0,
        LLVMCodeModelJITDefault = 1,
        LLVMCodeModelSmall = 2,
        LLVMCodeModelKernel = 3,
        LLVMCodeModelMedium = 4,
        LLVMCodeModelLarge = 5
    }

    internal enum LLVMCodeGenFileType
    {
        LLVMAssemblyFile = 0,
        LLVMObjectFile = 1
    }

    internal enum LLVMLinkerMode
    {
        LLVMLinkerDestroySource = 0,
        LLVMLinkerPreserveSource_Removed = 1
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum llvm_lto_status
    {
        LLVM_LTO_UNKNOWN = 0,
        LLVM_LTO_OPT_SUCCESS = 1,
        LLVM_LTO_READ_SUCCESS = 2,
        LLVM_LTO_READ_FAILURE = 3,
        LLVM_LTO_WRITE_FAILURE = 4,
        LLVM_LTO_NO_TARGET = 5,
        LLVM_LTO_NO_WORK = 6,
        LLVM_LTO_MODULE_MERGE_FAILURE = 7,
        LLVM_LTO_ASM_FAILURE = 8,
        LLVM_LTO_NULL_OBJECT = 9
    }

#pragma warning disable CA1008 // Enums should have zero value.
    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_symbol_attributes
    {
        LTO_SYMBOL_ALIGNMENT_MASK = 31,
        LTO_SYMBOL_PERMISSIONS_MASK = 224,
        LTO_SYMBOL_PERMISSIONS_CODE = 160,
        LTO_SYMBOL_PERMISSIONS_DATA = 192,
        LTO_SYMBOL_PERMISSIONS_RODATA = 128,
        LTO_SYMBOL_DEFINITION_MASK = 1792,
        LTO_SYMBOL_DEFINITION_REGULAR = 256,
        LTO_SYMBOL_DEFINITION_TENTATIVE = 512,
        LTO_SYMBOL_DEFINITION_WEAK = 768,
        LTO_SYMBOL_DEFINITION_UNDEFINED = 1024,
        LTO_SYMBOL_DEFINITION_WEAKUNDEF = 1280,
        LTO_SYMBOL_SCOPE_MASK = 14336,
        LTO_SYMBOL_SCOPE_INTERNAL = 2048,
        LTO_SYMBOL_SCOPE_HIDDEN = 4096,
        LTO_SYMBOL_SCOPE_PROTECTED = 8192,
        LTO_SYMBOL_SCOPE_DEFAULT = 6144,
        LTO_SYMBOL_SCOPE_DEFAULT_CAN_BE_HIDDEN = 10240,
        LTO_SYMBOL_COMDAT = 16384,
        LTO_SYMBOL_ALIAS = 32768
    }
#pragma warning restore CA1008 // Enums should have zero value.

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_debug_model
    {
        LTO_DEBUG_MODEL_NONE = 0,
        LTO_DEBUG_MODEL_DWARF = 1
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_codegen_model
    {
        LTO_CODEGEN_PIC_MODEL_STATIC = 0,
        LTO_CODEGEN_PIC_MODEL_DYNAMIC = 1,
        LTO_CODEGEN_PIC_MODEL_DYNAMIC_NO_PIC = 2,
        LTO_CODEGEN_PIC_MODEL_DEFAULT = 3
    }

    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
    internal enum lto_codegen_diagnostic_severity_t
    {
        LTO_DS_ERROR = 0,
        LTO_DS_WARNING = 1,
        LTO_DS_REMARK = 3,
        LTO_DS_NOTE = 2
    }

    internal enum LLVMOrcErrorCode
    {
        LLVMOrcErrSuccess = 0,
        LLVMOrcErrGeneric = 1
    }

    internal static partial class NativeMethods
    {
        internal const string LibraryPath = "libLLVM";

        #region Misc...
        [DllImport( LibraryPath, EntryPoint = "LLVMSearchForAddressOfSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern IntPtr LLVMSearchForAddressOfSymbol( [MarshalAs( UnmanagedType.LPStr )] string symbolName );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMAddSymbol( [MarshalAs( UnmanagedType.LPStr )] string symbolName, IntPtr symbolValue );

        #endregion

        #region Attributes
        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKindForName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern uint LLVMGetEnumAttributeKindForName( [MarshalAs( UnmanagedType.LPStr )] string Name, size_t SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetLastEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetLastEnumAttributeKind( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetEnumAttributeKind( LLVMAttributeRef A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        internal static extern ulong LLVMGetEnumAttributeValue( LLVMAttributeRef A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeKind", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetStringAttributeKind( LLVMAttributeRef A, out uint Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeValue", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetStringAttributeValue( LLVMAttributeRef A, out uint Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsEnumAttribute( LLVMAttributeRef A );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsStringAttribute", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsStringAttribute( LLVMAttributeRef A );
        #endregion

        #region Attributes
        [DllImport( LibraryPath, EntryPoint = "LLVMGetGC", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetGC( LLVMValueRef Fn );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetGC", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMSetGC( LLVMValueRef Fn, [MarshalAs( UnmanagedType.LPStr )] string Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, LLVMAttributeRef A );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributeCountAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern uint LLVMGetAttributeCountAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributesAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGetAttributesAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, out LLVMAttributeRef Attrs );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMAttributeRef LLVMGetEnumAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMAttributeRef LLVMGetStringAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, [MarshalAs( UnmanagedType.LPStr )] string K, uint KLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveEnumAttributeAtIndex", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMRemoveEnumAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveStringAttributeAtIndex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern void LLVMRemoveStringAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, [MarshalAs( UnmanagedType.LPStr )] string K, uint KLen );
        #endregion

        #region Pass Manager
        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalPassRegistry", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassRegistryRef LLVMGetGlobalPassRegistry( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreatePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef LLVMCreatePassManager( );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManagerForModule", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef LLVMCreateFunctionPassManagerForModule( LLVMModuleRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMPassManagerRef LLVMCreateFunctionPassManager( LLVMModuleProviderRef MP );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMRunPassManager( LLVMPassManagerRef PM, LLVMModuleRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMInitializeFunctionPassManager( LLVMPassManagerRef FPM );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMRunFunctionPassManager( LLVMPassManagerRef FPM, LLVMValueRef F );

        [DllImport( LibraryPath, EntryPoint = "LLVMFinalizeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMFinalizeFunctionPassManager( LLVMPassManagerRef FPM );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposePassManager", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposePassManager( IntPtr PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCore", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeCore( LLVMPassRegistryRef R );
        #endregion

        #region Disassembler
        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef LLVMCreateDisasm( [MarshalAs( UnmanagedType.LPStr )] string TripleName, IntPtr DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasmCPU", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef LLVMCreateDisasmCPU( [MarshalAs( UnmanagedType.LPStr )] string Triple, [MarshalAs( UnmanagedType.LPStr )] string CPU, IntPtr DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateDisasmCPUFeatures", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMDisasmContextRef LLVMCreateDisasmCPUFeatures( [MarshalAs( UnmanagedType.LPStr )] string Triple, [MarshalAs( UnmanagedType.LPStr )] string CPU, [MarshalAs( UnmanagedType.LPStr )] string Features, IntPtr DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDisasmOptions", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMSetDisasmOptions( LLVMDisasmContextRef DC, int Options );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisasmDispose", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisasmDispose( LLVMDisasmContextRef DC );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisasmInstruction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern ulong LLVMDisasmInstruction( LLVMDisasmContextRef DC, IntPtr Bytes, long BytesSize, long PC, IntPtr OutString, size_t OutStringSize );
        #endregion

        #region Targets

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef LLVMGetModuleDataLayout( LLVMModuleRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetModuleDataLayout( LLVMModuleRef M, LLVMTargetDataRef DL );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetDataRef LLVMCreateTargetData( [MarshalAs( UnmanagedType.LPStr )] string StringRep );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTargetLibraryInfo", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddTargetLibraryInfo( LLVMTargetLibraryInfoRef TLI, LLVMPassManagerRef PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef LLVMGetFirstTarget( );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNextTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef LLVMGetNextTarget( LLVMTargetRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetRef LLVMGetTargetFromName( [MarshalAs( UnmanagedType.LPStr )] string Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromTriple", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMGetTargetFromTriple( [MarshalAs( UnmanagedType.LPStr )] string Triple, out LLVMTargetRef T, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetName", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetTargetName( LLVMTargetRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetDescription", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetTargetDescription( LLVMTargetRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasJIT", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTargetHasJIT( LLVMTargetRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTargetHasTargetMachine( LLVMTargetRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasAsmBackend", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTargetHasAsmBackend( LLVMTargetRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetMachine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMTargetMachineRef LLVMCreateTargetMachine( LLVMTargetRef T, [MarshalAs( UnmanagedType.LPStr )] string Triple, [MarshalAs( UnmanagedType.LPStr )] string CPU, [MarshalAs( UnmanagedType.LPStr )] string Features, LLVMCodeGenOptLevel Level, LLVMRelocMode Reloc, LLVMCodeModel CodeModel );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeTargetMachine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeTargetMachine( LLVMTargetMachineRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetRef LLVMGetTargetMachineTarget( LLVMTargetMachineRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTriple", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMGetTargetMachineTriple( LLVMTargetMachineRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineCPU", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMGetTargetMachineCPU( LLVMTargetMachineRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineFeatureString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        internal static extern string LLVMGetTargetMachineFeatureString( LLVMTargetMachineRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetDataLayout", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMTargetDataRef LLVMCreateTargetDataLayout( LLVMTargetMachineRef T );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetTargetMachineAsmVerbosity", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMSetTargetMachineAsmVerbosity( LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )]bool VerboseAsm );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        internal static extern LLVMStatus LLVMTargetMachineEmitToFile( LLVMTargetMachineRef T, LLVMModuleRef M, string Filename, LLVMCodeGenFileType codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string ErrorMessage );

        [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMStatus LLVMTargetMachineEmitToMemoryBuffer( LLVMTargetMachineRef T, LLVMModuleRef M, LLVMCodeGenFileType codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string ErrorMessage, out LLVMMemoryBufferRef OutMemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDefaultTargetTriple", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetDefaultTargetTriple( );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAnalysisPasses", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMAddAnalysisPasses( LLVMTargetMachineRef T, LLVMPassManagerRef PM );
        #endregion

        #region Optimization/Passes
        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTransformUtils", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeTransformUtils( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeScalarOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeScalarOpts( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeObjCARCOpts", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeObjCARCOpts( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeVectorization", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeVectorization( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstCombine", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeInstCombine( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPO", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeIPO( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstrumentation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeInstrumentation( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAnalysis", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeAnalysis( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPA", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeIPA( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCodeGen", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeCodeGen( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTarget", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMInitializeTarget( LLVMPassRegistryRef R );
        #endregion

        #region Object file Manipulation support
        [DllImport( LibraryPath, EntryPoint = "LLVMCreateObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMObjectFileRef LLVMCreateObjectFile( LLVMMemoryBufferRef MemBuf );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeObjectFile", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeObjectFile( LLVMObjectFileRef ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSections", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSectionIteratorRef LLVMGetSections( LLVMObjectFileRef ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSectionIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeSectionIterator( LLVMSectionIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsSectionIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsSectionIteratorAtEnd( LLVMObjectFileRef ObjectFile, LLVMSectionIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToNextSection( LLVMSectionIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToContainingSection", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToContainingSection( LLVMSectionIteratorRef Sect, LLVMSymbolIteratorRef Sym );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbols", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef LLVMGetSymbols( LLVMObjectFileRef ObjectFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSymbolIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeSymbolIterator( LLVMSymbolIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsSymbolIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsSymbolIteratorAtEnd( LLVMObjectFileRef ObjectFile, LLVMSymbolIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToNextSymbol( LLVMSymbolIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetSectionName( LLVMSectionIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSectionSize( LLVMSectionIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContents", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetSectionContents( LLVMSectionIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSectionAddress( LLVMSectionIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContainsSymbol", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMGetSectionContainsSymbol( LLVMSectionIteratorRef SI, LLVMSymbolIteratorRef Sym );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocations", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMRelocationIteratorRef LLVMGetRelocations( LLVMSectionIteratorRef Section );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeRelocationIterator", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMDisposeRelocationIterator( LLVMRelocationIteratorRef RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsRelocationIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsRelocationIteratorAtEnd( LLVMSectionIteratorRef Section, LLVMRelocationIteratorRef RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextRelocation", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMMoveToNextRelocation( LLVMRelocationIteratorRef RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetSymbolName( LLVMSymbolIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolAddress", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSymbolAddress( LLVMSymbolIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolSize", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetSymbolSize( LLVMSymbolIteratorRef SI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationOffset", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetRelocationOffset( LLVMRelocationIteratorRef RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationSymbol", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMSymbolIteratorRef LLVMGetRelocationSymbol( LLVMRelocationIteratorRef RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationType", CallingConvention = CallingConvention.Cdecl )]
        internal static extern int LLVMGetRelocationType( LLVMRelocationIteratorRef RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationTypeName", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetRelocationTypeName( LLVMRelocationIteratorRef RI );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationValueString", CallingConvention = CallingConvention.Cdecl )]
        internal static extern IntPtr LLVMGetRelocationValueString( LLVMRelocationIteratorRef RI );
        #endregion
    }
}
