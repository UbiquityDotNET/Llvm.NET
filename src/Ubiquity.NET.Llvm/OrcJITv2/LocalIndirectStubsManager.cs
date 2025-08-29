// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Local Indirect Stubs Manager</summary>
    public sealed class LocalIndirectStubsManager
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="LocalIndirectStubsManager"/> class.</summary>
        /// <param name="triple">Triple string for the manager</param>
        public LocalIndirectStubsManager( LazyEncodedString triple )
            : this( MakeHandle( triple ) )
        {
        }

        /// <inheritdoc/>
        public void Dispose( ) => Handle.Dispose();

        internal LocalIndirectStubsManager( LLVMOrcIndirectStubsManagerRef h )
        {
            Handle = h.Move();
        }

        internal LLVMOrcIndirectStubsManagerRef Handle { get; }

        private static LLVMOrcIndirectStubsManagerRef MakeHandle( LazyEncodedString triple, [CallerArgumentExpression( nameof( triple ) )] string? exp = null )
        {
            triple.ThrowIfNullOrWhiteSpace( exp );
            return LLVMOrcCreateLocalIndirectStubsManager( triple );
        }
    }
}
