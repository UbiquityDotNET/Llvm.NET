// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Originally FROM: https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
// Modified to support IEquatable<T> for caching
// Additional functionality as needed to generalize it.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Cacheable storage of nested class names for use in source generation</summary>
    public sealed class NestedClassName
        : IEquatable<NestedClassName>
    {
        /// <summary>Initializes a new instance of the <see cref="NestedClassName"/> class.</summary>
        /// <param name="keyword">Keyword for this declaration</param>
        /// <param name="name">Name of the type</param>
        /// <param name="constraints">Constraints for this type</param>
        /// <param name="children">Names of any nested child types to form hiearachies</param>
        /// <remarks>
        /// <paramref name="keyword"/> is normally one of ("class", "struct", "interface", "record [class|struct]?").
        /// </remarks>
        public NestedClassName(string keyword, string name, string constraints, params IEnumerable<NestedClassName> children)
        {
            Keyword = keyword;
            Name = name;
            Constraints = constraints;
            Children = children.ToImmutableArray().AsEquatableArray();
        }

        /// <summary>Gets child nested types</summary>
        public EquatableArray<NestedClassName> Children { get; }

        /// <summary>Gets the keyword for this type</summary>
        /// <remarks>
        /// This is normally one of ("class", "struct", "interface", "record [class|struct]?"
        /// </remarks>
        public string Keyword { get; }

        /// <summary>Gets the name of the nested type</summary>
        public string Name { get; }

        /// <summary>Gets the constraints for a nested type</summary>
        public string Constraints { get; }

        /// <summary>Gets a value indicating whether this name contains constraints</summary>
        public bool HasConstraints => !string.IsNullOrWhiteSpace(Constraints);

        /// <summary>Compares this instance with another <see cref="NestedClassName"/></summary>
        /// <param name="other">Value to compare this instance with</param>
        /// <returns><see cref="true"/> if the <paramref name="other"/> is equal to this instance</returns>
        /// <remarks>
        /// This is, at worst, a recursive O(n) operation! However, since it is used for nested types
        /// the actual depth is statistically rather small and nearly always 0 (Children is empty).
        /// Deeply nested type declarations is a VERY rare anti-pattern so not a real world problem.
        /// </remarks>
        public bool Equals(NestedClassName other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // NOTE: This is a recursive O(n) operation!
            return Equals(Children, other.Children)
                && Name.Equals( other.Name, StringComparison.Ordinal )
                && Constraints.Equals( other.Constraints, StringComparison.Ordinal );
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is NestedClassName parentClass && Equals(parentClass);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Children, Keyword, Name, Constraints);
        }
    }
}
