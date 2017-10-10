// <copyright file="TypeRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Types
{
    /// <summary>LLVM Type</summary>
    internal class TypeRef
        : ITypeRef
    {
        public IntPtr TypeHandle => TypeRefHandle.Pointer;

        /// <summary>Flag to indicate if the type is sized</summary>
        public bool IsSized
        {
            get
            {
                if( Kind == TypeKind.Function )
                {
                    return false;
                }

                return NativeMethods.TypeIsSized( TypeRefHandle );
            }
        }

        /// <summary>LLVM Type kind for this type</summary>
        public TypeKind Kind => ( TypeKind )NativeMethods.GetTypeKind( TypeRefHandle );

        public bool IsInteger=> Kind == TypeKind.Integer;

        // Return true if value is 'float', a 32-bit IEEE fp type.
        public bool IsFloat => Kind == TypeKind.Float32;

        // Return true if this is 'double', a 64-bit IEEE fp type
        public bool IsDouble => Kind == TypeKind.Float64;

        public bool IsVoid => Kind == TypeKind.Void;

        public bool IsStruct => Kind == TypeKind.Struct;

        public bool IsPointer => Kind == TypeKind.Pointer;

        /// <summary>Flag to indicate if the type is a sequence type</summary>
        public bool IsSequence => Kind == TypeKind.Array || Kind == TypeKind.Vector || Kind == TypeKind.Pointer;

        public bool IsFloatingPoint
        {
            get
            {
                switch( Kind )
                {
                case TypeKind.Float16:
                case TypeKind.Float32:
                case TypeKind.Float64:
                case TypeKind.X86Float80:
                case TypeKind.Float128m112:
                case TypeKind.Float128:
                    return true;

                default:
                    return false;
                }
            }
        }

        public bool IsPointerPointer
        {
            get
            {
                var ptrType = this as IPointerType;
                return ptrType != null && ptrType.ElementType.Kind == TypeKind.Pointer;
            }
        }

        /// <summary>Context that owns this type</summary>
        public Context Context => Context.GetContextFor( TypeRefHandle );

        /// <summary>Integer bid width of this type or 0 for non integer types</summary>
        public uint IntegerBitWidth
        {
            get
            {
                if( Kind != TypeKind.Integer )
                {
                    return 0;
                }

                return NativeMethods.GetIntTypeWidth( TypeRefHandle );
            }
        }

        /// <summary>Gets a null value (e.g. all bits = 0 ) for the type</summary>
        /// <remarks>This is a getter function instead of a property as it can throw exceptions</remarks>
        /// <returns><see cref="Constant"/> zero value for the type</returns>
        public Constant GetNullValue() => Constant.NullValueFor( this );

        /// <summary>Array type factory for an array with elements of this type</summary>
        /// <param name="count">Number of elements in the array</param>
        /// <returns><see cref="IArrayType"/> for the array</returns>
        public IArrayType CreateArrayType( uint count ) => FromHandle<IArrayType>( NativeMethods.ArrayType( TypeRefHandle, count ) );

        /// <summary>Get a <see cref="IPointerType"/> for a type that points to elements of this type in the default (0) address space</summary>
        /// <returns><see cref="IPointerType"/>corresponding to the type of a pointer that referns to elements of this type</returns>
        public IPointerType CreatePointerType( ) => CreatePointerType( 0 );

        /// <summary>Get a <see cref="IPointerType"/> for a type that points to elements of this type in the specified address space</summary>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns><see cref="IPointerType"/>corresponding to the type of a pointer that referns to elements of this type</returns>
        public IPointerType CreatePointerType( uint addressSpace )
        {
            if( IsVoid )
            {
                throw new InvalidOperationException( "Cannot create pointer to void in LLVM, use i8* instead" );
            }

            return FromHandle<IPointerType>( NativeMethods.PointerType( TypeRefHandle, addressSpace ) );
        }

        public bool TryGetExtendedPropertyValue<T>( string id, out T value ) => ExtensibleProperties.TryGetExtendedPropertyValue<T>( id, out value );

        public void AddExtendedPropertyValue( string id, object value ) => ExtensibleProperties.AddExtendedPropertyValue( id, value );

        /// <summary>Builds a string representation for this type in LLVM assembly language form</summary>
        /// <returns>Formatted string for this type</returns>
        public override string ToString( ) => NativeMethods.PrintTypeToString( TypeRefHandle );

        internal TypeRef( LLVMTypeRef typeRef )
        {
            TypeRefHandle = typeRef;
            if( typeRef.Pointer == IntPtr.Zero )
            {
                throw new ArgumentNullException( nameof( typeRef ) );
            }

#if DEBUG
            var ctx = Context.GetContextFor( typeRef );
            ctx.AssertTypeNotInterned( typeRef );
#endif
        }

        internal static TypeRef FromHandle( LLVMTypeRef typeRef ) => FromHandle<TypeRef>( typeRef );

        internal static T FromHandle<T>( LLVMTypeRef typeRef )
            where T : class, ITypeRef
        {
            if( typeRef.Pointer == IntPtr.Zero )
            {
                return null;
            }

            var ctx = Context.GetContextFor( typeRef );
            return ( T )ctx.GetTypeFor( typeRef, StaticFactory );
        }

        protected LLVMTypeRef TypeRefHandle { get; }

        private static ITypeRef StaticFactory( LLVMTypeRef typeRef )
        {
            var kind = ( TypeKind )NativeMethods.GetTypeKind( typeRef );
            switch( kind )
            {
            case TypeKind.Struct:
                return new StructType( typeRef );

            case TypeKind.Array:
                return new ArrayType( typeRef );

            case TypeKind.Pointer:
                return new PointerType( typeRef );

            case TypeKind.Vector:
                return new VectorType( typeRef );

            case TypeKind.Function: // NOTE: This is a signature rather than a Function, which is a Value
                return new FunctionType( typeRef );

            // other types not yet supported in Object wrappers
            // but the pattern for doing so should be pretty obvious...
            case TypeKind.Void:
            case TypeKind.Float16:
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
            case TypeKind.Label:
            case TypeKind.Integer:
            case TypeKind.Metadata:
            case TypeKind.X86MMX:
            default:
                return new TypeRef( typeRef );
            }
        }

        private readonly ExtensiblePropertyContainer ExtensibleProperties = new ExtensiblePropertyContainer( );
    }
}
