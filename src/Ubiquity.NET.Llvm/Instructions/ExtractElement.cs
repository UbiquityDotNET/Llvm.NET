// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
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
