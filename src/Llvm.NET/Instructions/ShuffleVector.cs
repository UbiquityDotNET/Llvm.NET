// -----------------------------------------------------------------------
// <copyright file="ShuffleVector.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to shuffle the elements of a vector</summary>
    public class ShuffleVector
        : Instruction
    {
        internal ShuffleVector( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
