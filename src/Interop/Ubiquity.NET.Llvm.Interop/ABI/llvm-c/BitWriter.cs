// -----------------------------------------------------------------------
// <copyright file="BitWriter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    public static partial class BitWriter
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMWriteBitcodeToFile(LLVMModuleRefAlias M, LazyEncodedString Path);

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
        public static unsafe partial LLVMMemoryBufferRef LLVMWriteBitcodeToMemoryBuffer(LLVMModuleRefAlias M);
    }
}
