// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Lazy Call Through Manager</summary>
    public sealed class LazyCallThroughManager
        : IDisposable
    {
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

        // MOVE semantics constructor, this instance will own disposal
        internal LazyCallThroughManager( LLVMOrcLazyCallThroughManagerRef h )
        {
            Handle = h;
        }

        internal LLVMOrcLazyCallThroughManagerRef Handle { get; private set; }
    }
}
