// -----------------------------------------------------------------------
// <copyright file="Trunc.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Integer truncate instruction</summary>
    public sealed class Trunc
        : Cast
    {
        internal Trunc( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
