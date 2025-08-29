// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling
{
    /// <summary>Represents a marshaller for LLVM strings that require a call to LLVMDisposeMessage to release the native representation.</summary>
    /// <remarks>
    /// Generally used for Return/OUT semantics as there are no LLVM APIs that accept a string requiring LLVMDisposeMessage().
    /// <note type="note">
    /// The P/Invoke APIs generally use a <see cref="LazyEncodedString"/>, in fact that's the general rule. The primary exceptions
    /// are "ToString" type functions and out error messages where the use of the value as a managed string is either given by
    /// definition or extremely likely (or impossible as a new param to some other native API), in such cases the signature is
    /// simply <see cref="string"/> to convert directly to a managed form as that is the only way it is reasonable to use.
    /// </note>
    /// </remarks>
    [CLSCompliant( false )]
    [CustomMarshaller( typeof( string ), MarshalMode.ManagedToUnmanagedOut, typeof( DisposeMessageMarshaller ) )]
    [CustomMarshaller( typeof( LazyEncodedString ), MarshalMode.ManagedToUnmanagedOut, typeof( LazyEncodedDisposeMessage_Out ) )]
    public static unsafe class DisposeMessageMarshaller
    {
        /// <summary>Converts an unmanaged string to a managed version.</summary>
        /// <param name="unmanaged">An unmanaged string to convert.</param>
        /// <returns>The converted managed string.</returns>
        public static string? ConvertToManaged( byte* unmanaged )
        {
            return LazyEncodedStringMarshaller.ConvertToManaged( unmanaged )?.ToString();
        }

        /// <summary>
        /// Frees the memory for the unmanaged string.
        /// </summary>
        /// <param name="unmanaged">The memory allocated for the unmanaged string.</param>
        public static void Free( byte* unmanaged )
        {
            if(unmanaged is not null)
            {
                LLVMDisposeMessage( unmanaged );
            }

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeMessage( byte* p );
        }

        /// <summary>Marshalling type to handle out/Return scenarios for a <see cref="LazyEncodedString"/> that requires LLVMDisposeMessage()</summary>
        [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Standard pattern for custom marshalers" )]
        public ref struct LazyEncodedDisposeMessage_Out
        {
            /// <summary>Converts the captured unmanaged pointer to a <see cref="LazyEncodedString"/></summary>
            /// <returns>A new <see cref="LazyEncodedString"/> from the native pointer</returns>
            /// <remarks>
            /// The native pointer used is captured in a preceding call to <see cref="FromUnmanaged(byte*)"/>.
            /// This two stage conversion is used to ensure that the unmanaged pointer is freed even if there
            /// is an exception in creating the managed version.
            /// </remarks>
            public readonly LazyEncodedString? ToManaged( )
            {
                return LazyEncodedString.FromUnmanaged( UnmanagedPtr );
            }

            /// <summary>Captures the unmanaged pointer</summary>
            /// <param name="unmanaged">Unmanaged pointer to convert</param>
            /// <remarks>
            /// The unmanaged pointer is captured in this method to ensure it
            /// is released even in the face of an exception in the subsequent call
            /// to <see cref="ToManaged()"/>. The generated code for marshalling will
            /// call <see cref="Free"/> from within a `finally` block.
            /// </remarks>
            public void FromUnmanaged( byte* unmanaged )
            {
                UnmanagedPtr = unmanaged;
            }

            /// <summary>Releases the native pointer (via <see cref="DisposeMessageMarshaller.Free(byte*)"/>)</summary>
            /// <remarks>
            /// <note type="important">
            /// The captured pointer is released but, for performance, not set to null. Thus, once freed this type is
            /// no longer useful unless <see cref="FromUnmanaged(byte*)"/> is called to replace the native pointer. This,
            /// is not a problem with the normal source generated marshalling. But might be a problem if used directly.
            /// You have been warned!
            /// </note>
            /// </remarks>
            public readonly void Free( )
            {
                DisposeMessageMarshaller.Free( UnmanagedPtr );
            }

            private byte* UnmanagedPtr;
        }
    }
}
