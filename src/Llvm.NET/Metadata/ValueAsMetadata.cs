// -----------------------------------------------------------------------
// <copyright file="ValueAsMetadata.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Interop;
using Llvm.NET.Types;
using Llvm.NET.Values;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Used to wrap an <see cref="Llvm.NET.Values.Value"/> in the Metadata hierarchy</summary>
    public class ValueAsMetadata
        : LlvmMetadata
    {
        /// <summary>Gets the <see cref="Value"/> this node wraps</summary>
        public Value Value => Value.FromHandle( LibLLVMValueAsMetadataGetValue( MetadataHandle ) );

        /// <summary>Gets the type of <see cref="Value"/> this node wraps</summary>
        public ITypeRef Type => Value?.NativeType;

        /// <summary>Gets the <see cref="Context"/> for the <see cref="Value"/> this node wraps</summary>
        public Context Context => Value?.Context;

        /// <summary>Implicit conversion to <see cref="Value"/></summary>
        /// <param name="md"><see cref="ValueAsMetadata"/> to get the value for</param>
        /// <remarks>This is a simple wrapper around the <see cref="Value"/> property</remarks>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "Value property provides this functionality already" )]
        public static implicit operator Value( ValueAsMetadata md ) => md.Value;

        private protected ValueAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
