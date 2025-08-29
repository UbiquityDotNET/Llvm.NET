// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Base class for macro related nodes in the debug hierarchy</summary>
    public class DIMacroNode
        : MDNode
    {
        internal DIMacroNode( LLVMMetadataRef handle )
           : base( handle )
        {
        }
    }
}
