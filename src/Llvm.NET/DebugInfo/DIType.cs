// <copyright file="DIType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Base class for Debug info types</summary>
    /// <seealso href="xref:llvm_langref#ditype">LLVM DIType</seealso>
   public class DIType
        : DIScope
    {
        /// <summary>Gets the containing scope for the type</summary>
        public override DIScope Scope => GetOperand<DIScope>( 1 );

        /// <summary>Gets the name of the type</summary>
        public override string Name => GetOperand<MDString>( 2 )?.ToString( ) ?? string.Empty;

        /// <summary>Gets the source line for the type</summary>
        public UInt32 Line => NativeMethods.LLVMDITypeGetLine( MetadataHandle );

        /// <summary>Gets the size of the type in bits</summary>
        public UInt64 BitSize => NativeMethods.LLVMDITypeGetSizeInBits( MetadataHandle );

        /// <summary>Gets the alignment of the type in bits</summary>
        public UInt64 BitAlignment => NativeMethods.LLVMDITypeGetAlignInBits( MetadataHandle );

        /// <summary>Gets the offset of the type in bits</summary>
        public UInt64 BitOffset => NativeMethods.LLVMDITypeGetOffsetInBits( MetadataHandle );

        /// <summary>Gets the flags that describe the behaviors fo</summary>
        public DebugInfoFlags DebugInfoFlags
        {
            get
            {
                if( MetadataHandle == default )
                {
                    return 0;
                }

                return ( DebugInfoFlags )NativeMethods.LLVMDITypeGetFlags( MetadataHandle );
            }
        }

        internal DIType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
