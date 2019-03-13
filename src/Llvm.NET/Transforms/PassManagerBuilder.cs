// -----------------------------------------------------------------------
// <copyright file="PassManagerBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Llvm.NET.Transforms
{
    /* TODO: Implement PassManagerBuilder */
    internal class PassManagerBuilder
    {
        internal static class NativeMethods
        {
            /*
            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderCreate", CallingConvention = CallingConvention.Cdecl )]
            private static extern LLVMPassManagerBuilderRef LLVMPassManagerBuilderCreate( );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderDispose", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderDispose( LLVMPassManagerBuilderRef PMB );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetOptLevel", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderSetOptLevel( LLVMPassManagerBuilderRef PMB, uint OptLevel );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetSizeLevel", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderSetSizeLevel( LLVMPassManagerBuilderRef PMB, uint SizeLevel );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnitAtATime", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderSetDisableUnitAtATime( LLVMPassManagerBuilderRef PMB, [MarshalAs( UnmanagedType.Bool )]bool Value );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableUnrollLoops", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderSetDisableUnrollLoops( LLVMPassManagerBuilderRef PMB, [MarshalAs( UnmanagedType.Bool )]bool Value );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderSetDisableSimplifyLibCalls", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderSetDisableSimplifyLibCalls( LLVMPassManagerBuilderRef PMB, [MarshalAs( UnmanagedType.Bool )]bool Value );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderUseInlinerWithThreshold", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderUseInlinerWithThreshold( LLVMPassManagerBuilderRef PMB, uint Threshold );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateFunctionPassManager", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderPopulateFunctionPassManager( LLVMPassManagerBuilderRef PMB, LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateModulePassManager", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderPopulateModulePassManager( LLVMPassManagerBuilderRef PMB, LLVMPassManagerRef PM );

            [DllImport( LibraryPath, EntryPoint = "LLVMPassManagerBuilderPopulateLTOPassManager", CallingConvention = CallingConvention.Cdecl )]
            private static extern void LLVMPassManagerBuilderPopulateLTOPassManager( LLVMPassManagerBuilderRef PMB, LLVMPassManagerRef PM, [MarshalAs( UnmanagedType.Bool )]bool privateize, [MarshalAs( UnmanagedType.Bool )]bool RunInliner );
            */
        }
    }
}
