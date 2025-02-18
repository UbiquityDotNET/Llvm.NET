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
        Any = 0,
        ExactMatch = 1,
        Largest = 2,
        NoDeduplicate = 3,
        SameSize = 4,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMComdatRef LLVMGetOrInsertComdat( LLVMModuleRef M, [MarshalUsing( typeof(AnsiStringMarshaller) )]string Name );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMComdatRef LLVMGetComdat( LLVMValueRef V );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LLVMSetComdat( LLVMValueRef V, LLVMComdatRef C );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMComdatSelectionKind LLVMGetComdatSelectionKind( LLVMComdatRef C );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LLVMSetComdatSelectionKind( LLVMComdatRef C, LLVMComdatSelectionKind Kind );
    }
}
