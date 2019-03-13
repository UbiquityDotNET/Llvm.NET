// <copyright file="Sanitizers.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>LLVM Sanitizer passes</summary>
    public static partial class Sanitizers
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddAddressSanitizerFunctionPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddAddressSanitizerModulePass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddThreadSanitizerPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddMemorySanitizerPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddDataFlowSanitizerPass( LLVMPassManagerRef PM, [MarshalAs( UnmanagedType.LPStr )] string ABIListFile );
        }
    }
}
