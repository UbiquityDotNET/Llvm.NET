// -----------------------------------------------------------------------
// <copyright file="Object.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LLVMBinaryType
        : Int32
    {
        LLVMBinaryTypeArchive = 0,
        LLVMBinaryTypeMachOUniversalBinary = 1,
        LLVMBinaryTypeCOFFImportFile = 2,
        LLVMBinaryTypeIR = 3,
        LLVMBinaryTypeWinRes = 4,
        LLVMBinaryTypeCOFF = 5,
        LLVMBinaryTypeELF32L = 6,
        LLVMBinaryTypeELF32B = 7,
        LLVMBinaryTypeELF64L = 8,
        LLVMBinaryTypeELF64B = 9,
        LLVMBinaryTypeMachO32L = 10,
        LLVMBinaryTypeMachO32B = 11,
        LLVMBinaryTypeMachO64L = 12,
        LLVMBinaryTypeMachO64B = 13,
        LLVMBinaryTypeWasm = 14,
        LLVMBinaryTypeOffload = 15,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBinaryRef LLVMCreateBinary(LLVMMemoryBufferRef MemBuf, LLVMContextRef Context, out DisposeMessageString ErrorMessage);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeBinary(LLVMBinaryRef BR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMemoryBufferRef LLVMBinaryCopyMemoryBuffer(LLVMBinaryRef BR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBinaryType LLVMBinaryGetType(LLVMBinaryRef BR);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBinaryRef LLVMMachOUniversalBinaryCopyObjectForArch(LLVMBinaryRef BR, string Arch, size_t ArchLen, out DisposeMessageString ErrorMessage);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSectionIteratorRef LLVMObjectFileCopySectionIterator(LLVMBinaryRef BR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMObjectFileIsSectionIteratorAtEnd(LLVMBinaryRef BR, LLVMSectionIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LLVMObjectFileCopySymbolIterator(LLVMBinaryRef BR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMObjectFileIsSymbolIteratorAtEnd(LLVMBinaryRef BR, LLVMSymbolIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToNextSection(LLVMSectionIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToContainingSection(LLVMSectionIteratorRef Sect, LLVMSymbolIteratorRef Sym);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToNextSymbol(LLVMSymbolIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMGetSectionName(LLVMSectionIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSectionSize(LLVMSectionIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGetSectionContents(LLVMSectionIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSectionAddress(LLVMSectionIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetSectionContainsSymbol(LLVMSectionIteratorRef SI, LLVMSymbolIteratorRef Sym);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMRelocationIteratorRef LLVMGetRelocations(LLVMSectionIteratorRef Section);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsRelocationIteratorAtEnd(LLVMSectionIteratorRef Section, LLVMRelocationIteratorRef RI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToNextRelocation(LLVMRelocationIteratorRef RI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMGetSymbolName(LLVMSymbolIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSymbolAddress(LLVMSymbolIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSymbolSize(LLVMSymbolIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetRelocationOffset(LLVMRelocationIteratorRef RI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LLVMGetRelocationSymbol(LLVMRelocationIteratorRef RI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetRelocationType(LLVMRelocationIteratorRef RI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetRelocationTypeName(LLVMRelocationIteratorRef RI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMGetRelocationValueString(LLVMRelocationIteratorRef RI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMObjectFileRef LLVMCreateObjectFile(LLVMMemoryBufferRef MemBuf);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSectionIteratorRef LLVMGetSections(LLVMObjectFileRef ObjectFile);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsSectionIteratorAtEnd(LLVMObjectFileRef ObjectFile, LLVMSectionIteratorRef SI);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LLVMGetSymbols(LLVMObjectFileRef ObjectFile);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsSymbolIteratorAtEnd(LLVMObjectFileRef ObjectFile, LLVMSymbolIteratorRef SI);
    }
}
