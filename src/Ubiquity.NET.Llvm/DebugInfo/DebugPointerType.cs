// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Binding between a <see cref="DIType"/> and an <see cref="IPointerType"/></summary>
    /// <seealso href="xref:llvm_langref#ditype">LLVM DIType</seealso>
    public sealed class DebugPointerType
        : DebugType<IPointerType, DIType/*DIDerivedType*/>
        , IPointerType
    {
        /// <summary>Initializes a new instance of the <see cref="DebugPointerType"/> class.</summary>
        /// <param name="debugElementType">Debug type of the pointee</param>
        /// <param name="diBuilder">Debug information builder to use in creating this instance</param>
        /// <param name="addressSpace">Target address space for the pointer [Default: 0]</param>
        /// <param name="name">Name of the type [Default: null]</param>
        /// <param name="alignment">Alignment on pointer</param>
        public DebugPointerType( IDebugType<ITypeRef, DIType> debugElementType, IDIBuilder diBuilder, uint addressSpace = 0, string? name = null, uint alignment = 0 )
            : this( debugElementType.ThrowIfNull().NativeType
                  , diBuilder
                  , debugElementType.DebugInfoType
                  , addressSpace
                  , name
                  , alignment
                  )
        {
            ElementType = debugElementType.NativeType;
        }

        /// <summary>Initializes a new instance of the <see cref="DebugPointerType"/> class.</summary>
        /// <param name="llvmElementType">Native type of the pointee</param>
        /// <param name="diBuilder">Debug information builder to use in creating this instance</param>
        /// <param name="elementType">Debug type of the pointee</param>
        /// <param name="addressSpace">Target address space for the pointer [Default: 0]</param>
        /// <param name="name">Name of the type [Default: null]</param>
        /// <param name="alignment">Alignment of pointer</param>
        public DebugPointerType( ITypeRef llvmElementType, IDIBuilder diBuilder, DIType? elementType, uint addressSpace = 0, string? name = null, uint alignment = 0 )
            : this( llvmElementType.ThrowIfNull().CreatePointerType( addressSpace )
                  , diBuilder
                  , elementType
                  , name
                  , alignment
                  )
        {
            ElementType = llvmElementType;
        }

        /// <summary>Initializes a new instance of the <see cref="DebugPointerType"/> class to bind with an existing pointer type</summary>
        /// <param name="llvmPtrType">Native type of the pointer</param>
        /// <param name="diBuilder">Debug information builder to use in creating this instance</param>
        /// <param name="elementType">Debug type of the pointee</param>
        /// <param name="name">Name of the type [Default: null]</param>
        /// <param name="alignment">Alignment for pointer type</param>
        /// <remarks>
        /// Due to the move to opaque pointers in LLVM, this form has a <see langword="null"/>
        /// <see cref="ElementType"/> property as the elements the pointer refers to are
        /// unknown.
        /// </remarks>
        public DebugPointerType( IPointerType llvmPtrType, IDIBuilder diBuilder, DIType? elementType, string? name = null, uint alignment = 0 )
            : base( llvmPtrType
                  , diBuilder.CreatePointerType( elementType
                                               , name
                                               , diBuilder.OwningModule.ThrowIfNull().Layout.BitSizeOf( llvmPtrType )
                                               , alignment
                                               )
                  )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DebugPointerType"/> class to bind with an existing pointer type</summary>
        /// <param name="llvmPtrType">Native type of the pointer</param>
        /// <param name="debugType">Debug type for the pointer</param>
        /// <remarks>
        /// This constructor is typically used when building typedefs to a basic type
        /// to provide namespace scoping for the typedef for languages that support
        /// such a concept. This is needed because basic types don't have any namespace
        /// information in the LLVM Debug information (they are implicitly in the global
        /// namespace)
        /// </remarks>
        public DebugPointerType( IPointerType llvmPtrType, DIType debugType )
            : base( llvmPtrType, debugType )
        {
        }

        /// <inheritdoc/>
        public uint AddressSpace => NativeType.AddressSpace;

        /// <summary>Gets the element type for this pointer</summary>
        /// <remarks>
        /// Since LLVM moved to opaque pointer types, this may be <see langword="null"/>
        /// depending on how it was constructed. The constructor overloads taking an
        /// <see cref="IPointerType"/> cannot provide an element type as the pointer is
        /// opaque and the type it refers to is unknown.
        /// </remarks>
        public ITypeRef? ElementType { get; init; }
    }
}
