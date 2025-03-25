// -----------------------------------------------------------------------
// <copyright file="DataLayout.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.DataLayoutBindings;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Owning implementation of <see cref="IDataLayout"/></summary>
    public sealed class DataLayout
        : IDataLayout
        , IGlobalHandleOwner<LLVMTargetDataRef>
        , IDisposable
        , IEquatable<DataLayout>
        , IEquatable<IDataLayout>
        , IUtf8SpanParsable<DataLayout>
    {
        #region IEquatable<>

        /// <inheritdoc/>
        public bool Equals(IDataLayout? other)
            => other is not null
            && (this.GetUnownedHandle().Equals(other.GetUnownedHandle()) || ToLazyEncodedString().Equals(other.ToLazyEncodedString()));

        /// <inheritdoc/>
        public bool Equals(DataLayout? other)
            => other is not null
            && (NativeHandle.Equals(other.NativeHandle) || ToLazyEncodedString().Equals(other.ToLazyEncodedString()));

        /// <inheritdoc/>
        public override bool Equals(object? obj)=> obj is DataLayout owner
                                                   ? Equals(owner)
                                                   : Equals(obj as IDataLayout);

        /// <inheritdoc/>
        public override int GetHashCode() => ToLazyEncodedString().GetHashCode();
        #endregion

        #region IDataLayout (via Impl)

        /// <inheritdoc/>
        public uint PointerSize( ) => Impl.PointerSize();

        /// <inheritdoc/>
        public uint PointerSize( uint addressSpace ) => Impl.PointerSize( addressSpace );

        /// <inheritdoc/>
        public ITypeRef IntPtrType( IContext context ) => Impl.IntPtrType( context );

        /// <inheritdoc/>
        public ITypeRef IntPtrType( IContext context, uint addressSpace ) => Impl.IntPtrType( context, addressSpace );

        /// <inheritdoc/>
        public ulong BitSizeOf( ITypeRef typeRef ) => Impl.BitSizeOf( typeRef );

        /// <inheritdoc/>
        public ulong StoreSizeOf( ITypeRef typeRef ) => Impl.StoreSizeOf( typeRef );

        /// <inheritdoc/>
        public ulong AbiSizeOf( ITypeRef typeRef ) => Impl.AbiSizeOf( typeRef );

        /// <inheritdoc/>
        public uint AbiAlignmentOf( ITypeRef typeRef ) => Impl.AbiAlignmentOf( typeRef );

        /// <inheritdoc/>
        public uint CallFrameAlignmentOf( ITypeRef typeRef ) => Impl.CallFrameAlignmentOf( typeRef );

        /// <inheritdoc/>
        public uint PreferredAlignmentOf( ITypeRef typeRef ) => Impl.PreferredAlignmentOf( typeRef );

        /// <inheritdoc/>
        public uint PreferredAlignmentOf( Value value ) => Impl.PreferredAlignmentOf( value );

        /// <inheritdoc/>
        public uint ElementAtOffset( IStructType structType, ulong offset ) => Impl.ElementAtOffset( structType, offset );

        /// <inheritdoc/>
        public ulong OffsetOfElement( IStructType structType, uint element ) => Impl.OffsetOfElement( structType, element );

        /// <inheritdoc/>
        public LazyEncodedString ToLazyEncodedString( ) => Impl.ToLazyEncodedString();

        /// <inheritdoc/>
        public ulong ByteSizeOf( ITypeRef llvmType ) => Impl.ByteSizeOf( llvmType );

        /// <inheritdoc/>
        public uint PreferredBitAlignmentOf( ITypeRef llvmType ) => Impl.PreferredBitAlignmentOf( llvmType );

        /// <inheritdoc/>
        public uint AbiBitAlignmentOf( ITypeRef llvmType ) => Impl.AbiBitAlignmentOf( llvmType );

        /// <inheritdoc/>
        public ulong BitOffsetOfElement( IStructType llvmType, uint element ) => Impl.BitOffsetOfElement( llvmType, element );

        /// <inheritdoc/>
        public ByteOrdering Endianness => Impl.Endianness;
        #endregion

        /// <summary>Gets a value indicating whether this instance is already disposed</summary>
        public bool IsDisposed => NativeHandle is null || NativeHandle.IsInvalid || NativeHandle.IsClosed;

        /// <inheritdoc/>
        public void Dispose( )
        {
            NativeHandle.Dispose();
        }

        /// <inheritdoc/>
        public static DataLayout Parse( ReadOnlySpan<byte> utf8Text, IFormatProvider? provider )
        {
            #pragma warning disable IDISP007 // Don't dispose injected
            // nativeRef is NOT injected, it's an OUT param and owned by this call site
            using var errRef = ParseLayout(utf8Text, out LLVMTargetDataRef nativeRef);
            using(nativeRef)
            {
                errRef.ThrowIfFailed();
                return new(nativeRef);
            }
            #pragma warning restore IDISP007 // Don't dispose injected
        }

        /// <inheritdoc/>
        public static bool TryParse( ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen( false )] out DataLayout result )
        {
            result = null;

            #pragma warning disable IDISP007 // Don't dispose injected
            // nativeRef is NOT injected, it's an OUT param and owned by this call site
            using var errRef = ParseLayout(utf8Text, out LLVMTargetDataRef nativeRef);
            using(nativeRef)
            {
                if (errRef.Failed)
                {
                    return false;
                }

                result = new(nativeRef);
                return true;
            }
            #pragma warning restore IDISP007 // Don't dispose injected
        }

        internal DataLayout( LLVMTargetDataRef targetDataHandle, [CallerArgumentExpression(nameof(targetDataHandle))] string? exp = null )
        {
            if( targetDataHandle is null || targetDataHandle.IsInvalid || targetDataHandle.IsClosed)
            {
                throw new ArgumentException("Invalid handle", exp);
            }

            NativeHandle = targetDataHandle.Move();
            AliasImpl = new(NativeHandle);
        }

        /// <inheritdoc/>
        internal static LLVMErrorRef ParseLayout(ReadOnlySpan<byte> utf8Text, out LLVMTargetDataRef nativeRef)
        {
            unsafe
            {
                fixed(byte* p = &MemoryMarshal.GetReference(utf8Text))
                {
                    return LibLLVMParseDataLayout(p, utf8Text.Length, out nativeRef );
                }
            }
        }

        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "internal interface" )]
        LLVMTargetDataRef IGlobalHandleOwner<LLVMTargetDataRef>.OwnedHandle => NativeHandle;

        /// <inheritdoc/>
        void IGlobalHandleOwner<LLVMTargetDataRef>.InvalidateFromMove( ) => NativeHandle.SetHandleAsInvalid();

        private DataLayoutAlias Impl
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                return AliasImpl;
            }
        }

        private readonly DataLayoutAlias AliasImpl;
        private readonly LLVMTargetDataRef NativeHandle;
    }
}
