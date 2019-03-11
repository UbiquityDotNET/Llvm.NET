// <copyright file="ValueCache.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Values
{
    internal partial class ValueCache
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueCacheRef LLVMCreateValueCache( IntPtr deletedCallback, IntPtr replacedCallback );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMValueCacheAdd( LLVMValueCacheRef cacheRef, LLVMValueRef value, IntPtr handle );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMValueCacheLookup( LLVMValueCacheRef cacheRef, LLVMValueRef valueRef );
        }
    }
}
