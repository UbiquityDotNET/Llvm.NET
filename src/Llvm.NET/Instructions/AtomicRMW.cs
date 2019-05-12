// -----------------------------------------------------------------------
// <copyright file="AtomicRMW.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Atmoic Read-Modify-Write instruction</summary>
    /// <seealso href="xref:llvm_langref#atomicrmw-instruction">LLVM atomicrmw instruction</seealso>
    public class AtomicRMW
        : Instruction
    {
        internal AtomicRMW( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
