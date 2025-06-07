// -----------------------------------------------------------------------
// <copyright file="AnalysisBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class AnalysisBindings
    {
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LibLLVMVerifyFunctionEx(
            LLVMValueRef Fn,
            LLVMVerifierFailureAction Action,
            out string OutMessages );
    }
}
