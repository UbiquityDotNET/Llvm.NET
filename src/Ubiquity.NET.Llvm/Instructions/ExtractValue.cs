// -----------------------------------------------------------------------
// <copyright file="ExtractValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to extract the value of a member field from an aggregate value</summary>
    /// <seealso href="xref:llvm_langref#extractvalue-instruction">LLVM extractvalue Instruction</seealso>
    public sealed class ExtractValue
        : UnaryInstruction
    {
        internal ExtractValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
