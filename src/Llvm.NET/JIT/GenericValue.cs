// <copyright file="GenericValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Properties;
using Llvm.NET.Types;
using Ubiquity.ArgValidators;

using static Llvm.NET.JIT.NativeMethods;

namespace Llvm.NET.JIT
{
    /// <summary>LLVM JIT discriminated union for a generic primitive value</summary>
    public sealed class GenericValue
    {
        /// <summary>Initializes a new instance of the <see cref="GenericValue"/> class with an integer value</summary>
        /// <param name="t">LLVM type describing the integer bit width</param>
        /// <param name="value">integer value</param>
        /// <param name="isSigned">Indicates if the value is signed</param>
        public GenericValue( ITypeRef t, UInt64 value, bool isSigned )
        {
            if( !t.ValidateNotNull( nameof( t ) ).IsInteger )
            {
                throw new ArgumentException( Resources.Type_must_be_an_integral_data_type, nameof( t ) );
            }

            Handle = LLVMCreateGenericValueOfInt( t.GetTypeRef( ), value, isSigned );
        }

        /// <summary>Initializes a new instance of the <see cref="GenericValue"/> class with a floating point value</summary>
        /// <param name="t">LLVM type describing the floating point format</param>
        /// <param name="value">floating point value</param>
        public GenericValue( ITypeRef t, double value )
        {
            if( !t.ValidateNotNull( nameof( t ) ).IsFloatingPoint )
            {
                throw new ArgumentException( Resources.Type_must_be_a_floating_point_data_type, nameof( t ) );
            }

            Handle = LLVMCreateGenericValueOfFloat( t.GetTypeRef( ), value );
        }

        /// <summary>Gets the bit width of the integer value</summary>
        public int IntegerBitWidth => ( int )LLVMGenericValueIntWidth( Handle );

        /// <summary>Gets the value as an <see cref="Int32"/></summary>
        public int ToInt32 => ( int )LLVMGenericValueToInt( Handle, true );

        /// <summary>Gets the value as an <see cref="UInt32"/></summary>
        public UInt32 ToUInt32 => ( UInt32 )LLVMGenericValueToInt( Handle, false );

        /// <summary>Gets the value as a <see cref="Single"/></summary>
        /// <param name="ctx">Context to use for the LLVM float type definition</param>
        /// <returns>Floating point value</returns>
        public float ToFloat( Context ctx ) => ( float )LLVMGenericValueToFloat( ctx.FloatType.GetTypeRef(), Handle );

        /// <summary>Gets the value as a <see cref="Double"/></summary>
        /// <param name="ctx">Context to use for the LLVM double type definition</param>
        /// <returns>Floating point value</returns>
        public double ToDouble( Context ctx ) => LLVMGenericValueToFloat( ctx.DoubleType.GetTypeRef( ), Handle );

        private readonly LLVMGenericValueRef Handle;
    }
}
