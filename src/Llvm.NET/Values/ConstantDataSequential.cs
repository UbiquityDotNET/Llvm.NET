using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Native;

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
    public class ConstantDataSequential : Constant
    {
        public bool IsString => NativeMethods.IsConstantString( ValueHandle );

        public string ExtractAsString()
        {
            if( !IsString )
            {
                throw new InvalidOperationException( "Value is not a string" );
            }

            return NativeMethods.GetAsString( ValueHandle, out size_t len );
        }

        internal ConstantDataSequential( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
