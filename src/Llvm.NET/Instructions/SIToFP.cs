// <copyright file="SIToFP.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class SIToFP : Cast
    {
        internal SIToFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
