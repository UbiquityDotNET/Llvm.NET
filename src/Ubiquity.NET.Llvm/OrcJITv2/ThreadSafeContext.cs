// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Thread safe ContextAlias for use with LLVM ORC JIT v2</summary>
    public sealed class ThreadSafeContext
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="ThreadSafeContext"/> class.</summary>
        public ThreadSafeContext( )
        {
            Handle = LLVMOrcCreateNewThreadSafeContext().ThrowIfInvalid();
        }

        /// <summary>Adds a module to this context</summary>
        /// <param name="perThreadModule">Module to add</param>
        /// <returns>ThreadSafe module added to this context</returns>
        /// <remarks>
        /// <para>This creates a new <see cref="ThreadSafeModule"/> in this context. The
        /// resulting <see cref="ThreadSafeModule"/> has a ref count on the context and
        /// therefore it is safe (and appropriate) to dispose of this instance.</para>
        /// <note type="important">
        /// Ownership of the input <paramref name="perThreadModule"/> is transferred to
        /// the returned value and is NOT usable after this call completes without exception
        /// (Except to call Dispose which is a NOP). However if an exception occurs, then
        /// ownership remains with the caller. Thus callers should ALWAYS call Dispose on the
        /// result and should NOT assume it is valid.
        /// </note>
        /// </remarks>
        public ThreadSafeModule AddModule( Module perThreadModule )
        {
            ArgumentNullException.ThrowIfNull( perThreadModule );
            ObjectDisposedException.ThrowIf( Handle.IsClosed || Handle.IsInvalid, this );
            using var moduleRef = perThreadModule.GetOwnedHandle();
            var retVal = new ThreadSafeModule(LLVMOrcCreateNewThreadSafeModule(moduleRef, Handle).ThrowIfInvalid());
            moduleRef.SetHandleAsInvalid(); // transfer to native complete.
            return retVal;
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            Handle.Dispose();
        }

        /// <summary>Gets the PerThreadContext for this instance</summary>
        public IContext PerThreadContext
        {
            get
            {
                ObjectDisposedException.ThrowIf( Handle.IsClosed || Handle.IsInvalid, this );

                return new ContextAlias( LLVMOrcThreadSafeContextGetContext( Handle ) );
            }
        }

        internal LLVMOrcThreadSafeContextRef Handle { get; }
    }
}
