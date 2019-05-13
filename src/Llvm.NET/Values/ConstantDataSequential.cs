// -----------------------------------------------------------------------
// <copyright file="ConstantDataSequential.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>
    /// A vector or array constant whose element type is a simple 1/2/4/8-byte integer
    /// or float/double, and whose elements are just  simple data values
    /// (i.e. ConstantInt/ConstantFP).
    /// </summary>
    /// <remarks>
    /// This Constant node has no operands because
    /// it stores all of the elements of the constant as densely packed data, instead
    /// of as <see cref="Value"/>s
    /// </remarks>
    public class ConstantDataSequential
        : ConstantData
    {
        /// <summary>Gets a value indicating whether this constant is a string</summary>
        public bool IsString => LLVMIsConstantString( ValueHandle );

        /// <summary>Extract a string value from the constant</summary>
        /// <returns>Extracted string</returns>
        /// <exception cref="InvalidOperationException">If the constant isn't a string</exception>
        public string ExtractAsString()
        {
            if( !IsString )
            {
                throw new InvalidOperationException( "Value is not a string" );
            }

            return LLVMGetAsString( ValueHandle, out size_t _ );
        }

        internal ConstantDataSequential( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
