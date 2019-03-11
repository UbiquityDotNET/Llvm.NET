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
        }
    }
}
