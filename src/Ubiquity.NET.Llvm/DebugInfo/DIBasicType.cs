﻿// -----------------------------------------------------------------------
// <copyright file="DIBasicType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information for a basic type</summary>
    /// <seealso cref="Ubiquity.NET.Llvm.DebugInfo.DIBuilder.CreateBasicType(LazyEncodedString, ulong, DiTypeKind, DebugInfoFlags)"/>
    /// <seealso href="xref:llvm_langref#dibasictype">LLVM DIBasicType</seealso>
    public class DIBasicType
        : DIType
    {
        /// <summary>Gets the encoding for the type</summary>
        public DiTypeKind Encoding => ( DiTypeKind )LibLLVMDIBasicTypeGetEncoding( Handle );

        /// <summary>Initializes a new instance of the <see cref="DIBasicType"/> class.</summary>
        /// <param name="handle"><see cref="LLVMMetadataRef"/> for a  DIBasicType to wrap</param>
        internal DIBasicType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
