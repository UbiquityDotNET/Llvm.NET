// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Generic wrapper to treat an MDTuple as an array of elements of a specific type</summary>
    /// <typeparam name="T">Type of elements</typeparam>
    /// <remarks>
    /// This implements a façade pattern that presents an <see cref="System.Collections.Generic.IReadOnlyCollection{T}"/> for the
    /// operands of an <see cref="MDTuple"/>. This allows treating the tuple like an array of nodes of a
    /// particular type.
    /// </remarks>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Collection doesn't make sense for this type" )]
    public class TupleTypedArrayWrapper<T>
        : IReadOnlyList<T?>
        where T : IrMetadata
    {
        /// <summary>Gets the underlying tuple for this wrapper</summary>
        public MDTuple? Tuple { get; }

        /// <summary>Gets the count of operands in the <see cref="MDTuple"/></summary>
        public int Count => Tuple?.Operands.Count ?? 0;

        /// <summary>Gets an item from the tuple</summary>
        /// <param name="index">Index of the item to retrieve</param>
        /// <returns>The element at <paramref name="index"/> in the tuple</returns>
        public T? this[ int index ]
        {
            get
            {
                index.ThrowIfOutOfRange( 0, Count - 1 );
                return Tuple!.Operands.GetOperand<T>( index );
            }
        }

        /// <summary>Gets an enumerator for the items in the <see cref="MDTuple"/></summary>
        /// <returns>Enumerator</returns>
        /// <remarks>If the underlying tuple is empty this is an empty enumeration</remarks>
        public IEnumerator<T?> GetEnumerator( )
        {
            return Tuple is null
                ? Enumerable.Empty<T>().GetEnumerator()
                : Tuple.Operands
                        .Cast<T>()
                        .GetEnumerator();
        }

        /// <summary>Gets an enumerator for the items in the <see cref="MDTuple"/></summary>
        /// <returns>Enumerator</returns>
        /// <remarks>If the underlying tuple is empty this is an empty enumeration</remarks>
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();

        internal TupleTypedArrayWrapper( MDTuple? tuple )
        {
            Tuple = tuple;
        }
    }
}
