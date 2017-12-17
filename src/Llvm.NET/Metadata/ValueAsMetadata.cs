// <copyright file="ValueAsMetadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Llvm.NET.Types;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Used to wrap an <see cref="Llvm.NET.Values.Value"/> in the Metadata hierarchy</summary>
    public class ValueAsMetadata
        : LlvmMetadata
    {
        /// <summary>Gets the <see cref="Value"/> this node wraps</summary>
        public Value Value => Value.FromHandle( LLVMValueAsMetadataGetValue( MetadataHandle ) );

        /// <summary>Gets the type of <see cref="Value"/> this node wraps</summary>
        public ITypeRef Type => Value?.NativeType;

        /// <summary>Gets the <see cref="Context"/> for the <see cref="Value"/> this node wraps</summary>
        public Context Context => Value?.Context;

        /// <summary>Implicit conversion to <see cref="Value"/></summary>
        /// <param name="md"><see cref="ValueAsMetadata"/> to get the value for</param>
        /// <remarks>This is a simple wrapper around the <see cref="Value"/> property</remarks>
        public static implicit operator Value( ValueAsMetadata md ) => md.Value;

        /*private protected*/
        internal ValueAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private static extern LLVMValueRef LLVMValueAsMetadataGetValue( LLVMMetadataRef vmd );
    }
}
