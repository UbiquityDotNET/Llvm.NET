// -----------------------------------------------------------------------
// <copyright file="MetadataOperandList.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Support class to provide read/update semantics to the operands of a container element</summary>
    /// <remarks>
    /// This class is used to implement Operand lists of elements including sub lists based on an offset.
    /// The latter case is useful for types that expose some fixed set of operands as properties and some
    /// arbitrary number of additional items as operands.
    /// </remarks>
    public sealed class MetadataOperandCollection
        : IOperandCollection<IrMetadata?>
    {
        /// <inheritdoc/>
        public IrMetadata? this[ int index ]
        {
            get => GetOperand<IrMetadata>( index );
            set
            {
                index.ThrowIfOutOfRange( 0, Count - 1 );
                LibLLVMMDNodeReplaceOperand( Container.Handle, ( uint )index, value?.Handle ?? default );
            }
        }

        /// <summary>Gets the count of operands in this collection</summary>
        public int Count => checked(( int )LibLLVMMDNodeGetNumOperands( Container.Handle ));

        /// <summary>Gets an enumerator for the operands in this collection</summary>
        /// <returns>Enumerator of operands</returns>
        public IEnumerator<IrMetadata?> GetEnumerator( )
        {
            for( int i = 0; i < Count; ++i )
            {
                yield return GetOperand<IrMetadata>( i );
            }
        }

        /// <summary>Gets an enumerator for the operands in this collection</summary>
        /// <returns>Enumerator of operands</returns>
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        /// <inheritdoc/>
        public bool Contains( IrMetadata? item ) => this.Any( n => EqualityComparer<IrMetadata>.Default.Equals(n, item) );

        /// <summary>Specialized indexer to get the element as a specific derived type</summary>
        /// <typeparam name="TItem">Type of the element (must be derived from <see cref="IrMetadata"/></typeparam>
        /// <param name="i">index for the item</param>
        /// <returns>Item at the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is out of range for the collection</exception>
        /// <exception cref="InvalidCastException">If the element at the index is not castable to <typeparamref name="TItem"/></exception>
        /// <remarks>This provides a common (and likely internally optimized) means of getting an element as a specific type</remarks>
        public TItem? GetOperand<TItem>( Index i )
            where TItem : IrMetadata
        {
            uint offset = ( uint )i.GetOffset(Count);
            offset.ThrowIfOutOfRange( 0u, ( uint )Count );
            var node = LibLLVMGetOperandNode( LibLLVMMDNodeGetOperand( Container.Handle, offset ) );
            return (TItem)node.CreateMetadata()!;
        }

        internal MetadataOperandCollection( MDNode container )
        {
            Container = container;
        }

        private readonly MDNode Container;
    }
}
