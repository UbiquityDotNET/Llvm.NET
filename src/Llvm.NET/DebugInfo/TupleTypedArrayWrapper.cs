// <copyright file="TupleTypedArrayWrapper.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Generic wrapper to treat an MDTuple as an array of elements of specific type</summary>
    /// <typeparam name="T">Type of elements</typeparam>
    /// <remarks>
    /// This implements a facade pattern that presents an <see cref="IReadOnlyCollection{T}"/> for the
    /// operands of an <see cref="MDTuple"/>. This allows treating the tuple like an array of nodes of a
    /// particular type.
    /// </remarks>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Collection doesn't make sense for this type" )]
    public class TupleTypedArrayWrapper<T>
        : IReadOnlyList<T>
        where T : LlvmMetadata
    {
        /// <summary>Gets the underlying tuple for this wrapper</summary>
        public MDTuple Tuple { get; }

        /// <inheritdoc />
        public int Count => Tuple.Operands.Count;

        /// <inheritdoc />
        public T this[ int index ]
        {
            get
            {
                if( Tuple == null )
                {
                    throw new InvalidOperationException( "Wrapped node is null" );
                }

                if( index > Tuple.Operands.Count )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                return Tuple.Operands[ index ] as T;
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator( )
        {
            return Tuple.Operands
                        .Cast<T>()
                        .GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();

        internal TupleTypedArrayWrapper( MDTuple tuple )
        {
            Tuple = tuple;
        }
    }
}
