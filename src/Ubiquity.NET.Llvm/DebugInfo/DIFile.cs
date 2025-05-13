// -----------------------------------------------------------------------
// <copyright file="DIFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

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
        public LazyEncodedString FileName => Handle == default
                                             ? LazyEncodedString.Empty
                                             : LLVMDIFileGetFilename( Handle ) ?? LazyEncodedString.Empty;

        /// <summary>Gets the Directory for this file</summary>
        public LazyEncodedString Directory => Handle == default
                                              ? LazyEncodedString.Empty
                                              : LLVMDIFileGetDirectory( Handle ) ?? LazyEncodedString.Empty;

        /// <summary>Gets the source of the file or an empty string if not available</summary>
        public LazyEncodedString Source => Handle == default
                                           ? LazyEncodedString.Empty
                                           : LLVMDIFileGetSource( Handle ) ?? LazyEncodedString.Empty;

        /// <summary>Gets the Checksum for this file</summary>
        public LazyEncodedString CheckSum => Handle == default
                                             ? LazyEncodedString.Empty
                                             : GetOperandString( 2 );

        /// <summary>Gets the full path for this file</summary>
        /// <remarks>
        /// <note type="information">
        /// This is NOT a <see cref="LazyEncodedString"/> as there is no native API
        /// equivalent of this method. Instead it is the result of path concatenation
        /// of the <see cref="Directory"/> and <see cref="FileName"/> properties.
        /// </note>
        /// </remarks>
        public string Path => Handle == default
                              ? string.Empty
                              : System.IO.Path.Combine( Directory, FileName );

        internal DIFile( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
