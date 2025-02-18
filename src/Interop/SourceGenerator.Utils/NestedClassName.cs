// Originally FROM: https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
// Modified to support IEquatable<T> for caching
// Additional functionality as needed to generalize it.

using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.Utils
{

    // CONSIDER: why isn't this a readonly record struct?

    /// <summary>Cacheable storage of nested class names for use in source generation</summary>
    public class NestedClassName
        : IEquatable<NestedClassName>
    {
        public NestedClassName(string keyword, string name, string constraints, NestedClassName? child)
        {
            Keyword = keyword;
            Name = name;
            Constraints = constraints;
            Child = child;
        }

        // This is NOT a derivation hierarchy, but a symbolic naming one
        public IEnumerable<NestedClassName> Hierarchy
        {
            get
            {
                yield return this;
                for (var current = this; current.Child != null; current = current.Child)
                {
                    yield return current;
                }
            }
        }

        public NestedClassName? Child { get; }

        /// <summary>"class", "struct", "interface", "record"</summary>
        public string Keyword { get; }

        public string Name { get; }

        public string Constraints { get; }

        public bool HasConstraints => !string.IsNullOrWhiteSpace(Constraints);

        public IEnumerable<string> Names => Hierarchy.Select(h => h.Name);

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
            //       However, since it is used for nested types the actual
            //       depth is statistically rather small and nearly always
            //       0 (Child is null)
            return Equals(Child, other.Child)
                && Name.Equals(other.Name)
                && Constraints.Equals(other.Constraints);
        }

        public override bool Equals(object obj)
        {
            return obj is NestedClassName parentClass && Equals(parentClass);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Child, Keyword, Name, Constraints);
        }
    }
}
