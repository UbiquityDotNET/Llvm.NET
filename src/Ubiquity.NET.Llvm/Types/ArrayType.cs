// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Interface+internal type matches file name
#pragma warning disable SA1649

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>Interface for an LLVM array type </summary>
    public interface IArrayType
        : ISequenceType
    {
        /// <summary>Gets the length of the array</summary>
        uint Length { get; }
    }

    /// <summary>Array type definition</summary>
    /// <remarks>
    /// Array's in LLVM are fixed length sequences of elements
    /// </remarks>
    internal sealed class ArrayType
        : SequenceType
        , IArrayType
    {
        /// <inheritdoc/>
        public uint Length => LLVMGetArrayLength( Handle );

        internal ArrayType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if(LLVMGetTypeKind( typeRef ) != LLVMTypeKind.LLVMArrayTypeKind)
            {
                throw new ArgumentException( Resources.Array_type_reference_expected, nameof( typeRef ) );
            }
        }
    }
}
