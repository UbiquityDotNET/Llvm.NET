using System;
using Llvm.NET.DebugInfo;
using Llvm.NET.Values;

namespace Llvm.NET.Types
{
    public interface ITypeRef 
        : IExtensiblePropertyContainer
    {
        IntPtr TypeHandle { get; }

        /// <summary>Flag to indicate if the type is sized</summary>
        bool IsSized { get; }
        
        /// <summary>LLVM Type kind for this type</summary>
        TypeKind Kind { get; }

        bool IsInteger { get; }

        // Return true if value is 'float', a 32-bit IEEE fp type.
         bool IsFloat { get; }

        // Return true if this is 'double', a 64-bit IEEE fp type
        bool IsDouble { get; }

        bool IsVoid { get; }

        bool IsStruct { get; }

        bool IsPointer { get; }

        /// <summary>Flag to indicate if the type is a sequence type</summary>
        bool IsSequence { get; }

        bool IsFloatingPoint { get; }

        bool IsPointerPointer { get; }

        /// <summary>Context that owns this type</summary>
        Context Context { get; }

        /// <summary>Integer bid width of this type or 0 for non integer types</summary>
        uint IntegerBitWidth { get; }

        /// <summary>Gets a null value (e.g. all bits = 0 ) for the type</summary>
        /// <remarks>This is a getter function instead of a property as it can throw exceptions</remarks>
        Constant GetNullValue( );

        DIType DIType { get; set; }

        /// <summary>Array type factory for an array with elements of this type</summary>
        /// <param name="count">Number of elements in the array</param>
        /// <returns><see cref="IArrayType"/> for the array</returns>
        IArrayType CreateArrayType( uint count );

        /// <summary>Get a <see cref="IPointerType"/> for a type that points to elements of this type in the default (0) address space</summary>
        /// <returns><see cref="IPointerType"/>corresponding to the type of a pointer that referns to elements of this type</returns>
        IPointerType CreatePointerType( );

        /// <summary>Get a <see cref="IPointerType"/> for a type that points to elements of this type in the specified address space</summary>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns><see cref="IPointerType"/>corresponding to the type of a pointer that referns to elements of this type</returns>
        IPointerType CreatePointerType( uint addressSpace );

        void ReplaceAllUsesOfDebugTypeWith( DICompositeType compositeType );

        /// <summary>Creates a pointer type with debug information</summary>
        /// <param name="module">Module to use for building debug information</param>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns><see cref="IPointerType"/></returns>
        IPointerType CreatePointerType( Module module, uint addressSpace );

        IArrayType CreateArrayType( Module module, uint lowerBound, uint count );
    }

    public static class ITypeRefExtensions
    {
        public static IPointerType CreatePointerType( this ITypeRef self, Module module ) => self.CreatePointerType( module, 0 );

        public static DIType CreateDIType( this ITypeRef self, Module module, string name, DIScope scope )
        {
            DIType retVal;
            switch( self.Kind )
            {
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.Float16:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
            case TypeKind.Integer:
                retVal = GetDiBasicType( self, module, name );
                break;

            case TypeKind.Struct:
                retVal =  module.DIBuilder.CreateReplaceableCompositeType( Tag.StructureType, name, scope, null, 0 );
                break;

            case TypeKind.Void:
            case TypeKind.Function:
            case TypeKind.Array:
                return null;

            case TypeKind.Pointer:
                retVal = ((IPointerType )self).ElementType.CreateDIType( module, $"{name}*", scope );
                break;

            default:
                throw new NotSupportedException( "Type not supported for this target/language" );
            }
            self.DIType = retVal;
            return retVal;
        }

        private static DIBasicType GetDiBasicType( ITypeRef llvmType, Module module, string name )
        {
            var bitSize = module.Layout.BitSizeOf( llvmType );
            var bitAlignment = module.Layout.AbiBitAlignmentOf( llvmType );

            switch( llvmType.Kind )
            {
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.Float16:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
                return module.DIBuilder.CreateBasicType( name, bitSize, bitAlignment, DiTypeKind.Float );

            case TypeKind.Integer:
                return module.DIBuilder.CreateBasicType( name, bitSize, bitAlignment, DiTypeKind.Signed );

            default:
                throw new NotSupportedException( "Not a basic type!" );
            }
        }


        internal static LLVMTypeRef GetTypeRef( this ITypeRef self ) => new LLVMTypeRef( self.TypeHandle );
    }
}
