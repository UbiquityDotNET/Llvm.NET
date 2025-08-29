// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Interface+internal type matches file name
#pragma warning disable SA1649

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>Interface for an LLVM vector type</summary>
    public interface IVectorType
        : ISequenceType
    {
        /// <summary>Gets the number of elements in the vector</summary>
        uint Size { get; }
    }

    internal sealed class VectorType
        : SequenceType
        , IVectorType
    {
        public uint Size => LLVMGetVectorSize( Handle );

        internal VectorType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if(LLVMGetTypeKind( typeRef ) != LLVMTypeKind.LLVMVectorTypeKind)
            {
                throw new ArgumentException( Resources.Vector_type_reference_expected, nameof( typeRef ) );
            }
        }
    }
}
