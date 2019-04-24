// <copyright file="ArrayType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Interop;
using Llvm.NET.Properties;

using static Llvm.NET.Interop.NativeMethods;

// Interface+internal type matches file name
#pragma warning disable SA1649

namespace Llvm.NET.Types
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
