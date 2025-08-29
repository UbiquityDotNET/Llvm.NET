// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Floating point constant value in LLVM</summary>
    public sealed class ConstantFP
        : ConstantData
    {
        /// <summary>Gets the value of the constant, possibly losing precision</summary>
        public double Value => GetValueWithLoss( out bool _ );

        /// <summary>Gets the value of the constant, possibly losing precision</summary>
        /// <param name="loosesInfo">flag indicating if precision is lost</param>
        /// <returns>The value of the constant</returns>
        /// <remarks>
        /// Loss can occur when getting a target specific high resolution value,
        /// such as an 80bit Floating point value.
        /// </remarks>
        public double GetValueWithLoss( out bool loosesInfo )
        {
            return LLVMConstRealGetDouble( Handle, out loosesInfo );
        }

        internal ConstantFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
