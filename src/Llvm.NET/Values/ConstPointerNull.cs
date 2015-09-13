using Llvm.NET.Types;

namespace Llvm.NET.Values
{
    public class ConstantPointerNull : Constant
    {
        public static ConstantPointerNull From( TypeRef type )
        {
            return FromHandle<ConstantPointerNull>( LLVMNative.ConstPointerNull( type.TypeHandle ) );
        }

        internal ConstantPointerNull( LLVMValueRef valueRef )
            : this( valueRef, false )
        {
        }

        internal ConstantPointerNull( LLVMValueRef valueRef, bool preValidated )
            : base( preValidated ? valueRef : ValidateConversion( valueRef, LLVMNative.IsAConstantPointerNull ) )
        {
        }
    }
}
