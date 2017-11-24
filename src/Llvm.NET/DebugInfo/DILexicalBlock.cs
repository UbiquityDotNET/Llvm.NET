// <copyright file="DILexicalBlock.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information for a Lexical block</summary>
    /// <seealso href="xref:llvm_langref#dilexicalblock">LLVM DILexicalBlock</seealso>
    public class DILexicalBlock
        : DILexicalBlockBase
    {
        /* non-operand properties
        uint Line { get; }
        uint Coulmn { get; }
        */

        internal DILexicalBlock( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
