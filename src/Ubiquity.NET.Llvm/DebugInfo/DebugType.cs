// -----------------------------------------------------------------------
// <copyright file="DebugType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Base class for Debug types bound with an LLVM type</summary>
    /// <typeparam name="TNative">Native LLVM type</typeparam>
    /// <typeparam name="TDebug">Debug type</typeparam>
    /// <remarks>
    /// <para>This is a generic type that represents a binding between a specific LLVM type AND a <see cref="DIType"/>
    /// that describes it. This maintains the relationship between the two different reals and types systems.</para>
    ///
    /// <para>Primitive types and function signature types are all interned in LLVM, thus there won't be a
    /// strict one to one relationship between an LLVM type and corresponding language specific debug
    /// type. (e.g. unsigned char, char, byte and signed byte might all be 8 bit integral values as far
    /// as LLVM is concerned.) Also, when using the pointer+alloca+memcpy pattern to pass by value the
    /// actual source debug info type is different than the LLVM function signature. This interface and
    /// it's implementations are used to construct native type and debug info pairing to allow applications
    /// to maintain a link from their AST or IR types into the LLVM native type and debug information.</para>
    ///
    /// <note type="note">
    /// It is important to note that the relationship from the <see cref="DebugInfoType"/> to it's <see cref="NativeType"/>
    /// properties is strictly ***one way***. That is, there is no way to take an arbitrary <see cref="ITypeRef"/>
    /// and re-associate it with the DebugInfoType or an implementation of this interface as there may be many such
    /// mappings to choose from.
    /// </note>
    /// </remarks>
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Interface, Generic type and static extension methods form a common API surface" )]
    public class DebugType<TNative, TDebug>
        : IDebugType<TNative, TDebug>
        , ITypeHandleOwner
        where TNative : class, ITypeRef
        where TDebug : DIType
    {
        /// <inheritdoc/>
        bool IEquatable<ITypeRef>.Equals( ITypeRef? other ) => other is ITypeHandleOwner tho && tho.Equals( this );

        /// <inheritdoc/>
        bool IEquatable<ITypeHandleOwner>.Equals( ITypeHandleOwner? other )
            => other is not null && ((ITypeHandleOwner)this).Handle.Equals( other.Handle );

        /// <summary>Gets or sets the Debug information type for this binding</summary>
        /// <remarks>
        /// <para>Setting the debug type is only allowed when the debug type is null or <see cref="MDNode.IsTemporary"/>
        /// is <see langword="true"/>. If the debug type node is a temporary setting the type will replace all uses
        /// of the temporary type automatically, via <see cref="MDNode.ReplaceAllUsesWith(IrMetadata)"/></para>
        /// <para>Since setting this property will replace all uses with (RAUW) the new value then setting this property
        /// with <see langword="null"/> is not allowed. However, until set this property will be <see  langword="null"/></para>
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">The type is not <see langword="null"/> or not a temporary</exception>
        [DisallowNull]
        public TDebug? DebugInfoType
        {
            get => RawDebugInfoType;
            set
            {
                ArgumentNullException.ThrowIfNull( value );

                if((RawDebugInfoType is not null) && RawDebugInfoType.IsTemporary)
                {
                    if(value.IsTemporary)
                    {
                        throw new InvalidOperationException( Resources.Cannot_replace_a_temporary_with_another_temporary );
                    }

                    RawDebugInfoType.ReplaceAllUsesWith( value );
                    RawDebugInfoType = value;
                }
                else
                {
                    throw new InvalidOperationException( Resources.Cannot_replace_non_temporary_DIType_with_a_new_Type );
                }
            }
        }

        /// <summary>Gets the native LLVM type for this debug type binding</summary>
        public TNative NativeType { get; init; }

        /// <summary>Gets an intentionally undocumented value</summary>
        /// <remarks>internal use only</remarks>
        LLVMTypeRef ITypeHandleOwner.Handle => NativeType.GetTypeRef();

        /// <inheritdoc/>
        public bool IsSized => NativeType.IsSized;

        /// <inheritdoc/>
        public TypeKind Kind => NativeType.Kind;

        /// <inheritdoc/>
        public IContext Context => NativeType.Context;

        /// <inheritdoc/>
        public uint IntegerBitWidth => NativeType.IntegerBitWidth;

        /// <inheritdoc/>
        public bool IsInteger => NativeType.IsInteger;

        /// <inheritdoc/>
        public bool IsFloat => NativeType.IsFloat;

        /// <inheritdoc/>
        public bool IsDouble => NativeType.IsDouble;

        /// <inheritdoc/>
        public bool IsVoid => NativeType.IsVoid;

        /// <inheritdoc/>
        public bool IsStruct => NativeType.IsStruct;

        /// <inheritdoc/>
        public bool IsPointer => NativeType.IsPointer;

        /// <inheritdoc/>
        public bool IsSequence => NativeType.IsSequence;

        /// <inheritdoc/>
        public bool IsFloatingPoint => NativeType.IsFloatingPoint;

        /// <inheritdoc/>
        public Constant GetNullValue( ) => NativeType.GetNullValue();

        /// <inheritdoc/>
        public IArrayType CreateArrayType( uint count ) => NativeType.CreateArrayType( count );

        /// <inheritdoc/>
        public IPointerType CreatePointerType( ) => NativeType.CreatePointerType();

        /// <inheritdoc/>
        public IPointerType CreatePointerType( uint addressSpace ) => NativeType.CreatePointerType( addressSpace );

        /// <inheritdoc/>
        public DebugPointerType CreatePointerType( IDIBuilder diBuilder, uint addressSpace )
        {
            if(DebugInfoType == null)
            {
                throw new InvalidOperationException( Resources.Type_does_not_have_associated_Debug_type_from_which_to_construct_a_pointer_type );
            }

            var nativePointer = NativeType.CreatePointerType( addressSpace );
            return new DebugPointerType( nativePointer, diBuilder, DebugInfoType, string.Empty );
        }

        /// <inheritdoc/>
        public DebugArrayType CreateArrayType( IDIBuilder diBuilder, uint lowerBound, uint count )
        {
            if(DebugInfoType == null)
            {
                throw new InvalidOperationException( Resources.Type_does_not_have_associated_Debug_type_from_which_to_construct_an_array_type );
            }

            var llvmArray = NativeType.CreateArrayType( count );
            return new DebugArrayType( llvmArray, diBuilder, DebugInfoType, count, lowerBound );
        }

        /// <summary>Converts a <see cref="DebugType{TNative, TDebug}"/> to <typeparamref name="TDebug"/> by accessing the <see cref="DebugInfoType"/> property</summary>
        /// <param name="self">The type to convert</param>
        [SuppressMessage( "Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "DebugInfoType is available as a property, this is for convenience" )]
        public static implicit operator TDebug?( DebugType<TNative, TDebug> self ) => self.ThrowIfNull().DebugInfoType;

        internal DebugType( TNative llvmType, TDebug? debugInfoType )
        {
            ArgumentNullException.ThrowIfNull( llvmType );

            NativeType = llvmType;
            RawDebugInfoType = debugInfoType;
        }

        private TDebug? RawDebugInfoType;
    }

    /// <summary>Utility class to provide mix-in type extensions and support for Debug Types</summary>
    public static class DebugType
    {
        /// <summary>Creates a new <see cref="DebugType"/> instance inferring the generic arguments from the parameters</summary>
        /// <typeparam name="TNative">Type of the Native LLVM type for the association</typeparam>
        /// <typeparam name="TDebug">Type of the debug information type for the association</typeparam>
        /// <param name="nativeType"><typeparamref name="TNative"/> type instance for this association</param>
        /// <param name="debugType"><typeparamref name="TDebug"/> type instance for this association (use <see langword="null"/> for void)</param>
        /// <returns><see cref="IDebugType{NativeT, DebugT}"/> implementation for the specified association</returns>
        public static IDebugType<TNative, TDebug> Create<TNative, TDebug>( TNative nativeType, TDebug? debugType )
            where TNative : class, ITypeRef
            where TDebug : DIType
        {
            return new DebugType<TNative, TDebug>( nativeType, debugType );
        }

        /// <summary>Convenience extension for determining if the <see cref="DIType"/> property is valid</summary>
        /// <param name="debugType">Debug type to test for valid Debug information</param>
        /// <remarks>In LLVM Debug information of <see langword="null"/> for a <see cref="Ubiquity.NET.Llvm.DebugInfo.DIType"/>
        /// is used to represent the void type. Thus, looking only at the <see cref="DIType"/> property is
        /// insufficient to distinguish between a type with no debug information and one representing the void
        /// type. This extension method is used to disambiguate the two possibilities.
        /// </remarks>
        /// <returns><see langword="true"/> if the type has debug information</returns>
        public static bool HasDebugInfo( this IDebugType<ITypeRef, DIType> debugType )
        {
            ArgumentNullException.ThrowIfNull( debugType );

            return debugType.DebugInfoType != null
                || (debugType.NativeType is ITypeRef nt && nt.IsVoid); // second test is to see if null debugInfo == VOID type
        }
    }
}
