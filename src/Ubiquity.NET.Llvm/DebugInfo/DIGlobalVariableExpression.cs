// -----------------------------------------------------------------------
// <copyright file="DIGlobalVariableExpression.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Metadata;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug Global variable expression</summary>
    /// <remarks>This node binds a <see cref="DIGlobalVariable"/> and a <see cref="DIExpression"/></remarks>
    /// <seealso href="xref:llvm_langref#diglobalvariable">LLVM DIGlobalVariable</seealso>
    /// <seealso href="xref:llvm_langref#diexpression">LLVM DIExpression</seealso>
    public class DIGlobalVariableExpression
        : MDNode
    {
        /// <summary>Gets the <see cref="DIGlobalVariable"/> for this node</summary>
        public DIGlobalVariable Variable
            => (DIGlobalVariable)LLVMDIGlobalVariableExpressionGetVariable( Handle ).ThrowIfInvalid( ).CreateMetadata( )!;

        /// <summary>Gets the <see cref="DIExpression"/> for this node</summary>
        public DIExpression Expression
            => (DIExpression)LLVMDIGlobalVariableExpressionGetExpression( Handle ).ThrowIfInvalid( ).CreateMetadata( )!;

        internal DIGlobalVariableExpression( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
