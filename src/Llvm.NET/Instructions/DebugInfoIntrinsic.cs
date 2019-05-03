// <copyright file="DebugInfoIntrinsic.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Base class for debug information intrinsic functions in LLVM IR</summary>
    public class DebugInfoIntrinsic
        : Intrinsic
    {
        internal DebugInfoIntrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
