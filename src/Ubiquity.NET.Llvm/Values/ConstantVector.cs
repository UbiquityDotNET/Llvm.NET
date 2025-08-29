// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Values
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
