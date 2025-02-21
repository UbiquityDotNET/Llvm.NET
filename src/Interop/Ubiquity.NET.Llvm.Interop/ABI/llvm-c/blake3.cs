// -----------------------------------------------------------------------
// <copyright file="blake3.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

using static Ubiquity.NET.Llvm.Interop.Constants;

namespace Ubiquity.NET.Llvm.Interop
{
    [SuppressMessage( "Redundancies in Symbol Declarations", "RECS0001:Class is declared partial but has only one part", Justification = "Establishes pattern, others may be added in future" )]
    public static partial class Constants
    {
        // These were all originally untyped #defines in the LLVM source
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

    [InlineArray( LLVM_BLAKE3_BLOCK_LEN )]
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

    [InlineArray( (LLVM_BLAKE3_MAX_DEPTH + 1) * LLVM_BLAKE3_OUT_LEN )]
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

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string llvm_blake3_version();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init(out llvm_blake3_hasher self);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init_keyed(out llvm_blake3_hasher self, [In][MarshalUsing( ConstantElementCount = LLVM_BLAKE3_KEY_LEN )] byte[] key);

        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init_derive_key(out llvm_blake3_hasher self, string context);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_init_derive_key_raw(out llvm_blake3_hasher self, void* context, size_t context_len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_update(out llvm_blake3_hasher self, void* input, size_t input_len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_finalize(out llvm_blake3_hasher self, out byte @out, size_t out_len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_finalize_seek(out llvm_blake3_hasher self, UInt64 seek, out byte @out, size_t out_len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void llvm_blake3_hasher_reset(out llvm_blake3_hasher self);
    }
}
