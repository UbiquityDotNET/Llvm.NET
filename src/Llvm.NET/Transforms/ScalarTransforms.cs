// <copyright file="ScalarTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>Utility class for Adding Scalar transform passses to a <see cref="PassManager"/></summary>
    public static class ScalarTransforms
    {
        public static T AddAggressiveDCEPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddAggressiveDCEPass( passManager.Handle );
            return passManager;
        }

        public static T AddBitTrackingDCEPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddBitTrackingDCEPass( passManager.Handle );
            return passManager;
        }

        public static T AddAlignmentFromAssumptionsPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddAlignmentFromAssumptionsPass( passManager.Handle );
            return passManager;
        }

        public static T AddCFGSimplificationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddCFGSimplificationPass( passManager.Handle );
            return passManager;
        }

        public static T AddLateCFGSimplificationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLateCFGSimplificationPass( passManager.Handle );
            return passManager;
        }

        public static T AddDeadStoreEliminationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddDeadStoreEliminationPass( passManager.Handle );
            return passManager;
        }

        public static T AddScalarizerPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddScalarizerPass( passManager.Handle );
            return passManager;
        }

        public static T AddMergedLoadStoreMotionPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddMergedLoadStoreMotionPass( passManager.Handle );
            return passManager;
        }

        public static T AddGVNPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddGVNPass( passManager.Handle );
            return passManager;
        }

        public static T AddNewGVNPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddNewGVNPass( passManager.Handle );
            return passManager;
        }

        public static T AddIndVarSimplifyPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddIndVarSimplifyPass( passManager.Handle );
            return passManager;
        }

        public static T AddInstructionCombiningPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddInstructionCombiningPass( passManager.Handle );
            return passManager;
        }

        public static T AddJumpThreadingPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddJumpThreadingPass( passManager.Handle );
            return passManager;
        }

        public static T AddLICMPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLICMPass( passManager.Handle );
            return passManager;
        }

        public static T AddLoopDeletionPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLoopDeletionPass( passManager.Handle );
            return passManager;
        }

        public static T AddLoopIdiomPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLoopIdiomPass( passManager.Handle );
            return passManager;
        }

        public static T AddLoopRotatePass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLoopRotatePass( passManager.Handle );
            return passManager;
        }

        public static T AddLoopRerollPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLoopRerollPass( passManager.Handle );
            return passManager;
        }

        public static T AddLoopUnrollPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLoopUnrollPass( passManager.Handle );
            return passManager;
        }

        public static T AddLoopUnswitchPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLoopUnswitchPass( passManager.Handle );
            return passManager;
        }

        public static T AddMemCpyOptPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddMemCpyOptPass( passManager.Handle );
            return passManager;
        }

        public static T AddPartiallyInlineLibCallsPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddPartiallyInlineLibCallsPass( passManager.Handle );
            return passManager;
        }

        public static T AddLowerSwitchPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLowerSwitchPass( passManager.Handle );
            return passManager;
        }

        public static T AddPromoteMemoryToRegisterPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddPromoteMemoryToRegisterPass( passManager.Handle );
            return passManager;
        }

        public static T AddReassociatePass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddReassociatePass( passManager.Handle );
            return passManager;
        }

        public static T AddSCCPPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddSCCPPass( passManager.Handle );
            return passManager;
        }

        public static T AddScalarReplAggregatesPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddScalarReplAggregatesPass( passManager.Handle );
            return passManager;
        }

        public static T AddScalarReplAggregatesPassSSA<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddScalarReplAggregatesPassSSA( passManager.Handle );
            return passManager;
        }

        public static T AddScalarReplAggregatesPassWithThreshold<T>( this T passManager, int threshold )
            where T : PassManager
        {
            LLVMAddScalarReplAggregatesPassWithThreshold( passManager.Handle, threshold );
            return passManager;
        }

        public static T AddSimplifyLibCallsPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddSimplifyLibCallsPass( passManager.Handle );
            return passManager;
        }

        public static T AddTailCallEliminationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddTailCallEliminationPass( passManager.Handle );
            return passManager;
        }

        public static T AddConstantPropagationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddConstantPropagationPass( passManager.Handle );
            return passManager;
        }

        public static T AddDemoteMemoryToRegisterPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddDemoteMemoryToRegisterPass( passManager.Handle );
            return passManager;
        }

        public static T AddVerifierPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddVerifierPass( passManager.Handle );
            return passManager;
        }

        public static T AddCorrelatedValuePropagationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddCorrelatedValuePropagationPass( passManager.Handle );
            return passManager;
        }

        public static T AddEarlyCSEPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddEarlyCSEPass( passManager.Handle );
            return passManager;
        }

        public static T AddEarlyCSEMemSSAPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddEarlyCSEMemSSAPass( passManager.Handle );
            return passManager;
        }

        public static T AddLowerExpectIntrinsicPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLowerExpectIntrinsicPass( passManager.Handle );
            return passManager;
        }

        public static T AddTypeBasedAliasAnalysisPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddTypeBasedAliasAnalysisPass( passManager.Handle );
            return passManager;
        }

        public static T AddScopedNoAliasAAPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddScopedNoAliasAAPass( passManager.Handle );
            return passManager;
        }

        public static T AddBasicAliasAnalysisPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddBasicAliasAnalysisPass( passManager.Handle );
            return passManager;
        }
    }
}
