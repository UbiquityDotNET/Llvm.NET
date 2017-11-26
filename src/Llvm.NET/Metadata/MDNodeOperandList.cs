// <copyright file="MDNodeOperandList.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

/* TODO: Consider an interface that allows updating elements without growing the list */
/* TODO: Consider a generic implementation of this as it is mosly a duplicate of the UserOperandList operand list support
         (core difference is the GetOperand() and GetNumOperands calls)
*/

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
                index.ValidateRange( 0, Count - 1, nameof( index ) );

                return GetElement( index );
            }
        }

        public int Count
        {
            get
            {
                long count = LLVMMDNodeGetNumOperands( OwningNode.MetadataHandle ) - Offset;
                return ( int )Math.Min( count, int.MaxValue );
            }
        }

        /// <inheritdoc/>
        public IEnumerator<MDOperand> GetEnumerator( )
        {
            for( int i = 0; i < Count; ++i )
            {
                var element = GetElement( i );
                if( element == null )
                {
                    yield break;
                }

                yield return element;
            }
        }

        /// <inheritdoc/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal MDNodeOperandList( MDNode owningNode )
            : this( owningNode, 0 )
        {
        }

        internal MDNodeOperandList( MDNode owningNode, int offset )
        {
            Offset = offset;
            OwningNode = owningNode;
        }

        private MDOperand GetElement( int index )
        {
            var handle = LLVMMDNodeGetOperand( OwningNode.MetadataHandle, checked(( uint )( index + Offset ) ) );
            if( handle == default )
            {
                return null;
            }

            return MDOperand.FromHandle( OwningNode, handle );
        }

        private int Offset;
        private readonly MDNode OwningNode;

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern UInt32 LLVMMDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMMDOperandRef LLVMMDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, UInt32 index );
    }
}
