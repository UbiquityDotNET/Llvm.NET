// <copyright file="Comdat.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Comdat entry for a module</summary>
    public partial class Comdat
    {
        internal static class NativeMethods
        {
            internal enum LLVMComdatSelectionKind
            {
                ANY,
                EXACTMATCH,
                LARGEST,
                NODUPLICATES,
                SAMESIZE
            }

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMComdatSelectionKind LLVMComdatGetKind( LLVMComdatRef comdatRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMComdatSetKind( LLVMComdatRef comdatRef, LLVMComdatSelectionKind kind );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMComdatGetName( LLVMComdatRef comdatRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMComdatRef LLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMComdatSelectionKind kind );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMModuleComdatClear( LLVMModuleRef module );

            [UnmanagedFunctionPointer( CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal delegate bool ComdatIteratorCallback( LLVMComdatRef comdatRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMModuleEnumerateComdats( LLVMModuleRef module, ComdatIteratorCallback callback );
        }
    }
}
