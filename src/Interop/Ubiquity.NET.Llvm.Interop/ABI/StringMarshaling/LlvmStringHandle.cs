// -----------------------------------------------------------------------
// <copyright file="LlvmStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Threading;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Abstract base class for types that represents an LLVM string</summary>
    /// <remarks>
    /// This base class provides most of the functionality for a string pointer except
    /// the disposal/release of the string. That is left to derived types to provide the
    /// specific operation to release the pointer. In particular this provides a simple
    /// copy by value marshalling and there is no copy made until <see cref="ToString()"/>
    /// is called. In particular, <see cref="Span"/> will NOT make a managed
    /// copy and the returned span is to the original unmanaged memory.
    /// </remarks>
    public abstract class LlvmStringHandle
        : SafeHandle
        , IEquatable<LlvmStringHandle>
    {
        /// <inheritdoc/>
        public override bool IsInvalid => handle == nint.Zero;

        /// <summary>Gets a readonly span for the data in this string</summary>
        /// <returns>Span of the ANSI characters in this string (as byte)</returns>
        /// <remarks>
        /// This does NOT make a managed copy of the underlying string memory. Instead
        /// the returned span refers directly to the unmanaged memory of the string.
        /// </remarks>
        public ReadOnlySpan<byte> ReadOnlySpan
        {
            get
            {
                // At the very least, this needs to know the length of the string
                // so make sure it is computed/scanned only once.
                LazyInitializer.EnsureInitialized( ref LazyStrLen, ref LengthInitialized, ref LazySyncLock, MakeLazyStringLen );
                unsafe
                {
                    return IsClosed || IsInvalid ? default : new ReadOnlySpan<byte>((void*)handle, LazyStrLen);
                }
            }
        }

        /// <summary>Converts the underlying ANSI pointer into a managed string</summary>
        /// <returns>Managed string for the provided pointer</returns>
        /// <remarks>
        /// The return is a managed string that is equivalent to the string of this pointer.
        /// It's lifetime is controlled by the runtime GC.
        /// </remarks>
        public override string ToString()
        {
            if (IsClosed || IsInvalid)
            {
                return string.Empty;
            }

            LazyInitializer.EnsureInitialized(ref LazyString, ref LazySyncLock, MakeLazyString);
            return LazyString;
        }

        public override bool Equals(object? obj)
        {
            return ((IEquatable<LlvmStringHandle>)this).Equals( obj as LlvmStringHandle );
        }

        public override int GetHashCode()
        {
            ReadOnlySpan<byte> data = ReadOnlySpan;

            // if there's no data, then the hash is 0
            if (data.Length == 0)
            {
                return 0;
            }

            // use BLAKE3 to compute a hash code of the span data, and then hash the resulting array.
            llvm_blake3_hasher hasher = default;
            llvm_blake3_hasher_init(ref hasher);
            unsafe
            {
                fixed(byte* p = &MemoryMarshal.GetReference(data))
                {
                    llvm_blake3_hasher_update(ref hasher, p, data.Length);
                }

                llvm_blake3_hasher_finalize(ref hasher, out byte[] hashCode, out nint _);
                return hashCode.GetHashCode();
            }
        }

        public bool Equals(LlvmStringHandle? other)
        {
            return other is not null && ReadOnlySpan.SequenceEqual(other.ReadOnlySpan);
        }

        private protected LlvmStringHandle()
            : base( nint.Zero, ownsHandle: true )
        {
        }

        private protected LlvmStringHandle(nint p)
            : this()
        {
            SetHandle( p );
        }

        private protected unsafe LlvmStringHandle(byte* p)
            : this( (nint)p )
        {
        }

        private string MakeLazyString()
        {
            string retVal = Marshal.PtrToStringAnsi( handle ) ?? string.Empty;
            LazyInitializer.EnsureInitialized(ref LazyStrLen, ref LengthInitialized, ref LazySyncLock, ()=> retVal.Length);
            return retVal;
        }

        private int MakeLazyStringLen()
        {
            unsafe
            {
                return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)handle).Length;
            }
        }

        private object LazySyncLock = new();
        private string? LazyString;
        private bool LengthInitialized;
        private int LazyStrLen;
    }
}
