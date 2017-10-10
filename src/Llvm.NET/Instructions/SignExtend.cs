// <copyright file="SignExtend.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class SignExtend
        : Cast
    {
        internal SignExtend( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
