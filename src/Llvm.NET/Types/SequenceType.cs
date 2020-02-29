// -----------------------------------------------------------------------
// <copyright file="SequenceType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Llvm.NET.Interop;
using Llvm.NET.Properties;

using static Llvm.NET.Interop.NativeMethods;

// Interface+internal type matches file name
#pragma warning disable SA1649

namespace Llvm.NET.Types
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
        public ITypeRef ElementType
        {
            get
            {
                var typeRef = LLVMGetElementType( this.GetTypeRef() );
                return FromHandle( typeRef.ThrowIfInvalid( ) )!;
            }
        }

        internal SequenceType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( !IsSequenceTypeRef( typeRef ) )
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
