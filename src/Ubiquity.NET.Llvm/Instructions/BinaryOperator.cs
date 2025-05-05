// -----------------------------------------------------------------------
// <copyright file="BinaryOperator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Base class for a binary operator</summary>
    public sealed class BinaryOperator
        : Instruction
    {
        internal BinaryOperator( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
