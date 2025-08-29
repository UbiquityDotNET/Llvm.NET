// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Synchronization instruction to introduce "happens-before" edges between operations</summary>
    /// <seealso href="xref:llvm_langref#fence-instruction">LLVM fence Instruction</seealso>
    public sealed class Fence
        : Instruction
    {
        internal Fence( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
