// <copyright file="ScalarTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

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

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderCreate", CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMPassManagerBuilderRef LLVMPassManagerBuilderCreate( );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderDispose", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderDispose( LLVMPassManagerBuilderRef @PMB );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetOptLevel", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderSetOptLevel( LLVMPassManagerBuilderRef @PMB, uint @OptLevel );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetSizeLevel", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderSetSizeLevel( LLVMPassManagerBuilderRef @PMB, uint @SizeLevel );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnitAtATime", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderSetDisableUnitAtATime( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnrollLoops", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderSetDisableUnrollLoops( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableSimplifyLibCalls", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderSetDisableSimplifyLibCalls( LLVMPassManagerBuilderRef @PMB, [MarshalAs( UnmanagedType.Bool )]bool @Value );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderUseInlinerWithThreshold", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderUseInlinerWithThreshold( LLVMPassManagerBuilderRef @PMB, uint @Threshold );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderPopulateFunctionPassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateModulePassManager", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderPopulateModulePassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateLTOPassManager", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMPassManagerBuilderPopulateLTOPassManager( LLVMPassManagerBuilderRef @PMB, LLVMPassManagerRef @PM, [MarshalAs( UnmanagedType.Bool )]bool @privateize, [MarshalAs( UnmanagedType.Bool )]bool @RunInliner );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAggressiveDCEPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddAggressiveDCEPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddBitTrackingDCEPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddBitTrackingDCEPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAlignmentFromAssumptionsPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddAlignmentFromAssumptionsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCFGSimplificationPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddCFGSimplificationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLateCFGSimplificationPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLateCFGSimplificationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDeadStoreEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddDeadStoreEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarizerPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddScalarizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddMergedLoadStoreMotionPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddMergedLoadStoreMotionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddGVNPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddGVNPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddNewGVNPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddNewGVNPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddIndVarSimplifyPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddIndVarSimplifyPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddInstructionCombiningPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddInstructionCombiningPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddJumpThreadingPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddJumpThreadingPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLICMPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLICMPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopDeletionPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLoopDeletionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopIdiomPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLoopIdiomPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopRotatePass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLoopRotatePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopRerollPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLoopRerollPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopUnrollPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLoopUnrollPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLoopUnswitchPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLoopUnswitchPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddMemCpyOptPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddMemCpyOptPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddPartiallyInlineLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddPartiallyInlineLibCallsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLowerSwitchPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLowerSwitchPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddPromoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddPromoteMemoryToRegisterPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddReassociatePass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddReassociatePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSCCPPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddSCCPPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddScalarReplAggregatesPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassSSA", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddScalarReplAggregatesPassSSA( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScalarReplAggregatesPassWithThreshold", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddScalarReplAggregatesPassWithThreshold( LLVMPassManagerRef @PM, int @Threshold );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddSimplifyLibCallsPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddSimplifyLibCallsPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTailCallEliminationPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddTailCallEliminationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddConstantPropagationPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddConstantPropagationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDemoteMemoryToRegisterPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddDemoteMemoryToRegisterPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddVerifierPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddVerifierPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddCorrelatedValuePropagationPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddCorrelatedValuePropagationPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddEarlyCSEPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddEarlyCSEPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddEarlyCSEMemSSAPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddEarlyCSEMemSSAPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddLowerExpectIntrinsicPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddLowerExpectIntrinsicPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddTypeBasedAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddTypeBasedAliasAnalysisPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddScopedNoAliasAAPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddScopedNoAliasAAPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddBasicAliasAnalysisPass", CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMAddBasicAliasAnalysisPass( LLVMPassManagerRef @PM );
    }
}
