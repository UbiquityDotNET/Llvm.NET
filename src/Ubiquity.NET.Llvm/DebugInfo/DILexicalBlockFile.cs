// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
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
