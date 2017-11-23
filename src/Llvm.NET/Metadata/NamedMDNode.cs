// <copyright file="NamedMDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Wraps an LLVM NamedMDNode</summary>
    /// <remarks>Despite its name a NamedMDNode is not itself an MDNode.</remarks>
    public class NamedMDNode
    {
        /* TODO:
        public string Name { get; }
        public void AddOperand(MDNode node) {...}
        */

        /// <summary>Gets the operands for the node</summary>
        public IReadOnlyList<MDNode> Operands { get; }

        /// <summary>Gets the module that owns this node</summary>
        public BitcodeModule ParentModule => BitcodeModule.FromHandle( LLVMNamedMDNodeGetParentModule( NativeHandle ) );

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
                    var nodeHanlde = LLVMNamedMDNodeGetOperand( OwningNode.NativeHandle, (uint)index );
                    return LlvmMetadata.FromHandle<MDNode>( OwningNode.ParentModule.Context, nodeHanlde );
                }

                /* TODO:
                set
                {   index.VerifyRange(0, Count, nameof(index));
                    LLVMNamedMDNodeSetOperand( index, value.NativeHandle );
                }
                */
            }

            public int Count => (int)LLVMNamedMDNodeGetNumOperands( OwningNode.NativeHandle );

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

            private readonly NamedMDNode OwningNode;
        }
    }
}
