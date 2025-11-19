// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1649 // Filename must match type name
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Type_or_Member'

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    public interface IJitMemoryAllocator
    {
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
        nuint AllocateCodeSection(nuint size, UInt32 alignment, UInt32 sectionId, LazyEncodedString sectionName );

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
        nuint AllocateDataSection(nuint size, UInt32 alignment, UInt32 sectionId, LazyEncodedString sectionName, bool isReadOnly);

        /// <summary>Finalizes a previous allocation by applying page settings for the allocation</summary>
        /// <param name="errMsg">Error message in the event of a failure</param>
        /// <returns><see langword="true"/> if successful (<paramref name="errMsg"/> is <see langword="null"/>); <see langword="false"/> if not (<paramref name="errMsg"/> has the reason)</returns>
        bool FinalizeMemory([NotNullWhen(false)] out LazyEncodedString? errMsg);

        /// <summary>Release the context for the memory. No further callbacks will occur for this allocator</summary>
        /// <remarks>
        /// This is similar to a call to <see cref="IDisposable.Dispose"/> except that it releases only the native context
        /// in response to a callback, not the handle for allocator itself. That MUST live longer than the JIT as any memory
        /// it allocated MAY still be in use as code or data in the JIT. (This interface only deals with WHOLE JIT memory
        /// allocation. It is at least plausible to have an allocator per JitDyLib but that would end up needing to leverage
        /// a global one to ensure that section ordering and size limits of the underlying OS are met. If such a things is
        /// ever implemented, it would use a different interface for clarity.)
        /// </remarks>
        void ReleaseContext();
    }
}
