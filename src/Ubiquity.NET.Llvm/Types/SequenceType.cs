﻿// -----------------------------------------------------------------------
// <copyright file="SequenceType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

// Interface+internal type matches file name
#pragma warning disable SA1649

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
