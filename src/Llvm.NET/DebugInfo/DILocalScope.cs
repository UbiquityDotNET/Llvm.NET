// <copyright file="DILocalScope.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Legal scope for lexical blocks, local variables, and debug info locations</summary>
    public class DILocalScope
        : DIScope
    {
        // returns "this" if the scope is a subprogram, otherwise walks up the scopes to find
        // the containing subprogram.
        public DISubProgram SubProgram => FromHandle<DISubProgram>( NativeMethods.LLVMDILocalScopeGetSubProgram( MetadataHandle ) );

        internal DILocalScope( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
