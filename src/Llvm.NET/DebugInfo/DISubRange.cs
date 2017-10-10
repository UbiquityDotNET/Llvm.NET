// <copyright file="DISubRange.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubrange"/></summary>
    public class DISubRange : DINode
    {
        internal DISubRange( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
