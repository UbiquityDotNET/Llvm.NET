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
    internal delegate IntPtr LLVMMemoryManagerAllocateCodeSectionCallback( IntPtr opaque, int size, uint alignment, uint sectionID, [MarshalAs( UnmanagedType.LPStr )] string sectionName );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate IntPtr LLVMMemoryManagerAllocateDataSectionCallback( IntPtr opaque, int size, uint alignment, uint sectionID, [MarshalAs( UnmanagedType.LPStr )] string sectionName, int isReadOnly );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate int LLVMMemoryManagerFinalizeMemoryCallback( IntPtr opaque, out IntPtr errMsg );

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    internal delegate void LLVMMemoryManagerDestroyCallback( IntPtr opaque );

    internal enum LLVMVerifierFailureAction
    {
        LLVMAbortProcessAction = 0,
        LLVMPrintMessageAction = 1,
        LLVMReturnStatusAction = 2
    }

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

    #region LTO
    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
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
    #endregion

    internal static partial class NativeMethods
    {
        internal const string LibraryPath = "libLLVM";

        #region Misc...
        /*[DllImport( LibraryPath, EntryPoint = "LLVMSearchForAddressOfSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        //internal static extern IntPtr LLVMSearchForAddressOfSymbol( [MarshalAs( UnmanagedType.LPStr )] string symbolName );

        //[DllImport( LibraryPath, EntryPoint = "LLVMAddSymbol", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        //internal static extern void LLVMAddSymbol( [MarshalAs( UnmanagedType.LPStr )] string symbolName, IntPtr symbolValue );
        */
        #endregion
    }
}
