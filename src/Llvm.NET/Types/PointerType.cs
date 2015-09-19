using System;

namespace Llvm.NET.Types
{
    public interface IPointerType
        : ISequenceType
    {
        uint AddressSpace { get; }
    }

    /// <summary>LLVM pointer type</summary>
    internal class PointerType
        : SequenceType
        , IPointerType
    {
        /// <summary>Address space the pointer refers to</summary>
        public uint AddressSpace => LLVMNative.GetPointerAddressSpace( TypeHandle_ );

        internal PointerType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( LLVMNative.GetTypeKind( typeRef ) != LLVMTypeKind.LLVMPointerTypeKind )
                throw new ArgumentException( "Pointer type reference expected", nameof( typeRef ) );
        }
    }
}
