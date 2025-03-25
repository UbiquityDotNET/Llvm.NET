// -----------------------------------------------------------------------
// <copyright file="DataLayoutBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class DataLayoutBindings
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMParseDataLayout(byte* layoutString, size_t strLen, out LLVMTargetDataRef outRetVal);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial byte* LibLLVMGetDataLayoutString(LLVMTargetDataRefAlias dataLayout, out size_t outLen);
    }
}
