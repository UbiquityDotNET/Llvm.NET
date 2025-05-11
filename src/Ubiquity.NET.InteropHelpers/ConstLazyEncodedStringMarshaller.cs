// -----------------------------------------------------------------------
// <copyright file="ConstLazyEncodedStringMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Represents a marshaller for `char const*` strings that are simple aliases requiring no release</summary>
    /// <remarks>
    /// <para>This marshaller is strictly one-way, from unmanaged pointer into a managed <see cref="LazyEncodedString"/>.
    /// It is normally applied to return values of interop methods.</para>
    /// <para>
    /// This is distinct from <see cref="ExecutionEncodingStringMarshaller"/> in that the <see cref="Free(byte*)"/>
    /// method is explicitly a NOP. There is no release of the data for the string as the string is not any
    /// sort of copy that needs release. This is counter to how normal string marshalling works in .NET but
    /// is actually the most efficient form as it avoids the overhead of the [native allocate], [Native Copy],
    /// [Managed Marshal], [Native Release] sequence. The reduced form used here is [Native return `char const*`],
    /// [copy string], done. Thus the overhead of native allocation, and then release is avoided.
    /// </para>
    /// </remarks>
    [CustomMarshaller(typeof(LazyEncodedString), MarshalMode.ManagedToUnmanagedOut, typeof(ConstLazyEncodedStringMarshaller))]
    public static unsafe class ConstLazyEncodedStringMarshaller
    {
        /// <summary>Converts an unmanaged string to a managed version.</summary>
        /// <param name="unmanaged">The unmanaged string to convert.</param>
        /// <returns>A managed string.</returns>
        /// <seealso cref="ExecutionEncodingStringMarshaller.Encoding"/>
        public static LazyEncodedString? ConvertToManaged(byte* unmanaged)
        {
            return new(unmanaged);
        }

        /// <summary> [Intentional NOP] Frees the memory for the unmanaged string.</summary>
        /// <param name="unmanaged">The memory allocated for the unmanaged string.</param>
        [SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "Required by design, not used here" )]
        public static void Free(byte* unmanaged)
        {
            // Intentional NOP, there is NOTHING to free, this shouldn't even be called but the
            // Source generator has an issue where it ALWAYS generates a call to this with the native
            // pointer. There is no way to specify that there isn't a need to free the string. It is
            // just a `char*` without any allocator - no ownership is transferred, so there's nothing
            // to free/release. The default handling, even for UnmanagedType.LPStr, is to assume the
            // pointer was allocated via the CoTask Allocator and then free it. Which is, of course,
            // wrong for any OUT or return pointers that weren't allocated that way. (The normal case
            // for interop with C/C++) The existing free always assumes the native pointer is allocated
            // via Marshal.AllocCoTaskMem() or a native equivalent so that it can call Marshal.FreeCoTaskMem()
            // on the native pointer.
        }
    }
}
