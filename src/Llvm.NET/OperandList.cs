// -----------------------------------------------------------------------
// <copyright file="OperandList.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiquity.ArgValidators;

namespace Llvm.NET
{
    /// <summary>Support class to provide read/update semantics to the operands of a container element</summary>
    /// <typeparam name="T">Element type of an operand</typeparam>
    /// <remarks>
    /// This class is used to implement Operand lists of elements including sub lists based on an offset.
    /// The latter case is useful for types that expose some fixed set of operands as properties and some
    /// arbitrary number of additional items as operands.
    /// </remarks>
    internal class OperandList<T>
        : IList<T>
        where T : class
    {
        /// <inheritdoc/>
        public T this[ int index ]
        {
            get
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );
                return Container[ index + Offset ];
            }

            set
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );
                Container[ index + Offset ] = value;
            }
        }

        /// <inheritdoc/>
        public int Count => ( int )Math.Min( Container.Count - Offset, int.MaxValue );

        /// <inheritdoc/>
        public bool IsReadOnly { get; }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator( )
        {
            for( int i = 0; i < Count; ++i )
            {
                var element = Container[ i + Offset ];
                if( element == null )
                {
                    yield break;
                }

                yield return element;
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        /// <inheritdoc/>
        public int IndexOf( T item )
        {
            for( int i = 0; i < Count; ++i )
            {
                if( this[ i ] == item )
                {
                    return i;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public bool Contains( T item ) => this.Any( n => n == item );

        /// <inheritdoc/>
        public void CopyTo( T[ ] array, int arrayIndex )
        {
            arrayIndex.ValidateRange( 0, array.Length - Count, nameof( arrayIndex ) );
            for( int i = 0; i < Count; ++i )
            {
                array[ i + arrayIndex ] = this[ i ];
            }
        }

        /// <inheritdoc/>
        public void Insert( int index, T item )
        {
            throw new NotSupportedException( );
        }

        /// <inheritdoc/>
        public void RemoveAt( int index )
        {
            throw new NotSupportedException( );
        }

        /// <inheritdoc/>
        public void Add( T item )
        {
            Container.Add( item );
        }

        /// <inheritdoc/>
        public void Clear( )
        {
            throw new NotSupportedException( );
        }

        /// <inheritdoc/>
        public bool Remove( T item )
        {
            throw new NotSupportedException( );
        }

        internal OperandList( IOperandContainer<T> owningNode, bool isReadOnly = false )
            : this( owningNode, 0, isReadOnly )
        {
        }

        internal OperandList( IOperandContainer<T> container, int offset, bool isReadOnly = false )
        {
            Offset = offset;
            Container = container;
            IsReadOnly = isReadOnly;
        }

        private readonly int Offset;
        private readonly IOperandContainer<T> Container;
    }
}
