// -----------------------------------------------------------------------
// <copyright file="IntCmp.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to perform an integer compare</summary>
    /// <seealso href="xref:llvm_langref#intcmp-instruction">LLVM intcmp Instruction</seealso>
    public sealed class IntCmp
        : Cmp
    {
        internal IntCmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
