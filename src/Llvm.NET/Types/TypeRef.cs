// -----------------------------------------------------------------------
// <copyright file="TypeRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Interop;
using Llvm.NET.Values;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Types
{
    /// <summary>LLVM Type</summary>
    internal class TypeRef
        : ITypeRef
        , ITypeHandleOwner
    {
        /// <inheritdoc/>
        public LLVMTypeRef TypeHandle => TypeRefHandle;

        /// <inheritdoc/>
        public bool IsSized => Kind != TypeKind.Function && LLVMTypeIsSized( TypeRefHandle );

        /// <inheritdoc/>
        public TypeKind Kind => ( TypeKind )LLVMGetTypeKind( TypeRefHandle );

        /// <inheritdoc/>
        public bool IsInteger=> Kind == TypeKind.Integer;

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

        /// <inheritdoc/>
        public bool IsPointerPointer => (this is IPointerType ptrType) && ptrType.ElementType.Kind == TypeKind.Pointer;

        /// <inheritdoc/>
        public Context Context => GetContextFor( TypeRefHandle );

        /// <inheritdoc/>
        public uint IntegerBitWidth => Kind != TypeKind.Integer ? 0 : LLVMGetIntTypeWidth( TypeRefHandle );

        /// <inheritdoc/>
        public Constant GetNullValue() => Constant.NullValueFor( this );

        /// <inheritdoc/>
        public IArrayType CreateArrayType( uint count ) => FromHandle<IArrayType>( LLVMArrayType( TypeRefHandle, count ) );

        /// <inheritdoc/>
        public IPointerType CreatePointerType( ) => CreatePointerType( 0 );

        /// <inheritdoc/>
        public IPointerType CreatePointerType( uint addressSpace )
        {
            if( IsVoid )
            {
                throw new InvalidOperationException( "Cannot create pointer to void in LLVM, use i8* instead" );
            }

            return FromHandle<IPointerType>( LLVMPointerType( TypeRefHandle, addressSpace ) );
        }

        public bool TryGetExtendedPropertyValue<T>( string id, out T value )
            => ExtensibleProperties.TryGetExtendedPropertyValue( id, out value );

        public void AddExtendedPropertyValue( string id, object value )
            => ExtensibleProperties.AddExtendedPropertyValue( id, value );

        /// <summary>Builds a string representation for this type in LLVM assembly language form</summary>
        /// <returns>Formatted string for this type</returns>
        public override string ToString( ) => LLVMPrintTypeToString( TypeRefHandle );

        internal TypeRef( LLVMTypeRef typeRef )
        {
            TypeRefHandle = typeRef;
            if( typeRef == default )
            {
                throw new ArgumentNullException( nameof( typeRef ) );
            }
        }

        internal static TypeRef FromHandle( LLVMTypeRef typeRef ) => FromHandle<TypeRef>( typeRef );

        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "Context is owned and disposed by global ContextCache" )]
        internal static T FromHandle<T>( LLVMTypeRef typeRef )
            where T : class, ITypeRef
        {
            if( typeRef == default )
            {
                return null;
            }

            var ctx = GetContextFor( typeRef );
            return ( T )ctx.GetTypeFor( typeRef );
        }

        internal class InterningFactory
            : HandleInterningMap<LLVMTypeRef, ITypeRef>
        {
            internal InterningFactory( Context context )
                : base( context )
            {
            }

            private protected override ITypeRef ItemFactory( LLVMTypeRef handle )
            {
                var kind = ( TypeKind )LLVMGetTypeKind( handle );
                switch( kind )
                {
                case TypeKind.Struct:
                    return new StructType( handle );

                case TypeKind.Array:
                    return new ArrayType( handle );

                case TypeKind.Pointer:
                    return new PointerType( handle );

                case TypeKind.Vector:
                    return new VectorType( handle );

                case TypeKind.Function: // NOTE: This is a signature rather than a Function, which is a Value
                    return new FunctionType( handle );

                // other types not yet supported in Object wrappers
                // but the pattern for doing so should be pretty obvious...
                // case TypeKind.Void:
                // case TypeKind.Float16:
                // case TypeKind.Float32:
                // case TypeKind.Float64:
                // case TypeKind.X86Float80:
                // case TypeKind.Float128m112:
                // case TypeKind.Float128:
                // case TypeKind.Label:
                // case TypeKind.Integer:
                // case TypeKind.Metadata:
                // case TypeKind.X86MMX:
                default:
                    return new TypeRef( handle );
                }
            }
        }

        protected LLVMTypeRef TypeRefHandle { get; }

        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "Context created here is owned, and disposed of via the ContextCache" )]
        private static Context GetContextFor( LLVMTypeRef handle )
        {
            if( handle == default )
            {
                return null;
            }

            var hContext = LLVMGetTypeContext( handle );
            Debug.Assert( hContext != default, "Should not get a null pointer from LLVM" );
            return ContextCache.GetContextFor( hContext );
        }

        private readonly ExtensiblePropertyContainer ExtensibleProperties = new ExtensiblePropertyContainer( );
    }
}
