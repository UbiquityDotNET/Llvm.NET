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
    /// This treats the operands of a tuple as the elements of the array
    /// </remarks>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Collection doesn't make sense for this type" )]
    public class TupleTypedArrayWrapper<T>
        : IReadOnlyList<T>
        where T : LlvmMetadata
    {
        public TupleTypedArrayWrapper( MDTuple tuple )
        {
            Tuple = tuple;
        }

        public MDTuple Tuple { get; }

        public int Count => Tuple.Operands.Count;

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

                return Tuple.Operands[ index ].Metadata as T;
            }
        }

        public IEnumerator<T> GetEnumerator( )
        {
            return Tuple.Operands
                        .Select( n => n.Metadata as T )
                        .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();
    }
}
