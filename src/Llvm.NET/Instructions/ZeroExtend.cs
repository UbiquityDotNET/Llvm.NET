// -----------------------------------------------------------------------
// <copyright file="ZeroExtend.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to zero extend a value</summary>
    public class ZeroExtend
        : Cast
    {
        internal ZeroExtend( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
