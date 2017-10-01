// despite claims from MS to the contrary net47 isn't fully netstandard2.0 compliant
// in particular it does not provide the Append or Prepend Enumerable extension methods
// see: https://developercommunity.visualstudio.com/content/problem/123356/enumerableappend-extension-method-is-missing-in-ne.html
#if NET47
using System;
using System.Collections.Generic;

namespace Llvm.NET
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
