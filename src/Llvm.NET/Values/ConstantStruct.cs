// <copyright file="ConstantStruct.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    public class ConstantStruct : Constant
    {
        internal ConstantStruct( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
