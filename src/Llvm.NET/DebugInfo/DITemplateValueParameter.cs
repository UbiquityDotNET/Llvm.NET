// <copyright file="DITemplateValueParameter.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DITemplateValueParameter : DITemplateParameter
    {
        internal DITemplateValueParameter( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
