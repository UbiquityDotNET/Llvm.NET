// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Used to wrap an <see cref="Ubiquity.NET.Llvm.Values.Value"/> in the IrMetadata hierarchy</summary>
    public class ValueAsMetadata
        : IrMetadata
    {
        /// <summary>Gets the <see cref="Value"/> this node wraps</summary>
        public Value? Value => Value.FromHandle( LibLLVMValueAsMetadataGetValue( Handle ) );

        /// <summary>Gets the type of <see cref="Value"/> this node wraps</summary>
        public ITypeRef? Type => Value?.NativeType;

        /// <summary>Gets the <see cref="Context"/> for the <see cref="Value"/> this node wraps</summary>
        public IContext? Context => Value?.Context;

        /// <summary>Implicit conversion to <see cref="Value"/></summary>
        /// <param name="md"><see cref="ValueAsMetadata"/> to get the value for</param>
        /// <remarks>This is a simple wrapper around the <see cref="Value"/> property</remarks>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "Value property provides this functionality already" )]
        public static implicit operator Value?( ValueAsMetadata md ) => md.ThrowIfNull().Value;

        private protected ValueAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
