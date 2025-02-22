// -----------------------------------------------------------------------
// <copyright file="BitReader.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Ubiquity.NET.Llvm.Interop.Handles;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class NativeMethods
    {
        [Obsolete( "Use LLVMParseBitcode2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMParseBitcode(LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule, out DisposeMessageString OutMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMParseBitcode2(LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule);

        [Obsolete( "Use LLVMParseBitcodeInContext2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMParseBitcodeInContext(LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule, out DisposeMessageString OutMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMParseBitcodeInContext2(LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule);

        [Obsolete( "Use LLVMGetBitcodeModuleInContext2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetBitcodeModuleInContext(LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM, out DisposeMessageString OutMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetBitcodeModuleInContext2(LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM);

        [Obsolete( "Use LLVMGetBitcodeModule2 instead" )]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMGetBitcodeModule(LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM, out DisposeMessageString OutMessage);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMGetBitcodeModule2(LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM);
    }
}
