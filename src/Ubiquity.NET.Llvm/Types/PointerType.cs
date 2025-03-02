// -----------------------------------------------------------------------
// <copyright file="PointerType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

// Interface+internal type matches file name
#pragma warning disable SA1649

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>LLVM pointer type</summary>
    internal sealed class PointerType
        : TypeRef
        , IPointerType
    {
        public uint AddressSpace => LLVMGetPointerAddressSpace( TypeRefHandle );

        public ITypeRef? ElementType { get; internal set; }

        [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Internal interface implementation" )]
        void IPointerType.TrySetElementType(ITypeRef? elementType)
        {
            ElementType ??= elementType;
        }

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
