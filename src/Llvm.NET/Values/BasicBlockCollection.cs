// <copyright file="BasicBlockCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Values
{
    internal class BasicBlockCollection
        : ICollection<BasicBlock>
    {
        /// <summary>Gets a count of the blocks in the collection</summary>
        public int Count => checked((int)LLVMCountBasicBlocks( ContainingFunction.ValueHandle ));

        /// <summary>Add a block to the underlying function</summary>
        /// <param name="item"><see cref="BasicBlock"/> to add to the function</param>
        /// <remarks>
        /// The block is appended to the end of the list of blocks owned by the function
        /// </remarks>
        public void Add( BasicBlock item )
        {
            LLVMFunctionAppendBasicBlock( ContainingFunction.ValueHandle, item.BlockHandle );
        }

        /// <inheritdoc/>
        public IEnumerator<BasicBlock> GetEnumerator( )
        {
            uint count = LLVMCountBasicBlocks( ContainingFunction.ValueHandle );
            var buf = new LLVMBasicBlockRef[ count ];
            if( count > 0 )
            {
                LLVMGetBasicBlocks( ContainingFunction.ValueHandle, out buf[ 0 ] );
            }

            return buf.Select( BasicBlock.FromHandle ).GetEnumerator( );
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        public void Clear( )
        {
            var items = this.ToList( );
            foreach( var bb in items )
            {
                Remove( bb );
            }
        }

        public bool Contains( BasicBlock item )
        {
            return item.ContainingFunction == ContainingFunction;
        }

        public void CopyTo( BasicBlock[ ] array, int arrayIndex )
        {
            if( Count + arrayIndex > array.Length )
            {
                throw new ArgumentOutOfRangeException( nameof( arrayIndex ) );
            }

            foreach(var block in this)
            {
                array[ arrayIndex++ ] = block;
            }
        }

        public bool Remove( BasicBlock item )
        {
            if( item.ContainingFunction != ContainingFunction )
            {
                return false;
            }

            LLVMRemoveBasicBlockFromParent( item.BlockHandle );
            return true;
        }

        public bool IsReadOnly => false;

        internal BasicBlockCollection( Function function )
        {
            ContainingFunction = function;
        }

        private Function ContainingFunction;

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private static extern void LLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block );
    }
}
