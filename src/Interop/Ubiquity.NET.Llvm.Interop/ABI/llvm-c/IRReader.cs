// -----------------------------------------------------------------------
// <copyright file="IRReader.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    public static partial class IrReader
    {
        // Takes ownership of the provided memory buffer.
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMParseIRInContext(
            LLVMContextRefAlias ContextRef,
            LLVMMemoryBufferRef MemBuf,
            out LLVMModuleRef OutM,
            out string OutMessage
            );
    }
}
