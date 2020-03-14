// -----------------------------------------------------------------------
// <copyright file="ArrayType.cs" company="Ubiquity.NET Contributors">
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
    internal class ArrayType
        : SequenceType
        , IArrayType
    {
        /// <inheritdoc/>
        public uint Length => LLVMGetArrayLength( TypeRefHandle );

        internal ArrayType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( LLVMGetTypeKind( typeRef ) != LLVMTypeKind.LLVMArrayTypeKind )
            {
                throw new ArgumentException( Resources.Array_type_reference_expected, nameof( typeRef ) );
            }
        }
    }
}
