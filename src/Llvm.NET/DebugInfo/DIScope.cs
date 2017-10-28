// <copyright file="DIScope.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Base class for all Debug info scopes</summary>
    public class DIScope : DINode
    {
        public DIFile File
        {
            get
            {
                var handle = NativeMethods.LLVMDIScopeGetFile( MetadataHandle );
                if( handle == default )
                {
                    return null;
                }

                return FromHandle<DIFile>( handle );
            }
        }

        internal DIScope( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
