// <copyright file="ConstantFP.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Values
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
            return LLVMConstRealGetDouble( ValueHandle, out loosesInfo );
        }

        internal ConstantFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
