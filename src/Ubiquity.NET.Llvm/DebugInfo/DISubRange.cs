// -----------------------------------------------------------------------
// <copyright file="DISubRange.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Sub range</summary>
    /// <seealso href="xref:llvm_langref#disubrange">LLVM DISubRange</seealso>
    public class DISubRange
        : DINode
    {
        /* TODO: non-operand properties need direct API to extract...
        public int64 LowerBound {get;}
        // count is Operands[0] and could be ConstantInt wrapped in ConstantAsMetadata
        // or DIVariable (e.g. count is either ConstantInt or DIVariable)
        public int64 Count {get;}
        */

        internal DISubRange( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
