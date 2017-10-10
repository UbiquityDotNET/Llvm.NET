// <copyright file="ZeroExtend.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class ZeroExtend
        : Cast
    {
        internal ZeroExtend( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
