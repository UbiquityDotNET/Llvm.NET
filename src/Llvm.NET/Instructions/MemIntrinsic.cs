// <copyright file="MemIntrinsic.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Base class for memory intrinsic instructions</summary>
    public class MemIntrinsic
        : Intrinsic
    {
        internal MemIntrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
