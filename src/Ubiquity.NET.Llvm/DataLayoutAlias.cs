// -----------------------------------------------------------------------
// <copyright file="DataLayoutAlias.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.DataLayoutBindings;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Owning implementation of <see cref="IDataLayout"/></summary>
    internal sealed class DataLayoutAlias
        : IDataLayout
        , IEquatable<IDataLayout>
        , IHandleWrapper<LLVMTargetDataRefAlias>
    {
        #region IEquatable<IDataLayout>

        public bool Equals(IDataLayout? other)
            => other is not null
            && (NativeHandle.Equals(other) || ToLazyEncodedString().Equals(other.ToLazyEncodedString()));

        public override bool Equals(object? obj)=> obj is DataLayoutAlias alias
                                                 ? Equals(alias)
                                                 : Equals(obj as IDataLayout);

        public override int GetHashCode() => ToLazyEncodedString().GetHashCode();
        #endregion

        public ByteOrdering Endianness => ( ByteOrdering )LLVMByteOrder( NativeHandle );

        public uint PointerSize( ) => LLVMPointerSize( NativeHandle );

        public uint PointerSize( uint addressSpace ) => LLVMPointerSizeForAS( NativeHandle, addressSpace );

        public ITypeRef IntPtrType( IContext context )
        {
            ArgumentNullException.ThrowIfNull( context );

            LLVMTypeRef typeRef = LLVMIntPtrTypeInContext( context.GetUnownedHandle(), NativeHandle );
            return typeRef.ThrowIfInvalid( ).CreateType();
        }

        /* TODO: Additional properties for DataLayout
        bool IsLegalIntegerWidth(UInt64 width);
        bool ExceedsNaturalStackAlignment(UInt64 width);
        UInt32 StackAlignment { get; }
        UInt32 AllocaAddrSpace { get; }
        bool HasMicrosoftFastStdCallMangling { get; }
        string LinkerPrivateGlobalPrefix { get; }
        char GlobalPrefix { get; }
        string PrivateGlobalPrefix { get; }
        ImmutableList<UInt32> NonIntegralAddressSpaces { get; }
        bool IsNonIntegralPointerType( IPointerType t );
        ITypeRef GetSmallestLegalIntType( ContextAlias context, UInt32 width );
        ITypeRef GetLargestLegalIntType( ContextAlias context, UInt32 width );
        UInt32 GetLargestLegalIntTypeSizeInBits();
        UInt64 GetIndexedOffsetInType(ITypeRef t, Value index0, param Value[] indices);
        StructLayout GetStructLayout(IStructType t);
        */

        public ITypeRef IntPtrType( IContext context, uint addressSpace )
        {
            ArgumentNullException.ThrowIfNull( context );

            var typeHandle = LLVMIntPtrTypeForASInContext( context.GetUnownedHandle(), NativeHandle, addressSpace );
            return typeHandle.ThrowIfInvalid( ).CreateType();
        }

        public ulong BitSizeOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMSizeOfTypeInBits( NativeHandle, typeRef.GetTypeRef( ) );
        }

        public ulong StoreSizeOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMStoreSizeOfType( NativeHandle, typeRef.GetTypeRef( ) );
        }

        public ulong AbiSizeOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMABISizeOfType( NativeHandle, typeRef.GetTypeRef( ) );
        }

        public uint AbiAlignmentOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMABIAlignmentOfType( NativeHandle, typeRef.GetTypeRef( ) );
        }

        public uint CallFrameAlignmentOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMCallFrameAlignmentOfType( NativeHandle, typeRef.GetTypeRef( ) );
        }

        public uint PreferredAlignmentOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMPreferredAlignmentOfType( NativeHandle, typeRef.GetTypeRef( ) );
        }

        public uint PreferredAlignmentOf( Value value )
        {
            ArgumentNullException.ThrowIfNull( value );

            VerifySized( value.NativeType, nameof( value ) );
            return LLVMPreferredAlignmentOfGlobal( NativeHandle, value.Handle );
        }

        public uint ElementAtOffset( IStructType structType, ulong offset )
        {
            VerifySized( structType, nameof( structType ) );
            return LLVMElementAtOffset( NativeHandle, structType.GetTypeRef( ), offset );
        }

        public ulong OffsetOfElement( IStructType structType, uint element )
        {
            VerifySized( structType, nameof( structType ) );
            return LLVMOffsetOfElement( NativeHandle, structType.GetTypeRef( ), element );
        }

        public LazyEncodedString ToLazyEncodedString()
        {
            unsafe
            {
                byte* pStr = LibLLVMGetDataLayoutString(NativeHandle, out size_t len);
                return new(new ReadOnlySpan<byte>(pStr, len.ToInt32()));
            }
        }

        public override string? ToString( )
        {
            return LLVMCopyStringRepOfTargetData( NativeHandle );
        }

        public ulong ByteSizeOf( ITypeRef llvmType ) => BitSizeOf( llvmType ) / 8u;

        public uint PreferredBitAlignmentOf( ITypeRef llvmType ) => PreferredAlignmentOf( llvmType ) * 8;

        public uint AbiBitAlignmentOf( ITypeRef llvmType ) => AbiAlignmentOf( llvmType ) * 8;

        public ulong BitOffsetOfElement( IStructType llvmType, uint element ) => OffsetOfElement( llvmType, element ) * 8;

        internal DataLayoutAlias( LLVMTargetDataRefAlias targetDataHandle )
        {
            NativeHandle = targetDataHandle;
        }

        private static void VerifySized( ITypeRef type, string name )
        {
            if( type == null )
            {
                throw new ArgumentNullException( name );
            }

            if( !type.IsSized )
            {
                throw new ArgumentException( Resources.Type_must_be_sized_to_get_target_size_information, name );
            }
        }

        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "internal interface; NOT PUBLIC" )]
        LLVMTargetDataRefAlias IHandleWrapper<LLVMTargetDataRefAlias>.Handle => NativeHandle;

        private readonly LLVMTargetDataRefAlias NativeHandle;
    }
}
