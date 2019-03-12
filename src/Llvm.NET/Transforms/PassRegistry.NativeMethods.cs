// -----------------------------------------------------------------------
// <copyright file="PassRegistry.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#if LEGACY_PASS_REGISTRY
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    internal partial class PassRegistry
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalPassRegistry", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMPassRegistryRef LLVMGetGlobalPassRegistry( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCore", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeCore( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTransformUtils", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeTransformUtils( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeScalarOpts", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeScalarOpts( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeObjCARCOpts", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeObjCARCOpts( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeVectorization", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeVectorization( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstCombine", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeInstCombine( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPO", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeIPO( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeInstrumentation", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeInstrumentation( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAnalysis", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAnalysis( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeIPA", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeIPA( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCodeGen", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeCodeGen( LLVMPassRegistryRef R );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeTarget( LLVMPassRegistryRef R );
        }
    }
}
#endif
