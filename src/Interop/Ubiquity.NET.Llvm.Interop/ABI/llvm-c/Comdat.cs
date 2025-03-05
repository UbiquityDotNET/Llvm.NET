// -----------------------------------------------------------------------
// <copyright file="Comdat.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LLVMComdatSelectionKind
        : Int32
    {
        LLVMAnyComdatSelectionKind = 0,
        LLVMExactMatchComdatSelectionKind = 1,
        LLVMLargestComdatSelectionKind = 2,
        LLVMNoDeduplicateComdatSelectionKind = 3,
        LLVMSameSizeComdatSelectionKind = 4,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LLVMGetOrInsertComdat(LLVMModuleRef M, string Name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatRef LLVMGetComdat(LLVMValueRef V);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetComdat(LLVMValueRef V, LLVMComdatRef C);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMComdatSelectionKind LLVMGetComdatSelectionKind(LLVMComdatRef C);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetComdatSelectionKind(LLVMComdatRef C, LLVMComdatSelectionKind Kind);
    }
}
