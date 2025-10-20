// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.OrcEE;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Base class for an allocator for use as an Object layer in OrcJITv2</summary>
    public abstract class RTDyldObjMemoryAllocatorBase
        : ObjectLayer
        , IJitMemoryAllocator
    {
        /// <summary>Initializes a new instance of the <see cref="RTDyldObjMemoryAllocatorBase"/> class.</summary>
        /// <param name="session">Session for this object layer</param>
        protected RTDyldObjMemoryAllocatorBase(ExecutionSession session)
            : base()
        {
            AllocatedSelf = new(this);
            unsafe
            {
                Handle = LLVMOrcCreateRTDyldObjectLinkingLayerWithMCJITMemoryManagerLikeCallbacks(
                            session.Handle,
                            (void*)AddRefAndGetNativeContext(),
                            &MemoryAllocatorNativeCallbacks.CreateContext,
                            &MemoryAllocatorNativeCallbacks.DestroyMemory, // Notify Terminating
                            &MemoryAllocatorNativeCallbacks.AllocateCodeSection,
                            &MemoryAllocatorNativeCallbacks.AllocateDataSection,
                            &MemoryAllocatorNativeCallbacks.FinalizeMemory,
                            &MemoryAllocatorNativeCallbacks.DestroyContext // NOP
                         );
            }
        }

        /// <summary>Allocate a block of contiguous memory for use as code execution by the native code JIT engine</summary>
        /// <param name="size">Size of the block</param>
        /// <param name="alignment">alignment requirements of the block</param>
        /// <param name="sectionId">ID for the section</param>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>Address of the first byte of the allocated memory</returns>
        /// <remarks>
        /// If the memory is allocated from the managed heap then the returned address <em><i>MUST</i></em>
        /// remain pinned until <see cref="IDisposable.Dispose"/> is called on this allocator
        /// <note type="important">
        /// The Execute only page setting and any other page properties is not applied to the returned
        /// address (or entire memory of the allocated section) until <see cref="FinalizeMemory"/> is called.
        /// This allows the JIT to write code into the memory area even if it is ultimately Execute-Only.
        /// </note>
        /// </remarks>
        public abstract nuint AllocateCodeSection(nuint size, UInt32 alignment, UInt32 sectionId, LazyEncodedString sectionName );

        /// <summary>Allocate a block of contiguous memory for use as data by the native code JIT engine</summary>
        /// <param name="size">Size of the block</param>
        /// <param name="alignment">alignment requirements of the block</param>
        /// <param name="sectionId">ID for the section</param>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="isReadOnly">Memory section is Read-Only</param>
        /// <returns>Address of the first byte of the allocated memory</returns>
        /// <remarks>
        /// If the memory is allocated from the managed heap then the returned address <em><i>MUST</i></em>
        /// remain pinned until <see cref="IDisposable.Dispose"/> is called on this allocator.
        /// <note type="important">
        /// The <paramref name="isReadOnly"/> and any other page properties is not applied to the returned
        /// address (or entire memory of the allocated section) until <see cref="FinalizeMemory"/> is called.
        /// This allows the JIT to write initial data into the memory even if it is ultimately Read-Only.
        /// </note>
        /// </remarks>
        public abstract nuint AllocateDataSection(nuint size, UInt32 alignment, UInt32 sectionId, LazyEncodedString sectionName, bool isReadOnly);

        /// <summary>Finalizes a previous allocation by applying page settings for the allocation</summary>
        /// <param name="errMsg">Error message in the event of a failure</param>
        /// <returns><see langword="true"/> if successfull (<paramref name="errMsg"/> is <see langword="null"/>); <see langword="false"/> if not (<paramref name="errMsg"/> has the reason)</returns>
        public abstract bool FinalizeMemory([NotNullWhen(false)] out LazyEncodedString? errMsg);

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if(disposing && !AllocatedSelf.IsInvalid && !AllocatedSelf.IsClosed)
            {
                // Decrements the ref count on the handle
                // might not actually destroy anything
                AllocatedSelf.Dispose();
            }

            base.Dispose( disposing );
        }

        internal unsafe nint AddRefAndGetNativeContext( )
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            return AllocatedSelf.AddRefAndGetNativeContext();
        }

        // This is the key to ref counted behavior to hold this instance (and anything it references)
        // alive for the GC. The "ownership" of the refcount is handed to native code while the
        // calling code is free to no longer reference this instance as it holds an allocated
        // GCHandle for itself and THAT is kept alive by a ref count that is "owned" by native code.
        private SafeGCHandle AllocatedSelf { get; }
    }
}
