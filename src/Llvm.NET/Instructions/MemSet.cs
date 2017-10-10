// <copyright file="MemSet.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class MemSet
        : MemIntrinsic
    {
        internal MemSet( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
