// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm
{
    internal class OperandCollectionSlice<T>
        : IOperandCollection<T>
    {
        public T this[ int index ]
        {
            get
            {
                index.ThrowIfOutOfRange( 0, Count - 1 );
                return InnerCollection[ Offset + index ];
            }

            set
            {
                index.ThrowIfOutOfRange( 0, Count - 1 );
                InnerCollection[ Offset + index ] = value;
            }
        }

        public int Count { get; }

        public bool Contains( T item )
        {
            return this.Any( e => EqualityComparer<T>.Default.Equals( e, item ) );
        }

        public IEnumerator<T> GetEnumerator( )
        {
            int end = Offset + Count;
            for(int i = Offset; i < end; ++i)
            {
                yield return InnerCollection[ i ];
            }
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();

        internal OperandCollectionSlice( IOperandCollection<T> innerCollection, Range sliceRange )
        {
            (Offset, Count) = sliceRange.GetOffsetAndLength( innerCollection.Count );
            InnerCollection = innerCollection;
        }

        private readonly int Offset;
        private readonly IOperandCollection<T> InnerCollection;
    }
}
