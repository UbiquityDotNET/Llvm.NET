// <copyright file="FPToSI.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FPToSI : Cast
    {
        internal FPToSI( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
