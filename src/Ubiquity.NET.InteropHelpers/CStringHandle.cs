// -----------------------------------------------------------------------
// <copyright file="CStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ubiquity.NET.InteropHelpers
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
    public abstract class CStringHandle
        : SafeHandle
        , IEquatable<CStringHandle>
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
                    return new((void*)handle, LazyStrLen.Value);
                }
            }
        }

        /// <summary>Converts the underlying string pointer into a managed string</summary>
        /// <returns>Managed string for the provided pointer</returns>
        /// <remarks>
        /// The return is a managed string that is equivalent to the string of this pointer.
        /// It's lifetime is controlled by the runtime GC.
        /// </remarks>
        public override string? ToString()
        {
            ObjectDisposedException.ThrowIf(IsClosed || IsInvalid, this);
            return ManagedString.Value;
        }

        public override bool Equals(object? obj)
        {
            return ((IEquatable<CStringHandle>)this).Equals( obj as CStringHandle );
        }

        [SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Justification = "Matches string API")]
        public override int GetHashCode()
        {
            ObjectDisposedException.ThrowIf(IsClosed || IsInvalid, this);
            return ToString()?.GetHashCode() ?? 0;
        }

        public int GetHashCode(StringComparison comparisonType)
        {
            ObjectDisposedException.ThrowIf(IsClosed || IsInvalid, this);
            return ToString()?.GetHashCode(comparisonType) ?? 0;
        }

        public bool Equals(CStringHandle? other)
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

        protected CStringHandle()
            : base( nint.Zero, ownsHandle: true )
        {
            unsafe
            {
                ManagedString = new(()=>ExecutionEncodingStringMarshaller.ConvertToManaged((byte*)handle), LazyThreadSafetyMode.ExecutionAndPublication);
                LazyStrLen = new(()=>MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)handle).Length, LazyThreadSafetyMode.ExecutionAndPublication);
            }
        }

        protected CStringHandle(nint p)
            : this()
        {
            SetHandle( p );
        }

        protected unsafe CStringHandle(byte* p)
            : this( (nint)p )
        {
        }

        private readonly Lazy<string?> ManagedString;
        private readonly Lazy<int> LazyStrLen; // count of bytes in the native string (Not including null terminator)
    }
}
