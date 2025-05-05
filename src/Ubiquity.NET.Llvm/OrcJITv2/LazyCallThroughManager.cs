// -----------------------------------------------------------------------
// <copyright file="LazyCallThroughManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Lazy Call Through Manager</summary>
    public sealed class LazyCallThroughManager
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose()
        {
            Handle.Dispose();
        }

        internal LazyCallThroughManager(LLVMOrcLazyCallThroughManagerRef h)
        {
            Handle = h.Move();
        }

        internal LLVMOrcLazyCallThroughManagerRef Handle { get; init; }
    }
}
