// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    public static partial class BitReader
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMParseBitcode2( LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMParseBitcodeInContext2( LLVMContextRefAlias ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetBitcodeModuleInContext2( LLVMContextRefAlias ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetBitcodeModule2( LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM );
    }
}
