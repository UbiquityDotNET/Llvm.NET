// <copyright file="VectorType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Properties;

// Interface+internal type matches file name
#pragma warning disable SA1649

namespace Llvm.NET.Types
{
    /// <summary>Interface for an LLVM vector type</summary>
    public interface IVectorType
        : ISequenceType
    {
        /// <summary>Gets the number of elements in the vector</summary>
        uint Size { get; }
    }

    internal class VectorType
        : SequenceType
        , IVectorType
    {
        public uint Size => NativeMethods.LLVMGetVectorSize( TypeRefHandle );

        internal VectorType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( NativeMethods.LLVMGetTypeKind( typeRef ) != LLVMTypeKind.LLVMVectorTypeKind )
            {
                throw new ArgumentException( Resources.Vector_type_reference_expected, nameof( typeRef ) );
            }
        }
    }
}
