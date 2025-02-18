// -----------------------------------------------------------------------
// <copyright file="AnalysisBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LibLLVMVerifyFunctionEx(LLVMValueRef Fn, LLVMVerifierFailureAction Action, [MarshalUsing( typeof( DisposeMessageMarshaller ) )] out string OutMessages);
    }
}
