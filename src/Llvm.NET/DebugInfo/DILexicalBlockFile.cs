// <copyright file="DILexicalBlockFile.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information for a file lexical block</summary>
    /// <seealso href="xref:llvm_langref#dilexicalblockfile">LLVM DILexicalBlockBase</seealso>
    public class DILexicalBlockFile
        : DILexicalBlockBase
    {
        /* TODO: non-operand property
        unsigned Discriminator { get; }
        */

        internal DILexicalBlockFile( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
