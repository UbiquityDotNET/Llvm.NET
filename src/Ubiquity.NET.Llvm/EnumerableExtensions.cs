// -----------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NET_COREAPP
using System;
using System.Collections.Generic;

namespace Ubiquity.NET.Llvm
{
    /// <summary>This is an internal duplicate of Extensions added to CoreFx.</summary>
    /// <remarks>
    /// This is duped here to enable use in down-level run-times. Furthermore, it uses a different
    /// name and is marked internal to prevent conflicts with the official implementation when
    /// built for run-times supporting that. (See: https://github.com/dotnet/corefx/pull/5947)
    /// </remarks>
    internal static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Append<TSource>( this IEnumerable<TSource> source, TSource element )
        {
            if( source == null )
            {
                throw new ArgumentNullException( nameof( source ) );
            }

            return AppendIterator( source, element );
        }

        public static IEnumerable<TSource> Prepend<TSource>( this IEnumerable<TSource> source, TSource element )
        {
            if( source == null )
            {
                throw new ArgumentNullException( nameof( source ) );
            }

            return PrependIterator( source, element );
        }

        private static IEnumerable<TSource> AppendIterator<TSource>( IEnumerable<TSource> source, TSource element )
        {
            foreach( TSource e1 in source )
            {
                yield return e1;
            }

            yield return element;
        }

        private static IEnumerable<TSource> PrependIterator<TSource>( IEnumerable<TSource> source, TSource element )
        {
            yield return element;

            foreach( TSource e1 in source )
            {
                yield return e1;
            }
        }
    }
}
#endif
