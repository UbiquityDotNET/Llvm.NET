// -----------------------------------------------------------------------
// <copyright file="MemSet.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
