// <copyright file="ConstantVector.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    public class ConstantVector : Constant
    {
        internal ConstantVector( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
