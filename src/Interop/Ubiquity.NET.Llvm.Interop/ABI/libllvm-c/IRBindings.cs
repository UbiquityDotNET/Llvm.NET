// -----------------------------------------------------------------------
// <copyright file="IRBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LibLLVMVersionInfo
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMGetVersionInfo(out LibLLVMVersionInfo pVersionInfo);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMHasUnwindDest(LLVMValueRef Invoke);
    }
}
