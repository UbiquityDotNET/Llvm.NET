// -----------------------------------------------------------------------
// <copyright file="DISubRange.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Sub range</summary>
    /// <seealso href="xref:llvm_langref#disubrange">LLVM DISubRange</seealso>
    public class DISubRange
        : DINode
    {
        /* TODO: non-operand properties need direct API to extract...
        public int64 LowerBound {get;}
        public int64 Count {get;}
        */

        internal DISubRange( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
