// <copyright file="Constant.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Types;

using static Llvm.NET.Types.TypeRef.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>Contains an LLVM Constant value</summary>
    public class Constant
        : User
    {
        /// <summary>Gets a value indicating whether the constant is a Zero value for the its type</summary>
        public bool IsZeroValue => NativeMethods.LLVMIsConstantZeroValue( ValueHandle );

        /// <summary>Create a NULL pointer for a given type</summary>
        /// <param name="typeRef">Type of pointer to create a null vale for</param>
        /// <returns>Constant NULL pointer of the specified type</returns>
        public static Constant NullValueFor( ITypeRef typeRef )
        {
            if( typeRef == null )
            {
                throw new ArgumentNullException( nameof( typeRef ) );
            }

            var kind = typeRef.Kind;
            if( kind == TypeKind.Label || kind == TypeKind.Function || ( typeRef is StructType structType && structType.IsOpaque ) )
            {
                throw new ArgumentException( "Cannot get a Null value for labels, functions and opaque types" );
            }

            return FromHandle<Constant>( LLVMConstNull( typeRef.GetTypeRef( ) ) );
        }

        /// <summary>Creates a constant instance of <paramref name="typeRef"/> with all bits in the instance set to 1</summary>
        /// <param name="typeRef">Type of value to create</param>
        /// <returns>Constant for the type with all instance bits set to 1</returns>
        public static Constant AllOnesValueFor( ITypeRef typeRef ) => FromHandle<Constant>( LLVMConstAllOnes( typeRef.GetTypeRef( ) ) );

        /// <summary>Creates an <see cref="Constant"/> representing an undefined value for <paramref name="typeRef"/></summary>
        /// <param name="typeRef">Type to create the undefined value for</param>
        /// <returns>
        /// <see cref="Constant"/> representing an undefined value of <paramref name="typeRef"/>
        /// </returns>
        public static Constant UndefinedValueFor( ITypeRef typeRef ) => FromHandle<Constant>( LLVMGetUndef( typeRef.GetTypeRef( ) ) );

        /// <summary>Create a constant NULL pointer for a given type</summary>
        /// <param name="typeRef">Type of pointer to create a null value for</param>
        /// <returns>Constant NULL pointer of the specified type</returns>
        public static Constant ConstPointerToNullFor( ITypeRef typeRef ) => FromHandle<Constant>( LLVMConstPointerNull( typeRef.GetTypeRef( ) ) );

        internal Constant( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
