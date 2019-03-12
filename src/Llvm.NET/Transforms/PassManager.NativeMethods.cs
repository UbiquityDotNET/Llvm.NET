// <copyright file="cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>Common base class for pass managers</summary>
    public partial class PassManager
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMCreatePassManager", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMPassManagerRef LLVMCreatePassManager( );

            [DllImport( LibraryPath, EntryPoint = "LLVMRunPassManager", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMRunPassManager( LLVMPassManagerRef PM, LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManagerForModule", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMPassManagerRef LLVMCreateFunctionPassManagerForModule( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMPassManagerRef LLVMCreateFunctionPassManager( LLVMModuleProviderRef MP );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMInitializeFunctionPassManager( LLVMPassManagerRef FPM );

            [DllImport( LibraryPath, EntryPoint = "LLVMRunFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMRunFunctionPassManager( LLVMPassManagerRef FPM, LLVMValueRef F );

            [DllImport( LibraryPath, EntryPoint = "LLVMFinalizeFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMFinalizeFunctionPassManager( LLVMPassManagerRef FPM );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddTargetLibraryInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddTargetLibraryInfo( LLVMTargetLibraryInfoRef TLI, LLVMPassManagerRef PM );
        }
    }
}
