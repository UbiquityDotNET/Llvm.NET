// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class ObjectFileBindings
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSymbolIteratorRef LibLLVMSymbolIteratorClone( LLVMSymbolIteratorRef @ref );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMSectionIteratorRef LibLLVMSectionIteratorClone( LLVMSectionIteratorRef @ref );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMRelocationIteratorRef LibLLVMRelocationIteratorClone( LLVMRelocationIteratorRef @ref );
    }
}
