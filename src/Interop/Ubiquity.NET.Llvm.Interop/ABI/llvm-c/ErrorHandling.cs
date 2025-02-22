// -----------------------------------------------------------------------
// <copyright file="ErrorHandling.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMFatalErrorHandler = delegate* unmanaged[Cdecl]<byte* /*Reason*/, void /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    public static partial class NativeMethods
    {
        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstallFatalErrorHandler(LLVMFatalErrorHandler Handler);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMResetFatalErrorHandler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMEnablePrettyStackTrace();
    }
}
