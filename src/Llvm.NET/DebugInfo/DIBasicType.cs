// <copyright file="DIBasicType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information for a basic type</summary>
    /// <seealso cref="Llvm.NET.DebugInfo.DebugInfoBuilder.CreateBasicType(string, ulong, DiTypeKind)"/>
    /// <seealso href="xref:llvm_langref#dibasictype">LLVM DIBasicType</seealso>
    public class DIBasicType
        : DIType
    {
        /// <summary>Gets the encoding for the type</summary>
        public DiTypeKind Encoding => ( DiTypeKind )LLVMDIBasicTypeGetEncoding( MetadataHandle );

        /// <summary>Initializes a new instance of the <see cref="DIBasicType"/> class.</summary>
        /// <param name="handle"><see cref="LLVMMetadataRef"/> for a  DIBasicType to wrap</param>
        internal DIBasicType( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern uint LLVMDIBasicTypeGetEncoding( LLVMMetadataRef /*DIBasicType*/ basicType );
    }
}
