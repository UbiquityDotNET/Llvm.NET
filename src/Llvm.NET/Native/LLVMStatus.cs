// <copyright file="Generated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET.Native
{
    internal enum LLVMVerifierFailureAction
    {
        LLVMAbortProcessAction = 0,
        LLVMPrintMessageAction = 1,
        LLVMReturnStatusAction = 2
    }

    // maps to LLVMBool in LLVM-C for methods that return
    // 0 on success. This was hand added to help clarify use
    // when a return value is not really a bool but a status
    // where (0==SUCCESS)
    internal struct LLVMStatus
    {
        public LLVMStatus( int value )
        {
            ErrorCode = value;
        }

        public bool Succeeded => ErrorCode == 0;

        public bool Failed => !Succeeded;

        public int ErrorCode { get; }
    }

    internal static class NativeMethods
    {
        internal const string LibraryPath = "libLLVM";
    }
}
