// -----------------------------------------------------------------------
// <copyright file="Sanitizers.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.ArgValidators;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Transforms
{
    /// <summary>LLVM Sanitizer passes</summary>
    public static class Sanitizers
    {
        /// <summary>Adds an Address Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddAddressSanitizerPass( [ValidatedNotNull] this FunctionPassManager passManager )
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LibLLVMAddAddressSanitizerFunctionPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Address Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static ModulePassManager AddSanitizerPass( [ValidatedNotNull] this ModulePassManager passManager )
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LibLLVMAddAddressSanitizerModulePass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Thread Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddThreadSanitizerPass( [ValidatedNotNull] this FunctionPassManager passManager )
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LibLLVMAddThreadSanitizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Memory Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddMemorySanitizerPass( [ValidatedNotNull] this FunctionPassManager passManager )
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LibLLVMAddMemorySanitizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a DataFlow Sanitizer Module pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <param name="abiListFile">ABI List Files</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static ModulePassManager AddDataFlowSanitizerPass( [ValidatedNotNull] this ModulePassManager passManager, string[ ] abiListFile )
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            if( abiListFile == null )
            {
                abiListFile = Array.Empty<string>( );
            }

            passManager.ValidateNotNull( nameof( passManager ) );
            LibLLVMAddDataFlowSanitizerPass( passManager.Handle, abiListFile.Length, abiListFile );
            return passManager;
        }
    }
}
