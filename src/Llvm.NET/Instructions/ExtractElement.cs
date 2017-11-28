// <copyright file="ExtractElement.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to extract a single scalar element from a vector at a specified index.</summary>
    /// <seealso href="xref:llvm_langref#extractelement-instruction">LLVM extractelement Instruction</seealso>
    public class ExtractElement
        : Instruction
    {
        internal ExtractElement( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
