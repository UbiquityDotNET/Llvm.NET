// <copyright file="NamedMDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiquity.ArgValidators;

using static Llvm.NET.NamedMDNode.NativeMethods;

namespace Llvm.NET
{
    /// <content>Private partial class implementation to contain the Operand list/iterator for NamedMDNode</content>
    public partial class NamedMDNode
    {
        // internal iterator for Metadata operands
        private class OperandIterator
            : IList<MDNode>
        {
            public MDNode this[ int index ]
            {
                get
                {
                    index.ValidateRange( 0, Count, nameof( index ) );
                    var nodeHanlde = LLVMNamedMDNodeGetOperand( OwningNode.NativeHandle, ( uint )index );
                    return LlvmMetadata.FromHandle<MDNode>( OwningNode.ParentModule.Context, nodeHanlde );
                }

                set
                {
                    index.ValidateRange(0, Count, nameof(index));
                    LLVMNamedMDNodeSetOperand( OwningNode.NativeHandle, (uint)index, value.MetadataHandle );
                }
            }

            public int Count => ( int )LLVMNamedMDNodeGetNumOperands( OwningNode.NativeHandle );

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
                    if( this[ i ] == item )
                    {
                        return i;
                    }
                }

                return -1;
            }

            public void Add( MDNode item )
            {
                item.ValidateNotNull( nameof( item ) );
                /* ReSharper disable once PossibleNullReferenceException */
                LLVMNamedMDNodeAddOperand( OwningNode.NativeHandle, item.MetadataHandle );
            }

            public void Clear( )
            {
                LLVMNamedMDNodeClearOperands( OwningNode.NativeHandle );
            }

            public bool Contains( MDNode item ) => this.Any( n => n == item );

            public void CopyTo( MDNode[ ] array, int arrayIndex )
            {
                arrayIndex.ValidateRange( 0, array.Length - Count, nameof( arrayIndex ) );
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
    }
}
