// -----------------------------------------------------------------------
// <copyright file="Disassembler.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMOpInfoCallback = delegate* unmanaged[Cdecl]<void* /*DisInfo*/, UInt64 /*PC*/, UInt64 /*Offset*/, UInt64 /*OpSize*/, UInt64 /*InstSize*/, int /*TagType*/, void* /*TagBuf*/, int /*retVal*/>;
    using unsafe LLVMSymbolLookupCallback = delegate* unmanaged[Cdecl]<void* /*DisInfo*/, UInt64 /*ReferenceValue*/, UInt64* /*ReferenceType*/, UInt64 /*ReferencePC*/, byte** /*ReferenceName*/, byte* /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    public static partial class NativeMethods
    {
        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDisasmContextRef LLVMCreateDisasm(string TripleName, void* DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp);

        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDisasmContextRef LLVMCreateDisasmCPU(string Triple, string CPU, void* DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp);

        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDisasmContextRef LLVMCreateDisasmCPUFeatures(string Triple, string CPU, string Features, void* DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMSetDisasmOptions(LLVMDisasmContextRef DC, UInt64 Options);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial size_t LLVMDisasmInstruction(LLVMDisasmContextRef DC, nint Bytes, UInt64 BytesSize, UInt64 PC, byte* OutString, size_t OutStringSize);
    }
}
