// <copyright file="DISubroutineType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubroutinetype"/></summary>
    public class DISubroutineType : DICompositeType
    {
        internal DISubroutineType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
