// <copyright file="BitCast.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>This class represents a no-op cast from one type to another</summary>
    public class BitCast
        : Cast
    {
        internal BitCast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
