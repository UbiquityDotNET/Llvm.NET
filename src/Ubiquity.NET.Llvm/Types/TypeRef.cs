// -----------------------------------------------------------------------
// <copyright file="TypeRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>LLVM Type</summary>
    internal class TypeRef
        : ITypeRef
        , ITypeHandleOwner
    {
        /// <inheritdoc/>
        bool IEquatable<ITypeRef>.Equals(ITypeRef? other) => other is ITypeHandleOwner tho && tho.Equals((ITypeHandleOwner)this);

        public bool Equals(ITypeHandleOwner? other) => other is not null && Handle.Equals( other.Handle );

        /// <inheritdoc/>
        public LLVMTypeRef Handle { get; }

        /// <inheritdoc/>
        public bool IsSized => Kind != TypeKind.Function
                            && LLVMTypeIsSized( Handle );

        /// <inheritdoc/>
        public TypeKind Kind => (TypeKind)LLVMGetTypeKind( Handle );

        /// <inheritdoc/>
        public bool IsInteger => Kind == TypeKind.Integer;

        /// <inheritdoc/>
        public bool IsFloat => Kind == TypeKind.Float32;

        /// <inheritdoc/>
        public bool IsDouble => Kind == TypeKind.Float64;

        /// <inheritdoc/>
        public bool IsVoid => Kind == TypeKind.Void;

        /// <inheritdoc/>
        public bool IsStruct => Kind == TypeKind.Struct;

        /// <inheritdoc/>
        public bool IsPointer => Kind == TypeKind.Pointer;

        /// <inheritdoc/>
        public bool IsSequence => Kind == TypeKind.Array || Kind == TypeKind.Vector || Kind == TypeKind.Pointer;

        /// <inheritdoc/>
        public bool IsFloatingPoint => Kind switch
        {
            TypeKind.Float16 or
            TypeKind.Float32 or
            TypeKind.Float64 or
            TypeKind.X86Float80 or
            TypeKind.Float128m112 or
            TypeKind.Float128 => true,
            _ => false,
        };

        /// <inheritdoc/>
        public IContext Context => new ContextAlias(LLVMGetTypeContext( Handle ));

        /// <inheritdoc/>
        public uint IntegerBitWidth => Kind != TypeKind.Integer ? 0 : LLVMGetIntTypeWidth( Handle );

        /// <inheritdoc/>
        public Constant GetNullValue() => Constant.NullValueFor( this );

        /// <inheritdoc/>
        public IArrayType CreateArrayType(uint count) => (IArrayType)LLVMArrayType( Handle, count ).ThrowIfInvalid().CreateType();

        /// <inheritdoc/>
        public IPointerType CreatePointerType() => CreatePointerType( 0 );

        /// <inheritdoc/>
        public IPointerType CreatePointerType(uint addressSpace)
        {
            // create the opaque pointer then set this type as the ElementType.
            return (IPointerType)LLVMPointerType( Handle, addressSpace )
                                .ThrowIfInvalid( )
                                .CreateType( this );
        }

        /// <summary>Builds a string representation for this type in LLVM assembly language form</summary>
        /// <returns>Formatted string for this type</returns>
        public override string? ToString()
        {
            using var nativeRetVal = LLVMPrintTypeToString( Handle );
            return nativeRetVal.ToString();
        }

        internal TypeRef(LLVMTypeRef typeRef)
        {
            Handle = typeRef;
            if(typeRef == default)
            {
                throw new ArgumentNullException( nameof( typeRef ) );
            }
        }
    }
}
