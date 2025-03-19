// -----------------------------------------------------------------------
// <copyright file="ReturnInstruction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Return instruction</summary>
    public sealed class ReturnInstruction
        : Terminator
    {
        internal ReturnInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
