// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class OrcJITv2Bindings
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMExecutionSessionRemoveDyLib( LLVMOrcExecutionSessionRef session, LLVMOrcJITDylibRef lib );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMOrcSymbolStringPoolIsEmpty(LLVMOrcSymbolStringPoolRef ssp);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LibLLVMOrcSymbolStringPoolGetDiagnosticRepresentation( LLVMOrcSymbolStringPoolRef ssp );
    }
}
