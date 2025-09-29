// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
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
    /// <note type="important">
    /// The ref count nature of an Entry is NOT consistent across LLVM APIs and requires the
    /// caller to know the behavior of the API with respect to the ref count:
    /// <list type="number">
    /// <item>Simple Reference. [Temporary] ownership remains with the caller</item>
    /// <item>API takes ownership. [Move] ownership is transferred to native implementation</item>
    /// <item>API performs an addref. [Native 'Clone'] ownership of original Entry remains with caller</item>
    /// </list>
    /// This can make use very problematic. To simplify this and keep things consistent, the implementation
    /// of the wrappers handles the special case of "Move" semantics, effectively converting it to a "Native
    /// Clone" scenario. Thus, the caller owns the ref count of any entries and the odd and inconsistent
    /// use of "move" semantics is handled internally so callers need not deal with that case directly.
    /// </note>
    /// </remarks>
    public sealed class SymbolStringPoolEntry
        : IEquatable<SymbolStringPoolEntry>
#if NET9_0_OR_GREATER
        , IEquatable<ReadOnlySpan<byte>>
#endif
        , IDisposable
    {
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
            ObjectDisposedException.ThrowIf( IsDisposed, this );

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
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            return other is not null
                && (Handle!.Equals( other.Handle ) || Equals( other.ReadOnlySpan ));
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

        /// <summary>Gets the name of this symbol</summary>
        public LazyEncodedString Name => DeferredInitSymbolString.Value;

#if DEBUG
        /// <summary>Gets the current ref count for this entry</summary>
        /// <remarks>
        /// <note type="important">
        /// The ref count is inherently an unreliable value as it is an atomic count that
        /// can change from ANY thread. Thus, it is limited to only debug scenarios for
        /// diagnosis of incorrect ref count handling. (which is sadly, rather easy, given
        /// the inconsistent API usage where it sometimes assumes the ref count (MOVE semantics)
        /// and other cases does it's own AddRef) <em><b>This requires a debug build of the Interop
        /// library to get the underlying reference count.</b></em>
        /// </note>
        /// </remarks>
        public nuint RefCount => Handle?.GetRefCount() ?? 0;
#endif

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

        /// <summary>Release the reference to the string</summary>
        public void Dispose( )
        {
            if(!IsDisposed && Handle.IsOwned)
            {
                Handle.Dispose();
                Handle = default;
            }
        }

        /// <summary>Adds a reference count for the symbol resulting in a distinct managed instance with it's own ref count (released via <see cref="Dispose"/>)</summary>
        /// <returns>New managed wrapper instance with it's own ref count to control the lifetime of the underlying symbol</returns>
        /// <exception cref="ObjectDisposedException">Object is already disposed and therefore has no reference count</exception>
        [MustUseReturnValue]
        public SymbolStringPoolEntry AddRef()
        {
           ObjectDisposedException.ThrowIf(IsDisposed, this);

           return new SymbolStringPoolEntry(Handle, addRef: true);
        }

        internal LLVMOrcSymbolStringPoolEntryRef MoveToNative()
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            return Handle.NativeAddRef();
        }

        /// <summary>This gets the underlying ABI handle</summary>
        /// <param name="addRef">indicates if this operation should include a native addref on the underlying handle</param>
        /// <returns>Underlying ABI handle</returns>
        /// <remarks>
        /// This is considered a "Dangerous" API as it gets at the underlying handle. If <paramref Name="addRef"/> is set
        /// then the returned handle requires disposal.
        /// </remarks>
        [MustUseReturnValue]
        internal LLVMOrcSymbolStringPoolEntryRef DangerousGetHandle( bool addRef = false )
        {
            ObjectDisposedException.ThrowIf( IsDisposed, this );

            return addRef ? Handle.NativeAddRef() : Handle;
        }

        internal SymbolStringPoolEntry( LLVMOrcSymbolStringPoolEntryRef h, bool addRef = false )
        {
            if(h is null || h.IsInvalid || h.IsClosed)
            {
                throw new ArgumentNullException( nameof( h ) );
            }

            Handle = addRef && h.IsOwned ? h.NativeAddRef() : new(h.DangerousGetHandle(), h.IsOwned);
            DeferredInitSymbolString = new(()=>LLVMOrcSymbolStringPoolEntryStr( Handle ), LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>Initializes a new instance of the <see cref="SymbolStringPoolEntry"/> class.</summary>
        /// <param name="abiHandle">Native ABI handle</param>
        /// <param name="alias">Flag to indicate whether this handle is an owned alias</param>
        /// <remarks>Wraps a native ABI handle in a managed projected type</remarks>
        internal SymbolStringPoolEntry( nint abiHandle, bool alias = false )
        {
            Handle = new( abiHandle, !alias );
            DeferredInitSymbolString = new(()=>LLVMOrcSymbolStringPoolEntryStr( Handle ), LazyThreadSafetyMode.PublicationOnly);
        }

        private LLVMOrcSymbolStringPoolEntryRef? Handle { get; set; }

        [MemberNotNullWhen(false, nameof(Handle))]
        internal bool IsDisposed => Handle is null || Handle.IsClosed || Handle.IsInvalid;

        // Callers might never need the contents of the string so it is lazily
        // initialized when needed. String pool entries are interned and thus are immutable, like a .NET string
        private readonly Lazy<LazyEncodedString> DeferredInitSymbolString;
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1400:Access modifier should be declared", Justification = "'file' is an accessibility" )]
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "It's file scoped - where else is it supposed to go!?" )]
    file static class ValidationExtensions
    {
        internal static SymbolStringPoolEntry Validate( [NotNull] this SymbolStringPoolEntry self, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
        {
            ArgumentNullException.ThrowIfNull( self, exp );
            ObjectDisposedException.ThrowIf( self.IsDisposed, self );
            return self;
        }
    }
}
