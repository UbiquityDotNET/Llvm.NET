// -----------------------------------------------------------------------
// <copyright file="ExecutionEncodingStringMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Represents a marshaller for native strings using <see cref="Encoding"/>.</summary>
    [CustomMarshaller(typeof(string), MarshalMode.Default, typeof(ExecutionEncodingStringMarshaller))]
    [CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
    public static unsafe class ExecutionEncodingStringMarshaller
    {
        /// <summary>Gets the ExecutionEncoding for the native code</summary>
        /// <remarks>
        /// On Windows and MSVC the default encoding is that of the OS/Platform runtime. On Linux UTF8 has reached a level
        /// of common standard that you can assume it is the default unless documented otherwise. Not so much on Windows.
        /// [To be fair there's a LOT more legacy client code targeting Windows to contend with.] This often doesn't matter
        /// for const strings as they tend to fall in the ASCII/ANSI (Latin1) encoding ranges. However, that isn't guaranteed,
        /// which makes for all sorts of "interesting" latent bugs. Even as well documented and well thought out as LLVM is,
        /// it remains silent on this point. Spelunking the build system generated for LLVM itself by CMake there is NOTHING
        /// to set either the source or execution encodings for Windows, MSVC or any other toolset that I can see so they seem
        /// to be left at defaults.
        /// </remarks>
        /// <ImplementationNotes>
        /// This is the one place that deals with the encoding for ALL of the interop library. Nothing in the code base should
        /// use any other form of string marshalling. This allows one place to deal with any error or target platform/runtime
        /// peculiarities for the encoding. This doesn't matter much for any string consts, but matters a LOT for symbol names
        /// etc... that a user might want to set.
        /// </ImplementationNotes>
        public static Encoding Encoding
            => OperatingSystem.IsWindows()
            ? Encoding.Default
            : Encoding.UTF8;

        public static ReadOnlySpan<byte> ReadOnlySpanFromNullTerminated(byte* nativePtr)
        {
            return MemoryMarshal.CreateReadOnlySpanFromNullTerminated( nativePtr );
        }

        /// <summary>
        /// Converts a string to an unmanaged version.
        /// </summary>
        /// <param name="managed">The managed string to convert.</param>
        /// <returns>An unmanaged string.</returns>
        public static byte* ConvertToUnmanaged(string? managed)
        {
            if (managed is null)
            {
                return null;
            }

            int exactByteCount = Encoding.GetByteCount(managed) + 1; // Includes null terminator
            byte* mem = (byte*)Marshal.AllocCoTaskMem(exactByteCount);
            Span<byte> buffer = new (mem, exactByteCount);

            Encoding.GetBytes(managed, buffer); // Does NOT include null terminator
            buffer[^1] = 0; // now it has the null terminator! 8^)
            return mem;
        }

        /// <summary>
        /// Converts an unmanaged string to a managed version.
        /// </summary>
        /// <param name="unmanaged">The unmanaged string to convert.</param>
        /// <returns>A managed string.</returns>
        public static string? ConvertToManaged(byte* unmanaged)
            => Encoding.MarshalString(unmanaged);

        /// <summary>
        /// Frees the memory for the unmanaged string.
        /// </summary>
        /// <param name="unmanaged">The memory allocated for the unmanaged string.</param>
        public static void Free(byte* unmanaged)
            => Marshal.FreeCoTaskMem((IntPtr)unmanaged);

        /// <summary>
        /// Custom marshaller to marshal a managed string as a ANSI unmanaged string.
        /// </summary>
        [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Standard pattern for custom marshallers" )]
        public ref struct ManagedToUnmanagedIn
        {
            /// <summary>
            /// Gets the requested buffer size for optimized marshalling.
            /// </summary>
            public static int BufferSize => 0x100;

            /// <summary>
            /// Initializes the marshaller with a managed string and requested buffer.
            /// </summary>
            /// <param name="managed">The managed string to initialize the marshaller with.</param>
            /// <param name="buffer">A request buffer of at least size <see cref="BufferSize"/>.</param>
            public void FromManaged(string? managed, Span<byte> buffer)
            {
                AllocatedByThisMarshaller = false;

                if (managed is null)
                {
                    NativePointer = null;
                    return;
                }

                // >= for null terminator
                if (Encoding.GetMaxByteCount(managed.Length) >= buffer.Length)
                {
                    // Calculate accurate byte count when the provided stack-allocated buffer is not sufficient
                    int exactByteCount = Encoding.GetByteCount(managed) + 1; // + 1 to includes null terminator
                    if (exactByteCount > buffer.Length)
                    {
                        buffer = new Span<byte>((byte*)NativeMemory.Alloc((nuint)exactByteCount), exactByteCount);
                        AllocatedByThisMarshaller = true;
                    }
                }

                NativePointer = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(buffer));

                Encoding.GetBytes(managed, buffer);
                buffer[^1] = 0;
            }

            /// <summary>
            /// Converts the current managed string to an unmanaged string.
            /// </summary>
            /// <returns>The converted unmanaged string.</returns>
            public readonly byte* ToUnmanaged() => NativePointer;

            /// <summary>Frees any allocated unmanaged string memory.</summary>
            public readonly void Free()
            {
                if (AllocatedByThisMarshaller)
                {
                    NativeMemory.Free(NativePointer);
                }
            }

            private byte* NativePointer;
            private bool AllocatedByThisMarshaller;
        }
    }
}
