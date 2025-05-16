// -----------------------------------------------------------------------
// <copyright file="DebugInfoType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Base class for Debug info types</summary>
    /// <seealso href="xref:llvm_langref#ditype">LLVM DebugInfoType</seealso>
    public class DIType
        : DIScope
    {
        /// <summary>Gets the containing scope for the type</summary>
        public override DIScope? Scope => Handle == default ? null : GetOperand<DIScope>( 1 );

        /// <summary>Gets the name of the type</summary>
        public override LazyEncodedString Name => Handle == default ? string.Empty : GetOperand<MDString>( 2 )?.ToString() ?? string.Empty;

        /// <summary>Gets the source line for the type</summary>
        public UInt32 Line => Handle == default ? 0 : LLVMDITypeGetLine( Handle );

        /// <summary>Gets the size of the type in bits</summary>
        public UInt64 BitSize => Handle == default ? 0 : LLVMDITypeGetSizeInBits( Handle );

        /// <summary>Gets the alignment of the type in bits</summary>
        public UInt64 BitAlignment => Handle == default ? 0 : LLVMDITypeGetAlignInBits( Handle );

        /// <summary>Gets the offset of the type in bits</summary>
        public UInt64 BitOffset => Handle == default ? 0 : LLVMDITypeGetOffsetInBits( Handle );

        /// <summary>Gets the flags that describe the behaviors for</summary>
        public DebugInfoFlags DebugInfoFlags => Handle == default ? 0 : ( DebugInfoFlags )LLVMDITypeGetFlags( Handle );

        internal DIType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
