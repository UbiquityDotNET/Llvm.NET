// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class TargetMachineBindings
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMGetTargetMachineAsmVerbosity( LLVMTargetMachineRef tm );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMGetTargetMachineFastISel( LLVMTargetMachineRef tm );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMGetTargetMachineGlobalISel( LLVMTargetMachineRef tm );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMGlobalISelAbortMode LibLLVMGetTargetMachineGlobalISelAbort( LLVMTargetMachineRef tm );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMGetTargetMachineMachineOutliner( LLVMTargetMachineRef tm );

        public static LazyEncodedString? LibLLVMTargetMachineOptionsGetCPU( LLVMTargetMachineOptionsRef Options )
        {
            unsafe
            {
                byte* p = LibLLVMTargetMachineOptionsGetCPU(Options, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LibLLVMTargetMachineOptionsGetCPU( LLVMTargetMachineOptionsRef Options, out nuint len );

        public static LazyEncodedString? LibLLVMTargetMachineOptionsGetFeatures( LLVMTargetMachineOptionsRef Options )
        {
            unsafe
            {
                byte* p = LibLLVMTargetMachineOptionsGetFeatures(Options, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LibLLVMTargetMachineOptionsGetFeatures( LLVMTargetMachineOptionsRef Options, out nuint len );

        public static LazyEncodedString? LibLLVMTargetMachineOptionsGetABI( LLVMTargetMachineOptionsRef Options )
        {
            unsafe
            {
                byte* p = LibLLVMTargetMachineOptionsGetABI(Options, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LibLLVMTargetMachineOptionsGetABI( LLVMTargetMachineOptionsRef Options, out nuint len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMCodeGenOptLevel LibLLVMTargetMachineOptionsGetCodeGenOptLevel( LLVMTargetMachineOptionsRef Options );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMRelocMode LibLLVMTargetMachineOptionsGetRelocMode( LLVMTargetMachineOptionsRef Options );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMCodeModel LibLLVMTargetMachineOptionsGetCodeModel( LLVMTargetMachineOptionsRef Options );
    }
}
