// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    public static partial class BitWriter
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMWriteBitcodeToFile( LLVMModuleRefAlias M, LazyEncodedString Path );

        // It is debatable if the .NET projections should deal with a raw C "File descriptor", which is
        // exclusively a C/C++ runtime construct that does NOT exist in managed code.
#if SUPPORT_FILE_DESCRIPTORS
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMWriteBitcodeToFD(
            LLVMModuleRef M,
            int FD,
            [MarshalAs(UnmanagedType.Bool)] bool ShouldClose,
            [MarshalAs(UnmanagedType.Bool)] bool Unbuffered = false
            );

        [Obsolete("Use LLVMWriteBitcodeToFD instead")]
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMWriteBitcodeToFileHandle(LLVMModuleRef M, int Handle);
#endif

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMemoryBufferRef LLVMWriteBitcodeToMemoryBuffer( LLVMModuleRefAlias M );
    }
}
