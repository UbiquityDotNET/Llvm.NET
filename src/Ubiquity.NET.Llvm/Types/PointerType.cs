// -----------------------------------------------------------------------
// <copyright file="PointerType.cs" company="Ubiquity.NET Contributors">
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
        public uint AddressSpace => LLVMGetPointerAddressSpace( TypeRefHandle );

        internal PointerType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
            if( LLVMGetTypeKind( typeRef ) != LLVMTypeKind.LLVMPointerTypeKind )
            {
                throw new ArgumentException( Resources.Pointer_type_reference_expected, nameof( typeRef ) );
            }
        }
    }
}
