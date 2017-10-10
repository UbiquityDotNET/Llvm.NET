// <copyright file="UIToFP.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class UIToFP : Cast
    {
        internal UIToFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
