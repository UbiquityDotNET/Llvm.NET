// <copyright file="DIGlobalVariableExpression.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug Global variable expression</summary>
    /// <remarks>This node binds a <see cref="DIGlobalVariable"/> and a <see cref="DIExpression"/></remarks>
    /// <seealso href="xref:llvm_langref#diglobalvariable">LLVM DIGlobalVariable</seealso>
    /// <seealso href="xref:llvm_langref#diespression">LLVM DIExpression</seealso>
    public class DIGlobalVariableExpression
        : MDNode
    {
        /// <summary>Gets the <see cref="DIGlobalVariable"/> for this node</summary>
        public DIGlobalVariable Variable => GetOperand<DIGlobalVariable>( 0 );

        /// <summary>Gets the <see cref="DIExpression"/> for this node</summary>
        public DIExpression Expression => GetOperand<DIExpression>( 1 );

        internal DIGlobalVariableExpression( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
