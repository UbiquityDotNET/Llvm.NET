// <copyright file="FPTrunc.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FPTrunc : Cast
    {
        internal FPTrunc( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
