// <copyright file="MDNodeOperandList.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET.Native;

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

                var handle = NativeMethods.LLVMMDNodeGetOperand( OwningNode.MetadataHandle, ( uint )index );
                return MDOperand.FromHandle( OwningNode, handle );
            }
        }

        public int Count
        {
            get
            {
                uint count = NativeMethods.LLVMMDNodeGetNumOperands( OwningNode.MetadataHandle );
                return ( int )Math.Min( count, int.MaxValue );
            }
        }

        public IEnumerator<MDOperand> GetEnumerator( )
        {
            for( uint i = 0; i < Count; ++i )
            {
                LLVMMDOperandRef handle = NativeMethods.LLVMMDNodeGetOperand( OwningNode.MetadataHandle, i );
                if( handle.Pointer == IntPtr.Zero )
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

        private MDNode OwningNode;
    }
}
