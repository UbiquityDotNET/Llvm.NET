// <copyright file="DITemplateTypeParameter.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DITemplateTypeParameter : DITemplateParameter
    {
        internal DITemplateTypeParameter( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
