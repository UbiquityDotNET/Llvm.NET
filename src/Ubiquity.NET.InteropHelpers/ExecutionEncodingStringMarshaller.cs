// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Represents a marshaller for native strings using <see cref="Encoding"/>.</summary>
    /// <remarks>
    /// This will handle marshalling of <see cref="string"/>s to/from native code. This is very similar
    /// to the default string marshalling support except that it does all conversion to/from native code via
    /// a static <see cref="Encoding"/> property, [defaults to UTF8] so that applications can have control of that.
    /// </remarks>
#if !NETSTANDARD2_0
    [CustomMarshaller( typeof( string ), MarshalMode.Default, typeof( ExecutionEncodingStringMarshaller ) )]
    [CustomMarshaller( typeof( string ), MarshalMode.ManagedToUnmanagedIn, typeof( ManagedToUnmanagedIn ) )]
#endif
    public static unsafe class ExecutionEncodingStringMarshaller
    {
        /// <summary>Gets or sets the ExecutionEncoding for the native code</summary>
        /// <remarks>
        /// On Windows and MSVC (.NET Framework) the default encoding is that of the OS/Platform runtime. On .NET Core/Linux
        /// UTF8 has reached a level of common standard that you can assume it is the default unless documented otherwise.
        /// Not so much on Windows. [To be fair. there's a LOT more legacy client code targeting Windows to contend with.]
        /// This often doesn't matter for const strings as they tend to fall in the ASCII/ANSI (Latin1) encoding ranges.
        /// However, that isn't guaranteed, which makes for all sorts of "interesting" latent bugs.
        ///
        /// <note type="note">Even as well documented and well thought out as LLVM is, it remains silent on this point.
        /// Spelunking the build system generated for LLVM itself by CMake there is NOTHING to set either the source or
        /// execution encoding for Windows, MSVC or any other tool-set that I can see so they seem to be left at defaults.
        /// (LLVM is just an example as it was where this library began, but it is not limited to that.)
        /// </note>
        ///
        /// <note type="information">
        /// For .NET Core, which includes .NET 5+, <see cref="System.Text.Encoding.Default"/> is the same as
        /// <see cref="System.Text.Encoding.UTF8"/> even on Windows. So that is the assumed encoding used here.
        /// </note>
        /// </remarks>
        public static Encoding Encoding { get; set; } = Encoding.UTF8;

#if !NETSTANDARD2_0
        /// <summary>Creates a ReadOnlySpan from a null terminated native string</summary>
        /// <param name="nativePtr">The null terminated string</param>
        /// <returns>ReadOnlySpan for the string</returns>
        /// <remarks>
        /// This does NOT perform any encoding or decoding, just gets the span of the
        /// underlying native bytes.
        /// </remarks>
        public static ReadOnlySpan<byte> ReadOnlySpanFromNullTerminated( byte* nativePtr )
        {
            return MemoryMarshal.CreateReadOnlySpanFromNullTerminated( nativePtr );
        }

        /// <summary>Converts a string to an unmanaged version.</summary>
        /// <param name="managed">The managed string to convert.</param>
        /// <returns>An unmanaged string.</returns>
        public static byte* ConvertToUnmanaged( string? managed )
        {
            if(managed is null)
            {
                return null;
            }

            int exactByteCount = Encoding.GetByteCount(managed) + 1; // Includes null terminator
            byte* mem = (byte*)NativeMemory.Alloc((nuint)exactByteCount);
            Span<byte> buffer = new (mem, exactByteCount);

            int numBytes = Encoding.GetBytes(managed, buffer); // Does NOT include null terminator
            buffer[ numBytes ] = 0; // now it has the null terminator! 8^)
            Debug.Assert( exactByteCount == numBytes + 1, "Mismatched lengths, likely result in bogus native string!" );
            return mem;
        }

        /// <summary>Frees the memory for the unmanaged string representation allocated by <see cref="ConvertToUnmanaged"/></summary>
        /// <param name="unmanaged">The memory allocated for the unmanaged string.</param>
        public static void Free( byte* unmanaged )
        {
            NativeMemory.Free( unmanaged );
        }
#endif

        /// <summary>Converts an unmanaged string to a managed version.</summary>
        /// <param name="unmanaged">The unmanaged string to convert.</param>
        /// <returns>A managed string.</returns>
        public static string? ConvertToManaged( byte* unmanaged )
        {
            return Encoding.MarshalString( unmanaged );
        }

#if !NETSTANDARD2_0
        /// <summary>Custom marshaller to marshal a managed string as an unmanaged string using the <see cref="Encoding"/> property for encoding the native string.</summary>
        [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Standard pattern for custom marshalers" )]
        public ref struct ManagedToUnmanagedIn
        {
            /// <summary>Gets the default buffer size for optimized marshalling.</summary>
            public static int BufferSize => 0x100;

            /// <summary>Initializes the marshaller with a managed string and requested buffer.</summary>
            /// <param name="managed">The managed string to initialize the marshaller with.</param>
            /// <param name="buffer">A request buffer of at least size <see cref="BufferSize"/>.</param>
            /// <remarks>
            /// If the buffer is not large enough to handle a native representation of <paramref name="managed"/>
            /// then a new buffer is allocated and is not released until <see cref="Free"/> is called to release
            /// it.
            /// </remarks>
            public void FromManaged( string? managed, Span<byte> buffer )
            {
                AllocatedByThisMarshaller = false;

                if(managed is null)
                {
                    NativePointer = null;
                    return;
                }

                // >= for null terminator
                if(Encoding.GetMaxByteCount( managed.Length ) >= buffer.Length)
                {
                    // Calculate accurate byte count when the provided stack-allocated buffer is not sufficient
                    int exactByteCount = Encoding.GetByteCount(managed) + 1; // + 1 to include null terminator
                    if(exactByteCount > buffer.Length)
                    {
                        buffer = new Span<byte>( (byte*)NativeMemory.Alloc( (nuint)exactByteCount ), exactByteCount );
                        AllocatedByThisMarshaller = true;
                    }
                }

                NativePointer = (byte*)Unsafe.AsPointer( ref MemoryMarshal.GetReference( buffer ) );

                int numBytes = Encoding.GetBytes(managed, buffer);
                buffer[ numBytes ] = 0; // Enforce termination assumption in native code
            }

            /// <summary>Converts the current managed string to an unmanaged string.</summary>
            /// <returns>The converted unmanaged string.</returns>
            public readonly byte* ToUnmanaged( ) => NativePointer;

            /// <summary>Frees any allocated unmanaged string memory.</summary>
            public readonly void Free( )
            {
                if(AllocatedByThisMarshaller)
                {
                    NativeMemory.Free( NativePointer );
                }
            }

            private byte* NativePointer;
            private bool AllocatedByThisMarshaller;
        }
#endif
    }
}
