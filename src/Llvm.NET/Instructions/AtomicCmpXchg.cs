// -----------------------------------------------------------------------
// <copyright file="AtomicCmpXchg.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

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
