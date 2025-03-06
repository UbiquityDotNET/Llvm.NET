// -----------------------------------------------------------------------
// <copyright file="LazyMarshalledString.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Lazily encoded string with implicit casting to a read only span of bytes or a normal managed string</summary>
    /// <remarks>
    /// <para>This class handles capturing a managed string AND a lazily evaluated representation of the string as
    /// a sequence of native bytes. The encoding to bytes ONLY happens once, and ONLY when needed the first time.
    /// This reduces the overhead to a onetime hit for any strings that "sometimes" get passed to native code.</para>
    /// <para>This is essentially a pair of <see cref="Lazy{T}"/> members to handle lazy conversions in one direction.
    /// Constructors exist for an existing <see cref="ReadOnlySpan{T}"/> of bytes and another for a string. Each constructor
    /// will pre-initialize one of the lazy values and set up the lazy evaluation function for the other. Thus the string
    /// is encoded/decoded ONLY if needed and then, only once.</para>
    /// <para>This class handles all the subtle complexity regarding terminators as most of the encoding will drop/ignore
    /// a string terminator but native code absolutely needs it. Thus, this ensures the presence of a terminator even
    /// if the span provided to the constructor doesn't include one. (It has to copy the string anyway so why not be nice
    /// and robust)</para>
    /// </remarks>
    public class LazyEncodedString
    {
        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from an existing managed string</summary>
        /// <param name="managed">string to lazy encode for native code use</param>
        public LazyEncodedString(string managed)
        {
            // Pre-Initialize with the provided string
            ManagedString = new(managed);
            NativeBytes = new(GetNativeArrayWithTerminator);

            unsafe byte[] GetNativeArrayWithTerminator()
            {
                var encoding = ExecutionEncodingStringMarshaller.Encoding;

                int nativeByteLen = encoding.GetByteCount(managed) + 1; // +1 for terminator
                byte[] retVal = new byte[nativeByteLen];

                int numBytes = ExecutionEncodingStringMarshaller.Encoding.GetBytes(ManagedString.Value, retVal);
                Debug.Assert(numBytes == nativeByteLen - 1, "Invalid terminator length assumptions!"); // -1 as numBytes does not account for terminator
                retVal[numBytes] = 0; // force null termination so it is viable with native code
                return retVal;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from an existing span of native bytes</summary>
        /// <param name="span">span of native bytes</param>
        /// <remarks>This has some performance overhead as it MUST make a copy of the contents of the span. The lifetime
        /// of <paramref name="span"/> is not guaranteed beyond this call.
        /// </remarks>
        public LazyEncodedString(ReadOnlySpan<byte> span)
        {
            NativeBytes = new(GetNativeArrayWithTerminator(span));
            ManagedString = new(ConvertString, LazyThreadSafetyMode.ExecutionAndPublication);

            // drop the terminator for conversion to managed so it won't appear in the string
            string ConvertString() => ExecutionEncodingStringMarshaller.Encoding.GetString(NativeBytes.Value[..^1]);

            // This incurs the cost of a copy but the lifetime of the span is not known or
            // guaranteed beyond this call.
            static byte[] GetNativeArrayWithTerminator(ReadOnlySpan<byte> span)
            {
                // If it already has a terminator just use it
                if(span[^1] == 0)
                {
                    return span.ToArray();
                }

                byte[] retVal = new byte[span.Length + 1];
                span.CopyTo(retVal);
                retVal[^1] = 0; // force terminator
                return retVal;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from a raw native pointer</summary>
        /// <param name="nativePtr">pointer to create this instance from</param>
        public unsafe LazyEncodedString(byte* nativePtr)
            : this(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(nativePtr)) // WRONG This doesn't contain the terminator!
        {
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This will perform conversion if <see cref="LazyEncodedString.LazyEncodedString(ReadOnlySpan{byte})"/>
        /// was used to construct this instance and conversion has not yet occurred. Otherwise it will provide the
        /// string it was constructed with or a previously converted one.
        /// </remarks>
        public override string ToString() => ManagedString.Value;

        /// <summary>Gets a <see cref="ReadOnlySpan{T}"/> of bytes for the native encoding of the string</summary>
        /// <returns>Span for the encoded bytes of the string</returns>
        /// <remarks>
        /// <para>This will perform conversion if the <see cref="LazyEncodedString.LazyEncodedString(string)"/>
        /// was used to construct this instance anf conversion has not yet occurred. Otherwise it will provide
        /// the span it was constructed with or a previously converted one.</para>
        /// <note type="important">
        /// The returned span ***INCLUDES*** the null terminator, thus it's length is the number of characters
        /// in the string + 1. This is to allow passing a pointer to the first element of the span to native
        /// code with a fixed statement. If the native API includes a length parameter then callers must subtract
        /// 1 from the length to get the number of bytes not including the terminator.
        /// </note>
        /// </remarks>
        public ReadOnlySpan<byte> ToReadOnlySpan() => new(NativeBytes.Value);

        /// <summary>Implicit cast to a string via <see cref="ToString"/></summary>
        /// <param name="self">instance to cast</param>
        public static implicit operator string(LazyEncodedString self) => self.ThrowIfNull().ToString();

        /// <summary>Implicit cast to a span via <see cref="ToReadOnlySpan"/></summary>
        /// <param name="self">instance to cast</param>
        public static implicit operator ReadOnlySpan<byte>(LazyEncodedString self) => self.ThrowIfNull().ToReadOnlySpan();

        private readonly Lazy<string> ManagedString;

        // The native array MUST include the terminator so it is useable as a fixed pointer in native code
        private readonly Lazy<byte[]> NativeBytes;
    }
}
