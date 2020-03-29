// -----------------------------------------------------------------------
// <copyright file="ScalarTransforms.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.ArgValidators;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Transforms
{
    /// <summary>Utility class for Adding Scalar transform passes to a <see cref="PassManager"/></summary>
    public static class ScalarTransforms
    {
        /// <summary>Adds an Aggressive Dead Code Elimination (DCE) pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#adce-aggressive-dead-code-elimination">LLVM: Aggressive Dead Elimination</seealso>
        public static T AddAggressiveDCEPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddAggressiveDCEPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Add Dead Code Elimination pass</summary>
        /// <typeparam name="T">Type of pass manager</typeparam>
        /// <param name="passManager">PassManager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#dce-dead-code-elimination"/>
        public static T AddDCEPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddDCEPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Bit tracking DCE pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddBitTrackingDCEPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddBitTrackingDCEPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Alignment from assumptions pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddAlignmentFromAssumptionsPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddAlignmentFromAssumptionsPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Simplify CFG pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#passes-simplifycfg">LLVM: Simplify CFG</seealso>
        public static T AddCFGSimplificationPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddCFGSimplificationPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Dead Store Elimination pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#dse-dead-store-elimination">LLVM: Dead Store Elimination</seealso>
        public static T AddDeadStoreEliminationPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddDeadStoreEliminationPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Scalarizer pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddScalarizerPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddScalarizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Merged Load Store Motion pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddMergedLoadStoreMotionPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddMergedLoadStoreMotionPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Global Value Numbering pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#gvn-global-value-numbering">LLVM: Global Value Numbering</seealso>
        public static T AddGVNPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddGVNPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds the new GVN pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddNewGVNPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddNewGVNPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Canonicalize Induction Variables pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#indvars-canonicalize-induction-variables">LLVM: Canonicalize Induction Variables</seealso>
        public static T AddIndVarSimplifyPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddIndVarSimplifyPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds the Instruction Combining pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#instcombine-combine-redundant-instructions">LLVM: Combine redundant instructions</seealso>
        public static T AddInstructionCombiningPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddInstructionCombiningPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a jump threading pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#jump-threading-jump-threading">LLVM: Jump Threading</seealso>
        public static T AddJumpThreadingPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddJumpThreadingPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Loop Invariant Code Motion pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#licm-loop-invariant-code-motion">LLVM: Loop Invariant Code Motion</seealso>
        public static T AddLICMPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLICMPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Delete dead loops pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#loop-deletion-delete-dead-loops">LLVM: Delete dead loops</seealso>
        public static T AddLoopDeletionPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopDeletionPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Loop Idiom pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddLoopIdiomPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopIdiomPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Rotate Loops pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#loop-rotate-rotate-loops">LLVM: Rotate Loops</seealso>
        public static T AddLoopRotatePass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopRotatePass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Loop Reroll pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddLoopRerollPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopRerollPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Loop Unroll pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#loop-unroll-unroll-loops">LLVM: Unroll Loops</seealso>
        public static T AddLoopUnrollPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopUnrollPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Loop Unroll and Jam pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#loop-unroll-unroll-loops">LLVM: Unroll Loops</seealso>
        public static T AddLoopUnrollAndJamPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopUnrollAndJamPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Loop Unswitch pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#loop-unswitch-unswitch-loops">LLVM: Unswitch loops</seealso>
        public static T AddLoopUnswitchPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopUnswitchPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Lower Atomic pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#loweratomic-lower-atomic-intrinsics-to-non-atomic-form">LLVM: Lower atomic</seealso>
        public static T LowerAtomicPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLowerAtomicPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an MemCpy Optimization pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#memcpyopt-memcpy-optimization">LLVM: MemCpy Optimization</seealso>
        public static T AddMemCpyOptPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddMemCpyOptPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Partial Inliner pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#partial-inliner-partial-inliner">LLVM: Partial Inliner</seealso>
        public static T AddPartiallyInlineLibCallsPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddPartiallyInlineLibCallsPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Lower Switch pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#lowerswitch-lower-switchinsts-to-branches">LLVM: Lower SwitchInst to branches</seealso>
        public static T AddLowerSwitchPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLowerSwitchPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Promote memory to Register pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#mem2reg-promote-memory-to-register">LLVM: Promote Memory to Register</seealso>
        public static T AddPromoteMemoryToRegisterPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddPromoteMemoryToRegisterPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a reassociate expressions pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#reassociate-reassociate-expressions">LLVM: Reassociate expressions</seealso>
        public static T AddReassociatePass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddReassociatePass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an  pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#sccp-sparse-conditional-constant-propagation">LLVM: Sparse Conditional Constant Propagation</seealso>
        public static T AddSCCPPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddSCCPPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Scalar Replacement of Aggregates pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#sroa-scalar-replacement-of-aggregates">LLVM: Scalar Replacement of Aggregates</seealso>
        public static T AddScalarReplAggregatesPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddScalarReplAggregatesPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Scalar Replacement of Aggregates pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#sroa-scalar-replacement-of-aggregates">LLVM: Scalar Replacement of Aggregates</seealso>
        public static T AddScalarReplAggregatesPassSSA<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddScalarReplAggregatesPassSSA( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Scalar Replacement of Aggregates pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <param name="threshold">Threshold for this pass</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#sroa-scalar-replacement-of-aggregates">LLVM: Scalar Replacement of Aggregates</seealso>
        public static T AddScalarReplAggregatesPassWithThreshold<T>( [ValidatedNotNull] this T passManager, int threshold )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddScalarReplAggregatesPassWithThreshold( passManager.Handle, threshold );
            return passManager;
        }

        /// <summary>Adds a Simplify Lib Calls pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddSimplifyLibCallsPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddSimplifyLibCallsPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Tail Call Elimination pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#tailcallelim-tail-call-elimination">LLVM: Tail Call Elimination</seealso>
        public static T AddTailCallEliminationPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddTailCallEliminationPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Simple constant propagation pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#constprop-simple-constant-propagation">LLVM: Simple constant propagation</seealso>
        public static T AddConstantPropagationPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddConstantPropagationPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Reg2Mem pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#reg2mem-demote-all-values-to-stack-slots">LLVM: Demote all values to stack slots</seealso>
        public static T AddDemoteMemoryToRegisterPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddDemoteMemoryToRegisterPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Module Verifier pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#verify-module-verifier">LLVM: Module Verifier</seealso>
        public static T AddVerifierPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddVerifierPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Correlated Value Propagation pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddCorrelatedValuePropagationPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddCorrelatedValuePropagationPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Early CSE pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddEarlyCSEPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddEarlyCSEPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Early CSE Mem SSA pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddEarlyCSEMemSSAPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddEarlyCSEMemSSAPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Lower Expect Instrinsic pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddLowerExpectIntrinsicPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLowerExpectIntrinsicPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Type Based Alias Analysis pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddTypeBasedAliasAnalysisPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddTypeBasedAliasAnalysisPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Scoped No Alias AA pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddScopedNoAliasAAPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddScopedNoAliasAAPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Basic Alias Analysis pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        /// <seealso href="xref:llvm_docs_passes#basicaa-basic-alias-analysis-stateless-aa-impl">LLVM: Basic Alias Analysis</seealso>
        public static T AddBasicAliasAnalysisPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddBasicAliasAnalysisPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Lower Constant Intrinsics pass to the manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">THe pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddLowerConstantIntrinsicsPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLowerConstantIntrinsicsPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Add Discriminators Pass to the pass manager</summary>
        /// <typeparam name="T">Type of pass manager to add the pass to</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static T AddAddDiscriminatorsPass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddAddDiscriminatorsPass( passManager.Handle );
            return passManager;
        }
    }
}
