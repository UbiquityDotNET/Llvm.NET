// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Interface+internal type matches file name
#pragma warning disable SA1649

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>Basic kind of a type</summary>
    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "This is NOT a flags type - tooling should get over it." )]
    public enum TypeKind
    {
        /// <summary>Type with no size</summary>
        Void = LLVMTypeKind.LLVMVoidTypeKind,

        /// <summary>16 bit floating point type</summary>
        Float16 = LLVMTypeKind.LLVMHalfTypeKind,

        /// <summary>32 bit floating point type</summary>
        Float32 = LLVMTypeKind.LLVMFloatTypeKind,

        /// <summary>64 bit floating point type</summary>
        Float64 = LLVMTypeKind.LLVMDoubleTypeKind,

        /// <summary>80 bit floating point type (X87)</summary>
        X86Float80 = LLVMTypeKind.LLVMX86_FP80TypeKind,

        /// <summary>128 bit floating point type (112-bit mantissa)</summary>
        Float128m112 = LLVMTypeKind.LLVMFP128TypeKind,

        /// <summary>128 bit floating point type (two 64-bits)</summary>
        Float128 = LLVMTypeKind.LLVMPPC_FP128TypeKind,

        /// <summary><see cref="Ubiquity.NET.Llvm.Values.BasicBlock"/> instruction label</summary>
        Label = LLVMTypeKind.LLVMLabelTypeKind,

        /// <summary>Arbitrary bit width integers</summary>
        Integer = LLVMTypeKind.LLVMIntegerTypeKind,

        /// <summary><see cref="Ubiquity.NET.Llvm.Types.IFunctionType"/></summary>
        Function = LLVMTypeKind.LLVMFunctionTypeKind,

        /// <summary><see cref="Ubiquity.NET.Llvm.Types.IStructType"/></summary>
        Struct = LLVMTypeKind.LLVMStructTypeKind,

        /// <summary><see cref="Ubiquity.NET.Llvm.Types.IArrayType"/></summary>
        Array = LLVMTypeKind.LLVMArrayTypeKind,

        /// <summary><see cref="Ubiquity.NET.Llvm.Types.IPointerType"/></summary>
        Pointer = LLVMTypeKind.LLVMPointerTypeKind,

        /// <summary>SIMD 'packed' format, or other <see cref="Ubiquity.NET.Llvm.Types.IVectorType"/> implementation</summary>
        Vector = LLVMTypeKind.LLVMVectorTypeKind,

        /// <summary><see cref="Ubiquity.NET.Llvm.Metadata.IrMetadata"/></summary>
        Metadata = LLVMTypeKind.LLVMMetadataTypeKind,

        /// <summary>Exception handler token</summary>
        Token = LLVMTypeKind.LLVMTokenTypeKind,

        /// <summary>Scalable vector</summary>
        ScalableVector = LLVMTypeKind.LLVMScalableVectorTypeKind,

        /// <summary>B Float type</summary>
        BFloat = LLVMTypeKind.LLVMBFloatTypeKind,

        /// <summary>x86 AMX data type</summary>
        X86AMX = LLVMTypeKind.LLVMX86_AMXTypeKind,

        /// <summary>Target specific extended type</summary>
        TargetSpecific = LLVMTypeKind.LLVMTargetExtTypeKind,
    }

    /// <summary>Interface for a Type in LLVM</summary>
    /// <remarks>
    /// <note type="important">
    /// The <see cref="IEquatable{T}"/> on this type ONLY deals with the native reference equality. That is,
    /// any reference type uses reference equality by default, but <see cref="IEquatable{T}.Equals"/> uses what
    /// is implemented or a default of reference equality for reference types. In this case it determines if
    /// the type refers to the exact same type instance. (Not an equivalent but different type). This is normally
    /// valid as LLVM underneath is uniqueing types within a context. As an example a managed implementation of
    /// <see cref="ITypeRef"/> `A` is a wrapper around an LLVM handle `H` that refers to an native LLVM type `T`.
    /// If there is also a <see cref="ITypeRef"/> `B`, then `A.Equals(B)` is true ONLY if `B` also wraps an LLVM
    /// handle that refers to native type `T`.
    /// </note>
    /// </remarks>
    public interface ITypeRef
        : IEquatable<ITypeRef>
    {
        /// <summary>Gets a value indicating whether the type is sized</summary>
        bool IsSized { get; }

        /// <summary>Gets the LLVM Type kind for this type</summary>
        TypeKind Kind { get; }

        /// <summary>Gets a value indicating whether this type is an integer</summary>
        bool IsInteger { get; }

        /// <summary>Gets a value indicating whether the type is a 32-bit IEEE floating point type</summary>
        bool IsFloat { get; }

        /// <summary>Gets a value indicating whether the type is a 64-bit IEEE floating point type</summary>
        bool IsDouble { get; }

        /// <summary>Gets a value indicating whether this type represents the void type</summary>
        bool IsVoid { get; }

        /// <summary>Gets a value indicating whether this type is a structure type</summary>
        bool IsStruct { get; }

        /// <summary>Gets a value indicating whether this type is a pointer</summary>
        bool IsPointer { get; }

        /// <summary>Gets a value indicating whether this type is a sequence type</summary>
        bool IsSequence { get; }

        /// <summary>Gets a value indicating whether this type is a floating point type</summary>
        bool IsFloatingPoint { get; }

        /// <summary>Gets the ContextAlias that owns this type</summary>
        IContext Context { get; }

        /// <summary>Gets the integer bit width of this type or 0 for non integer types</summary>
        [SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = "Value is the bit width of an integer, name is appropriate" )]
        uint IntegerBitWidth { get; }

        /// <summary>Gets a null value (e.g. all bits == 0 ) for the type</summary>
        /// <remarks>
        /// This is a getter function instead of a property as it can throw exceptions
        /// for types that don't support such a thing (i.e. void )
        /// </remarks>
        /// <returns><see cref="Constant"/> that represents a null (0) value of this type</returns>
        [SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "See comment remarks" )]
        Constant GetNullValue( );

        /// <summary>Array type factory for an array with elements of this type</summary>
        /// <param name="count">Number of elements in the array</param>
        /// <returns><see cref="IArrayType"/> for the array</returns>
        IArrayType CreateArrayType( uint count );

        /// <summary>Get a <see cref="IPointerType"/> for a type that points to elements of this type in the default (0) address space</summary>
        /// <returns><see cref="IPointerType"/>corresponding to the type of a pointer that refers to elements of this type</returns>
        IPointerType CreatePointerType( );

        /// <summary>Get a <see cref="IPointerType"/> for a type that points to elements of this type in the specified address space</summary>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns><see cref="IPointerType"/>corresponding to the type of a pointer that refers to elements of this type</returns>
        IPointerType CreatePointerType( uint addressSpace );
    }

    /// <summary>Internal interface for getting access to the raw type handle internally</summary>
    /// <remarks>This is usually implemented as an explicit interface implementation so that it isn't exposed publicly</remarks>
    internal interface ITypeHandleOwner
        : ITypeRef
        , IEquatable<ITypeHandleOwner>
    {
        /// <summary>Gets the LibLLVM handle for the type</summary>
        LLVMTypeRef Handle { get; }
    }

    internal static class TypeRefExtensions
    {
        internal static LLVMTypeRef GetTypeRef( this ITypeRef self )
        {
            ArgumentNullException.ThrowIfNull( self );
            return ((ITypeHandleOwner)self).Handle;
        }
    }
}
