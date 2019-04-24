// <copyright file="Trunc.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Integer truncate instruction</summary>
    public class Trunc
        : Cast
    {
        internal Trunc( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
