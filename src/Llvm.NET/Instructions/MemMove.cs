// -----------------------------------------------------------------------
// <copyright file="MemMove.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Intrinsic call to target optimized memmove</summary>
    public class MemMove
        : MemIntrinsic
    {
        internal MemMove( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
