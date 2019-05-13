// -----------------------------------------------------------------------
// <copyright file="SignExtend.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Sign extension instruction</summary>
    public class SignExtend
        : Cast
    {
        internal SignExtend( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
