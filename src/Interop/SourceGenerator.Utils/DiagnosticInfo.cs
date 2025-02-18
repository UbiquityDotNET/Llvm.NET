using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

// Modified from idea in blog post: https://andrewlock.net/creating-a-source-generator-part-9-avoiding-performance-pitfalls-in-incremental-generators/

namespace SourceGenerator.Utils
{
    // Enhanced from original to support message parameters and create the final diagnostic when needed (lazy).
    [Obsolete("Source generators should NOT produce diagnostics; use an analyzer for that and filter erroneous nodes to effectively ignore them")]
    public sealed record DiagnosticInfo
    {
        public DiagnosticInfo(DiagnosticDescriptor descriptor, Location? location, params IEnumerable<string> msgArgs)
        {
            Descriptor = descriptor;
            Location = location;
            Params = msgArgs.ToImmutableArray();
        }

        public EquatableArray<string> Params { get; }

        public DiagnosticDescriptor Descriptor { get; }

        // Location is an abstract type but all derived types implement IEquatable<T> where T is Location
        // Thus a location is equatable even though the base abstract type doesn't implement that interface.
        public Location? Location { get; }

        public Diagnostic CreateDiagnostic()
        {
            return Diagnostic.Create(Descriptor, Location, Params.ToArray());
        }
    }
}
