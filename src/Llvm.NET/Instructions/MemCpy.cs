using System.Diagnostics;
using Llvm.NET.Types;

namespace Llvm.NET.Instructions
{
    public class MemCpy
        : MemIntrinsic
    {
        internal MemCpy( LLVMValueRef valueRef )
            : base( ValidateConversion( valueRef, LLVMNative.IsAMemCpyInst ) )
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Language", "CSE0003:Use expression-bodied members", Justification = "Readability" )]
        internal new static MemCpy FromHandle( LLVMValueRef valueRef )
        {
            return (MemCpy)Context.CurrentContext.GetValueFor( valueRef, ( h )=>new MemCpy( h ) );
        }

        internal static string GetIntrinsicNameForArgs( PointerType dst, PointerType src, TypeRef len )
        {
            Debug.Assert( dst != null && dst.ElementType.IsInteger && dst.ElementType.IntegerBitWidth > 0 );
            Debug.Assert( src != null && src.ElementType.IsInteger && src.ElementType.IntegerBitWidth > 0 );
            Debug.Assert( len.IsInteger && len.IntegerBitWidth > 0 );
            return $"llvm.memcpy.p{dst.AddressSpace}i{dst.ElementType.IntegerBitWidth}.p{src.AddressSpace}i{src.ElementType.IntegerBitWidth}.i{len.IntegerBitWidth}";
        }
    }
}
