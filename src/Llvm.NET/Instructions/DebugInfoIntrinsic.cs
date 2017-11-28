// <copyright file="DebugInfoIntrinsic.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Base class for deubg information instrinsic functions in LLVM IR</summary>
    public class DebugInfoIntrinsic
        : Intrinsic
    {
        internal DebugInfoIntrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
