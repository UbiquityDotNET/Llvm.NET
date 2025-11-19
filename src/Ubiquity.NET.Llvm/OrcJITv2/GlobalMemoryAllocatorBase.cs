// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.OrcEE;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Base class for a Global allocator for use as an Object layer in OrcJITv2</summary>
    /// <remarks>
    /// <para>Instances of this are provided as an object layer via an implementation of <see cref="ObjectLayerFactory"/>.
    /// While this type implements <see cref="IDisposable"/> via <see cref="ObjectLayer"/> it is ONLY intended for
    /// clean up on exceptions. A factory returns the instances to the native JIT, which takes over ownership.</para>
    /// <para>
    /// Derived types need not be concerned with any of the low level native interop. Instead
    /// they simply implement the abstract methods to perform the required allocations. This
    /// base type handles all of the interop, including the reverse P/Invoke marshaling for
    /// callbacks.
    /// </para>
    /// </remarks>
    [Experimental("LLVMEXP004")]
    public abstract class GlobalMemoryAllocatorBase
        : ObjectLayer
        , IJitMemoryAllocator
    {
        /// <summary>Initializes a new instance of the <see cref="GlobalMemoryAllocatorBase"/> class.</summary>
        /// <param name="session">Session for this object layer</param>
        protected GlobalMemoryAllocatorBase(ExecutionSession session)
            : base()
        {
            unsafe
            {
                CallbackContext = this.AsNativeContext();
                Handle = LLVMOrcCreateRTDyldObjectLinkingLayerWithMCJITMemoryManagerLikeCallbacks(
                            session.Handle,
                            CallbackContext,
                            &MemoryAllocatorNativeCallbacks.CreatePerObjContextAsGlobalContext,
                            &MemoryAllocatorNativeCallbacks.NotifyTerminating, // Releases the global context
                            &MemoryAllocatorNativeCallbacks.AllocateCodeSection,
                            &MemoryAllocatorNativeCallbacks.AllocateDataSection,
                            &MemoryAllocatorNativeCallbacks.FinalizeMemory,
                            &MemoryAllocatorNativeCallbacks.DestroyPerObjContextNOP // NOP
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

        //protected override void Dispose( bool disposing )
        //{
        //   Do NOT overload this, the base will dispose the object layer ReleaseContext() is called to release the call back context
        //}

        private unsafe void* CallbackContext;
    }
}
