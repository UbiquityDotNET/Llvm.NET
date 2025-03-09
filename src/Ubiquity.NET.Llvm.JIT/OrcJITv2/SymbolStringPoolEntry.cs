// -----------------------------------------------------------------------
// <copyright file="SymbolStringPoolEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

// Regions help hide the boiler plate equality stuff
#pragma warning disable SA1124 // Do not use regions

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
        /// <summary>Initializes a new instance of the <see cref="SymbolStringPoolEntry"/> class from another entry (Add ref construction)</summary>
        /// <param name="other">Other string to make a new entry from</param>
        /// <remarks>
        /// In LLVM a <see cref="SymbolStringPoolEntry"/> is a pointer to a reference counted string. This constructor will create a new
        /// entry that "owns" a ref count bump (AddRef) on a source string (<paramref name="other"/>). Callers must dispose of the new
        /// instance the same as the original one or the ref count is never reduced and the native memory never reclaimed (That is, it leaks!)
        /// </remarks>
        public SymbolStringPoolEntry(SymbolStringPoolEntry other)
            : this(other.ThrowIfNull().AddRef())
        {
            // TODO: optimize this to use any lazy evaluated values available in other
        }

        /// <summary>Gets the managed string form of the native string</summary>
        /// <returns>managed string for this handle</returns>
        public override string? ToString()
        {
            ThrowIfDisposed();
            return ManagedString.Value;
        }

        #region IEquatable<SymbolStringPoolEntry>

        /// <inheritdoc/>
        public override bool Equals(object? obj)
#pragma warning restore SA1124 // Do not use regions
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
        [SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Justification = "Matches string API")]
        public override int GetHashCode()
        {
            ThrowIfDisposed();
            return ToString()?.GetHashCode() ?? 0;
        }

        /// <summary>Gets the hash code for the managed string</summary>
        /// <param name="comparisonType">Kind of comparison to perform when computing the hash code</param>
        /// <returns>Hash code for the managed string</returns>
        public int GetHashCode(StringComparison comparisonType)
        {
            ThrowIfDisposed();
            return ToString()?.GetHashCode(comparisonType) ?? 0;
        }
        #endregion

        /// <summary>Release the reference to the string</summary>
        public void Dispose()
        {
            Handle.Dispose();
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
                    return new((void*)NativeStringPtr.Value, LazyStrLen.Value);
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
            unsafe
            {
                NativeStringPtr = new(()=>(nint)LLVMOrcSymbolStringPoolEntryStr( Handle ), LazyThreadSafetyMode.ExecutionAndPublication);
                ManagedString = new(()=>ExecutionEncodingStringMarshaller.ConvertToManaged((byte*)NativeStringPtr.Value), LazyThreadSafetyMode.ExecutionAndPublication);
                LazyStrLen = new(()=>MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)NativeStringPtr.Value).Length, LazyThreadSafetyMode.ExecutionAndPublication);
            }
        }

        internal LLVMOrcSymbolStringPoolEntryRef Handle { get; init; }

        private LLVMOrcSymbolStringPoolEntryRef AddRef()
        {
            ThrowIfDisposed();
            LLVMOrcRetainSymbolStringPoolEntry(Handle);
            return new(Handle.DangerousGetHandle(), owner: true);
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(Handle is null || Handle.IsClosed || Handle.IsInvalid, this);
        }

        private readonly Lazy<string?> ManagedString;
        private readonly Lazy<int> LazyStrLen; // count of bytes in the native string (Not including null terminator)
        private readonly Lazy<nint> NativeStringPtr;
    }
}
