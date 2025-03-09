// -----------------------------------------------------------------------
// <copyright file="ConstantDataSequential.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Text;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>
    /// A vector or array constant whose element type is a simple 1/2/4/8-byte integer
    /// or float/double, and whose elements are just simple data values
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
        public bool IsString => LibLLVMIsConstantCString( ValueHandle );

        /// <summary>Gets a value indicating whether this constant is a sequence of 8bit integral values</summary>
        public bool IsI8Sequence => LLVMIsConstantString( ValueHandle );

        /// <summary>Extract a string value from the constant (Assumes encoding ASCII)</summary>
        /// <returns>Extracted string</returns>
        /// <exception cref="InvalidOperationException">If IsI8Sequence isn't <see langword="true"/></exception>
        public string ExtractAsString( )
        {
            return ExtractAsString(Encoding.ASCII);
        }

        /// <summary>summary</summary>
        /// <param name="encoding">encoding</param>
        /// <returns>string</returns>
        public string ExtractAsString(Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull( encoding );

            // ignore terminating \0 for C strings
            var rawData = IsString ? RawData[0..^1] : RawData;
            return IsI8Sequence ? encoding.GetString( rawData ) : throw new InvalidOperationException( "Value is not a string" );
        }

        /// <summary>Gets the raw Data for the data sequential as a <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/></summary>
        /// <remarks>
        /// This retrieves the underlying data, which may be empty, independent of the actual element type. Thus,
        /// issues of endian mismatch can occur between host assumptions and target. Thus, caution is warranted
        /// when using this property.
        /// </remarks>
        public ReadOnlySpan<byte> RawData
        {
            get
            {
                unsafe
                {
                    return new( LibLLVMGetConstantDataSequentialRawData( ValueHandle, out size_t len ), len );
                }
            }
        }

        /// <summary>Gets the count of elements in this <see cref="ConstantDataSequential"/></summary>
        public uint Count => LibLLVMGetConstantDataSequentialElementCount( ValueHandle );

        internal ConstantDataSequential( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
