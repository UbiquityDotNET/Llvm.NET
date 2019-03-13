// <copyright file="ScalarTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>Utility class for Adding Scalar transform passes to a <see cref="PassManager"/></summary>
    public static partial class ScalarTransforms
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMAddAggressiveDCEPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddAggressiveDCEPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddBitTrackingDCEPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddBitTrackingDCEPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddAlignmentFromAssumptionsPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddAlignmentFromAssumptionsPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddCFGSimplificationPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddCFGSimplificationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLateCFGSimplificationPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLateCFGSimplificationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddDeadStoreEliminationPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddDeadStoreEliminationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarizerPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddScalarizerPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddMergedLoadStoreMotionPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddMergedLoadStoreMotionPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddGVNPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddGVNPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddNewGVNPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddNewGVNPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddIndVarSimplifyPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddIndVarSimplifyPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddInstructionCombiningPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddInstructionCombiningPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddJumpThreadingPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddJumpThreadingPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLICMPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLICMPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopDeletionPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLoopDeletionPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopIdiomPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLoopIdiomPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopRotatePass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLoopRotatePass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopRerollPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLoopRerollPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopUnrollPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLoopUnrollPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopUnswitchPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLoopUnswitchPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddMemCpyOptPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddMemCpyOptPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddPartiallyInlineLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddPartiallyInlineLibCallsPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLowerSwitchPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLowerSwitchPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddPromoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddPromoteMemoryToRegisterPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddReassociatePass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddReassociatePass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddSCCPPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddSCCPPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddScalarReplAggregatesPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassSSA", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddScalarReplAggregatesPassSSA( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassWithThreshold", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddScalarReplAggregatesPassWithThreshold( LLVMPassManagerRef PM, int Threshold );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddSimplifyLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddSimplifyLibCallsPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddTailCallEliminationPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddTailCallEliminationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddConstantPropagationPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddConstantPropagationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddDemoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddDemoteMemoryToRegisterPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddVerifierPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddVerifierPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddCorrelatedValuePropagationPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddCorrelatedValuePropagationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddEarlyCSEPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddEarlyCSEPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddEarlyCSEMemSSAPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddEarlyCSEMemSSAPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddLowerExpectIntrinsicPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLowerExpectIntrinsicPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddTypeBasedAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddTypeBasedAliasAnalysisPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddScopedNoAliasAAPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddScopedNoAliasAAPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddBasicAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddBasicAliasAnalysisPass( LLVMPassManagerRef PM );
        }
    }
}
