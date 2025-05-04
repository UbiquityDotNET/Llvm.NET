// -----------------------------------------------------------------------
// <copyright file="Linker.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    public enum LLVMLinkerMode
        : Int32
    {
        LLVMLinkerDestroySource = 0,
        LLVMLinkerPreserveSource_Removed = 1,
    }

    public static partial class Linker
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMLinkModules2(LLVMModuleRefAlias Dest, LLVMModuleRef Src);
    }
}
