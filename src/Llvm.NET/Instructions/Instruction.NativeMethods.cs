// <copyright file="Instruction.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;
using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET.Instructions
{
    /// <summary>LLVM Instruction opcodes</summary>
    public partial class Instruction
    {
        internal static new class NativeMethods
        {
            internal enum LLVMAttributeIndex
            {
                LLVMAttributeReturnIndex = 0,
                LLVMAttributeFunctionIndex = -1
            }

#pragma warning disable CA1008 // Enums should have zero value.
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

            [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionParent", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetInstructionParent( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextInstruction", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetNextInstruction( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousInstruction", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetPreviousInstruction( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMInstructionRemoveFromParent", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInstructionRemoveFromParent( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMInstructionEraseFromParent", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInstructionEraseFromParent( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionOpcode", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMOpcode LLVMGetInstructionOpcode( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetICmpPredicate", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMIntPredicate LLVMGetICmpPredicate( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFCmpPredicate", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMRealPredicate LLVMGetFCmpPredicate( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMInstructionClone", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMInstructionClone( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNumArgOperands", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetNumArgOperands( LLVMValueRef Instr );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetInstructionCallConv( LLVMValueRef Instr, uint CC );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetInstructionCallConv", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetInstructionCallConv( LLVMValueRef Instr );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetInstrParamAlignment", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetInstrParamAlignment( LLVMValueRef Instr, uint index, uint Align );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddCallSiteAttribute", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddCallSiteAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, LLVMAttributeRef A );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteAttributeCount", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetCallSiteAttributeCount( LLVMValueRef C, LLVMAttributeIndex Idx );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteAttributes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMGetCallSiteAttributes( LLVMValueRef C, LLVMAttributeIndex Idx, out LLVMAttributeRef attributes );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMAttributeRef LLVMGetCallSiteEnumAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, uint KindID );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMAttributeRef LLVMGetCallSiteStringAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, [MarshalAs( UnmanagedType.LPStr )] string K, uint KLen );

            [DllImport( LibraryPath, EntryPoint = "LLVMRemoveCallSiteEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMRemoveCallSiteEnumAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, uint KindID );

            [DllImport( LibraryPath, EntryPoint = "LLVMRemoveCallSiteStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMRemoveCallSiteStringAttribute( LLVMValueRef C, LLVMAttributeIndex Idx, [MarshalAs( UnmanagedType.LPStr )] string K, uint KLen );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCalledValue", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetCalledValue( LLVMValueRef Instr );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsTailCall", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsTailCall( LLVMValueRef CallInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetTailCall", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetTailCall( LLVMValueRef CallInst, [MarshalAs( UnmanagedType.Bool )]bool isTailCall );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNormalDest", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetNormalDest( LLVMValueRef InvokeInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetUnwindDest( LLVMValueRef InvokeInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetNormalDest", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetNormalDest( LLVMValueRef InvokeInst, LLVMBasicBlockRef B );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetUnwindDest", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetUnwindDest( LLVMValueRef InvokeInst, LLVMBasicBlockRef B );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNumSuccessors", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetNumSuccessors( LLVMValueRef Term );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSuccessor", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetSuccessor( LLVMValueRef Term, uint i );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetSuccessor", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetSuccessor( LLVMValueRef Term, uint i, LLVMBasicBlockRef block );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsConditional", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsConditional( LLVMValueRef Branch );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCondition", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetCondition( LLVMValueRef Branch );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetCondition", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetCondition( LLVMValueRef Branch, LLVMValueRef Cond );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSwitchDefaultDest", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetSwitchDefaultDest( LLVMValueRef SwitchInstr );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetAllocatedType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMGetAllocatedType( LLVMValueRef Alloca );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsInBounds", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsInBounds( LLVMValueRef GEP );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetIsInBounds", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetIsInBounds( LLVMValueRef GEP, [MarshalAs( UnmanagedType.Bool )]bool InBounds );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddIncoming", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddIncoming( LLVMValueRef PhiNode, out LLVMValueRef IncomingValues, out LLVMBasicBlockRef IncomingBlocks, uint Count );

            [DllImport( LibraryPath, EntryPoint = "LLVMCountIncoming", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMCountIncoming( LLVMValueRef PhiNode );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetIncomingValue", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetIncomingValue( LLVMValueRef PhiNode, uint Index );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetIncomingBlock", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetIncomingBlock( LLVMValueRef PhiNode, uint Index );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNumIndices", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetNumIndices( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetIndices", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetIndices( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddCase", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddCase( LLVMValueRef Switch, LLVMValueRef OnVal, LLVMBasicBlockRef Dest );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddDestination", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddDestination( LLVMValueRef IndirectBr, LLVMBasicBlockRef Dest );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNumClauses", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetNumClauses( LLVMValueRef LandingPad );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetClause", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetClause( LLVMValueRef LandingPad, uint Idx );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddClause", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddClause( LLVMValueRef LandingPad, LLVMValueRef ClauseVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsCleanup", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsCleanup( LLVMValueRef LandingPad );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetCleanup", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetCleanup( LLVMValueRef LandingPad, [MarshalAs( UnmanagedType.Bool )]bool Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetVolatile", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMGetVolatile( LLVMValueRef MemoryAccessInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetVolatile", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetVolatile( LLVMValueRef MemoryAccessInst, [MarshalAs( UnmanagedType.Bool )]bool IsVolatile );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetOrdering", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMAtomicOrdering LLVMGetOrdering( LLVMValueRef MemoryAccessInst );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetOrdering", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetOrdering( LLVMValueRef MemoryAccessInst, LLVMAtomicOrdering Ordering );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern UInt32 LLVMLookupInstrinsicId( [MarshalAs( UnmanagedType.LPStr )] string name );
        }
    }
}
