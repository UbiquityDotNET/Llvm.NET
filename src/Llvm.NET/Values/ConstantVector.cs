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

        /* TODO: add support for Splat to ConstantVector
        [CanBeNull]
        Constant SplatValue { get; }

        // result may be ConstantVector or ConstantDataVector if the splat is compatible with a ConstantDataVector
        static Constant Splat(uint numelements, Constant element);
        */
    }
}
