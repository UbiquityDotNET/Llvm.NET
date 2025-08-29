// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Lazily encoded string with implicit casting to a read only span of bytes or a normal managed string</summary>
    /// <remarks>
    /// <para>This class handles capturing a managed string or a span of bytes for a native one. It supports a lazily
    /// evaluated representation of the string as a sequence of native bytes or a managed string. The encoding ONLY happens
    /// once, and ONLY when needed the first time. This reduces the overhead to a onetime hit for any strings that "sometimes"
    /// get passed to native code or native strings that "sometimes" get used in managed code as a string.</para>
    /// <para>This is essentially a pair of <see cref="Lazy{T}"/> members to handle conversions in one direction.
    /// Constructors exist for an existing <see cref="ReadOnlySpan{T}"/> of bytes and another for a string. Each constructor
    /// will pre-initialize one of the lazy values and set up the evaluation function for the other. Thus the string
    /// is encoded/decoded ONLY if needed and then, only once.</para>
    /// <para>This class handles all the subtle complexity regarding terminators as most of the encoding APIs in .NET will
    /// drop/ignore a string terminator but native code usually, but not always, requires it. Thus, this ensures the presence
    /// of a terminator even if the span provided to the constructor doesn't include one. (It has to copy the string anyway
    /// so why not be nice and robust at the cost of one byte of allocated space)</para>
    /// </remarks>
    [NativeMarshalling( typeof( LazyEncodedStringMarshaller ) )]
    public sealed class LazyEncodedString
        : IEquatable<LazyEncodedString?>
        , IEquatable<string?>
        , IEqualityOperators<LazyEncodedString, LazyEncodedString, bool>
    {
        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from an existing managed string</summary>
        /// <param name="managed">string to lazy encode for native code use</param>
        /// <param name="encoding">Encoding to use for the string [Optional: Defaults to <see cref="Encoding.UTF8"/> if not provided]</param>
        public LazyEncodedString( string managed, Encoding? encoding = null )
        {
            EncodingCodePage = (encoding ?? Encoding.UTF8).CodePage;

            // Pre-Initialize with the provided string
            ManagedString = new( managed );
            NativeBytes = new( GetNativeArrayWithTerminator );

            unsafe byte[] GetNativeArrayWithTerminator( )
            {
                int nativeByteLen = Encoding.GetByteCount(managed) + 1; // +1 for terminator
                byte[] retVal = new byte[nativeByteLen];

                int numBytes = Encoding.GetBytes(ManagedString.Value, retVal);
                Debug.Assert( numBytes == nativeByteLen - 1, "Invalid terminator length assumptions!" ); // -1 as numBytes does not account for terminator
                retVal[ numBytes ] = 0; // force null termination so it is viable with native code
                return retVal;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="LazyEncodedString"/> class from an existing span of native bytes</summary>
        /// <param name="span">span of native bytes</param>
        /// <param name="encoding">Encoding to use for the string [Optional: Defaults to <see cref="Encoding.UTF8"/> if not provided]</param>
        /// <remarks>This has some performance overhead as it MUST make a copy of the contents of the span. The lifetime
        /// of <paramref name="span"/> is not guaranteed beyond this call and is not allowed as a field of this type.
        /// </remarks>
        public LazyEncodedString( ReadOnlySpan<byte> span, Encoding? encoding = null )
        {
            EncodingCodePage = (encoding ?? Encoding.UTF8).CodePage;
            NativeBytes = new( GetNativeArrayWithTerminator( span ) );
            ManagedString = new( ConvertString, LazyThreadSafetyMode.ExecutionAndPublication );

            // drop the terminator for conversion to managed so it won't appear in the string
            string ConvertString( ) => NativeBytes.Value.Length > 0 ? Encoding.GetString( NativeBytes.Value[ ..^1 ] ) : string.Empty;

            // This incurs the cost of a copy but the lifetime of the span is not known or
            // guaranteed beyond this call.
            static byte[] GetNativeArrayWithTerminator( ReadOnlySpan<byte> span )
            {
                // If it already has a terminator just use it
                if(span.IsEmpty || span[ ^1 ] == 0)
                {
                    return span.ToArray();
                }

                // need to account for terminator so manually allocate and copy the span
                byte[] retVal = new byte[span.Length + 1];
                span.CopyTo( retVal );
                retVal[ ^1 ] = 0; // force terminator
                return retVal;
            }
        }

        /// <summary>Gets the encoding used for this instance</summary>
        public Encoding Encoding => Encoding.GetEncoding( EncodingCodePage );

        /// <summary>Gets a value indicating whether this instance represents an empty string</summary>
        /// <remarks>Never incurs the cost of conversion</remarks>
        public bool IsEmpty => ManagedString.IsValueCreated
                             ? ManagedString.Value.Length == 0
                             : !NativeBytes.IsValueCreated || NativeBytes.Value.Length == 0;

        /// <inheritdoc/>
        /// <remarks>
        /// This will perform conversion if <see cref="LazyEncodedString.LazyEncodedString(ReadOnlySpan{byte},Encoding)"/>
        /// was used to construct this instance and conversion has not yet occurred. Otherwise it will provide the
        /// string it was constructed with or a previously converted one.
        /// </remarks>
        public override string ToString( ) => ManagedString.Value;

        /// <summary>Gets a <see cref="ReadOnlySpan{T}"/> of bytes for the native encoding of the string</summary>
        /// <param name="includeTerminator">Indicates whether the span includes the terminator character [default: false]</param>
        /// <returns>Span for the encoded bytes of the string</returns>
        /// <remarks>
        /// <para>This will perform conversion if the <see cref="LazyEncodedString.LazyEncodedString(string,Encoding)"/>
        /// was used to construct this instance and conversion has not yet occurred. Otherwise it will provide
        /// the span it was constructed with or a previously converted one.</para>
        /// <para>
        /// When passing a string view (pointer+length) then the terminator is not normally included in the length.
        /// When passing a string as a null terminated sequence of characters (pointer) then the null terminator is
        /// also not normally included in the length. Thus the default behavior is to not include the terminator.
        /// If a span that include any allocated space for a terminator is needed then a <see langword="true"/> value
        /// for <paramref name="includeTerminator"/> will result in a span that includes the terminator.
        /// </para>
        /// </remarks>
        public ReadOnlySpan<byte> ToReadOnlySpan( bool includeTerminator = false )
            => new( NativeBytes.Value, 0, checked((int)(includeTerminator ? NativeLength : NativeStrLen)) );

        /// <summary>Pins the native representation of this memory for use in native APIs</summary>
        /// <returns>MemoryHandle that owns the pinned data</returns>
        public MemoryHandle Pin( )
        {
            return NativeBytes.Value.AsMemory().Pin();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// May incur the cost of conversion to native if both strings don't have the same
        /// form. If both have, a managed representation already that is used, otherwise the
        /// native data is used for the comparison. This only performs an ordinal comparison
        /// of the values.
        /// </remarks>
        public bool Equals( LazyEncodedString? other )
        {
            if(other is null)
            {
                return false;
            }

            if(ReferenceEquals( this, other ))
            {
                return true;
            }

            // both have Managed version of string so compare that...
            if(ManagedString.IsValueCreated && other.ManagedString.IsValueCreated)
            {
                return Equals( other.ManagedString.Value );
            }

            // Otherwise at least one has the native form, so use that for both
            // this might incur the cost of conversion for one of them
            return ToReadOnlySpan().SequenceEqual( other.ToReadOnlySpan() );
        }

        /// <inheritdoc/>
        public bool Equals( string? other )
        {
            return ManagedString.Value.Equals( other, StringComparison.Ordinal );
        }

        /// <inheritdoc/>
        public override bool Equals( object? obj )
        {
            return (obj is LazyEncodedString les && Equals( les ))
                || (obj is string s && Equals( s ));
        }

        /// <summary>Gets the native size (in bytes, including the terminator) of the memory for this string</summary>
        public nuint NativeLength => checked((nuint)NativeBytes.Value.LongLength);

        /// <summary>Gets the native length (in bytes, NOT including the terminator) of the native form of the string</summary>
        public nuint NativeStrLen => NativeLength - 1;

        /// <inheritdoc/>
        /// <remarks>
        /// For consistency of the computed hash code value, this will use the managed form of the
        /// string. This might incur a one time perf hit to encode it, but that is generally assumed
        /// needed when this is called anyway, so not a major hit.
        /// </remarks>
        public override int GetHashCode( )
        {
            return ManagedString.Value.GetHashCode( StringComparison.Ordinal );
        }

        /// <summary>Tests if the given <see cref="LazyEncodedString"/> is <see langword="null"/> or Empty</summary>
        /// <param name="self">string to test</param>
        /// <returns><see langword="true"/> if <paramref name="self"/> is <see langword="null"/> or Empty</returns>
        /// <remarks>
        /// This test does NOT have the side effect of performing any conversions. Testing for null or empty
        /// is viable on either form directly as-is.
        /// </remarks>
        public static bool IsNullOrEmpty( [NotNullWhen( false )] LazyEncodedString? self )
        {
            if(self is null)
            {
                return true;
            }

            if(self.ManagedString.IsValueCreated)
            {
                return string.IsNullOrEmpty( self.ManagedString.Value );
            }
            else if(self.NativeBytes.IsValueCreated)
            {
                return self.NativeStrLen == 0;
            }

            return false;
        }

        /// <summary>Gets a value indicating if the provided instance is <see langword="null"/> or all whitespace</summary>
        /// <param name="self">instance to check</param>
        /// <returns><see langword="true"/> if <paramref name="self"/> is <see langword="null"/> or all whitespace</returns>
        /// <remarks>
        /// This might have the performance overhead of converting the native representation to managed in order to perform
        /// the test.
        /// </remarks>
        public static bool IsNullOrWhiteSpace( [NotNullWhen( false )] LazyEncodedString? self )
        {
            // easy check first as secondary level might require conversion to a managed string
            // for detection of whitespace.
            return self is null
                || string.IsNullOrWhiteSpace( self.ManagedString.Value );
        }

        /// <summary>Creates a nullable <see cref="LazyEncodedString"/> from an unmanaged view</summary>
        /// <param name="p">pointer to first character of view</param>
        /// <param name="len">length of the view</param>
        /// <returns><see cref="LazyEncodedString"/></returns>
        /// <remarks>
        /// To preserve the potential meaning distinction between null and empty strings, this will treat a
        /// <see langword="null"/> for <paramref name="p"/> as a <see langword="null"/> return value. Empty,
        /// strings are a <see cref="LazyEncodedString.Empty"/>.
        /// <note type="note">
        /// This method handles safely converting (down casting) the length to an <see cref="int"/>  as required
        /// by .NET runtime types. [<see cref="nuint"/> is used as managed equivalent of size_t, though the formal C/C++
        /// definition is compiler/platform specific. Most use an unsigned 64 bit value for a 64 bit platform and an
        /// unsigned 32 bit value for 32 bit platforms.]
        /// </note>
        /// </remarks>
        public static unsafe LazyEncodedString? FromUnmanaged( byte* p, nuint len )
        {
            if(p is null)
            {
                return null;
            }

            // attempt to convert all empty strings to same instance to reduce
            // pressure on GC Heap.
            var span = new ReadOnlySpan<byte>(p, checked((int)len));
            return span.IsEmpty ? Empty : new( span );
        }

        /// <summary>Creates a nullable <see cref="LazyEncodedString"/> from an unmanaged string pointer (terminated!)</summary>
        /// <param name="p">pointer to first character of string</param>
        /// <returns><see cref="LazyEncodedString"/></returns>
        /// <remarks>
        /// To preserve the potential meaning distinction between <see langword="null"/> and empty strings, this will treat a
        /// <see langword="null"/> for <paramref name="p"/> as a <see langword="null"/> return value. Empty,
        /// strings are always <see cref="LazyEncodedString.Empty"/>.
        /// </remarks>
        [return: NotNullIfNotNull( nameof( p ) )]
        public static unsafe LazyEncodedString? FromUnmanaged( byte* p )
        {
            if(p == null)
            {
                // until: https://github.com/dotnet/roslyn/issues/78550 is fixed, have to do the ugly suppression
#pragma warning disable CS8825 // Return value must be non-null because parameter is non-null.
                return null;
#pragma warning restore CS8825 // Return value must be non-null because parameter is non-null.
            }

            // attempt to convert all empty strings to same instance to reduce
            // pressure on GC Heap.
            var span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(p);
            return span.IsEmpty ? Empty : new( span );
        }

        /// <inheritdoc cref="string.Join{T}(char, IEnumerable{T})"/>
        /// <returns><see cref="LazyEncodedString"/> with the joined result</returns>
        /// <remarks>
        /// This will join the managed form of any LazyEncodedString (Technically the results
        /// of calling <see cref="object.ToString"/> on each value provided) to produce a final
        /// joined string. The result will only have the managed form created but will lazily
        /// provide the managed form if/when needed (conversion still only happens once).
        /// </remarks>
        public static LazyEncodedString Join<T>( char separator, params IEnumerable<T> values )
        {
            return new( string.Join( separator, values ) );
        }

        /// <summary>Specialized join that optimizes for <see cref="LazyEncodedString"/> values</summary>
        /// <param name="separator">Separator character</param>
        /// <param name="values">Values to join together</param>
        /// <returns>Result of the joined set of values</returns>
        public static LazyEncodedString Join( char separator, params IEnumerable<LazyEncodedString> values )
        {
            // TODO: Optimize this to deal with only the native UTF8 form.
            // Though "optimize" is a bit of a toss up. The normal case is that ALL of the values
            // are in the same form. If not, things get murky about any actual benefits of optimizations.
            // If all are already in native form then this could convert the separator and use that character
            // to join the contents of the native arrays.
            // for now just do the unoptimized variant.
            return new( string.Join( separator, values ) );
        }

        /// <summary>Gets a <see cref="LazyEncodedString"/> representation of an empty string</summary>
        public static LazyEncodedString Empty { get; } = new( string.Empty );

        /// <summary>Implicit cast to a string via <see cref="ToString"/></summary>
        /// <param name="self">instance to cast</param>
        public static implicit operator string( LazyEncodedString self )
        {
            ArgumentNullException.ThrowIfNull( self );

            return self.ToString();
        }

        /// <summary>Implicit cast to a span via <see cref="ToReadOnlySpan"/></summary>
        /// <param name="self">instance to cast</param>
        /// <remarks>
        /// The resulting span is a view of the characters that does NOT include the terminating 0
        /// </remarks>
        public static implicit operator ReadOnlySpan<byte>( LazyEncodedString self )
        {
            ArgumentNullException.ThrowIfNull( self );

            return self.ToReadOnlySpan();
        }

        /// <summary>Converts a managed string into a <see cref="LazyEncodedString"/></summary>
        /// <param name="managed">Input string to convert</param>
        /// <returns><see cref="LazyEncodedString"/> wrapping <paramref name="managed"/></returns>
        /// <remarks>
        /// If the input <paramref name="managed"/> is <see langword="null"/> then this will return
        /// a <see langword="null"/> to maintain intent and semantics that <see langword="null"/>
        /// may not have the same meaning as an empty string.
        /// </remarks>
        public static LazyEncodedString? From( string? managed )
        {
            return managed is null ? null : new( managed );
        }

        /// <summary>Convenient implicit conversion of a managed string into a Lazily encoded string</summary>
        /// <param name="managed">managed string to wrap with lazy encoding support</param>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "It has one, just not the dumb name analyzer wants" )]
        [return: NotNullIfNotNull( nameof( managed ) )]
        public static implicit operator LazyEncodedString?( string? managed ) => From( managed );

        /// <summary>Convenient implicit conversion of a managed string into a Lazily encoded string</summary>
        /// <param name="utf8Data">Span of UTF8 characters to wrap with lazy encoding support</param>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "It's a convenience wrapper around an existing constructor" )]
        public static implicit operator LazyEncodedString( ReadOnlySpan<byte> utf8Data ) => new( utf8Data );

        /// <inheritdoc/>
        public static bool operator ==( LazyEncodedString? left, LazyEncodedString? right )
        {
            return ReferenceEquals( left, right )
                || (left is not null && left.Equals( right ));
        }

        /// <inheritdoc/>
        public static bool operator !=( LazyEncodedString? left, LazyEncodedString? right )
        {
            return !ReferenceEquals( left, right )
                && (left is null || !left.Equals( right ));
        }

        private readonly int EncodingCodePage;
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
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Tightly coupled extensions" )]
    public static class LazyEncodedStringValidators
    {
        /// <summary>Throws an exception if the string is null or empty</summary>
        /// <param name="self">String to test</param>
        /// <param name="exp">Argument expression that is calling this test [Normally supplied by compiler]</param>
        /// <exception cref="ArgumentException">The provided string is empty</exception>
        /// <exception cref="ArgumentNullException">The provided string is null</exception>
        public static void ThrowIfNullOrEmpty( this LazyEncodedString? self, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
        {
            if(LazyEncodedString.IsNullOrEmpty( self ))
            {
                throw new ArgumentException( "String is null or empty", exp );
            }
        }

        /// <summary>Throws an exception if the string is null or empty</summary>
        /// <param name="self">String to test</param>
        /// <param name="exp">Argument expression that is calling this test [Normally supplied by compiler]</param>
        /// <exception cref="ArgumentException">The provided string is empty</exception>
        /// <exception cref="ArgumentNullException">The provided string is null</exception>
        public static void ThrowIfNullOrWhiteSpace( this LazyEncodedString? self, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
        {
            if(LazyEncodedString.IsNullOrWhiteSpace( self ))
            {
                throw new ArgumentException( "String is null or white space", exp );
            }
        }
    }
}
