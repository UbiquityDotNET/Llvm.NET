// <copyright file="Sanitizers.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

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
            LLVMAddAddressSanitizerFunctionPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds an Address Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static ModulePassManager AddSanitizerPass(this ModulePassManager passManager)
        {
            LLVMAddAddressSanitizerModulePass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Thread Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddThreadSanitizerPass( this FunctionPassManager passManager )
        {
            LLVMAddThreadSanitizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a Memory Sanitizer Function pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static FunctionPassManager AddMemorySanitizerPass( this FunctionPassManager passManager )
        {
            LLVMAddMemorySanitizerPass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a DataFlow Sanitizer Module pass</summary>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <param name="abiListFile">ABI List File</param>
        /// <returns><paramref name="passManager"/>for fluent support</returns>
        public static ModulePassManager AddDataFlowSanitizerPass( this ModulePassManager passManager, string abiListFile )
        {
            LLVMAddDataFlowSanitizerPass( passManager.Handle, abiListFile );
            return passManager;
        }

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMAddAddressSanitizerFunctionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMAddAddressSanitizerModulePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMAddThreadSanitizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMAddMemorySanitizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMAddDataFlowSanitizerPass( LLVMPassManagerRef @PM, [MarshalAs( UnmanagedType.LPStr )] string @ABIListFile );
    }
}
