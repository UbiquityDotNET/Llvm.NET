// -----------------------------------------------------------------------
// <copyright file="TupleTypedArrayWrapper.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Properties;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Generic wrapper to treat an MDTuple as an array of elements of specific type</summary>
    /// <typeparam name="T">Type of elements</typeparam>
    /// <remarks>
    /// This implements a facade pattern that presents an <see cref="System.Collections.Generic.IReadOnlyCollection{T}"/> for the
    /// operands of an <see cref="MDTuple"/>. This allows treating the tuple like an array of nodes of a
    /// particular type.
    /// </remarks>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Collection doesn't make sense for this type" )]
    public class TupleTypedArrayWrapper<T>
        : IReadOnlyList<T?>
        where T : LlvmMetadata
    {
        /// <summary>Gets the underlying tuple for this wrapper</summary>
        public MDTuple? Tuple { get; }

        /// <inheritdoc />
        public int Count => Tuple?.Operands.Count ?? 0;

        /// <inheritdoc />
        public T? this[ int index ]
        {
            get
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );
                return Tuple!.Operands.GetOperand<T>( index );
            }
        }

        /// <inheritdoc />
        public IEnumerator<T?> GetEnumerator( )
        {
            return Tuple is null
                ? Enumerable.Empty<T>( ).GetEnumerator( )
                : Tuple.Operands
                        .Cast<T>( )
                        .GetEnumerator( );
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal TupleTypedArrayWrapper( MDTuple? tuple )
        {
            Tuple = tuple;
        }
    }
}
