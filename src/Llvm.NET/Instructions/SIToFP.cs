// <copyright file="SIToFP.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction for converting a signed integer value into a floating point value</summary>
    public class SIToFP
        : Cast
    {
        internal SIToFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
