#if PROBABLY_DELETE_ME_LATER_AS_LAZYENCODEDSTRING_IS_BETTER
// -----------------------------------------------------------------------
// <copyright file="ConstStringMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Represents a marshaller for `char const*` strings that are simple aliases requiring no release</summary>
    /// <remarks>
    /// <para>This marshaller is strictly one-way, from unmanaged pointer into a managed <see cref="string"/>.
    /// It is normally applied to return values of interop methods. For support of the other direction
    /// use <see cref="ExecutionEncodingStringMarshaller"/>.</para>
    /// <para>
    /// This is distinct from <see cref="ExecutionEncodingStringMarshaller"/> in that the <see cref="Free(byte*)"/>
    /// method is explicitly a NOP. There is no release of the data for the string as the string is not any
    /// sort of copy that needs release. This is counter to how normal string marshalling works in .NET but
    /// is actually the most efficient form as it avoids the overhead of the [native allocate], [Native Copy],
    /// [Managed Marshal], [Native Release]. The reduced form used here is [Native return `char const*`],
    /// [Managed Marshal], done. Thus the overhead of native allocation, copy and then release is avoided.
    /// The managed marshal needs to copy the characters to convert from the native encoding to UTF16 expected
    /// as a string for .NET
    /// </para>
    /// </remarks>
    [CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ConstStringMarshaller))]
    public static unsafe class ConstStringMarshaller
    {
        /// <summary>Converts an unmanaged string to a managed version.</summary>
        /// <param name="unmanaged">The unmanaged string to convert.</param>
        /// <returns>A managed string.</returns>
        /// <seealso cref="ExecutionEncodingStringMarshaller.Encoding"/>
        public static string? ConvertToManaged(byte* unmanaged)
        {
            return ExecutionEncodingStringMarshaller.ConvertToManaged(unmanaged);
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
#endif
