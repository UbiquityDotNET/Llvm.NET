// <copyright file="MDOperand.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET
{
    public class MDOperand
    {
        public MDNode OwningNode { get; }

        public LlvmMetadata Metadata => LlvmMetadata.FromHandle<LlvmMetadata>( OwningNode.Context, NativeMethods.LLVMGetOperandNode( OperandHandle ) );

        internal MDOperand( MDNode owningNode, LLVMMDOperandRef handle )
        {
            if( handle.Handle == IntPtr.Zero )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

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
