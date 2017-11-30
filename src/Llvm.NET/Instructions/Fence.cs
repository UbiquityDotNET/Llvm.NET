// <copyright file="Fence.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Synchronization instruction to introduce "happens-before" edges between operations</summary>
    /// <seealso href="xref:llvm_langref#fence-instruction">LLVM fence Instruction</seealso>
    public class Fence
        : Instruction
    {
        internal Fence( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
