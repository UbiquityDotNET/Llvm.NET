﻿// -----------------------------------------------------------------------
// <copyright file="OperandCollectionSlice.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Ubiquity.ArgValidators;

namespace Ubiquity.NET.Llvm
{
    internal class OperandCollectionSlice<T>
        : IOperandCollection<T>
    {
        public T this[ int index ]
        {
            get
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );
                return InnerCollection[ Offset + index ];
            }

            set
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );
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
            for( int i = Offset; i < end; ++i )
            {
                yield return InnerCollection[ i ];
            }
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal OperandCollectionSlice( IOperandCollection<T> innerCollection, Range sliceRange )
        {
            (Offset, Count) = sliceRange.GetOffsetAndLength( innerCollection.Count );
            InnerCollection = innerCollection;
        }

        private readonly int Offset;
        private readonly IOperandCollection<T> InnerCollection;
    }
}
