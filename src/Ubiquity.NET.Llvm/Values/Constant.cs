// -----------------------------------------------------------------------
// <copyright file="Constant.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;
using Ubiquity.NET.Llvm.Types;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;
using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Contains an LLVM Constant value</summary>
    public class Constant
        : User
    {
        /// <summary>Gets a value indicating whether the constant is a Zero value for the its type</summary>
        public bool IsZeroValue => LibLLVMIsConstantZeroValue( ValueHandle );

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
                throw new ArgumentException( Resources.Cannot_get_null_for_labels_and_opaque_types );
            }

            return FromHandle<Constant>( LLVMConstNull( typeRef.GetTypeRef( ) ).ThrowIfInvalid( ) )!;
        }

        /// <summary>Gets the constant as a LlvmMetadata node</summary>
        /// <returns>Constant value as a metadata constant</returns>
        public ConstantAsMetadata ToMetadata( )
            => LlvmMetadata.FromHandle<ConstantAsMetadata>( Context, LibLLVMConstantAsMetadata( ValueHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Creates a constant instance of <paramref name="typeRef"/> with all bits in the instance set to 1</summary>
        /// <param name="typeRef">Type of value to create</param>
        /// <returns>Constant for the type with all instance bits set to 1</returns>
        public static Constant AllOnesValueFor( ITypeRef typeRef )
            => FromHandle<Constant>( LLVMConstAllOnes( typeRef.GetTypeRef( ) ).ThrowIfInvalid( ) )!;

        /// <summary>Creates an <see cref="Constant"/> representing an undefined value for <paramref name="typeRef"/></summary>
        /// <param name="typeRef">Type to create the undefined value for</param>
        /// <returns>
        /// <see cref="Constant"/> representing an undefined value of <paramref name="typeRef"/>
        /// </returns>
        public static Constant UndefinedValueFor( ITypeRef typeRef )
            => FromHandle<Constant>( LLVMGetUndef( typeRef.GetTypeRef( ) ).ThrowIfInvalid( ) )!;

        /// <summary>Create a constant NULL pointer for a given type</summary>
        /// <param name="typeRef">Type of pointer to create a null value for</param>
        /// <returns>Constant NULL pointer of the specified type</returns>
        public static Constant ConstPointerToNullFor( ITypeRef typeRef )
            => FromHandle<Constant>( LLVMConstPointerNull( typeRef.GetTypeRef( ) ).ThrowIfInvalid( ) )!;

        internal Constant( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
