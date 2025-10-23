// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1649 // Filename must match type name
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.ExecutionEngine;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Base class for a simple MCJIT style memory allocator</summary>
    /// <remarks>
    /// Derived types need not be concrend with any of the low level native interop. Instead
    /// they simply implement the abstract methods to perform the required allocations. This
    /// base type handles all of the interop, including the reverse P/Invoke marhsalling for
    /// callbacks.
    /// </remarks>
    [Experimental("LLVMEXP005")]
    public abstract class SimpleJitMemoryAllocatorBase
        : DisposableObject
        , IJitMemoryAllocator
    {
        protected SimpleJitMemoryAllocatorBase()
        {
            unsafe
            {
                CallbackContext = this.AsNativeContext();

                Handle = LLVMCreateSimpleMCJITMemoryManager(
                    CallbackContext,
                    &MemoryAllocatorNativeCallbacks.AllocateCodeSection,
                    &MemoryAllocatorNativeCallbacks.AllocateDataSection,
                    &MemoryAllocatorNativeCallbacks.FinalizeMemory,
                    &MemoryAllocatorNativeCallbacks.NotifyTerminating
                    );
            }
        }

        /// <inheritdoc/>
        public abstract nuint AllocateCodeSection(nuint size, UInt32 alignment, UInt32 sectionId, LazyEncodedString sectionName );

        /// <inheritdoc/>
        public abstract nuint AllocateDataSection(nuint size, UInt32 alignment, UInt32 sectionId, LazyEncodedString sectionName, bool isReadOnly);

        /// <inheritdoc/>
        public abstract bool FinalizeMemory([NotNullWhen(false)] out LazyEncodedString? errMsg);

        /// <inheritdoc/>
        public virtual void ReleaseContext()
        {
            unsafe
            {
                NativeContext.Release(ref CallbackContext);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );
            if(disposing)
            {
                if(!Handle.IsNull)
                {
                    Handle.Dispose();
                    Handle = default;
                }

                // Releases the allocated handle for the native code
                // might not actually destroy anything. This is INTENTIONALLY done
                // AFTER disposing the handle so that any pending call backs
                // use a valid context. After the memory manager handle is destroyed
                // no more call backs should occur so it is safe to release the
                // context. (Ideally, the ReleaseContext() callback already happened
                // but the LLVM docs, and code comments, are silent on the point. Thus,
                // that uses a ref parameter that is set to null and a null check is
                // applied here.)
                unsafe
                {
                    if(CallbackContext is not null)
                    {
                            NativeContext.Release(ref CallbackContext);
                    }
                }
            }
        }

        private unsafe void* CallbackContext;

        private LLVMMCJITMemoryManagerRef Handle;
    }
}
