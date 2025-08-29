// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.DataLayoutBindings;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Owning implementation of <see cref="IDataLayout"/></summary>
    /// <remarks>
    /// <note type="important">
    /// <para>There is currently no way to compute a proper hash code for this type. Any attempt to
    /// call <see cref="GetHashCode"/> will result in a <see cref="NotSupportedException"/>.</para>
    /// <para>The internal data values used for equality checks are not accessible, even to native
    /// C++ code, so no projection is possible. .NET has strict requirements on the consistency
    /// of a hash code with the behavior of equality comparisons, which the current LLVM
    /// declaration/implementation do not allow.</para>
    /// </note>
    /// </remarks>
    public sealed class DataLayout
        : IDataLayout
        , IGlobalHandleOwner<LLVMTargetDataRef>
        , IDisposable
        , IEquatable<DataLayout>
        , IEquatable<IDataLayout>
    {
        #region IEquatable<>

        /// <inheritdoc/>
        public bool Equals( IDataLayout? other )
            => other is not null
            && (this.GetUnownedHandle().Equals( other.GetUnownedHandle() ) || Impl.Equals( other ));

        /// <inheritdoc/>
        public bool Equals( DataLayout? other )
            => other is not null
            && (Handle.Equals( other.Handle ) || Impl.Equals( other ));

        /// <inheritdoc/>
        public override bool Equals( object? obj ) => obj is DataLayout owner
                                                   ? Equals( owner )
                                                   : Equals( obj as IDataLayout );

        /// <inheritdoc/>
        public override int GetHashCode( ) => Impl.GetHashCode();
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
        public bool IsDisposed => Handle is null || Handle.IsInvalid || Handle.IsClosed;

        /// <inheritdoc/>
        public void Dispose( )
        {
            Handle.Dispose();
        }

        /// <summary>Parses a data layout from a textual representation</summary>
        /// <param name="text">String representation to parse</param>
        /// <returns>Parsed <see cref="DataLayout"/></returns>
        public static DataLayout Parse( LazyEncodedString text )
        {
#pragma warning disable IDISP007 // Don't dispose injected
            // see: https://github.com/DotNetAnalyzers/IDisposableAnalyzers/issues/580
            // nativeRef is NOT injected, it's an OUT param and owned by this call site
            using var errRef = LibLLVMParseDataLayout(text, out LLVMTargetDataRef nativeRef);
            using(nativeRef)
            {
                errRef.ThrowIfFailed();
                return new( nativeRef );
            }
#pragma warning restore IDISP007 // Don't dispose injected
        }

        /// <summary>Tries to parse a <see cref="DataLayout"/> from a string representation</summary>
        /// <param name="txt">String representation of the data layout</param>
        /// <param name="result">Parsed data or <see langword="null"/> if <paramref name="txt"/> does not conform to a layout string</param>
        /// <returns>Result of the parse (<see langword="true"/> if successful if <see langword="false"/> not)</returns>
        public static bool TryParse( LazyEncodedString txt, [MaybeNullWhen( false )] out DataLayout result )
        {
            result = null;

#pragma warning disable IDISP007 // Don't dispose injected
            // see: https://github.com/DotNetAnalyzers/IDisposableAnalyzers/issues/580
            // nativeRef is NOT injected, it's an OUT param and owned by this call site
            using var errRef = LibLLVMParseDataLayout(txt, out LLVMTargetDataRef nativeRef);
            using(nativeRef)
            {
                if(errRef.Failed)
                {
                    return false;
                }

                result = new( nativeRef );
                return true;
            }
#pragma warning restore IDISP007 // Don't dispose injected
        }

        internal DataLayout( LLVMTargetDataRef targetDataHandle, [CallerArgumentExpression( nameof( targetDataHandle ) )] string? exp = null )
        {
            if(targetDataHandle is null || targetDataHandle.IsInvalid || targetDataHandle.IsClosed)
            {
                throw new ArgumentException( "Invalid handle", exp );
            }

            Handle = targetDataHandle.Move();

            // Implementation is an alias handle, which is a value type that is
            // essentially a "typedef" for the opaque handle [nint, (void*)]
            AliasImpl__ = new( Handle );
        }

        /// <inheritdoc/>
        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "internal interface" )]
        LLVMTargetDataRef IGlobalHandleOwner<LLVMTargetDataRef>.OwnedHandle => Handle;

        /// <inheritdoc/>
        void IGlobalHandleOwner<LLVMTargetDataRef>.InvalidateFromMove( ) => Handle.SetHandleAsInvalid();

        private readonly LLVMTargetDataRef Handle;

        // TODO: In C#14 convert this to using "field" keyword.
        private DataLayoutAlias Impl
        {
            get
            {
                ObjectDisposedException.ThrowIf( IsDisposed, this );
                return AliasImpl__;
            }
        }

        private readonly DataLayoutAlias AliasImpl__;
    }
}
