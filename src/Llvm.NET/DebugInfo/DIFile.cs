// <copyright file="DIFile.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information for a source file</summary>
    /// <seealso href="xref:llvm_langref#difile">LLVM DIFile</seealso>
    public class DIFile
        : DIScope
    {
        /* TODO: non-operand properties
            ChecksumKind CheckSumKind {get;}
        */

        /// <summary>Gets the file name for this file</summary>
        public string FileName => GetOperand<MDString>( 0 ).ToString( );

        /// <summary>Gets the Directory for this file</summary>
        public string Directory => GetOperand<MDString>( 1 ).ToString( );

        /// <summary>Gets the Checksum for this file</summary>
        public string CheckSum => GetOperand<MDString>( 2 ).ToString( );

        /// <summary>Gets the full path for this file</summary>
        public string Path => System.IO.Path.Combine( Directory, FileName );

        internal DIFile( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
