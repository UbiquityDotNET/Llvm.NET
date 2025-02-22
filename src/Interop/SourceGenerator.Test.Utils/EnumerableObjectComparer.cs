using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SourceGenerator.Test.Utils
{
    public class EnumerableObjectComparer
        : IEqualityComparer<IEnumerable<object>>
    {
        public bool Equals(IEnumerable<object>? x, IEnumerable<object>? y)
        {
            ArgumentNullException.ThrowIfNull( x );
            ArgumentNullException.ThrowIfNull( y );

            var xValues = x.ToImmutableArray();
            var yValues = x.ToImmutableArray();
            return xValues.Length == yValues.Length
                && xValues.Zip(yValues, (a, b) => a.Equals(b)).All(x => x);
        }

        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "NOT supported; don't call this")]
        public int GetHashCode(/*[DisallowNull]*/ IEnumerable<object> obj)
        {
            throw new NotSupportedException();
        }

        public static readonly EnumerableObjectComparer Default = new();
    }
}
