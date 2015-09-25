using System;
using Llvm.NET.DebugInfo;
using Llvm.NET.Values;

namespace Llvm.NET.Types
{
    /// <summary>Interface for a Type in LLVM</summary>
    public interface ITypeRef 
        : IExtensiblePropertyContainer
    {
        /// <summary>LibLLVM handle for the type</summary>
        IntPtr TypeHandle { get; }

        /// <summary>Flag to indicate if the type is sized</summary>
        bool IsSized { get; }
        
        /// <summary>LLVM Type kind for this type</summary>
        TypeKind Kind { get; }

        /// <summary>Flag to indicate if this type is an integer</summary>
        bool IsInteger { get; }

        // Return true if value is 'float', a 32-bit IEEE fp type.
        bool IsFloat { get; }

        // Return true if this is 'double', a 64-bit IEEE fp type
        bool IsDouble { get; }

        /// <summary>Flag to indicate if this type represents the void type</summary>
        bool IsVoid { get; }

        /// <summary>Flag to indicate if this type is a structure type</summary>
        bool IsStruct { get; }

        /// <summary>Flag to indicate if this type is a pointer</summary>
        bool IsPointer { get; }

        /// <summary>Flag to indicate if this type is a sequence type</summary>
        bool IsSequence { get; }

        /// <summary>Flag to indicate if this type is a floating point type</summary>
        bool IsFloatingPoint { get; }

        /// <summary>FLag to indicate if this type is a pointer to a pointer</summary>
        bool IsPointerPointer { get; }

        /// <summary>Context that owns this type</summary>
        Context Context { get; }

        /// <summary>Integer bid width of this type or 0 for non integer types</summary>
        uint IntegerBitWidth { get; }

        /// <summary>Gets a null value (e.g. all bits == 0 ) for the type</summary>
        /// <remarks>
        /// This is a getter function instead of a property as it can throw exceptions
        /// for types that don't support such a thing (i.e. void )
        /// </remarks>
        Constant GetNullValue( );

        /// <summary>Gets or sets the Debug Information tytype for this type</summary>
        /// <remarks>
        /// Some types do not support setting the Debug info. In particular, function signatures
        /// do not. The reason for this is that there isn't a one to one relationship for function
        /// signatures. Since an LLVM signature only contains types and debug information contains
        /// parameter names etc... it is entirely plausible that multiple debug information signatures
        /// map to a single LLVM signature.
        /// </remarks>
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

        /// <summary>Replace all uses of this types <see cref="DIType"/> with the provided debug information</summary>
        /// <param name="compositeType">Debug information to replace</param>
        /// <remarks>
        /// This is used when creating composite types using an temproary forward declaration then creating the final
        /// type with actual members and calling this method to replace the temproary.
        /// </remarks>
        void ReplaceAllUsesOfDebugTypeWith( DICompositeType compositeType );

        /// <summary>Creates a pointer type with debug information</summary>
        /// <param name="module">Module to use for building debug information</param>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns><see cref="IPointerType"/></returns>
        IPointerType CreatePointerType( Module module, uint addressSpace );

        /// <summary>Creates an Array type with known size and debug information</summary>
        /// <param name="module">Module to use when creating debug information for the type</param>
        /// <param name="lowerBound">Lower bound of the array</param>
        /// <param name="count">Count of elements in the array</param>
        /// <returns></returns>
        IArrayType CreateArrayType( Module module, uint lowerBound, uint count );
    }

    /// <summary>Extensions for ITypeRef interfaces</summary>
    /// <remarks>
    /// These static extension methods provide common functionality and support to
    /// implementations of the ITypeRef interface.
    /// </remarks>
    public static class ITypeRefExtensions
    {
        /// <summary>Creates pointer type from the specified type</summary>
        /// <param name="self">Type to create a pointer from</param>
        /// <param name="module">Module to use in creating a new type or retriving an existing type</param>
        /// <returns>Pointer type</returns>
        public static IPointerType CreatePointerType( this ITypeRef self, Module module ) => self.CreatePointerType( module, 0 );

        /// <summary>Creates a new <see cref="DIType"/> for an ITypeRef</summary>
        /// <param name="self">Type to create a pointer for</param>
        /// <param name="module">Module to use if creating a new type</param>
        /// <param name="name">Name of the type</param>
        /// <param name="scope">Scope for the type</param>
        /// <returns>Debug information type</returns>
        /// <remarks>
        /// The actual <see cref="DIType"/> returned depends on the <see cref="ITypeRef.Kind"/> property of the specified type.
        /// The following table lists the various options:
        /// <list type="table">
        /// <listheader>
        /// <term><see cref="ITypeRef.Kind"/></term><description>Debug information type</description>
        /// </listheader>
        /// <item><term><see cref="TypeKind.Float32"/></term><description><see cref="DIBasicType"/></description></item>
        /// <item><term><see cref="TypeKind.Float64"/></term><description><see cref="DIBasicType"/></description></item>
        /// <item><term><see cref="TypeKind.Float16"/></term><description><see cref="DIBasicType"/></description></item>
        /// <item><term><see cref="TypeKind.X86Float80"/></term><description><see cref="DIBasicType"/></description></item>
        /// <item><term><see cref="TypeKind.Float128m112"/></term><description><see cref="DIBasicType"/></description></item>
        /// <item><term><see cref="TypeKind.Float128"/></term><description><see cref="DIBasicType"/></description></item>
        /// <item><term><see cref="TypeKind.Integer"/></term><description><see cref="DIBasicType"/></description></item>
        /// <item>
        ///     <term><see cref="TypeKind.Struct"/></term>
        ///     <description>Temporary <see cref="DICompositeType"/> that must be replaced with a concrete type with <see cref="ITypeRef.ReplaceAllUsesOfDebugTypeWith(DICompositeType)"/></description>
        /// </item>
        /// <item><term><see cref="TypeKind.Void"/></term><description>null is used to represent the void type in debug information</description></item>
        /// <item><term><see cref="TypeKind.Function"/></term><description>null - debug information for a function signature can only be created by calling one of the <see cref="O:Llvm.NET.Context.CreateFunctionType"/>s</description></item>
        /// <item><term><see cref="TypeKind.Array"/></term><description>null - debug information for an array can only be created by calling <see cref="ITypeRef.CreateArrayType(Module, uint, uint)"/></description></item>
        /// <item><term><see cref="TypeKind.Pointer"/></term><description><see cref="DIDerivedType"/></description></item>
        /// </list>
        /// </remarks>
        /// <conceptualLink target="71ceaf97-cbf9-4d0d-8c1a-a7a1565d61dc">How-to create structure types with debug information</conceptualLink>
        public static DIType CreateDIType( this ITypeRef self, Module module, string name, DIScope scope )
        {
            if( self.DIType != null )
                return self.DIType;

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
