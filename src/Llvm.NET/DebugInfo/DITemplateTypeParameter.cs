// <copyright file="DITemplateTypeParameter.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Template type parameter</summary>
    /// <seealso href="xref:llvm_langref#ditemplatetypeparameter">LLVM DITemplateTypeParameter</seealso>
    public class DITemplateTypeParameter
        : DITemplateParameter
    {
        internal DITemplateTypeParameter( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
