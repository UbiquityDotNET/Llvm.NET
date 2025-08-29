// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>LLVM pointer type</summary>
    internal sealed class PointerType
        : TypeRef
        , IPointerType
    {
        public uint AddressSpace => LLVMGetPointerAddressSpace( Handle );

        public ITypeRef? ElementType { get; init; }

        internal PointerType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if(LLVMGetTypeKind( typeRef ) != LLVMTypeKind.LLVMPointerTypeKind)
            {
                throw new ArgumentException( Resources.Pointer_type_reference_expected, nameof( typeRef ) );
            }
        }
    }
}
