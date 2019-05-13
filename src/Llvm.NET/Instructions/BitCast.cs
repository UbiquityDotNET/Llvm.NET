// -----------------------------------------------------------------------
// <copyright file="BitCast.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
