// -----------------------------------------------------------------------
// <copyright file="TypeRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>LLVM Type</summary>
    internal class TypeRef
        : ITypeRef
        , ITypeHandleOwner
    {
        /// <inheritdoc/>
        public LLVMTypeRef TypeRefHandle { get; }

        /// <inheritdoc/>
        public bool IsSized => Kind != TypeKind.Function
                            && LLVMTypeIsSized( TypeRefHandle );

        /// <inheritdoc/>
        public TypeKind Kind => ( TypeKind )LLVMGetTypeKind( TypeRefHandle );

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
        public Context Context => GetContextFor( TypeRefHandle );

        /// <inheritdoc/>
        public uint IntegerBitWidth => Kind != TypeKind.Integer ? 0 : LLVMGetIntTypeWidth( TypeRefHandle );

        /// <inheritdoc/>
        public Constant GetNullValue( ) => Constant.NullValueFor( this );

        /// <inheritdoc/>
        public IArrayType CreateArrayType( uint count ) => FromHandle<IArrayType>( LLVMArrayType( TypeRefHandle, count ).ThrowIfInvalid( ) )!;

        /// <inheritdoc/>
        public IPointerType CreatePointerType( ) => CreatePointerType( 0 );

        /// <inheritdoc/>
        public IPointerType CreatePointerType( uint addressSpace )
        {
            // create the opaque pointer then set this type as the ElementType.
            var retVal = FromHandle<IPointerType>( LLVMPointerType( TypeRefHandle, addressSpace ).ThrowIfInvalid( ) )!;
            retVal.TrySetElementType(this);
            return retVal;
        }

        /// <summary>Builds a string representation for this type in LLVM assembly language form</summary>
        /// <returns>Formatted string for this type</returns>
        public override string? ToString( ) => LLVMPrintTypeToString( TypeRefHandle ).ToString();

        internal TypeRef( LLVMTypeRef typeRef )
        {
            TypeRefHandle = typeRef;
            if( typeRef == default )
            {
                throw new ArgumentNullException( nameof( typeRef ) );
            }
        }

        internal static TypeRef? FromHandle( LLVMTypeRef typeRef ) => FromHandle<TypeRef>( typeRef );

        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "Context is owned and disposed by global ContextCache" )]
        internal static T? FromHandle<T>( LLVMTypeRef typeRef, ITypeRef? elementType = null )
            where T : class, ITypeRef
        {
            if( typeRef.IsNull )
            {
                return null;
            }

            var ctx = GetContextFor( typeRef );
            return ctx.GetTypeFor( typeRef ) as T;
        }

        internal class InterningFactory
            : HandleInterningMapWithContext<LLVMTypeRef, ITypeRef>
        {
            internal InterningFactory( Context context )
                : base( context )
            {
            }

            private protected override ITypeRef ItemFactory( LLVMTypeRef handle )
            {
                var kind = ( TypeKind )LLVMGetTypeKind( handle );
                return kind switch
                {
                    TypeKind.Struct => new StructType( handle ),
                    TypeKind.Array => new ArrayType( handle ),
                    TypeKind.Pointer => new PointerType( handle ),
                    TypeKind.Vector => new VectorType( handle ),
                    TypeKind.Function => new FunctionType( handle ), // NOTE: This is a signature rather than a Function, which is a Value
                    /* other types not yet supported in Object wrappers as LLVM itself doesn't
                    // have any specific types for them (except for IntegerType)
                    // but the pattern for doing so should be pretty obvious...
                    // case TypeKind.Void:
                    // case TypeKind.Float16:
                    // case TypeKind.Float32:
                    // case TypeKind.Float64:
                    // case TypeKind.X86Float80:
                    // case TypeKind.Float128m112:
                    // case TypeKind.Float128:
                    // case TypeKind.Label:
                    // case TypeKind.Integer: => IntegerType
                    // case TypeKind.LlvmMetadata:
                    // case TypeKind.X86MMX:
                    */
                    _ => new TypeRef( handle ),
                };
            }
        }

        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "Context created here is owned, and disposed of via the ContextCache" )]
        private static Context GetContextFor( LLVMTypeRef handle )
        {
            if( handle.IsNull )
            {
                throw new ArgumentException( "Context Handle is null", nameof( handle ) );
            }

            var hContext = LLVMGetTypeContext( handle );
            return ContextCache.GetContextFor( hContext.ThrowIfInvalid( ) );
        }
    }
}
