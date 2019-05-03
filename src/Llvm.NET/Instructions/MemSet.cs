// <copyright file="MemSet.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction for the LLVM intrinsic memset function</summary>
    public class MemSet
        : MemIntrinsic
    {
        internal MemSet( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
