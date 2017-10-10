// <copyright file="DITemplateParameter.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DITemplateParameter : DINode
    {
        internal DITemplateParameter( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
