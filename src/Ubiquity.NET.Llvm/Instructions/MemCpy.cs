// -----------------------------------------------------------------------
// <copyright file="MemCpy.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction for the LLVM intrinsic llvm.memcpy instruction</summary>
    public sealed class MemCpy
        : MemIntrinsic
    {
        internal MemCpy( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
