// <copyright file="DIModule.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DIModule : DIScope
    {
        internal DIModule( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
