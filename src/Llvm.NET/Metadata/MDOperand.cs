// <copyright file="MDOperand.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Native.Handles;

namespace Llvm.NET
{
    /// <summary>Operand for an <see cref="MDNode"/></summary>
    public class MDOperand
    {
        /// <summary>Gets the node that owns this operand</summary>
        public MDNode OwningNode { get; }

        /// <summary>Gets the metadata contents of the operand</summary>
        public LlvmMetadata Metadata => LlvmMetadata.FromHandle<LlvmMetadata>( OwningNode.Context, NativeMethods.LLVMGetOperandNode( OperandHandle ) );

        internal MDOperand( MDNode owningNode, LLVMMDOperandRef handle )
        {
            handle.ValidateNotDefault( nameof( handle ) );

            OperandHandle = handle;
            OwningNode = owningNode;
        }

        internal static MDOperand FromHandle( MDNode owningNode, LLVMMDOperandRef handle )
        {
            return owningNode.Context.GetOperandFor( owningNode, handle );
        }

        private readonly LLVMMDOperandRef OperandHandle;
    }
}
