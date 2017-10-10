// <copyright file="MemIntrinsic.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class MemIntrinsic
        : Intrinsic
    {
        internal MemIntrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
