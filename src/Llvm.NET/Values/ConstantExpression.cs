using System;
using Llvm.NET.Types;

namespace Llvm.NET.Values
{
    /// <summary>While techincaly a type in LLVM ConstantExpression is primarily a static factory for Constants</summary>
    public class ConstantExpression
        : Constant
    {
        public Opcode Opcode => (Opcode)LLVMNative.GetConstOpcode( ValueHandle );

        public static Constant IntToPtrExpression( Constant value, ITypeRef type )
        {
            if( value.Type.Kind != TypeKind.Integer )
                throw new ArgumentException( "Integer Type expected", nameof( value ) );

            if( !( type is PointerType ) )
                throw new ArgumentException( "pointer type expected", nameof( type ) );

            return FromHandle<Constant>( LLVMNative.ConstIntToPtr( value.ValueHandle, type.GetTypeRef() ) );
        }

        public static Constant BitCast( Constant value, ITypeRef toType )
        {
            return FromHandle<Constant>( LLVMNative.ConstBitCast( value.ValueHandle, toType.GetTypeRef() ) );
        }

        internal ConstantExpression( LLVMValueRef valueRef )
            : this( valueRef, false )
        {
        }

        internal ConstantExpression( LLVMValueRef valueRef, bool preValidated )
            : base( preValidated ? valueRef : ValidateConversion( valueRef, LLVMNative.IsAConstantExpr ) )
        {
        }

    }
}
