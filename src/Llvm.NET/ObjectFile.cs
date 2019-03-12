// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    internal class ObjectFile
    {
        /* TODO: Implement support for ObjectFile */

        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMCreateObjectFile", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMObjectFileRef LLVMCreateObjectFile( LLVMMemoryBufferRef MemBuf );

            [DllImport( LibraryPath, EntryPoint = "LLVMDisposeObjectFile", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposeObjectFile( LLVMObjectFileRef ObjectFile );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSections", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMSectionIteratorRef LLVMGetSections( LLVMObjectFileRef ObjectFile );

            [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSectionIterator", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposeSectionIterator( LLVMSectionIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsSectionIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsSectionIteratorAtEnd( LLVMObjectFileRef ObjectFile, LLVMSectionIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSection", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMMoveToNextSection( LLVMSectionIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMMoveToContainingSection", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMMoveToContainingSection( LLVMSectionIteratorRef Sect, LLVMSymbolIteratorRef Sym );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbols", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMSymbolIteratorRef LLVMGetSymbols( LLVMObjectFileRef ObjectFile );

            [DllImport( LibraryPath, EntryPoint = "LLVMDisposeSymbolIterator", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposeSymbolIterator( LLVMSymbolIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsSymbolIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsSymbolIteratorAtEnd( LLVMObjectFileRef ObjectFile, LLVMSymbolIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextSymbol", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMMoveToNextSymbol( LLVMSymbolIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionName", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetSectionName( LLVMSectionIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionSize", CallingConvention = CallingConvention.Cdecl )]
            internal static extern int LLVMGetSectionSize( LLVMSectionIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContents", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetSectionContents( LLVMSectionIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionAddress", CallingConvention = CallingConvention.Cdecl )]
            internal static extern int LLVMGetSectionAddress( LLVMSectionIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSectionContainsSymbol", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMGetSectionContainsSymbol( LLVMSectionIteratorRef SI, LLVMSymbolIteratorRef Sym );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocations", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMRelocationIteratorRef LLVMGetRelocations( LLVMSectionIteratorRef Section );

            [DllImport( LibraryPath, EntryPoint = "LLVMDisposeRelocationIterator", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposeRelocationIterator( LLVMRelocationIteratorRef RI );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsRelocationIteratorAtEnd", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsRelocationIteratorAtEnd( LLVMSectionIteratorRef Section, LLVMRelocationIteratorRef RI );

            [DllImport( LibraryPath, EntryPoint = "LLVMMoveToNextRelocation", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMMoveToNextRelocation( LLVMRelocationIteratorRef RI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolName", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetSymbolName( LLVMSymbolIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolAddress", CallingConvention = CallingConvention.Cdecl )]
            internal static extern int LLVMGetSymbolAddress( LLVMSymbolIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetSymbolSize", CallingConvention = CallingConvention.Cdecl )]
            internal static extern int LLVMGetSymbolSize( LLVMSymbolIteratorRef SI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationOffset", CallingConvention = CallingConvention.Cdecl )]
            internal static extern int LLVMGetRelocationOffset( LLVMRelocationIteratorRef RI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationSymbol", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMSymbolIteratorRef LLVMGetRelocationSymbol( LLVMRelocationIteratorRef RI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern int LLVMGetRelocationType( LLVMRelocationIteratorRef RI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationTypeName", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetRelocationTypeName( LLVMRelocationIteratorRef RI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetRelocationValueString", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetRelocationValueString( LLVMRelocationIteratorRef RI );
        }
    }
}
