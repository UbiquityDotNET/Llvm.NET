// -----------------------------------------------------------------------
// <copyright file="DILexicalBlockBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Base for lexical blocks</summary>
    public class DILexicalBlockBase
        : DILocalScope
    {
        internal DILexicalBlockBase( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
