// -----------------------------------------------------------------------
// <copyright file="DIFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.DebugInfo
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
        public string FileName => Handle == default ? string.Empty : LLVMDIFileGetFilename( Handle, out uint _ ) ?? string.Empty;

        /// <summary>Gets the Directory for this file</summary>
        public string Directory => Handle == default ? string.Empty : LLVMDIFileGetDirectory( Handle, out uint _ ) ?? string.Empty;

        /// <summary>Gets the source of the file or an empty string if not available</summary>
        public string Source => Handle == default ? string.Empty : LLVMDIFileGetSource( Handle, out uint _ ) ?? string.Empty;

        /// <summary>Gets the Checksum for this file</summary>
        public string CheckSum => Handle == default ? string.Empty : GetOperandString( 2 );

        /// <summary>Gets the full path for this file</summary>
        public string Path => Handle == default ? string.Empty : System.IO.Path.Combine( Directory, FileName );

        internal DIFile( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
