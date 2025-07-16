// -----------------------------------------------------------------------
// <copyright file="OrcJITv2Bindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class OrcJITv2Bindings
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMExecutionSessionRemoveDyLib( LLVMOrcExecutionSessionRef session, LLVMOrcJITDylibRef lib );
    }
}
