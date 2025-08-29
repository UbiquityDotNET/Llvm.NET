// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.DataLayoutBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Target;

namespace Ubiquity.NET.Llvm
{
#if NON_STABLE_ABI_TYPES
    /// Primitive type specification.
    [StructLayout(LayoutKind.Sequential, Pack=4, Size=8)]
    public readonly record struct PrimitiveSpec
    {
        public readonly UInt32 BitWidth;
        public readonly byte ABIAlign;
        public readonly byte PrefAlign;
    }

    /// Pointer type specification.
    [StructLayout(LayoutKind.Sequential, Pack=4, Size=20)]
    public readonly record struct PointerSpec
    {
        public readonly UInt32 AddrSpace;     // offsetof == 0
        public readonly UInt32 BitWidth;      // offsetof == 4
        public readonly byte ABIAlign;        // offsetof == 8
        public readonly byte PrefAlign;       // offsetof == 9
        // padding for alignment (offset 10, 11)
        public readonly UInt32 IndexBitWidth; // offsetof == 12

        // Pointers in this address space don't have a well-defined bitwise
        // representation (e.g. may be relocated by a copying garbage collector).
        // Additionally, they may also be non-integral (i.e. containing additional
        // metadata such as bounds information/permissions).
        public bool IsNonIntegral => IsNonIntegral_ != 0;

        // DANGER: C++ does NOT define the size of a "bool" the C++ form of
        // this structure is therefore NOT ABI stable. Most compilers, runtimes,
        // environments use a byte value, but that's not guaranteed. The interop
        // ABI for this library specifies that this structure treats it as one byte.
        // if an implementation uses some other representation of bool it MUST convert
        // to a byte when using this structure. constexpr if can, and should be used
        // to handle that as it is a compile time constant.
        private readonly byte IsNonIntegral_; // offsetof == 16; sizeof(1) [Windows+MSVC at least]
        // padding for alignment (offset 17, 18, 19)
    }

    enum FunctionPtrAlignType
    {
        /// The function pointer alignment is independent of the function alignment.
        Independent,
        /// The function pointer alignment is a multiple of the function alignment.
        MultipleOfFunctionAlign,
    }
#endif

    /// <summary>Alias implementation of <see cref="IDataLayout"/></summary>
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
    internal sealed class DataLayoutAlias
        : IDataLayout
        , IEquatable<IDataLayout>
        , IHandleWrapper<LLVMTargetDataRefAlias>
    {
        #region IEquatable<IDataLayout>

        public bool Equals( IDataLayout? other )
            => other is not null
            && (Handle.Equals( other ) || LibLLVMTargeDataRefOpEquals( Handle, other.GetUnownedHandle() ));

        public override bool Equals( object? obj ) => obj is DataLayoutAlias alias
                                                 ? Equals( alias )
                                                 : Equals( obj as IDataLayout );

        // TODO: IMPLEMENT support for DataLayout.GetHashCode()
        // Sadly, the raw values used for equality checks are not available in the LLVM-C API
        // (Not even to native code consumers in C++)
        public override int GetHashCode( ) => throw new NotSupportedException( "Computation of a hash code for DataLayout is not currently possible" );
        #endregion

        public ByteOrdering Endianness => (ByteOrdering)LLVMByteOrder( Handle );

        public uint PointerSize( ) => LLVMPointerSize( Handle );

        public uint PointerSize( uint addressSpace ) => LLVMPointerSizeForAS( Handle, addressSpace );

        public ITypeRef IntPtrType( IContext context )
        {
            ArgumentNullException.ThrowIfNull( context );

            LLVMTypeRef typeRef = LLVMIntPtrTypeInContext(context.GetUnownedHandle(), Handle);
            return typeRef.ThrowIfInvalid().CreateType();
        }

        /* TODO: Additional properties for DataLayout

        // These properties are used for equality, and therefore are the only
        // properties that should be used for GetHashCode()...
        // NOTE: These are all PRIVATE members of the native class, so it isn't possible
        // to get them. using a simple ReadOnlySpan<byte> of the class in it's entirety
        // would include the non-canonicalized string from used to create the layout.
        // Thus it is NOT possible to implement a proper hash code for this type.
        bool BigEndian,
        UInt32 AllocaAddrSpace,
        UInt32 ProgramAddrSpace,
        UInt32 DefaultGlobalsAddrSpace,
        MaybeAlign StackNaturalAlign,
        MaybeAlign FunctionPtrAlign,
        FunctionPtrAlignType TheFunctionPtrAlignType
        ManglingModeT ManglingMode,
        ReadOnlySpan<byte> LegalIntWidths,
        ReadOnlySpan<PrimitiveSpec> IntSpecs,
        ReadOnlySpan<PrimitiveSpec> FloatSpecs,
        ReadOnlySpan<PrimitiveSpec> VectorSpecs,
        ReadOnlySpan<PointerSpec> PointerSpecs,
        byte StructABIAlignment,
        byte StructPrefAlignment


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

            var typeHandle = LLVMIntPtrTypeForASInContext(context.GetUnownedHandle(), Handle, addressSpace);
            return typeHandle.ThrowIfInvalid().CreateType();
        }

        public ulong BitSizeOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMSizeOfTypeInBits( Handle, typeRef.GetTypeRef() );
        }

        public ulong StoreSizeOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMStoreSizeOfType( Handle, typeRef.GetTypeRef() );
        }

        public ulong AbiSizeOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMABISizeOfType( Handle, typeRef.GetTypeRef() );
        }

        public uint AbiAlignmentOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMABIAlignmentOfType( Handle, typeRef.GetTypeRef() );
        }

        public uint CallFrameAlignmentOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMCallFrameAlignmentOfType( Handle, typeRef.GetTypeRef() );
        }

        public uint PreferredAlignmentOf( ITypeRef typeRef )
        {
            VerifySized( typeRef, nameof( typeRef ) );
            return LLVMPreferredAlignmentOfType( Handle, typeRef.GetTypeRef() );
        }

        public uint PreferredAlignmentOf( Value value )
        {
            ArgumentNullException.ThrowIfNull( value );

            VerifySized( value.NativeType, nameof( value ) );
            return LLVMPreferredAlignmentOfGlobal( Handle, value.Handle );
        }

        public uint ElementAtOffset( IStructType structType, ulong offset )
        {
            VerifySized( structType, nameof( structType ) );
            return LLVMElementAtOffset( Handle, structType.GetTypeRef(), offset );
        }

        public ulong OffsetOfElement( IStructType structType, uint element )
        {
            VerifySized( structType, nameof( structType ) );
            return LLVMOffsetOfElement( Handle, structType.GetTypeRef(), element );
        }

        public LazyEncodedString ToLazyEncodedString( )
        {
            // This is mostly to prevent crashes in test debugging
            // As the handle is null if a breakpoint or step is
            // in the constructor BEFORE this value is initialized.
            if(Handle.IsNull)
            {
                return LazyEncodedString.Empty;
            }

            LazyEncodedString? retVal = LibLLVMGetDataLayoutString(Handle);
            Debug.Assert( retVal is not null, "Layout should always have a string representation" );
            return retVal;
        }

        public override string? ToString( )
        {
            // LLVM-C has this variant but it requires allocation in native and then dispose from
            // managed which is overhead that isn't needed, so use the lower overhead form and
            // force the full encoding here.
            // return LLVMCopyStringRepOfTargetData( NativeHandle );
            return ToLazyEncodedString()?.ToString();
        }

        public ulong ByteSizeOf( ITypeRef llvmType ) => BitSizeOf( llvmType ) / 8u;

        public uint PreferredBitAlignmentOf( ITypeRef llvmType ) => PreferredAlignmentOf( llvmType ) * 8;

        public uint AbiBitAlignmentOf( ITypeRef llvmType ) => AbiAlignmentOf( llvmType ) * 8;

        public ulong BitOffsetOfElement( IStructType llvmType, uint element ) => OffsetOfElement( llvmType, element ) * 8;

        internal DataLayoutAlias( LLVMTargetDataRefAlias targetDataHandle )
        {
            Handle = targetDataHandle;
        }

        private static void VerifySized( ITypeRef type, string name )
        {
            if(type == null)
            {
                throw new ArgumentNullException( name );
            }

            if(!type.IsSized)
            {
                throw new ArgumentException( Resources.Type_must_be_sized_to_get_target_size_information, name );
            }
        }

        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "internal interface; NOT PUBLIC" )]
        public LLVMTargetDataRefAlias Handle { get; }
    }
}
