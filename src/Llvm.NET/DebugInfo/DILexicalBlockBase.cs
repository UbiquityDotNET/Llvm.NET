// <copyright file="DILexicalBlockBase.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

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
