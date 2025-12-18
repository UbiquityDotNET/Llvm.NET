// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm
{
    /// <summary>Byte ordering for target code generation and data type layout</summary>
    public enum ByteOrdering
    {
        /// <summary>Little-Endian layout format</summary>
        LittleEndian = LLVMByteOrdering.LLVMLittleEndian,

        /// <summary>Big-Endian layout format</summary>
        BigEndian = LLVMByteOrdering.LLVMBigEndian
    }

    /// <summary>Provides access to LLVM target data layout information</summary>
    /// <remarks>
    /// <para>There is a distinction between various sizes and alignment for a given type
    /// that are target dependent.</para>
    /// <para>The following table illustrates the differences in sizes and their meaning
    ///  for a sample set of types.</para>
    /// <list type="table">
    /// <listheader>
    ///     <term> Type </term><term> SizeInBits </term><term> StoreSizeInBits </term><term> AbiSizeInBits </term>
    /// </listheader>
    /// <item> <description>i1      </description><description> 1   </description><description> 8   </description><description> 8   </description></item>
    /// <item> <description>i8      </description><description> 8   </description><description> 8   </description><description> 8   </description></item>
    /// <item> <description>i19     </description><description> 19  </description><description> 24  </description><description> 32  </description></item>
    /// <item> <description>i32     </description><description> 32  </description><description> 32  </description><description> 32  </description></item>
    /// <item> <description>i10     </description><description> 100 </description><description> 104 </description><description> 128 </description></item>
    /// <item> <description>i128    </description><description> 128 </description><description> 128 </description><description> 128 </description></item>
    /// <item> <description>Float   </description><description> 32  </description><description> 32  </description><description> 32  </description></item>
    /// <item> <description>Double  </description><description> 64  </description><description> 64  </description><description> 64  </description></item>
    /// <item> <description>X86_FP80</description><description> 80  </description><description> 80  </description><description> 96  </description></item>
    /// </list>
    ///
    /// <note type="note">
    /// The allocation size depends on the alignment, and thus on the target.
    /// The values in the example table are for x86-32-linux.
    /// </note>
    /// <list type="table">
    /// <listheader>
    ///     <term>Property</term><term>Definition</term>
    /// </listheader>
    /// <item> <description>SizeInBits      </description><description> Minimum number of bits needed to represent the full range of values for the type </description></item>
    /// <item> <description>StoreSizeInBits </description><description> Minimum number of bits needed to actually store a *single* value of the type </description></item>
    /// <item> <description>AbiSizeInBits   </description><description> Total number of bits used to store a value in a sequence, including any alignment padding </description></item>
    /// </list>
    /// The allocation size determines the total size of each entry in a sequence so that the "next" element is computed
    /// by adding the size to the start address of the current element.
    /// </remarks>
    public interface IDataLayout
    {
        /// <summary>Gets the byte ordering for this target</summary>
        public ByteOrdering Endianness { get; }

        /// <summary>Gets the size (in bytes) of a pointer for the default address space of the target</summary>
        /// <returns>Size of a pointer to the default address space</returns>
        public uint PointerSize( );

        /// <summary>Retrieves the size (in bytes) of a pointer for a given address space of the target</summary>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns>Size of a pointer</returns>
        public uint PointerSize( uint addressSpace );

        /// <summary>Retrieves an LLVM integer type with the same bit width as a pointer for the default address space of the target</summary>
        /// <param name="context">LLVM <see cref="IContext"/> that owns the definition of the pointer type to retrieve</param>
        /// <returns>Integer type matching the bit width of a native pointer in the target's default address space</returns>
        public ITypeRef IntPtrType( IContext context );

        /* TODO: Additional properties for DataLayout
        bool IsLegalIntegerWidth(UInt64 width);
        bool ExceedsNaturalStackAlignment(UInt64 width);
        UInt32 StackAlignment { get; }
        UInt32 AllocaAddrSpace { get; }
        bool HasMicrosoftFastStdCallMangling { get; }
        string LinkerPrivateGlobalPrefix { get; }
        char GlobalPrefix { get; }
        string PrivateGlobalPrefix { get; }
        ImmutableList<UInt32> NonIntegralAddressSpaces { get; }
        bool IsNonIntegralPointerType( IPointerType t );
        ITypeRef GetSmallestLegalIntType( ContextAlias context, UInt32 width );
        ITypeRef GetLargestLegalIntType( ContextAlias context, UInt32 width );
        UInt32 GetLargestLegalIntTypeSizeInBits();
        UInt64 GetIndexedOffsetInType(ITypeRef t, Value index0, param Value[] indices);
        StructLayout GetStructLayout(IStructType t);
        */

        /// <summary>Retrieves an LLVM integer type with the same bit width as
        /// a pointer for the given address space of the target</summary>
        /// <param name="context">LLVM <see cref="IContext"/> that owns the definition of the pointer type to retrieve</param>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns>Integer type matching the bit width of a native pointer in the target's address space</returns>
        public ITypeRef IntPtrType( IContext context, uint addressSpace );

        /// <summary>Returns the number of bits necessary to hold the specified type.</summary>
        /// <param name="typeRef">Type to retrieve the size of</param>
        /// <remarks>
        /// <para>This method determines the bit size of a type (e.g. the minimum number of
        /// bits required to represent any value of the given type.) This is distinct from the storage
        /// and stack size due to various target alignment requirements.</para>
        /// </remarks>
        /// <returns>Size of the type in bits</returns>
        public ulong BitSizeOf( ITypeRef typeRef );

        /// <summary>Retrieves the number of bits required to store a value of the given type</summary>
        /// <param name="typeRef">Type to retrieve the storage size of</param>
        /// <returns>Number of bits required to store a value of the given type in the target</returns>
        /// <remarks>This method retrieves the storage size in bits of a given type. The storage size
        /// includes any trailing padding bits that may be needed if the target requires reading a wider
        /// word size. (e.g. most systems can't write a single bit value for an LLVM i1, thus the
        /// storage size is whatever the minimum number of bits that the target requires to store a value
        /// of the given type)
        /// </remarks>
        public ulong StoreSizeOf( ITypeRef typeRef );

        /// <summary>Retrieves the ABI specified size (in bytes) of the given type</summary>
        /// <param name="typeRef">Type to get the size from</param>
        /// <returns>Size of the type</returns>
        /// <remarks>
        /// Returns the offset in bytes between successive objects of the
        /// specified type, including alignment padding
        /// </remarks>
        public ulong AbiSizeOf( ITypeRef typeRef );

        /// <summary>Retrieves the ABI specified alignment, in bytes, for a specified type</summary>
        /// <param name="typeRef">Type to get the alignment for</param>
        /// <returns>ABI specified alignment</returns>
        public uint AbiAlignmentOf( ITypeRef typeRef );

        /// <summary>Retrieves the call frame alignment (in bytes) for a given type</summary>
        /// <param name="typeRef">type to get the alignment of</param>
        /// <returns>Alignment for the type</returns>
        public uint CallFrameAlignmentOf( ITypeRef typeRef );

        /// <summary>Gets the preferred alignment (in bytes) for an LLVM type</summary>
        /// <param name="typeRef">Type to get the alignment of</param>
        /// <returns>Preferred alignment</returns>
        public uint PreferredAlignmentOf( ITypeRef typeRef );

        /// <summary>Gets the preferred alignment (in bytes) for a <see cref="Value"/></summary>
        /// <param name="value">Value to get the alignment of</param>
        /// <returns>Preferred alignment</returns>
        public uint PreferredAlignmentOf( Value value );

        /// <summary>Gets the element index for a specific offset in a given structure</summary>
        /// <param name="structType">Type of the structure</param>
        /// <param name="offset">Offset to determine the index of</param>
        /// <returns>Index of the element</returns>
        public uint ElementAtOffset( IStructType structType, ulong offset );

        /// <summary>Gets the offset of an element in a structure</summary>
        /// <param name="structType">Type of the structure</param>
        /// <param name="element">index of the element in the structure</param>
        /// <returns>Offset of the element from the beginning of the structure</returns>
        public ulong OffsetOfElement( IStructType structType, uint element );

        /// <summary>Gets the string representation of this data layout as a <see cref="LazyEncodedString"/></summary>
        /// <returns>Representation of the data layout</returns>
        /// <remarks>
        /// The returned <see cref="LazyEncodedString"/> retains a copy of the native code form of the string.
        /// This value is ONLY marshaled to a managed string when needed (and only once, it is cached). This
        /// behavior allows for lower overhead re-use of this string in additional APIs as NO marshalling
        /// needs to occur. This does have the overhead of making a copy of the strings contents as the
        /// lifetime of the underlying native string is generally unknown and thus not reliable.
        /// </remarks>
        public LazyEncodedString ToLazyEncodedString( );

        /// <summary>Gets the byte size of a type</summary>
        /// <param name="llvmType">Type to determine the size of</param>
        /// <returns>Size of the type in bytes</returns>
        public ulong ByteSizeOf( ITypeRef llvmType );

        /// <summary>Gets the preferred alignment of the type in bits</summary>
        /// <param name="llvmType">Type to get the alignment of</param>
        /// <returns>Alignment of the type</returns>
        public uint PreferredBitAlignmentOf( ITypeRef llvmType );

        /// <summary>Gets the ABI alignment of the type in bits</summary>
        /// <param name="llvmType">Type to get the alignment of</param>
        /// <returns>Alignment of the type</returns>
        public uint AbiBitAlignmentOf( ITypeRef llvmType );

        /// <summary>Gets the offset of a structure element in bits</summary>
        /// <param name="llvmType">Structure type to get the element offset of</param>
        /// <param name="element">Index of the element in the structure</param>
        /// <returns>Offset of the element in bits</returns>
        public ulong BitOffsetOfElement( IStructType llvmType, uint element );
    }

    internal static class DataLayoutExtensions
    {
        // TODO: Is this needed? Owned handles are implicitly castable to the unowned forms
        internal static LLVMTargetDataRefAlias GetUnownedHandle( this IDataLayout self )
        {
            if(self is DataLayoutAlias alias)
            {
                return alias.Handle;
            }
            else if(self is DataLayout owner)
            {
                // implicitly cast to the alias handle
                return owner.Handle;
            }
            else
            {
                throw new ArgumentException( "Internal Error - Unknown context type!", nameof( self ) );
            }
        }
    }
}
