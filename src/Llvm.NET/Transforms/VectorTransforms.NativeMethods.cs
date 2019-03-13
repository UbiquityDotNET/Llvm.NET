// <copyright file="VectorTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>Extension methods for adding vector transform passes</summary>
    public static partial class VectorTransforms
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddLoopVectorizePass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddSLPVectorizePass( LLVMPassManagerRef PM );
        }
    }
}
