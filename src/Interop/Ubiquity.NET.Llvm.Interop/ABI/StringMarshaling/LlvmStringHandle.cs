// -----------------------------------------------------------------------
// <copyright file="LlvmStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

using Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling;

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
                ObjectDisposedException.ThrowIf(IsClosed || IsInvalid, this);
                unsafe
                {
                    return LazyValue.GetReadOnlySpan((byte*)handle);
                }
            }
        }

        /// <summary>Converts the underlying string pointer into a managed string</summary>
        /// <returns>Managed string for the provided pointer</returns>
        /// <remarks>
        /// The return is a managed string that is equivalent to the string of this pointer.
        /// It's lifetime is controlled by the runtime GC.
        /// </remarks>
        public override string ToString()
        {
            ObjectDisposedException.ThrowIf(IsClosed || IsInvalid, this);
            unsafe
            {
                return LazyValue.ToString((byte*)handle) ?? string.Empty;
            }
        }

        public override bool Equals(object? obj)
        {
            return ((IEquatable<LlvmStringHandle>)this).Equals( obj as LlvmStringHandle );
        }

        public override int GetHashCode()
        {
            ObjectDisposedException.ThrowIf(IsClosed || IsInvalid, this);
            return ByteSpanHelpers.ComputeHashCode(ReadOnlySpan);
        }

        public bool Equals(LlvmStringHandle? other)
        {
            // perf optimization to skip longer scan if possible (null input or exact same handle value)
            return other is not null
                && ( (handle == other.handle) || Equals(other.ReadOnlySpan));
        }

        /// <summary>Tests if the span of characters for this string is identical to the provided span</summary>
        /// <param name="otherSpan">Span of bytes to compare this string to</param>
        /// <returns><see langword="true"/> if the spans contain the same data or <see langword="false"/> if not</returns>
        public bool Equals(ReadOnlySpan<byte> otherSpan)
        {
            return ReadOnlySpan.SequenceEqual(otherSpan);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if( disposing )
            {
                LazyValue.Dispose();
            }
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

        private readonly LazyInitializedString LazyValue = new();
    }
}
