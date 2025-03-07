// -----------------------------------------------------------------------
// <copyright file="AnalysisBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public static partial class AnalysisBindings
    {
        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LibLLVMVerifyFunctionEx(LLVMValueRef Fn, LLVMVerifierFailureAction Action, out DisposeMessageString OutMessages);
    }
}
