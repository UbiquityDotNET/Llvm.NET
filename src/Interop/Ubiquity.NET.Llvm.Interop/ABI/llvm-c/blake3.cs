// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
{
    // These were all originally untyped #defines in the LLVM source
    public static class Blake3Constants
    {
        public const string LLVM_BLAKE3_VERSION_STRING = "1.3.1";
        public const int LLVM_BLAKE3_KEY_LEN = 32;
        public const int LLVM_BLAKE3_OUT_LEN = 32;
        public const int LLVM_BLAKE3_BLOCK_LEN = 64;
        public const int LLVM_BLAKE3_CHUNK_LEN = 1024;
        public const int LLVM_BLAKE3_MAX_DEPTH = 54;
    }

    [InlineArray( 8 )]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "InlineArray" )]
    public struct llvm_blake3_chunk_state_cv_t
    {
        private UInt32 Element;
    }

    [InlineArray( Blake3Constants.LLVM_BLAKE3_BLOCK_LEN )]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "InlineArray" )]
    public struct llvm_blake3_chunk_state_buf_t
    {
        private byte Element;
    }

    [InlineArray( 8 )]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "InlineArray" )]
    public struct llvm_blake3_hasher_key_t
    {
        private UInt32 Element;
    }

    [InlineArray( (Blake3Constants.LLVM_BLAKE3_MAX_DEPTH + 1) * Blake3Constants.LLVM_BLAKE3_OUT_LEN )]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "InlineArray" )]
    public struct llvm_blake3_hasher_cv_stack_t
    {
        private byte Element;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct llvm_blake3_chunk_state
    {
        public readonly llvm_blake3_chunk_state_cv_t cv;
        public readonly UInt64 chunk_counter;
        public readonly llvm_blake3_chunk_state_buf_t buf;
        public readonly byte buf_len;
        public readonly byte blocks_compressed;
        public readonly byte flags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct llvm_blake3_hasher
    {
        public readonly llvm_blake3_hasher_key_t key;
        public readonly llvm_blake3_chunk_state chunk;
        public readonly byte cv_stack_len;
        public readonly llvm_blake3_hasher_cv_stack_t cv_stack;
    }

    public static partial class Blake3
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString? llvm_blake3_version( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init( ref llvm_blake3_hasher self );

        /// <summary>P/Invoke for LLVM support</summary>
        /// <param name="self">Reference to the unmanaged data structure to work with</param>
        /// <param name="key">Key for the operation</param>
        /// <remarks>
        /// <note type="important">It is worth noting that the marshalling doesn't care about the size of an array
        /// with `[In]` semantics, but the native code does! It expects a pointer to a buffer of at least the specified
        /// size [LLVM_BLAKE3_KEY_LEN]. Any attempts to call this using a smaller array will result in an access violation
        /// and app crash!</note>
        /// </remarks>
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init_keyed( ref llvm_blake3_hasher self, /*[In]sizeis(LLVM_BLAKE3_KEY_LEN)*/ byte* key );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init_derive_key( ref llvm_blake3_hasher self, LazyEncodedString context );

        /// <summary>P/Invoke for LLVM support</summary>
        /// <param name="self">Reference to the unmanaged data structure to work with</param>
        /// <param name="context">context for the operation</param>
        /// <param name="context_len">length of the array <paramref name="context"/></param>
        /// <remarks>
        /// <note type="important">It is worth noting that the marshalling doesn't care about the size of an array
        /// with `[In]` semantics, it just pins it and passes the pointer. The native code does care however! It expects a pointer
        /// to a buffer of at least the specified size. Any attempts to call this using a smaller array will result in an access
        /// violation and app crash!</note>
        /// </remarks>
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init_derive_key_raw( ref llvm_blake3_hasher self, byte* context, nuint context_len );

        /// <summary>P/Invoke for LLVM support</summary>
        /// <param name="self">Reference to the unmanaged data structure to work with</param>
        /// <param name="input">input for the operation</param>
        /// <param name="input_len">length of the array <paramref name="input"/></param>
        /// <remarks>
        /// <note type="important">It is worth noting that the marshalling doesn't care about the size of an array
        /// with `[In]` semantics, it just pins it and passes the pointer. The native code does care however! It expects a pointer
        /// to a buffer of at least the specified size. Any attempts to call this using a smaller array will result in an access
        /// violation and app crash!</note>
        /// </remarks>
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_update( ref llvm_blake3_hasher self, byte* input, nuint input_len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_finalize( ref llvm_blake3_hasher self, [MarshalUsing( CountElementName = nameof( out_len ) )] out byte[] @out, out nint out_len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_finalize_seek( ref llvm_blake3_hasher self, UInt64 seek, [MarshalUsing( CountElementName = nameof( out_len ) )] out byte[] @out, out nint out_len );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_reset( ref llvm_blake3_hasher self );
    }
}
