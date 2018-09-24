// <copyright file="ReadOnlyAstSlice.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Kaleidoscope.Grammar.AST
{
    internal class ReadOnlyListSlice<TBase,T>
        : IReadOnlyList<T>
        where T : TBase
    {
        public ReadOnlyListSlice( IImmutableList<TBase> wrappedList, int offset, int length)
        {
            if( (offset + length) > wrappedList.Count )
            {
                throw new IndexOutOfRangeException( );
            }

            WrappedList = wrappedList;
            Offset = offset;
            Count = length;
        }

        public T this[ int index ]
        {
            get
            {
                if( index >= Count )
                {
                    throw new IndexOutOfRangeException( );
                }

                return (T)WrappedList[ index + Offset ];
            }
        }

        public int Count { get; }

        public IEnumerator<T> GetEnumerator( )
        {
            for(int i = Offset; i < Count + Offset; ++i )
            {
                yield return (T)WrappedList[ i ];
            }
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        private readonly int Offset;
        private readonly IImmutableList<TBase> WrappedList;
    }
}
