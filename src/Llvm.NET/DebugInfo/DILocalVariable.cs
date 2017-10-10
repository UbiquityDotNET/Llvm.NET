// <copyright file="DILocalVariable.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dilocalvariable"/></summary>
    public class DILocalVariable : DIVariable
    {
        public new DILocalScope Scope => base.Scope as DILocalScope;

        internal DILocalVariable( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
