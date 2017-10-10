// <copyright file="MDTuple.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET
{
    public class MDTuple : MDNode
    {
        internal MDTuple( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
