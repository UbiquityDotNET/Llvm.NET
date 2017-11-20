// <copyright file="ConstantVector.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Vector of constant values</summary>
    public sealed class ConstantVector
        : ConstantAggregate
    {
        internal ConstantVector( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        /* TODO:
        Constant SplatValue { get; }
        static ConstantVector Splat(uint numelements, Constant element);
        */
    }
}
