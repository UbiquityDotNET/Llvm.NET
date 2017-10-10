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
        public string FileName => NativeMethods.GetDIFileName( MetadataHandle );

        public string Directory => NativeMethods.GetDIFileDirectory( MetadataHandle );

        public string Path => System.IO.Path.Combine( Directory, FileName );

        internal DIFile( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
