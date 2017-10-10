// <copyright file="NamedMDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Wraps an LLVM NamedMDNode</summary>
    /// <remarks>Despite its name a NamedMDNode is not itself an MDNode.</remarks>
    public class NamedMDNode
    {
        /* TODO: Enable retrieving the name from LibLLVM
        // public string Name { get; }
        */

        public IReadOnlyList<MDNode> Operands { get; }

        public NativeModule ParentModule => NativeModule.FromHandle( NativeMethods.NamedMDNodeGetParentModule( NativeHandle ) );

        internal NamedMDNode( LLVMNamedMDNodeRef nativeNode )
        {
            NativeHandle = nativeNode;
            Operands = new OperandIterator( this );
        }

        private LLVMNamedMDNodeRef NativeHandle;

        // internal iterator for Metadata operands
        private class OperandIterator
            : IReadOnlyList<MDNode>
        {
            public MDNode this[ int index ]
            {
                get
                {
                    var nodeHanlde = NativeMethods.NamedMDNodeGetOperand( OwningNode.NativeHandle, (uint)index );
                    return LlvmMetadata.FromHandle<MDNode>( OwningNode.ParentModule.Context, nodeHanlde );
                }
            }

            public int Count => (int)NativeMethods.NamedMDNodeGetNumOperands( OwningNode.NativeHandle );

            public IEnumerator<MDNode> GetEnumerator( )
            {
                for( int i = 0; i < Count; ++i )
                {
                    yield return this[ i ];
                }
            }

            IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

            internal OperandIterator( NamedMDNode owner )
            {
                OwningNode = owner;
            }

            private NamedMDNode OwningNode;
        }
    }
}
