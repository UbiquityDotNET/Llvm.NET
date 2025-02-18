// -----------------------------------------------------------------------
// <copyright file="BitReader.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMParseBitcode2(LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMParseBitcodeInContext2(LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetBitcodeModuleInContext2(LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetBitcodeModule2(LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM);
    }
}
