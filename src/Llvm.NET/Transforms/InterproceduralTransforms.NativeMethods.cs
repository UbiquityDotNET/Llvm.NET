// <copyright file="InterproceduralTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>Utility class for adding the Inter-procedural transform passes to a <see cref="PassManager"/></summary>
    public static partial class InterproceduralTransforms
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddArgumentPromotionPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddConstantMergePass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddDeadArgEliminationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddFunctionAttrsPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddFunctionInliningPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddAlwaysInlinerPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddGlobalDCEPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddGlobalOptimizerPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddIPConstantPropagationPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddPruneEHPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddIPSCCPPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddInternalizePass( LLVMPassManagerRef param0, [MarshalAs( UnmanagedType.Bool )]bool AllButMain );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddStripDeadPrototypesPass( LLVMPassManagerRef PM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddStripSymbolsPass( LLVMPassManagerRef PM );
        }
    }
}
