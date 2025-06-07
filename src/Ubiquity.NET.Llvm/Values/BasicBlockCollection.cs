// -----------------------------------------------------------------------
// <copyright file="BasicBlockCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    internal class BasicBlockCollection
        : ICollection<BasicBlock>
    {
        /// <summary>Gets a count of the blocks in the collection</summary>
        public int Count => checked((int)LLVMCountBasicBlocks( ContainingFunction.Handle ));

        /// <summary>Add a block to the underlying function</summary>
        /// <param name="item"><see cref="BasicBlock"/> to add to the function</param>
        /// <remarks>
        /// The block is appended to the end of the list of blocks owned by the function
        /// </remarks>
        public void Add( BasicBlock item )
        {
            ArgumentNullException.ThrowIfNull( item );

            if(item.ContainingFunction == null)
            {
                LibLLVMFunctionAppendBasicBlock( ContainingFunction.Handle, item.BlockHandle );
            }

            if(!EqualityComparer<Function>.Default.Equals( item.ContainingFunction, ContainingFunction ))
            {
                throw new ArgumentException( Resources.Cannot_add_a_block_belonging_to_a_different_function, nameof( item ) );
            }

            throw new ArgumentException( Resources.Block_already_exists_in_function, nameof( item ) );
        }

        /// <inheritdoc/>
        public IEnumerator<BasicBlock> GetEnumerator( )
        {
            LLVMBasicBlockRef blockRef = LLVMGetFirstBasicBlock( ContainingFunction.Handle );
            while(blockRef != default)
            {
                yield return BasicBlock.FromHandle( blockRef )!;
                blockRef = LLVMGetNextBasicBlock( blockRef );
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();

        public void Clear( )
        {
            var items = this.ToList( );
            foreach(var bb in items)
            {
                Remove( bb );
            }
        }

        public bool Contains( BasicBlock item )
        {
            ArgumentNullException.ThrowIfNull( item );
            return EqualityComparer<Function>.Default.Equals( item.ContainingFunction, ContainingFunction );
        }

        public void CopyTo( BasicBlock[] array, int arrayIndex )
        {
            if(Count + arrayIndex > array.Length)
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
            ArgumentNullException.ThrowIfNull( item );
            if(!EqualityComparer<Function>.Default.Equals( item.ContainingFunction, ContainingFunction ))
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

        private readonly Function ContainingFunction;
    }
}
