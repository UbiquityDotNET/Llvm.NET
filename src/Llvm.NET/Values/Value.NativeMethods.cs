// <copyright file="Value.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using static Llvm.NET.Instructions.Instruction.NativeMethods;
using static Llvm.NET.Native.NativeMethods;
using CC = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Value</summary>
    public partial class Value
    {
        internal static class NativeMethods
        {
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

            // Retrieves the raw underlying native C++ ValueKind enumeration for a value
            // This is generally only used in the mapping of an LLVMValueRef to the Llvm.NET
            // instance wrapping it. The Stable C API uses a distinct enum for the instruction
            // codes, they don't actually match the underlying C++ kind and actually overlap
            // it in incompatible ways. So, this uses the underlying enum to build up the
            // correct .NET types for a given LLVMValueRef.
            internal static ValueKind LLVMGetValueIdAsKind( LLVMValueRef valueRef ) => ( ValueKind )LLVMGetValueID( valueRef );

            [DllImport( LibraryPath, EntryPoint = "LLVMTypeOf", CallingConvention = CC.Cdecl )]
            internal static extern LLVMTypeRef LLVMTypeOf( LLVMValueRef Val );

            /* excluded in favor of custom version that redirects to GetValueId
            // [DllImport(libraryPath, EntryPoint = "LLVMGetValueKind", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
            // internal static extern LLVMValueKind LLVMGetValueKindLLVMValueRef( @Val);
            */

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern int LLVMGetValueID( LLVMValueRef val );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMComdatRef LLVMGlobalObjectGetComdat( LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMGlobalObjectSetComdat( LLVMValueRef Val, LLVMComdatRef comdatRef );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsConstantZeroValue( LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMRemoveGlobalFromParent( LLVMValueRef /*GlobalVariable*/ Val );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMConstantAsMetadata( LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMSetMetadata2( LLVMValueRef Inst, UInt32 KindID, LLVMMetadataRef MD );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetParamAlignment", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetParamAlignment( LLVMValueRef Arg, uint Align );

            [DllImport( LibraryPath, EntryPoint = "LLVMHasMetadata", CallingConvention = CC.Cdecl )]
            internal static extern int LLVMHasMetadata( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetMetadata", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetMetadata( LLVMValueRef Val, uint KindID );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetMetadata", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetMetadata( LLVMValueRef Val, uint KindID, LLVMValueRef Node );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAtomicSingleThread", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsAtomicSingleThread( LLVMValueRef AtomicInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetAtomicSingleThread", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetAtomicSingleThread( LLVMValueRef AtomicInst, [MarshalAs( UnmanagedType.Bool )]bool SingleThread );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCmpXchgSuccessOrdering", CallingConvention = CC.Cdecl )]
            internal static extern LLVMAtomicOrdering LLVMGetCmpXchgSuccessOrdering( LLVMValueRef CmpXchgInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetCmpXchgSuccessOrdering", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetCmpXchgSuccessOrdering( LLVMValueRef CmpXchgInst, LLVMAtomicOrdering Ordering );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCmpXchgFailureOrdering", CallingConvention = CC.Cdecl )]
            internal static extern LLVMAtomicOrdering LLVMGetCmpXchgFailureOrdering( LLVMValueRef CmpXchgInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetCmpXchgFailureOrdering", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetCmpXchgFailureOrdering( LLVMValueRef CmpXchgInst, LLVMAtomicOrdering Ordering );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetValueName", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetValueName( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetValueName", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMSetValueName( LLVMValueRef Val, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMDumpValue", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMDumpValue( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMPrintValueToString", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMPrintValueToString( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMReplaceAllUsesWith", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMReplaceAllUsesWith( LLVMValueRef OldVal, LLVMValueRef NewVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsConstant", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsConstant( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsUndef", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsUndef( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAArgument", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAArgument( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsABasicBlock", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsABasicBlock( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAInlineAsm", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAInlineAsm( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAUser", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAUser( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstant", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstant( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsABlockAddress", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsABlockAddress( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantAggregateZero", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantAggregateZero( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantArray", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantArray( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataSequential", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantDataSequential( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataArray", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantDataArray( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantDataVector", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantDataVector( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantExpr", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantExpr( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantFP", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantFP( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantInt", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantInt( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantPointerNull", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantPointerNull( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantStruct", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantStruct( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantTokenNone", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantTokenNone( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAConstantVector", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAConstantVector( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalValue", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAGlobalValue( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalAlias", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAGlobalAlias( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalObject", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAGlobalObject( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAFunction", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAFunction( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAGlobalVariable", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAGlobalVariable( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAUndefValue", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAUndefValue( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAInstruction", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAInstruction( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsABinaryOperator", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsABinaryOperator( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsACallInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsACallInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAIntrinsicInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAIntrinsicInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsADbgInfoIntrinsic", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsADbgInfoIntrinsic( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsADbgDeclareInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsADbgDeclareInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemIntrinsic", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAMemIntrinsic( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemCpyInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAMemCpyInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemMoveInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAMemMoveInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAMemSetInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAMemSetInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsACmpInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsACmpInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAFCmpInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAFCmpInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAICmpInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAICmpInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAExtractElementInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAExtractElementInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAGetElementPtrInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAGetElementPtrInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAInsertElementInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAInsertElementInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAInsertValueInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAInsertValueInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsALandingPadInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsALandingPadInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAPHINode", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAPHINode( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsASelectInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsASelectInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAShuffleVectorInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAShuffleVectorInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAStoreInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAStoreInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsATerminatorInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsATerminatorInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsABranchInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsABranchInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAIndirectBrInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAIndirectBrInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAInvokeInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAInvokeInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAReturnInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAReturnInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsASwitchInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsASwitchInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAUnreachableInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAUnreachableInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAResumeInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAResumeInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsACleanupReturnInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsACleanupReturnInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsACatchReturnInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsACatchReturnInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAFuncletPadInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAFuncletPadInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsACatchPadInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsACatchPadInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsACleanupPadInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsACleanupPadInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAUnaryInstruction", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAUnaryInstruction( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAAllocaInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAAllocaInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsACastInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsACastInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAAddrSpaceCastInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAAddrSpaceCastInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsABitCastInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsABitCastInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPExtInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAFPExtInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPToSIInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAFPToSIInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPToUIInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAFPToUIInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAFPTruncInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAFPTruncInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAIntToPtrInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAIntToPtrInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAPtrToIntInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAPtrToIntInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsASExtInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsASExtInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsASIToFPInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsASIToFPInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsATruncInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsATruncInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAUIToFPInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAUIToFPInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAZExtInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAZExtInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAExtractValueInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAExtractValueInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsALoadInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsALoadInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAVAArgInst", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAVAArgInst( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAMDNode", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAMDNode( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsAMDString", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMIsAMDString( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetUser", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetUser( LLVMUseRef U );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetUsedValue", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetUsedValue( LLVMUseRef U );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetOperandUse", CallingConvention = CC.Cdecl )]
            internal static extern LLVMUseRef LLVMGetOperandUse( LLVMValueRef Val, uint Index );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetAsString", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetAsString( LLVMValueRef c, out size_t Length );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsNull", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsNull( LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl )]
            internal static extern LLVMStatus LVMVerifyFunction( LLVMValueRef Fn, LLVMVerifierFailureAction Action );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl )]
            internal static extern void LLVMViewFunctionCFG( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMViewFunctionCFGOnly", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMViewFunctionCFGOnly( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMBlockAddress", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMBlockAddress( LLVMValueRef F, LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalParent", CallingConvention = CC.Cdecl )]
            internal static extern LLVMModuleRef LLVMGetGlobalParent( LLVMValueRef Global );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsDeclaration", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsDeclaration( LLVMValueRef Global );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLinkage", CallingConvention = CC.Cdecl )]
            internal static extern LLVMLinkage LLVMGetLinkage( LLVMValueRef Global );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetLinkage", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetLinkage( LLVMValueRef Global, LLVMLinkage Linkage );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSection", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetSection( LLVMValueRef Global );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetSection", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMSetSection( LLVMValueRef Global, [MarshalAs( UnmanagedType.LPStr )] string Section );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetVisibility", CallingConvention = CC.Cdecl )]
            internal static extern LLVMVisibility LLVMGetVisibility( LLVMValueRef Global );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetVisibility", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetVisibility( LLVMValueRef Global, LLVMVisibility Viz );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetDLLStorageClass", CallingConvention = CC.Cdecl )]
            internal static extern LLVMDLLStorageClass LLVMGetDLLStorageClass( LLVMValueRef Global );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetDLLStorageClass", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetDLLStorageClass( LLVMValueRef Global, LLVMDLLStorageClass Class );

            [DllImport( LibraryPath, EntryPoint = "LLVMHasUnnamedAddr", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMHasUnnamedAddr( LLVMValueRef Global );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetUnnamedAddr", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetUnnamedAddr( LLVMValueRef Global, [MarshalAs( UnmanagedType.Bool )]bool hasUnnamedAddr );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetAlignment", CallingConvention = CC.Cdecl )]
            internal static extern uint LLVMGetAlignment( LLVMValueRef V );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetAlignment", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetAlignment( LLVMValueRef V, uint Bytes );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetInitializer", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetInitializer( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetInitializer", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetInitializer( LLVMValueRef GlobalVar, LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsThreadLocal", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsThreadLocal( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetThreadLocal", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetThreadLocal( LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isThreadLocal );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsGlobalConstant", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsGlobalConstant( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetGlobalConstant", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetGlobalConstant( LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool isConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetThreadLocalMode", CallingConvention = CC.Cdecl )]
            internal static extern LLVMThreadLocalMode LLVMGetThreadLocalMode( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetThreadLocalMode", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetThreadLocalMode( LLVMValueRef GlobalVar, LLVMThreadLocalMode Mode );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsExternallyInitialized", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsExternallyInitialized( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetExternallyInitialized", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetExternallyInitialized( LLVMValueRef GlobalVar, [MarshalAs( UnmanagedType.Bool )]bool IsExtInit );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMSetDebugLoc( LLVMValueRef inst, UInt32 line, UInt32 column, LLVMMetadataRef scope );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMSetDILocation( LLVMValueRef inst, LLVMMetadataRef location );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl )]
            internal static extern void LLVMGlobalVariableAddDebugExpression( LLVMValueRef variable, LLVMMetadataRef metadataHandle );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMUseRef LLVMGetFirstUse( LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMUseRef LLVMGetNextUse( LLVMUseRef U );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetOperand( LLVMValueRef Val, uint Index );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMSetOperand( LLVMValueRef User, uint Index, LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern int LLVMGetNumOperands( LLVMValueRef Val );
        }
    }
}
