// -----------------------------------------------------------------------
// <copyright file="ObjectFileBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class ObjectFileBindings
    {
        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LibLLVMSymbolIteratorClone(LLVMSymbolIteratorRef @ref);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSectionIteratorRef LibLLVMSectionIteratorClone(LLVMSectionIteratorRef @ref);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMRelocationIteratorRef LibLLVMRelocationIteratorClone(LLVMRelocationIteratorRef @ref);
    }
}
