// -----------------------------------------------------------------------
// <copyright file="NamedMDNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;
using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Wraps an LLVM NamedMDNode</summary>
    /// <remarks>Despite its name a NamedMDNode is not itself an MDNode. It is owned directly by a
    /// a <see cref="Module"/> and contains a list of <see cref="MDNode"/> operands.</remarks>
    public class NamedMDNode
    {
        /// <summary>Gets the name of the node</summary>
        public LazyEncodedString Name => LLVMGetNamedMetadataName( NativeHandle ) ?? LazyEncodedString.Empty;

        /// <summary>Gets the operands for the node</summary>
        public IList<MDNode> Operands { get; }

        /// <summary>Gets the module that owns this node</summary>
        public IModule ParentModule => new ModuleAlias(LibLLVMNamedMetadataGetParentModule( NativeHandle ));

        /// <summary>Erases this node from its parent</summary>
        public void EraseFromParent( ) => LibLLVMNamedMetadataEraseFromParent( NativeHandle );

        internal NamedMDNode( LLVMNamedMDNodeRef nativeNode )
        {
            NativeHandle = nativeNode;
            Operands = new OperandIterator( this );
        }

        // private iterator for IrMetadata operands
        private class OperandIterator
            : IList<MDNode>
        {
            public MDNode this[ int index ]
            {
                get
                {
                    index.ThrowIfOutOfRange( 0, Count );
                    var nodeHandle = LibLLVMNamedMDNodeGetOperand( OwningNode.NativeHandle, ( uint )index );
                    return (MDNode)nodeHandle.CreateMetadata()!;
                }

                set
                {
                    index.ThrowIfOutOfRange( 0, Count );
                    LibLLVMNamedMDNodeSetOperand( OwningNode.NativeHandle, ( uint )index, value.Handle );
                }
            }

            public int Count => ( int )LibLLVMNamedMDNodeGetNumOperands( OwningNode.NativeHandle );

            public IEnumerator<MDNode> GetEnumerator( )
            {
                for( int i = 0; i < Count; ++i )
                {
                    yield return this[ i ];
                }
            }

            IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

            public int IndexOf( MDNode item )
            {
                for( int i = 0; i < Count; ++i )
                {
                    if( this[ i ].Equals( item ) )
                    {
                        return i;
                    }
                }

                return -1;
            }

            public void Add( MDNode item )
            {
                ArgumentNullException.ThrowIfNull( item );
                LibLLVMNamedMDNodeAddOperand( OwningNode.NativeHandle, item.Handle );
            }

            public void Clear( )
            {
                LibLLVMNamedMDNodeClearOperands( OwningNode.NativeHandle );
            }

            public bool Contains( MDNode item ) => this.Any( n => n.Equals( item ) );

            public void CopyTo( MDNode[ ] array, int arrayIndex )
            {
                arrayIndex.ThrowIfOutOfRange( 0, array.Length - Count );
                for( int i = 0; i < Count; ++i )
                {
                    array[ i + arrayIndex ] = this[ i ];
                }
            }

            public bool Remove( MDNode item )
            {
                throw new NotSupportedException( );
            }

            public void Insert( int index, MDNode item )
            {
                throw new NotSupportedException( );
            }

            public void RemoveAt( int index )
            {
                throw new NotSupportedException( );
            }

            public bool IsReadOnly => false;

            internal OperandIterator( NamedMDNode owner )
            {
                OwningNode = owner;
            }

            private readonly NamedMDNode OwningNode;
        }

        private readonly LLVMNamedMDNodeRef NativeHandle;
    }
}
