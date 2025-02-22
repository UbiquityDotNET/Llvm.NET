// -----------------------------------------------------------------------
// <copyright file="Linker.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LLVMLinkerMode
        : Int32
    {
        LLVMLinkerDestroySource = 0,
        LLVMLinkerPreserveSource_Removed = 1,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMLinkModules2(LLVMModuleRef Dest, LLVMModuleRef Src);
    }
}
