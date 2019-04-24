// <copyright file="ConstantAggregateZero.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Values
{
    /// <summary>Constant aggregate of value 0</summary>
    public class ConstantAggregateZero
        : ConstantData
    {
        internal ConstantAggregateZero( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
