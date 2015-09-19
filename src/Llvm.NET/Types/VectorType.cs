using System;

namespace Llvm.NET.Types
{
    public interface IVectorType
        : ISequenceType
    {
        uint Size { get; }
    }

    internal class VectorType
        : SequenceType
        , IVectorType
    {
        public uint Size => LLVMNative.GetVectorSize( TypeHandle_ );

        internal VectorType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( LLVMNative.GetTypeKind( typeRef ) != LLVMTypeKind.LLVMVectorTypeKind )
                throw new ArgumentException( "Vector type reference expected", nameof( typeRef ) );
        }
    }
}
