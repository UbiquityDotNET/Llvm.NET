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
    public static class TypeRefExtensions
    {
        /// <summary>Creates pointer type from the specified type</summary>
        /// <param name="self">Type to create a pointer from</param>
        /// <param name="module">Module to use in creating a new type or retriving an existing type</param>
        /// <returns>Pointer type</returns>
        public static IPointerType CreatePointerType( this ITypeRef self, Module module ) => self.CreatePointerType( module, 0 );

        /// <summary>Creates a new <see cref="DIType"/> for an ITypeRef</summary>
        /// <param name="typeRef">Type to create a pointer for</param>
        /// <param name="module">Module to use if creating a new type</param>
        /// <param name="name">Name of the type</param>
        /// <param name="scope">Scope for the type</param>
        /// <returns><see cref="ITypeRef"/> with debug information, which, for basic types and function signatures will not be <paramref name="typeRef"/></returns>
        /// <remarks>
        /// <para>This method is used to construct Debug type information and attach it to a given type.</para>
        /// <note type="note">
        /// The returned <see cref="ITypeRef"/> may not be the same instance as <paramref name="typeRef"/>. This is due to the fact that, in LLVM function
        /// signature types and basic types are uniqued. However, when creating debug information language specific definitions are attached which means
        /// that a simple 8 bit value could be a byte or an ASCII character. LLVM bit code doesn't neeed to care about that distinction so it treats all
        /// 8 bit values the same. However a debugger needs to know the difference to display information in a form as close to the original source language
        /// as is plausible within the debug environment. Thus, for basic types and function signatures this actually returns a new <see cref="ITypeRef"/>
        /// instance from <see cref="DebugTypePair{T}"/> with the original type from <paramref name="typeRef"/> and a newly created Debug information type.
        /// </note>
        /// <para>The actual type of debug information in the <see cref="ITypeRef.DIType"/> property of the returned <see cref="ITypeRef"/>  depends on the
        /// <see cref="ITypeRef.Kind"/> property of from the <paramref name="typeRef"/> parameter.
        /// The following table lists the various options:</para>
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
        public static ITypeRef CreateDIType( this ITypeRef typeRef, Module module, string name, DIScope scope )
        {
            if( typeRef.DIType != null )
                throw new ArgumentException( "Cannot replace debug information with this method - use ITypeRef.ReplaceAllUsesOfDebugTypeWith() instead" );

            var diType  = InternalCreateDIType( typeRef, module, name, scope );
            // basic times are uniqued in LLVM so wrap them in a DebugTypePair
            // to allow for distinct native+debug type pairing
            var basicType = diType as DIBasicType;
            if( basicType != null )
            {
                typeRef = new DebugTypePair<DIBasicType>( typeRef, basicType );
            }
            else
            {
                typeRef.DIType = diType;
            }

            return typeRef;
        }

        private static DIType InternalCreateDIType( ITypeRef self, Module module, string name, DIScope scope )
        {
            if( self.DIType != null )
                return self.DIType;

            switch( self.Kind )
            {
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.Float16:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
            case TypeKind.Integer:
                return GetDiBasicType( self, module, name );

            case TypeKind.Struct:
                return module.DIBuilder.CreateReplaceableCompositeType( Tag.StructureType, name, scope, null, 0 );

            case TypeKind.Void:
            case TypeKind.Function:
            case TypeKind.Array:
                return null;

            case TypeKind.Pointer:
                var elementType = InternalCreateDIType( (( IPointerType )self ).ElementType, module, null, scope );
                return module.DIBuilder.CreatePointerType( elementType, null, module.Layout.BitSizeOf( self ), module.Layout.AbiAlignmentOf( self ) );

            default:
                throw new NotSupportedException( "Type not supported for this target/language" );
            }
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
