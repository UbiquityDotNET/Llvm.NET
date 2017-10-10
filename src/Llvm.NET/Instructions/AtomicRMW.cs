// <copyright file="AtomicRMW.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

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
