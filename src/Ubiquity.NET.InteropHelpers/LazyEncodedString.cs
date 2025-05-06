// -----------------------------------------------------------------------
// <copyright file="LazyMarshalledString.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Ubiquity.NET.InteropHelpers
{
    // TODO: Add marshaller for LazyEncodedString so it is easy to use in signatures instead of string

    /// <summary>Lazily encoded string with implicit casting to a read only span of bytes or a normal managed string</summary>
    /// <remarks>
    /// <para>This class handles capturing a managed string or a span of bytes for a native one. It supports a lazily
    /// evaluated representation of the string as a sequence of native bytes or a managed string. The encoding ONLY happens
    /// once, and ONLY when needed the first time. This reduces the overhead to a onetime hit for any strings that "sometimes"
    /// get passed to native code or "sometimes" get used in managed code as a string.</para>
    /// <para>This is essentially a pair of <see cref="Lazy{T}"/> members to handle conversions in one direction.
    /// Constructors exist for an existing <see cref="ReadOnlySpan{T}"/> of bytes and another for a string. Each constructor
    /// will pre-initialize one of the lazy values and set up the evaluation function for the other. Thus the string
    /// is encoded/decoded ONLY if needed and then, only once.</para>
    /// <para>This class handles all the subtle complexity regarding terminators as most of the encoding APIs in .NET will
    /// drop/ignore a string terminator but native code usually needs it. Thus, this ensures the presence of a terminator
    /// even if the span provided to the constructor doesn't include one. (It has to copy the string anyway so why not be
    /// nice and robust at the cost of one byte of allocated space)</para>
    /// </remarks>
    public sealed class LazyEncodedString
        : IEquatable<LazyEncodedString>
    {
        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from an existing managed string</summary>
        /// <param name="managed">string to lazy encode for native code use</param>
        /// <param name="encoding">Encoding to use for the string [Optional: Defaults to <see cref="Encoding.UTF8"/> if not provided]</param>
        public LazyEncodedString(string managed, Encoding? encoding = null)
        {
            Encoding = encoding ?? Encoding.UTF8;

            // Pre-Initialize with the provided string
            ManagedString = new(managed);
            NativeBytes = new(GetNativeArrayWithTerminator);

            unsafe byte[] GetNativeArrayWithTerminator()
            {
                int nativeByteLen = Encoding.GetByteCount(managed) + 1; // +1 for terminator
                byte[] retVal = new byte[nativeByteLen];

                int numBytes = Encoding.GetBytes(ManagedString.Value, retVal);
                Debug.Assert(numBytes == nativeByteLen - 1, "Invalid terminator length assumptions!"); // -1 as numBytes does not account for terminator
                retVal[numBytes] = 0; // force null termination so it is viable with native code
                return retVal;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from an existing span of native bytes</summary>
        /// <param name="span">span of native bytes</param>
        /// <param name="encoding">Encoding to use for the string [Optional: Defaults to <see cref="Encoding.UTF8"/> if not provided]</param>
        /// <remarks>This has some performance overhead as it MUST make a copy of the contents of the span. The lifetime
        /// of <paramref name="span"/> is not guaranteed beyond this call.
        /// </remarks>
        public LazyEncodedString(ReadOnlySpan<byte> span, Encoding? encoding = null)
        {
            Encoding = encoding ?? Encoding.UTF8;
            NativeBytes = new(GetNativeArrayWithTerminator(span));
            ManagedString = new(ConvertString, LazyThreadSafetyMode.ExecutionAndPublication);

            // drop the terminator for conversion to managed so it won't appear in the string
            string ConvertString() => NativeBytes.Value.Length > 0 ? Encoding.GetString(NativeBytes.Value[..^1]) : string.Empty;

            // This incurs the cost of a copy but the lifetime of the span is not known or
            // guaranteed beyond this call.
            static byte[] GetNativeArrayWithTerminator(ReadOnlySpan<byte> span)
            {
                // If it already has a terminator just use it
                if(span.IsEmpty || span[^1] == 0)
                {
                    return span.ToArray();
                }

                // need to account for terminator so manually allocate and copy the span
                byte[] retVal = new byte[span.Length + 1];
                span.CopyTo(retVal);
                retVal[^1] = 0; // force terminator
                return retVal;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from a raw native pointer</summary>
        /// <param name="nativePtr">pointer to create this instance from</param>
        /// <param name="encoding">Encoding to use for the string [Optional: Defaults to <see cref="Encoding.UTF8"/> if not provided]</param>
        public unsafe LazyEncodedString(byte* nativePtr, Encoding? encoding = null)
            : this(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(nativePtr), encoding)
        {
        }

        /// <summary>Gets a value indicating whether this instance represents an empty string</summary>
        public bool IsEmpty => ManagedString.IsValueCreated
                             ? ManagedString.Value.Length == 0
                             : !NativeBytes.IsValueCreated || NativeBytes.Value.Length == 0;

        /// <inheritdoc/>
        /// <remarks>
        /// This will perform conversion if <see cref="LazyEncodedString.LazyEncodedString(ReadOnlySpan{byte},Encoding)"/>
        /// was used to construct this instance and conversion has not yet occurred. Otherwise it will provide the
        /// string it was constructed with or a previously converted one.
        /// </remarks>
        public override string ToString() => ManagedString.Value;

        /// <summary>Gets a <see cref="ReadOnlySpan{T}"/> of bytes for the native encoding of the string</summary>
        /// <returns>Span for the encoded bytes of the string</returns>
        /// <remarks>
        /// <para>This will perform conversion if the <see cref="LazyEncodedString.LazyEncodedString(string,Encoding)"/>
        /// was used to construct this instance and conversion has not yet occurred. Otherwise it will provide
        /// the span it was constructed with or a previously converted one.</para>
        /// <note type="important">
        /// The returned span ***INCLUDES*** the null terminator, thus it's length is the number of characters
        /// in the string + 1. This is to allow passing a pointer to the first element of the span to native
        /// code with a fixed statement. If the native API includes a length parameter then callers must subtract
        /// 1 from the length to get the number of bytes not including the terminator.
        /// </note>
        /// </remarks>
        public ReadOnlySpan<byte> ToReadOnlySpan() => new(NativeBytes.Value);

        /// <summary>Pins the native representation of this memory for use in native APIs</summary>
        /// <returns>MemoryHandle that owns the pinned data</returns>
        public MemoryHandle Pin()
        {
            return NativeBytes.Value.AsMemory().Pin();
        }

        /// <inheritdoc/>
        public bool Equals(LazyEncodedString? other)
        {
            return other is not null && (ReferenceEquals( this, other) || ManagedString.Value.Equals(other.ManagedString.Value, StringComparison.Ordinal));
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is LazyEncodedString s && Equals(s);

        // TODO: These should be size_t for max compatibility
        /// <summary>Gets the native size (in bytes, including the terminator) of the memory for this string</summary>
        public int NativeLength => NativeBytes.Value.Length;

        /// <summary>Gets the native length (in bytes, NOT including the terminator) of the native form of the string</summary>
        public int NativeStrLen => NativeLength - 1;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return ManagedString.Value.GetHashCode(StringComparison.Ordinal);
        }

        /// <summary>Gets a <see cref="LazyEncodedString"/> representation of an empty string</summary>
        public static LazyEncodedString Empty {get; } = new(string.Empty);

        /// <summary>Implicit cast to a string via <see cref="ToString"/></summary>
        /// <param name="self">instance to cast</param>
        public static implicit operator string(LazyEncodedString self)
        {
            ArgumentNullException.ThrowIfNull(self);

            return self.ToString();
        }

        /// <summary>Implicit cast to a span via <see cref="ToReadOnlySpan"/></summary>
        /// <param name="self">instance to cast</param>
        public static implicit operator ReadOnlySpan<byte>(LazyEncodedString self)
        {
            ArgumentNullException.ThrowIfNull(self);

            return self.ToReadOnlySpan();
        }

        /// <summary>Convenient implicit conversion of a managed string into a Lazily encoded string</summary>
        /// <param name="managed">managed string to wrap with lazy encoding support</param>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "It's a convenience wrapper around an existing constructor" )]
        public static implicit operator LazyEncodedString(string managed) => new(managed);

        /// <summary>Convenient implicit conversion of a managed string into a Lazily encoded string</summary>
        /// <param name="utf8Data">Span of UTF8 characters to wrap with lazy encoding support</param>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "It's a convenience wrapper around an existing constructor" )]
        public static implicit operator LazyEncodedString(ReadOnlySpan<byte> utf8Data) => new(utf8Data);

        private readonly Encoding Encoding;
        private readonly Lazy<string> ManagedString;

        // The native array MUST include the terminator so it is usable as a fixed pointer in native code
        private readonly Lazy<byte[]> NativeBytes;
    }

    /// <summary>Utility extensions to validate a <see cref="LazyEncodedString"/></summary>
    /// <remarks>
    /// These are extension methods to allow use of the <see cref="CallerArgumentExpressionAttribute"/>
    /// on the <see cref="LazyEncodedString"/> instance. Otherwise there is no way to get the name/expression
    /// that is tested.
    /// </remarks>
    public static class LazyEncodedStringValidators
    {
        /// <summary>Throws an exception if the string is null or empty</summary>
        /// <param name="self">String to test</param>
        /// <param name="exp">Argument expression that is calling this test [Normally supplied by compiler]</param>
        /// <exception cref="ArgumentException">The provided string is empty</exception>
        /// <exception cref="ArgumentNullException">The provided string is null</exception>
        public static void ThrowIfNullOrEmpty(this LazyEncodedString? self, [CallerArgumentExpression(nameof(self))] string? exp = null)
        {
            ArgumentNullException.ThrowIfNull(self, exp);
            if(self.IsEmpty)
            {
                throw new ArgumentException("String is empty", exp);
            }
        }
    }
}
