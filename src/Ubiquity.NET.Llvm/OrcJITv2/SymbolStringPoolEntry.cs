// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

// Regions help hide the boiler plate equality stuff
#pragma warning disable SA1124 // Do not use regions

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Reference to an entry in a symbol string pool for ORC JIT v2</summary>
    /// <remarks>
    /// This holds a reference to the symbol string which is ONLY marshaled/converted to
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
        , IEquatable<ReadOnlySpan<byte>>
        , IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="SymbolStringPoolEntry"/> class from another entry (Add ref construction)</summary>
        /// <param name="other">Other string to make a new entry from</param>
        /// <remarks>
        /// In LLVM a <see cref="SymbolStringPoolEntry"/> is a pointer to a reference counted string. This constructor will create a new
        /// entry that "owns" a ref count bump (AddRefHandle) on a source string (<paramref name="other"/>). Callers must dispose of the new
        /// instance the same as the original one or the ref count is never reduced and the native memory never reclaimed (That is, it leaks!)
        /// </remarks>
        public SymbolStringPoolEntry( SymbolStringPoolEntry other )
#pragma warning disable CA2000 // Dispose objects before losing scope
            : this( other.Validate().AddRefHandle() )
#pragma warning restore CA2000 // Dispose objects before losing scope
        {
            // TODO: optimize this to use any lazy evaluated values available in other
        }

        /// <summary>Gets the managed string form of the native string</summary>
        /// <returns>managed string for this handle</returns>
        public override string? ToString( )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            return DeferredInitSymbolString.Value.ToString();
        }

        #region IEquatable<SymbolStringPoolEntry>

        /// <inheritdoc/>
        public override bool Equals( object? obj )
        {
            return obj is SymbolStringPoolEntry other && Equals( other );
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
        public bool Equals( SymbolStringPoolEntry? other )
        {
            return other is not null
                && (Handle.Equals( other.Handle ) || Equals( other.ReadOnlySpan ));
        }

        /// <summary>Tests if the span of characters for this string is identical to the provided span</summary>
        /// <param name="otherSpan">Span of bytes to compare this string to</param>
        /// <returns><see langword="true"/> if the spans contain the same data or <see langword="false"/> if not</returns>
        public bool Equals( ReadOnlySpan<byte> otherSpan ) => ReadOnlySpan.SequenceEqual( otherSpan );

        /// <inheritdoc/>
        [SuppressMessage( "Globalization", "CA1307:Specify StringComparison for clarity", Justification = "Matches string API" )]
        public override int GetHashCode( )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            return ToString()?.GetHashCode() ?? 0;
        }

        /// <summary>Gets the hash code for the managed string</summary>
        /// <param name="comparisonType">Kind of comparison to perform when computing the hash code</param>
        /// <returns>Hash code for the managed string</returns>
        public int GetHashCode( StringComparison comparisonType )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            return ToString()?.GetHashCode( comparisonType ) ?? 0;
        }
        #endregion

        /// <summary>Release the reference to the string</summary>
        public void Dispose( )
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
                ObjectDisposedException.ThrowIf( IsDisposed, this );

                unsafe
                {
                    return DeferredInitSymbolString.Value.ToReadOnlySpan();
                }
            }
        }

        internal LLVMOrcSymbolStringPoolEntryRefAlias ToABI( )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            DangerousAddRef();
            return LLVMOrcSymbolStringPoolEntryRefAlias.FromABI( Handle.DangerousGetHandle() );
        }

        internal SymbolStringPoolEntry( LLVMOrcSymbolStringPoolEntryRef h )
        {
            if(h.IsInvalid || h.IsClosed)
            {
                throw new ArgumentNullException( nameof( h ) );
            }

            Handle = h.Move();
            DeferredInitSymbolString = new(()=>LLVMOrcSymbolStringPoolEntryStr( Handle ), LazyThreadSafetyMode.PublicationOnly);
        }

        internal SymbolStringPoolEntry( nint abiHandle, bool alias = false )
        {
            Handle = new( abiHandle, !alias );
            DeferredInitSymbolString = new(()=>LLVMOrcSymbolStringPoolEntryStr( Handle ), LazyThreadSafetyMode.PublicationOnly);
        }

        internal LLVMOrcSymbolStringPoolEntryRef Handle { get; }

        /// <summary>Increases the ref count on this instance</summary>
        /// <remarks>
        /// This is generally only safe to do on an instance to pass into native code
        /// that takes ownership of the new ref count BUT the managed caller still
        /// needs to retain a reference to the string.
        /// </remarks>
        internal void DangerousAddRef( )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            LLVMOrcRetainSymbolStringPoolEntry( Handle );
        }

        /// <summary>Decreases the ref count on this instance</summary>
        /// <remarks>
        /// This is generally only safe to do in a catch handler to release
        /// and AddRef if an exception occurs in an attempt to transfer ownership
        /// into native code.
        /// </remarks>
        internal void DangerousRelease( )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            LLVMOrcReleaseSymbolStringPoolEntry( Handle );
        }

        internal bool IsDisposed => Handle is null || Handle.IsClosed || Handle.IsInvalid;

        private LLVMOrcSymbolStringPoolEntryRef AddRefHandle( )
        {
            DangerousAddRef();
            return new( Handle.DangerousGetHandle(), owner: true );
        }

        // Callers might never need the contents of the string so it is lazily
        // initialize when needed. String pool entries are interned and thus are immutable, like a .NET string
        private readonly Lazy<LazyEncodedString> DeferredInitSymbolString;
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1400:Access modifier should be declared", Justification = "'file' is an accessibility" )]
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "It's file scoped - where else is it supposed to go!?" )]
    file static class ValidationExtensions
    {
        internal static SymbolStringPoolEntry Validate( this SymbolStringPoolEntry self, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
        {
            ArgumentNullException.ThrowIfNull( self, exp );
            ObjectDisposedException.ThrowIf( self.IsDisposed, self );
            return self;
        }
    }
}
