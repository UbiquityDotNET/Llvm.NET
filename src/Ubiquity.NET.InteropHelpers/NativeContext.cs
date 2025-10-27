// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Ubiquity.NET.Extensions;

namespace Ubiquity.NET.InteropHelpers
{
    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be supressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx don't support the new syntax yet)
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

    /// <summary>Operations and extensions for a Native ABI context</summary>
    /// <remarks>
    /// <para>To interop with native ABI callbacks it is important to ensure that any
    /// context pointer provided is valid when the callback is called by native
    /// layers. This type provides support, as extension methods, for doing so using a
    /// <see cref="GCHandle"/>. The handle is allocated in <see cref="AsNativeContext{T}(T)"/>
    /// and then released in <see cref="Release(ref void*)"/>. The actual target is
    /// obtainable via <see cref="TryFrom{T}(void*, out T)"/> which is normally used in
    /// the callback to get back the original managed object the handle refers to.
    /// </para>
    /// <para>Since the handle is allocated the GC will keep the object it refers to alive
    /// until freed. Thus, the callback can use the object it gets. Normally, <see cref="Release(ref void*)"/>
    /// is called from a final callback method but it is also possible that the API guarantees
    /// only a single call to a provided method where it would release the native context
    /// after materializing it to a managed form. (After materialization, normal GC rules
    /// apply to the managed instance such that while it is in scope it is not released by
    /// the GC)
    /// </para>
    /// </remarks>
    public static class NativeContext
    {
        /// <summary>Extension method to get a native consumable context pointer for a managed object</summary>
        /// <typeparam name="T">Type of the managed object</typeparam>
        /// <param name="self">Managed object to get the context for</param>
        /// <returns>Native API consumable "pointer" for a GC handle</returns>
        /// <remarks>
        /// <para>This creates a <see cref="GCHandle"/> from the instance and gets a native ABI
        /// pointer from that. The returned value is a native pointer that is convertible
        /// back to a handle, which can then allow access to the original <see cref="GCHandle.Target"/>.
        /// </para><para>
        /// Normally the materialization of a managed target is done via <see cref="TryFrom{T}(void*, out T)"/>
        /// in a callback implementation. Ultimately the allocated handle is freed in a call to <see cref="Release(ref void*)"/>.
        /// </para>
        /// </remarks>
        [MustUseReturnValue]
        public static unsafe void* AsNativeContext<T>(this T self)
        {
            var handle = GCHandle.Alloc( self );
            try
            {
                return GCHandle.ToIntPtr(handle).ToPointer();
            }
            catch
            {
                handle.Free();
                throw;
            }
        }

        /// <summary>Materializes an instance from a native ABI context as a <c>void*</c></summary>
        /// <typeparam name="T">Type of the managed object to materialize</typeparam>
        /// <param name="ctx">Native context</param>
        /// <param name="value">Materialized value or <see langword="null"/> if not</param>
        /// <returns><see langword="true"/> if the value is succesfully materialized and <see langword="false"/> if not</returns>
        [MustUseReturnValue]
        public static unsafe bool TryFrom<T>(void* ctx, [MaybeNullWhen(false)] out T value)
        {
            if(ctx is not null && GCHandle.FromIntPtr( (nint)ctx ).Target is T managedInst)
            {
                value = managedInst;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>Releases a native context handle</summary>
        /// <param name="ctx">Reference to the context to release</param>
        /// <remarks>
        /// This releases the native context AND sets <paramref name="ctx"/> to <see langword="null"/>
        /// so that no subsequent releases are not possible (or get an exception).
        /// </remarks>
        [SuppressMessage( "Design", "CA1045:Do not pass types by reference", Justification = "Allows controlled reset to null (invalid)" )]
        public static unsafe void Release(ref void* ctx)
        {
            Release(ctx);
            ctx = null;
        }

        /// <summary>Releases the context without resetting it to <see langword="null"/></summary>
        /// <param name="ctx">Context to release</param>
        /// <remarks>
        /// This does NOT re-assign the context as it is intended for use from within a call back
        /// that performs the release. In such a case any field or property of the containing type
        /// should be set to <see langword="null"/> to prevent a double free problem. Use of this
        /// method is discouraged as it requires more care, but is sometimes required.
        /// </remarks>
        public static unsafe void Release(void* ctx)
        {
            Debug.Assert(ctx is not null, "Attempting to release a NULL context - something is likely wrong in the caller!");

            // SAFETY - NOP if null, don't trigger a crash in native code.
            // Debugger assert above will trigger in debug builds.
            if(ctx is not null)
            {
                GCHandle.FromIntPtr((nint)ctx)
                        .Free();
            }
        }
    }
}
