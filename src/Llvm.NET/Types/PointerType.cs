using System;

namespace Llvm.NET.Types
{
    /// <summary>LLVM pointer type</summary>
    public class PointerType
        : SequenceType
    {
        /// <summary>Address space the pointer refers to</summary>
        public uint AddressSpace => LLVMNative.GetPointerAddressSpace( TypeHandle );

        internal PointerType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( LLVMNative.GetTypeKind( typeRef ) != LLVMTypeKind.LLVMPointerTypeKind )
                throw new ArgumentException( "Pointer type reference expected", nameof( typeRef ) );
        }
    }
}
