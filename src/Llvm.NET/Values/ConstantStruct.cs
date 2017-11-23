// <copyright file="ConstantStruct.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Constant Structure</summary>
    public sealed class ConstantStruct
        : ConstantAggregate
    {
        internal ConstantStruct( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
