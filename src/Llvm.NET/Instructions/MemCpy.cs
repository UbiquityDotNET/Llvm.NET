// <copyright file="MemCpy.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction for the LLVM intrinsic llvm.memcpy instruction</summary>
    public class MemCpy
        : MemIntrinsic
    {
        internal MemCpy( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
