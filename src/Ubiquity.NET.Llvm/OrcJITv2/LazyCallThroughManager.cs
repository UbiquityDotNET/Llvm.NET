// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Lazy Call Through Manager</summary>
    public sealed class LazyCallThroughManager
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose( )
        {
            Handle.Dispose();
        }

        internal LazyCallThroughManager( LLVMOrcLazyCallThroughManagerRef h )
        {
            Handle = h.Move();
        }

        internal LLVMOrcLazyCallThroughManagerRef Handle { get; init; }
    }
}
