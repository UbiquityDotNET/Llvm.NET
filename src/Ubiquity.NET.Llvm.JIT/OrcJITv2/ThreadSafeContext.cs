// -----------------------------------------------------------------------
// <copyright file="ThreadSafeContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Thread safe Context for use with LLVM ORC JIT v2</summary>
    public sealed class ThreadSafeContext
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="ThreadSafeContext"/> class.</summary>
        public ThreadSafeContext()
        {
            Handle = LLVMOrcCreateNewThreadSafeContext().ThrowIfInvalid();
        }

        /// <summary>Adds a module to this context</summary>
        /// <param name="perThreadModule">Module to add</param>
        /// <returns>ThreadSafe module added to this context</returns>
        /// <remarks>
        /// This creates a new <see cref="ThreadSafeModule"/> in this context. The
        /// resulting module has a ref count on the context and therefore it is
        /// safe (and appropriate) to dispose of this instance.
        /// </remarks>
        public ThreadSafeModule AddModule(BitcodeModule perThreadModule)
        {
            ArgumentNullException.ThrowIfNull(perThreadModule);
            ObjectDisposedException.ThrowIf(Handle.IsClosed || Handle.IsInvalid, this);

            return new(LLVMOrcCreateNewThreadSafeModule(perThreadModule.ModuleHandle, Handle).ThrowIfInvalid());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Handle.Dispose();
        }

        /// <summary>Gets the PerThreadContext for this instance</summary>
        public Context PerThreadContext
        {
            get
            {
                ObjectDisposedException.ThrowIf(Handle.IsClosed || Handle.IsInvalid, this);

                return ContextCache.GetContextFor(LLVMOrcThreadSafeContextGetContext(Handle));
            }
        }

        internal LLVMOrcThreadSafeContextRef Handle { get; init; }
    }
}
