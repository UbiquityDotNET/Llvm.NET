// <copyright file="DIType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

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
        public UInt32 Line => LLVMDITypeGetLine( MetadataHandle );

        /// <summary>Gets the size of the type in bits</summary>
        public UInt64 BitSize => LLVMDITypeGetSizeInBits( MetadataHandle );

        /// <summary>Gets the alignment of the type in bits</summary>
        public UInt64 BitAlignment => LLVMDITypeGetAlignInBits( MetadataHandle );

        /// <summary>Gets the offset of the type in bits</summary>
        public UInt64 BitOffset => LLVMDITypeGetOffsetInBits( MetadataHandle );

        /// <summary>Gets the flags that describe the behaviors for</summary>
        public DebugInfoFlags DebugInfoFlags
        {
            get
            {
                if( MetadataHandle == default )
                {
                    return 0;
                }

                return ( DebugInfoFlags )LLVMDITypeGetFlags( MetadataHandle );
            }
        }

        internal DIType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
