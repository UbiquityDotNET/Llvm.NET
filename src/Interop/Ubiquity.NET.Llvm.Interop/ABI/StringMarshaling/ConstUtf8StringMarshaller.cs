// -----------------------------------------------------------------------
// <copyright file="ConstUtf8StringMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices.Marshalling
{
    /// <summary>
    /// Represents a marshaller for Const UTF8 strings.
    /// </summary>
    [CLSCompliant(false)]
    [CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ConstUtf8StringMarshaller))]
    public static unsafe class ConstUtf8StringMarshaller
    {
        /// <summary>
        /// Converts a string to an unmanaged version.
        /// </summary>
        /// <param name="managed">The managed string to convert.</param>
        /// <returns>An unmanaged string.</returns>
        public static byte* ConvertToUnmanaged(string? managed)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Converts an unmanaged string to a managed version.
        /// </summary>
        /// <param name="unmanaged">The unmanaged string to convert.</param>
        /// <returns>A managed string.</returns>
        public static string? ConvertToManaged(byte* unmanaged)
            => Utf8StringMarshaller.ConvertToManaged(unmanaged);

        /// <summary>
        /// Frees the memory for the unmanaged string.
        /// </summary>
        /// <param name="unmanaged">The memory allocated for the unmanaged string.</param>
        [SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "Required by design, not used here" )]
        public static void Free(byte* unmanaged)
        {
            // Intentional NOP, there is NOTHING to free, this shouldn't even be called but the
            // Source generator has an issue where it ALWAYS generates a call to this with the native
            // pointer. There is no way to specify that there isn't a need to free the string. It is
            // just a `char*` without any allocator - no ownership is transferred, so there's nothing
            // to free/release. The default handling even for UnmanagedType.LPStr is to assume the
            // pointer was allocated and then free it. Which is, of course, wrong for any OUT or return
            // pointers that weren't allocated that way. (The normal case for interop with C/C++) The
            // existing free always assumes the native pointer is allocated via Marshal.AllocCoTaskMem()
            // or a native equivalent so that it can call Marshal.FreeCoTaskMem() on the native pointer.
        }
    }
}
