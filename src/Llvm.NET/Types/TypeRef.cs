using System;
using Llvm.NET.DebugInfo;
using Llvm.NET.Values;

namespace Llvm.NET.Types
{
    /// <summary>LLVM Type</summary>
    public class TypeRef 
        : IExtensiblePropertyContainer
    {
        /// <summary>Flag to indicate if the type is sized</summary>
        public bool IsSized
        {
            get
            {
                if( Kind == TypeKind.Function )
                    return false;

                return LLVMNative.TypeIsSized( TypeHandle );
            }
        }

        /// <summary>LLVM Type kind for this type</summary>
        public TypeKind Kind => ( TypeKind )LLVMNative.GetTypeKind( TypeHandle );

        public bool IsInteger => Kind == TypeKind.Integer;

        // Return true if value is 'float', a 32-bit IEEE fp type.
        public bool IsFloat => Kind == TypeKind.Float32;

        // Return true if this is 'double', a 64-bit IEEE fp type
        public bool IsDouble => Kind == TypeKind.Float64;

        public bool IsVoid => Kind == TypeKind.Void;

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

        public bool IsPointer => Kind == TypeKind.Pointer;
        public bool IsPointerPointer
        {
            get
            {
                var ptrType = this as PointerType;
                return ptrType != null && ptrType.ElementType.Kind == TypeKind.Pointer;
            }
        }

        public bool IsStruct => Kind == TypeKind.Struct;

        /// <summary>Context that owns this type</summary>
        public Context Context => Context.GetContextFor( TypeHandle );

        /// <summary>Integer bid width of this type or 0 for non integer types</summary>
        public uint IntegerBitWidth
        {
            get
            {
                if( Kind != TypeKind.Integer )
                    return 0;

                return LLVMNative.GetIntTypeWidth( TypeHandle );
            }
        }

        /// <summary>Flag to indicate if the type is a sequence type</summary>
        public bool IsSequence => Kind == TypeKind.Array || Kind == TypeKind.Vector || Kind == TypeKind.Pointer;

        /// <summary>Gets a null value (e.g. all bits = 0 ) for the type</summary>
        /// <remarks>This is a getter function instead of a property as it can throw exceptions</remarks>
        public Constant GetNullValue() => Constant.NullValueFor( this );

        public DIType DIType { get; internal set; }

        /// <summary>Retrieves an expression that results in the size of the type</summary>
        [Obsolete("Use TargetData layout information to compute size and create a constant from that")]
        public Constant GetSizeOfExpression()
        {
            if( !IsSized
                || Kind == TypeKind.Void
                || Kind == TypeKind.Function
                || ( Kind == TypeKind.Struct && ( LLVMNative.IsOpaqueStruct( TypeHandle ) ) )
                )
            {
                return Context.CreateConstant( 0 );
            }

            var hSize = LLVMNative.SizeOf( TypeHandle );
    
            // LLVM uses an expression to construct Sizeof, however it is hard coded to
            // use an i64 as the type for the size, which isn't valid for a 32 bit system
            if( LLVMNative.GetIntTypeWidth( LLVMNative.TypeOf( hSize ) ) > 32 )
                hSize = LLVMNative.ConstTrunc( hSize, Context.Int32Type.TypeHandle );

            var hConstExp =  LLVMNative.IsAConstantExpr( hSize );
            if( hConstExp.Pointer != null )
                return Value.FromHandle<ConstantExpression>( hConstExp );

            var hConstInt = LLVMNative.IsAConstantInt( hSize );
            if( hConstInt.Pointer != null )
                return Value.FromHandle<ConstantInt>( hConstInt );

            return Value.FromHandle<Constant>( hSize );
        }

        /// <summary>Array type factory for an array with elements of this type</summary>
        /// <param name="count">Number of elements in the array</param>
        /// <returns><see cref="ArrayType"/> for the array</returns>
        public ArrayType CreateArrayType( uint count ) => FromHandle<ArrayType>( LLVMNative.ArrayType( TypeHandle, count ) );

        /// <summary>Get a <see cref="PointerType"/> for a type that points to elements of this type in the default (0) address space</summary>
        /// <returns><see cref="PointerType"/>corresponding to the type of a pointer that referns to elements of this type</returns>
        public PointerType CreatePointerType( ) => CreatePointerType( 0 );

        /// <summary>Get a <see cref="PointerType"/> for a type that points to elements of this type in the specified address space</summary>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns><see cref="PointerType"/>corresponding to the type of a pointer that referns to elements of this type</returns>
        public PointerType CreatePointerType( uint addressSpace ) => FromHandle<PointerType>( LLVMNative.PointerType( TypeHandle, addressSpace ) );

        public bool TryGetExtendedPropertyValue<T>( string id, out T value ) => ExtensibleProperties.TryGetExtendedPropertyValue<T>( id, out value );
        public void AddExtendedPropertyValue( string id, object value ) => ExtensibleProperties.AddExtendedPropertyValue( id, value );

        /// <summary>Builds a string repersentation for this type in LLVM assembly language form</summary>
        /// <returns>Formatted string for this type</returns>
        public override string ToString( )
        {
            var msgString = LLVMNative.PrintTypeToString( TypeHandle );
            return LLVMNative.MarshalMsg( msgString );
        }

        public void ReplaceAllUsesOfDebugTypeWith( DICompositeType compositeType )
        {
            DIType.ReplaceAllUsesWith( compositeType );
            DIType = compositeType;
        }

        public PointerType CreatePointerType(  DebugInfoBuilder diBuilder, TargetData layout )
        {
            if( DIType == null )
                throw new ArgumentException( "Type does not have associated Debug type from which to construct a pointer type" );

            var retVal = CreatePointerType( );
            var diPtr = diBuilder.CreatePointerType( DIType, string.Empty, layout.BitSizeOf( retVal ), layout.AbiBitAlignmentOf( retVal ) );
            retVal.DIType = diPtr;
            return retVal;
        }

        public TypeRef CreateArrayType( DebugInfoBuilder diBuilder, TargetData layout, uint lowerBound, uint count )
        {
            if( DIType == null )
                throw new ArgumentException( "Type does not have associated Debug type from which to construct an array type" );

            var llvmArray = CreateArrayType( count );
            var diArray = diBuilder.CreateArrayType( layout.BitSizeOf( llvmArray )
                                                   , layout.AbiBitAlignmentOf( llvmArray )
                                                   , DIType
                                                   , diBuilder.CreateSubrange( lowerBound, count )
                                                   );
            llvmArray.DIType = diArray;
            return llvmArray;
        }

        public DIType CreateDIType( DebugInfoBuilder diBuilder, TargetData layout, string name, DIScope scope )
        {
            DIType retVal;
            switch( Kind )
            {
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.Float16:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
            case TypeKind.Integer:
                retVal = GetDiBasicType( this, diBuilder, layout, name );
                break;

            case TypeKind.Struct:
                retVal =  diBuilder.CreateReplaceableCompositeType( Tag.StructureType, name, scope, null, 0 );
                break;

            case TypeKind.Void:
            case TypeKind.Function:
            case TypeKind.Array:
                return null;

            case TypeKind.Pointer:
                retVal = ((PointerType )this).ElementType.CreateDIType( diBuilder, layout, $"{name}*", scope );
                break;

            default:
                throw new NotSupportedException( "Type not supported for this target/language" );
            }
            DIType = retVal;
            return retVal;
        }

        internal TypeRef( LLVMTypeRef typeRef )
        {
            TypeHandle = typeRef;
            if( typeRef.Pointer == IntPtr.Zero )
                throw new ArgumentNullException( nameof( typeRef ) );

#if DEBUG
            var ctx = Llvm.NET.Context.GetContextFor( typeRef );
            ctx.AssertTypeNotInterned( typeRef );
#endif
        }

        private static DIBasicType GetDiBasicType( TypeRef llvmType, DebugInfoBuilder diBuilder, TargetData layout, string name )
        {
            var bitSize = layout.BitSizeOf( llvmType );
            var bitAlignment = layout.AbiBitAlignmentOf( llvmType );

            switch( llvmType.Kind )
            {
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.Float16:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
                return diBuilder.CreateBasicType( name, bitSize, bitAlignment, DiTypeKind.Float );

            case TypeKind.Integer:
                return diBuilder.CreateBasicType( name, bitSize, bitAlignment, DiTypeKind.Signed );

            default:
                throw new NotSupportedException( "Not a basic type!" );
            }
        }

        internal static TypeRef FromHandle( LLVMTypeRef typeRef ) => FromHandle<TypeRef>( typeRef );
        internal static T FromHandle<T>( LLVMTypeRef typeRef )
            where T : TypeRef
        {
            if( typeRef.Pointer == IntPtr.Zero )
                return null;

            var ctx = Context.GetContextFor( typeRef );
            return ( T )ctx.GetTypeFor( typeRef, StaticFactory );
        }

        private static TypeRef StaticFactory( LLVMTypeRef typeRef )
        {
            var kind = (TypeKind)LLVMNative.GetTypeKind( typeRef );
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

        internal LLVMTypeRef TypeHandle { get; }
        private ExtensiblePropertyContainer ExtensibleProperties = new ExtensiblePropertyContainer( );
    }
}
