// <copyright file="PointerType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

// Interface+internal type matches file name
#pragma warning disable SA1649

namespace Llvm.NET.Types
{
    /// <summary>Interface for a pointer type in LLVM</summary>
    public interface IPointerType
        : ISequenceType
    {
        /// <summary>Gets the address space the pointer refers to</summary>
        uint AddressSpace { get; }
    }

    /// <summary>LLVM pointer type</summary>
    internal class PointerType
        : SequenceType
        , IPointerType
    {
        /// <summary>Gets the address space the pointer refers to</summary>
        public uint AddressSpace => NativeMethods.LLVMGetPointerAddressSpace( TypeRefHandle );

        internal PointerType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( NativeMethods.LLVMGetTypeKind( typeRef ) != LLVMTypeKind.LLVMPointerTypeKind )
            {
                throw new ArgumentException( "Pointer type reference expected", nameof( typeRef ) );
            }
        }
    }
}
