// -----------------------------------------------------------------------
// <copyright file="SelectInstruction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Select instruction</summary>
    public sealed class SelectInstruction
        : Instruction
    {
        internal SelectInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
