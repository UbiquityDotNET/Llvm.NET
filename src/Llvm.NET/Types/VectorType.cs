// <copyright file="VectorType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.Types
{
    public interface IVectorType
        : ISequenceType
    {
        uint Size { get; }
    }

    internal class VectorType
        : SequenceType
        , IVectorType
    {
        public uint Size => NativeMethods.GetVectorSize( TypeRefHandle );

        internal VectorType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( NativeMethods.GetTypeKind( typeRef ) != LLVMTypeKind.LLVMVectorTypeKind )
            {
                throw new ArgumentException( "Vector type reference expected", nameof( typeRef ) );
            }
        }
    }
}
