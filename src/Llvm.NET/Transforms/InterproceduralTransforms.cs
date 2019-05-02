// <copyright file="InterproceduralTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>Utility class for adding the Inter-procedural transform passes to a <see cref="PassManager"/></summary>
    /// <seealso href="xref:llvm_docs_passes">LLVM: Analysis and Transform Passes</seealso>
    public static class InterproceduralTransforms
    {
        /// <summary>This pass promotes “by reference” arguments to be “by value” arguments.</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <seealso href="xref:llvm_docs_passes#argpromotion-promote-by-reference-arguments-to-scalars">LLVM: Promote ‘by reference’ arguments to scalars</seealso>
        public static T AddArgumentPromotionPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddArgumentPromotionPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Merges duplicate global constants together into a single constant that is shared.</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <seealso href="xref:llvm_docs_passes#constmerge-merge-duplicate-global-constants">LLVM: Simple constant propagation</seealso>
        public static T AddConstantMergePass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddConstantMergePass( passManager.Handle );
            return passManager;
        }

        /// <summary>This pass deletes dead arguments from internal functions.</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <seealso href="xref:llvm_docs_passes#deadargelim-dead-argument-elimination">LLVM: Dead Argument Elimination</seealso>
        public static T AddDeadArgEliminationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddDeadArgEliminationPass( passManager.Handle );
            return passManager;
        }

        /// <summary>A simple inter-procedural pass which walks the call-graph to apply attributes that are statically verifiable</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <seealso href="xref:llvm_docs_passes#functionattrs-deduce-function-attributes">LLVM: Deduce function attributes</seealso>
        public static T AddFunctionAttrsPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddFunctionAttrsPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Bottom-up inlining of functions into callees.</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <seealso href="xref:llvm_docs_passes#inline-function-integration-inlining">LLVM: Function Integration/Inlining</seealso>
        public static T AddFunctionInliningPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddFunctionInliningPass( passManager.Handle );
            return passManager;
        }

        /// <summary>A custom inliner that handles only functions that are marked as “always inline”.</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <seealso href="xref:llvm_docs_passes#always-inline-inliner-for-always-inline-functions">LLVM: Inliner for always_inline functions</seealso>
        public static T AddAlwaysInlinerPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddAlwaysInlinerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Global Dead Code Elimination pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <remarks>
        /// This transform is designed to eliminate unreachable internal globals from the program. It uses
        /// an aggressive algorithm, searching out globals that are known to be alive. After it finds all
        /// of the globals which are needed, it deletes whatever is left over. This allows it to delete
        /// recursive chunks of the program which are unreachable.
        /// </remarks>
        /// <seealso href="xref:llvm_docs_passes#globaldce-dead-global-elimination">LLVM: Global Dead Code Elimination pass</seealso>
        public static T AddGlobalDCEPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddGlobalDCEPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Global Variable Optimizer pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <remarks>
        /// This pass transforms simple global variables that never have their address taken.
        /// If obviously true, it marks read/write globals as constant, deletes variables only stored to, etc.
        /// </remarks>
        /// <seealso href="xref:llvm_docs_passes#globalopt-global-variable-optimizer">LLVM: Global Variable Optimizer pass</seealso>
        public static T AddGlobalOptimizerPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddGlobalOptimizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Inter-procedural constant propagation pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <remarks>
        /// This pass implements an extremely simple inter-procedural constant propagation pass. It could
        /// certainly be improved in many different ways, like using a worklist. This pass makes arguments
        /// dead, but does not remove them. The existing dead argument elimination pass should be run after
        /// this to clean up the mess.
        /// </remarks>
        /// <seealso href="xref:llvm_docs_passes#ipconstprop-interprocedural-constant-propagation">LLVM: Inter-procedural constant propagation pass</seealso>
        public static T AddIPConstantPropagationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddIPConstantPropagationPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Prune unused exception handling info pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <remarks>
        /// This file implements a simple inter-procedural pass which walks the call-graph, turning invoke
        /// instructions into call instructions if and only if the callee cannot throw an exception. It
        /// implements this as a bottom-up traversal of the call-graph.
        /// </remarks>
        /// <seealso href="xref:llvm_docs_passes#prune-eh-remove-unused-exception-handling-info">LLVM: Prune unused exception handling info pass</seealso>
        public static T AddPruneEHPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddPruneEHPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Inter-procedural Sparse Conditional Constant Propagation pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <seealso href="xref:llvm_docs_passes#Sparse Conditional Constant Propagation"/>
        /// <seealso cref="ScalarTransforms.AddSCCPPass{T}(T)">LLVM: Inter-procedural Sparse Conditional Constant Propagation pass</seealso>
        public static T AddIPSCCPPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddIPSCCPPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Internalize Global Symbols pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <param name="allButMain">Flag to indicate if all globals except "main" are considered candidates for internalizing</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <remarks>
        /// This pass loops over all of the functions in the input module, looking for a main function. If a
        /// main function is found, all other functions and all global variables with initializers are marked as internal.
        /// </remarks>
        /// <seealso href="xref:llvm_docs_passes#internalize-internalize-global-symbols">LLVM: Internalize Global Symbols pass</seealso>
        public static T AddInternalizePass<T>( this T passManager, bool allButMain )
            where T : PassManager
        {
            LLVMAddInternalizePass( passManager.Handle, allButMain );
            return passManager;
        }

        /// <summary>Adds a Strip Unused Function Prototypes pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <remarks>
        /// This pass loops over all of the functions in the input module, looking for dead declarations and
        /// removes them. Dead declarations are declarations of functions for which no implementation is
        /// available (i.e., declarations for unused library functions).
        /// </remarks>
        /// <seealso href="xref:llvm_docs_passes#strip-dead-prototypes-strip-unused-function-prototypes">LLVM: Strip Unused Function Prototypes pass</seealso>
        public static T AddStripDeadPrototypesPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddStripDeadPrototypesPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Strip symbols from module pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        /// <remarks>
        /// Performs code stripping. This transformation can delete:
        ///  * names for virtual registers
        ///  * symbols for internal globals and functions
        ///  * debug information
        /// <note type="note">This transformation makes code much less readable, so it should only be used in situations
        /// where the strip utility would be used, such as reducing code size or making it harder to reverse engineer code.
        /// </note>
        /// </remarks>
        /// <seealso href="xref:llvm_docs_passes#strip-strip-all-symbols-from-a-module">LLVM: Strip symbols from module pass</seealso>
        public static T AddStripSymbolsPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddStripSymbolsPass( passManager.Handle );
            return passManager;
        }
    }
}
