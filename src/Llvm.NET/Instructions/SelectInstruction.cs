// -----------------------------------------------------------------------
// <copyright file="SelectInstruction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Select instruction</summary>
    public class SelectInstruction
        : Instruction
    {
        internal SelectInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
