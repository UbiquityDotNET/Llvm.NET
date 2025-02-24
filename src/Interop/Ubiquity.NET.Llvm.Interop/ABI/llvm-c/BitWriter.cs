// -----------------------------------------------------------------------
// <copyright file="BitWriter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMWriteBitcodeToFile(LLVMModuleRef M, string Path);

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

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMemoryBufferRef LLVMWriteBitcodeToMemoryBuffer(LLVMModuleRef M);
    }
}
