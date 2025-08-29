// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Represents a marshaller for <see cref="LazyEncodedString"/>.</summary>
    /// <remarks>
    /// This will handle marshalling of <see cref="LazyEncodedString"/>s to/from native code. This will
    /// use any pre-existing native representation in the string so that there is as little overhead
    /// as possible. If conversion has not yet occurred it is done once here.
    /// </remarks>
    [CustomMarshaller( typeof( LazyEncodedString ), MarshalMode.ManagedToUnmanagedIn, typeof( ManagedToUnmanagedIn ) )]
    [CustomMarshaller( typeof( LazyEncodedString ), MarshalMode.Default, typeof( LazyEncodedStringMarshaller ) )]
    public static unsafe class LazyEncodedStringMarshaller
    {
        /// <summary>Converts a native string to a <see cref="LazyEncodedString"/> using the default (UTF8) encoding</summary>
        /// <param name="unmanaged">Unmanaged string (in UTF8)</param>
        /// <returns>Wrapped string with lazy conversion to managed form</returns>
        public static LazyEncodedString? ConvertToManaged( byte* unmanaged )
        {
            return LazyEncodedString.FromUnmanaged( unmanaged );
        }

        // Generally, can't support the [In] direction of [Out,In]
        // The initialized "in" array might be marshaled to the native where it is "updated" and then needs marshaling
        // again on the return. This is NOT a supported scenario as conversion would need a stateful form to hold
        // a pinned element for the call. Thus the generated marshalling would need to hold an array of the pinned
        // managed values and unpin them after the call..., which is not an option for array elements.

        /// <summary>Converts a <see cref="LazyEncodedString"/> to a managed form</summary>
        /// <param name="managed">String to convert or retrieve the native form of</param>
        /// <returns>managed pointer</returns>
        /// <exception cref="NotSupportedException"><paramref name="managed"/> is not <see langword="null"/></exception>
        /// <remarks>
        /// Without state, this version can only convert default <see langword="null"/> values to a <see langword="null"/>
        /// return. Any other value results in an exception.
        /// </remarks>
        public static unsafe byte* ConvertToUnmanaged( LazyEncodedString? managed )
        {
            // NOTE: Care is needed here as the semantics are unknown and it is NOT legit to assume null means empty.
            //       It might not have that meaning to the caller and this implementation cannot know either way.
            //
            // Null is convertible without any state so go-ahead with that, anything else requires state.
            return managed is null
                    ? (byte*)null
                    : throw new NotSupportedException( "non-null element for [Out] scenarios is not supported" );
        }

        /// <summary>Stateful custom marshaller to marshal LazyEncodedString as an unmanaged string</summary>
        /// <remarks>A stateful marshaller is needed to hold the pinned state of the managed string</remarks>
        [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Standard pattern for custom marshalers" )]
        public ref struct ManagedToUnmanagedIn
        {
            /// <summary>Initializes the marshaller with a managed string and requested buffer.</summary>
            /// <param name="managed">The managed string to initialize the marshaller with.</param>
            public void FromManaged( LazyEncodedString? managed )
            {
                Managed = managed;
            }

            /// <summary>Converts the current managed string to an unmanaged string.</summary>
            /// <returns>The converted unmanaged string.</returns>
            public byte* ToUnmanaged( )
            {
                if(Managed is null)
                {
                    return null;
                }

                Handle.Dispose();
                Handle = Managed.Pin();
                return (byte*)Handle.Pointer;
            }

            /// <summary>Frees any allocated unmanaged string memory.</summary>
            public readonly void Free( )
            {
                Handle.Dispose();
            }

            [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP006:Implement IDisposable", Justification = "Covered by Free() method in pattern" )]
            private MemoryHandle Handle;
            private LazyEncodedString? Managed;
        }
    }
}
