// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
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

    public static partial class Object
    {
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBinaryRef LLVMCreateBinary( LLVMMemoryBufferRef MemBuf, LLVMContextRefAlias Context, out string ErrorMessage );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeBinary( LLVMBinaryRef BR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMemoryBufferRef LLVMBinaryCopyMemoryBuffer( LLVMBinaryRef BR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMBinaryType LLVMBinaryGetType( LLVMBinaryRef BR );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMBinaryRef LLVMMachOUniversalBinaryCopyObjectForArch(
            LLVMBinaryRef BR,
            LazyEncodedString Arch,
            out string ErrorMessage
            )
        {
            return LLVMMachOUniversalBinaryCopyObjectForArch(
                BR,
                Arch,
                Arch.NativeStrLen,
                out ErrorMessage
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMBinaryRef LLVMMachOUniversalBinaryCopyObjectForArch(
            LLVMBinaryRef BR,
            LazyEncodedString Arch, nuint ArchLen,
            [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string ErrorMessage
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSectionIteratorRef LLVMObjectFileCopySectionIterator( LLVMBinaryRef BR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMObjectFileIsSectionIteratorAtEnd( LLVMBinaryRef BR, LLVMSectionIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LLVMObjectFileCopySymbolIterator( LLVMBinaryRef BR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMObjectFileIsSymbolIteratorAtEnd( LLVMBinaryRef BR, LLVMSymbolIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToNextSection( LLVMSectionIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToContainingSection( LLVMSectionIteratorRef Sect, LLVMSymbolIteratorRef Sym );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToNextSymbol( LLVMSymbolIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetSectionName( LLVMSectionIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSectionSize( LLVMSectionIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nint LLVMGetSectionContents( LLVMSectionIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSectionAddress( LLVMSectionIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetSectionContainsSymbol( LLVMSectionIteratorRef SI, LLVMSymbolIteratorRef Sym );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMRelocationIteratorRef LLVMGetRelocations( LLVMSectionIteratorRef Section );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsRelocationIteratorAtEnd( LLVMSectionIteratorRef Section, LLVMRelocationIteratorRef RI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMoveToNextRelocation( LLVMRelocationIteratorRef RI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? LLVMGetSymbolName( LLVMSymbolIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSymbolAddress( LLVMSymbolIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetSymbolSize( LLVMSymbolIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetRelocationOffset( LLVMRelocationIteratorRef RI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LLVMGetRelocationSymbol( LLVMRelocationIteratorRef RI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMGetRelocationType( LLVMRelocationIteratorRef RI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial LazyEncodedString LLVMGetRelocationTypeName( LLVMRelocationIteratorRef RI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial LazyEncodedString LLVMGetRelocationValueString( LLVMRelocationIteratorRef RI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMObjectFileRef LLVMCreateObjectFile( LLVMMemoryBufferRef MemBuf );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSectionIteratorRef LLVMGetSections( LLVMObjectFileRef ObjectFile );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsSectionIteratorAtEnd( LLVMObjectFileRef ObjectFile, LLVMSectionIteratorRef SI );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LLVMGetSymbols( LLVMObjectFileRef ObjectFile );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMIsSymbolIteratorAtEnd( LLVMObjectFileRef ObjectFile, LLVMSymbolIteratorRef SI );
    }
}
