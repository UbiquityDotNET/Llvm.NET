// -----------------------------------------------------------------------
// <copyright file="ConstantExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>While technically a type in LLVM, ConstantExpression is primarily a static factory for Constants</summary>
    public class ConstantExpression
        : Constant
    {
        /// <summary>Gets the constant instruction expression op code</summary>
        public OpCode OpCode => ( OpCode )LLVMGetConstOpcode( Handle );

        /// <summary>Gets an IntToPtr expression to convert an integral value to a pointer</summary>
        /// <param name="value">Constant value to cast to a pointer</param>
        /// <param name="type">Type of the pointer to cast <paramref name="value"/> to</param>
        /// <returns>New pointer constant</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public static Constant IntToPtrExpression( Constant value, ITypeRef type )
        {
            ArgumentNullException.ThrowIfNull( value );

            if( value.NativeType.Kind != TypeKind.Integer )
            {
                throw new ArgumentException( Resources.Integer_type_expected, nameof( value ) );
            }

            if( type is not IPointerType)
            {
                throw new ArgumentException( Resources.Pointer_type_expected, nameof( type ) );
            }

            LLVMValueRef valueRef = LLVMConstIntToPtr( value.Handle, type.GetTypeRef( ) );
            return FromHandle<Constant>( valueRef.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a constant bit cast expression</summary>
        /// <param name="value">value to cast</param>
        /// <param name="toType">Type to cast to</param>
        /// <returns>Constant cast expression</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public static Constant BitCast( Constant value, ITypeRef toType )
        {
            ArgumentNullException.ThrowIfNull( value );

            var handle = LLVMConstBitCast( value.Handle, toType.GetTypeRef( ) );
            return FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        internal ConstantExpression( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
