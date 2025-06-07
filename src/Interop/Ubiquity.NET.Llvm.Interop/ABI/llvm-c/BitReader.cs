// -----------------------------------------------------------------------
// <copyright file="BitReader.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
