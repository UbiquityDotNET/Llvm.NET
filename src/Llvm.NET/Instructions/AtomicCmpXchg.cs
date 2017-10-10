// <copyright file="AtomicCmpXchg.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Atomic Compare and Exchange instruction</summary>
    /// <seealso href="xref:llvm_langref#cmpxchg-instruction">LLVM cmpxchg instruction</seealso>
    public class AtomicCmpXchg
        : Instruction
    {
        internal AtomicCmpXchg( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
