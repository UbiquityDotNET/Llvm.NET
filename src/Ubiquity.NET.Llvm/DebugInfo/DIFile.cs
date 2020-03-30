// -----------------------------------------------------------------------
// <copyright file="DIFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

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
        public string FileName => LLVMDIFileGetFilename( MetadataHandle, out uint _ );

        /// <summary>Gets the Directory for this file</summary>
        public string Directory => LLVMDIFileGetDirectory( MetadataHandle, out uint _ );

        /// <summary>Gets the source of the file or an empty string if not available</summary>
        public string Source => LLVMDIFileGetSource( MetadataHandle, out uint _ );

        /// <summary>Gets the Checksum for this file</summary>
        public string CheckSum => GetOperandString( 2 );

        /// <summary>Gets the full path for this file</summary>
        public string Path => System.IO.Path.Combine( Directory, FileName );

        internal DIFile( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
