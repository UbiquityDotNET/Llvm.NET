// -----------------------------------------------------------------------
// <copyright file="SymbolStringPoolEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Reference to an entry in a symbol string pool for ORC JIT v2</summary>
    /// <remarks>
    /// This holds a reference to the symbol string which is ONLY marshalled/converted to
    /// a managed string in the <see cref="ToString"/> method. This allows comparing strings
    /// etc... without the need of conversion.
    /// <note type="information">
    /// String conversion is lazy, so that once it is converted the managed string is cached
    /// and used as needed. Thus, the overhead of marshalling the string is realized only the
    /// first time it is needed.
    /// </note>
    /// </remarks>
    public sealed class SymbolStringPoolEntry
        : IEquatable<SymbolStringPoolEntry>
        /*, IEquatable<ReadOnlySpan<byte>> // Not allowed in C# 12/NET 8.0*/
        , IDisposable
    {
        /// <summary>Gets the managed string form of the native string</summary>
        /// <returns>managed string for this handle</returns>
        public override string ToString()
        {
            ThrowIfDisposed();
            unsafe
            {
                return LazyValue.ToString(LLVMOrcSymbolStringPoolEntryStr( Handle )) ?? string.Empty;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is SymbolStringPoolEntry other && Equals(other);
        }

        /// <summary>Compares this string with another to determine if they contain the same contents</summary>
        /// <param name="other">Other entry to compare against</param>
        /// <returns><see langword="true"/> if the strings contain the same data or <see langword="false"/> if not</returns>
        /// <remarks>
        /// <note type="information">
        /// A string pool interns the strings such that no two entries in a pool will ever be equal so this is
        /// only useful with entries from different pools.
        /// </note>
        /// </remarks>
        public bool Equals(SymbolStringPoolEntry? other)
        {
            return other is not null
                && ( Handle.Equals(other.Handle) || Equals(other.ReadOnlySpan) );
        }

        /// <summary>Tests if the span of characters for this string is identical to the provided span</summary>
        /// <param name="otherSpan">Span of bytes to compare this string to</param>
        /// <returns><see langword="true"/> if the spans contain the same data or <see langword="false"/> if not</returns>
        public bool Equals(ReadOnlySpan<byte> otherSpan) => ReadOnlySpan.SequenceEqual(otherSpan);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            ThrowIfDisposed();
            return ByteSpanHelpers.ComputeHashCode(ReadOnlySpan);
        }

        /// <summary>Release the reference to the string</summary>
        public void Dispose()
        {
            Handle.Dispose();
            LazyValue.Dispose();
        }

        /// <summary>Increments the ref count on this string and returns a new wrapper that will own the new count</summary>
        /// <returns>new string to own the updated ref count</returns>
        /// <remarks>
        /// This returns a new wrapper for the handle so that it has a distinct <see cref="Dispose"/> to release it.
        /// </remarks>
        public SymbolStringPoolEntry IncrementRefCount()
        {
            LLVMOrcRetainSymbolStringPoolEntry(Handle);
            return new(Handle);
        }

        /// <summary>Gets a readonly span for the data in this string</summary>
        /// <returns>Span of the native characters in this string (as byte)</returns>
        /// <remarks>
        /// This does NOT make a managed copy of the underlying string memory. Instead
        /// the returned span refers directly to the unmanaged memory of the string.
        /// </remarks>
        public ReadOnlySpan<byte> ReadOnlySpan
        {
            get
            {
                ThrowIfDisposed();
                unsafe
                {
                    return LazyValue.GetReadOnlySpan(LLVMOrcSymbolStringPoolEntryStr( Handle ));
                }
            }
        }

        internal SymbolStringPoolEntry(LLVMOrcSymbolStringPoolEntryRef h)
        {
            if (h.IsInvalid || h.IsClosed)
            {
                throw new ArgumentNullException(nameof(h));
            }

            Handle = h;
        }

        internal LLVMOrcSymbolStringPoolEntryRef Handle { get; init; }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(Handle is null || Handle.IsClosed || Handle.IsInvalid, new object());
        }

        private readonly LazyInitializedString LazyValue = new();
    }
}
