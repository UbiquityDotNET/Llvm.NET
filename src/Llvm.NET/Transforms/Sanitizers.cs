// -----------------------------------------------------------------------
// <copyright file="Sanitizers.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>LLVM Sanitizer passes</summary>
    public static class Sanitizers
    {
        /// <summary>Adds an Address Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddAddressSanitizerPass( this FunctionPassManager passManager )
        {
            LibLLVMAddAddressSanitizerFunctionPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Address Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static ModulePassManager AddSanitizerPass(this ModulePassManager passManager)
        {
            LibLLVMAddAddressSanitizerModulePass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Thread Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddThreadSanitizerPass( this FunctionPassManager passManager )
        {
            LibLLVMAddThreadSanitizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Memory Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddMemorySanitizerPass( this FunctionPassManager passManager )
        {
            LibLLVMAddMemorySanitizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a DataFlow Sanitizer Module pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <param name="abiListFile">ABI List Files</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static ModulePassManager AddDataFlowSanitizerPass( this ModulePassManager passManager, string[] abiListFile )
        {
            LibLLVMAddDataFlowSanitizerPass( passManager.Handle,abiListFile.Length, abiListFile );
            return passManager;
        }
    }
}
