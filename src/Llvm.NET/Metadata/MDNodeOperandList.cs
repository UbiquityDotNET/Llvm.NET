// <copyright file="MDNodeOperandList.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

/* TODO: Consider an interface that allows updating elements without growing the list */

namespace Llvm.NET
{
    /// <summary>Support class to provide readonly list semantics to the operands of an MDNode</summary>
    internal class MDNodeOperandList
        : IReadOnlyList<MDOperand>
    {
        public MDOperand this[ int index ]
        {
            get
            {
                if( index >= Count || index < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                var handle = LLVMMDNodeGetOperand( OwningNode.MetadataHandle, ( uint )index );
                return MDOperand.FromHandle( OwningNode, handle );
            }
        }

        public int Count
        {
            get
            {
                uint count = LLVMMDNodeGetNumOperands( OwningNode.MetadataHandle );
                return ( int )Math.Min( count, int.MaxValue );
            }
        }

        public IEnumerator<MDOperand> GetEnumerator( )
        {
            for( uint i = 0; i < Count; ++i )
            {
                LLVMMDOperandRef handle = LLVMMDNodeGetOperand( OwningNode.MetadataHandle, i );
                if( handle == default )
                {
                    yield break;
                }

                yield return MDOperand.FromHandle( OwningNode, handle );
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal MDNodeOperandList( MDNode owningNode )
        {
            OwningNode = owningNode;
        }

        private readonly MDNode OwningNode;
    }
}
