// -----------------------------------------------------------------------
// <copyright file="UIToFP.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to cast an unsigned integer to a float</summary>
    public class UIToFP
        : Cast
    {
        internal UIToFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
