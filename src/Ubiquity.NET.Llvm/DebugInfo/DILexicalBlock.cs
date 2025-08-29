// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information for a Lexical block</summary>
    /// <seealso href="xref:llvm_langref#dilexicalblock">LLVM DILexicalBlock</seealso>
    public class DILexicalBlock
        : DILexicalBlockBase
    {
        /* TODO: non-operand properties
        uint Line { get; }
        uint Column { get; }
        */

        internal DILexicalBlock( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
