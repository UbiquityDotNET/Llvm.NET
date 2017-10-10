// <copyright file="DIObjCProperty.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DIObjCProperty : DINode
    {
        internal DIObjCProperty( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
