// <copyright file="SequenceType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

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
                var typeRef = NativeMethods.LLVMGetElementType( this.GetTypeRef() );
                if( typeRef == default )
                {
                    return null;
                }

                return FromHandle( typeRef );
            }
        }

        internal SequenceType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( !IsSequenceTypeRef( typeRef ) )
            {
                throw new ArgumentException( "Expected a sequence type", nameof( typeRef ) );
            }
        }

        internal static bool IsSequenceTypeRef( LLVMTypeRef typeRef )
        {
            var kind = ( TypeKind )NativeMethods.LLVMGetTypeKind( typeRef );
            return kind == TypeKind.Array
                || kind == TypeKind.Vector
                || kind == TypeKind.Pointer;
        }
    }
}
