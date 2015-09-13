using System;

namespace Llvm.NET.Types
{
    public class VectorType : SequenceType
    {
        public uint Size => LLVMNative.GetVectorSize( TypeHandle );

        internal VectorType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( LLVMNative.GetTypeKind( typeRef ) != LLVMTypeKind.LLVMVectorTypeKind )
                throw new ArgumentException( "Vector type reference expected", nameof( typeRef ) );
        }
    }
}
