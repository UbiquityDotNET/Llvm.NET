// <copyright file="InterproceduralTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    public static class InterproceduralTransforms
    {
        public static T AddArgumentPromotionPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddArgumentPromotionPass( passManager.Handle );
            return passManager;
        }

        public static T AddConstantMergePass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddConstantMergePass( passManager.Handle );
            return passManager;
        }

        public static T AddDeadArgEliminationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddDeadArgEliminationPass( passManager.Handle );
            return passManager;
        }

        public static T AddFunctionAttrsPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddFunctionAttrsPass( passManager.Handle );
            return passManager;
        }

        public static T AddFunctionInliningPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddFunctionInliningPass( passManager.Handle );
            return passManager;
        }

        public static T AddAlwaysInlinerPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddAlwaysInlinerPass( passManager.Handle );
            return passManager;
        }

        public static T AddGlobalDCEPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddGlobalDCEPass( passManager.Handle );
            return passManager;
        }

        public static T AddGlobalOptimizerPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddGlobalOptimizerPass( passManager.Handle );
            return passManager;
        }

        public static T AddIPConstantPropagationPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddIPConstantPropagationPass( passManager.Handle );
            return passManager;
        }

        public static T AddPruneEHPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddPruneEHPass( passManager.Handle );
            return passManager;
        }

        public static T AddIPSCCPPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddIPSCCPPass( passManager.Handle );
            return passManager;
        }

        public static T AddInternalizePass<T>( this T passManager, bool allButMain )
            where T : PassManager
        {
            LLVMAddInternalizePass( passManager.Handle, allButMain );
            return passManager;
        }

        public static T AddStripDeadPrototypesPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddStripDeadPrototypesPass( passManager.Handle );
            return passManager;
        }

        public static T AddStripSymbolsPass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddStripSymbolsPass( passManager.Handle );
            return passManager;
        }
    }
}
