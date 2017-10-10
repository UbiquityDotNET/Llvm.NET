// <copyright file="DIGlobalVariableExpression.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DIGlobalVariableExpression
        : MDNode
    {
        public DIGlobalVariable Variable
        {
            get
            {
                LLVMMetadataRef handle = NativeMethods.DIGlobalVarExpGetVariable( MetadataHandle );
                if( handle.Pointer.IsNull( ) )
                {
                    return null;
                }

                return FromHandle<DIGlobalVariable>( handle );
            }
        }

        public DIExpression Expression { get; }

        internal DIGlobalVariableExpression( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
