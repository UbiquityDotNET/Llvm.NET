// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
// Interface+internal type matches file name
#pragma warning disable SA1649

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>Interface for an LLVM sequence type</summary>
    /// <remarks>
    /// Sequence types represent a sequence of elements of the same type
    /// that are contiguous in memory. These include Vectors, Arrays, and
    /// pointers.
    /// </remarks>
    public interface ISequenceType
        : ITypeRef
    {
        /// <summary>Gets the types of the elements in the sequence</summary>
        ITypeRef ElementType { get; }
    }

    internal class SequenceType
        : TypeRef
        , ISequenceType
    {
        public ITypeRef ElementType => LLVMGetElementType( this.GetTypeRef() ).CreateType();

        internal SequenceType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if(!IsSequenceTypeRef( typeRef ))
            {
                throw new ArgumentException( Resources.Expected_a_sequence_type, nameof( typeRef ) );
            }
        }

        internal static bool IsSequenceTypeRef( LLVMTypeRef typeRef )
        {
            var kind = ( TypeKind )LLVMGetTypeKind( typeRef );
            return kind == TypeKind.Array
                || kind == TypeKind.Vector
                || kind == TypeKind.Pointer;
        }
    }
}
