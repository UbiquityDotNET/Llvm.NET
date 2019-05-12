﻿// -----------------------------------------------------------------------
// <copyright file="DILexicalBlock.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.DebugInfo
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
