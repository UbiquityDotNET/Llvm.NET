// <copyright file="IntCmp.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to perform an integer compare</summary>
    /// <seealso href="xref:llvm_langref#intcmp-instruction">LLVM intcmp Instruction</seealso>
    public class IntCmp
        : Cmp
    {
        internal IntCmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
