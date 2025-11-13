// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ubiquity.NET.SourceGenerator.Test.Utils
{
    /// <summary>Comparer to test an array (element by element) for equality</summary>
    public class EnumerableObjectComparer
        : IEqualityComparer<IEnumerable<object>>
    {
        /// <inheritdoc/>
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "xValues and yValues are not hungarioan names" )]
        public bool Equals(IEnumerable<object>? x, IEnumerable<object>? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);
            var xValues = x.ToImmutableArray();
            var yValues = x.ToImmutableArray();
            return xValues.Length == yValues.Length
                && xValues.Zip(yValues, (a, b) => a.Equals(b)).All(x => x);
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] IEnumerable<object> obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>Default constructed comparer.</summary>
        public static readonly EnumerableObjectComparer Default = new();
    }
}
