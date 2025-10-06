// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
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
        {
            triple.ThrowIfNullOrWhiteSpace( );
            Handle = LLVMOrcCreateLocalIndirectStubsManager( triple );
        }

        /// <inheritdoc/>
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "Ownership transferred in constructor")]
        public void Dispose( )
        {
            if(!Handle.IsNull)
            {
                Handle.Dispose();
                Handle = default;
            }
        }

        internal LocalIndirectStubsManager( LLVMOrcIndirectStubsManagerRef h )
        {
            Handle = h;
        }

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP008:Don't assign member with injected and created disposables", Justification = "Constructor uses move semantics")]
        internal LLVMOrcIndirectStubsManagerRef Handle { get; private set; }
    }
}
