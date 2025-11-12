// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Modified from idea in blog post: https://andrewlock.net/creating-a-source-generator-part-9-avoiding-performance-pitfalls-in-incremental-generators/

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Contains diagnostic information collected for reporting to the host</summary>
    /// <remarks>
    /// This is an equatable type and therefore is legit for use in generators/analyzers where
    /// that is needed for caching. A <see cref="Diagnostic"/> is not, so this record bundles
    /// the parameters needed for creation of one and defers the construction until needed.
    /// </remarks>
    public sealed record DiagnosticInfo
    {
        /// <summary>Initializes a new instance of the <see cref="DiagnosticInfo"/> class.</summary>
        /// <param name="descriptor">Descriptor for the diagnostic</param>
        /// <param name="location">Location in the source file that triggered this diagnostic</param>
        /// <param name="msgArgs">Args for the message</param>
        public DiagnosticInfo(DiagnosticDescriptor descriptor, Location? location, params IEnumerable<string> msgArgs)
        {
            Descriptor = descriptor;
            Location = location;
            Params = msgArgs.ToImmutableArray();
        }

        /// <summary>Gets the parameters for this diagnostic</summary>
        public EquatableArray<string> Params { get; }

        /// <summary>Gets the descriptor for this diagnostic</summary>
        public DiagnosticDescriptor Descriptor { get; }

        // Location is an abstract type but all derived types implement IEquatable<T> where T is Location
        // Thus a location is equatable even though the base abstract type doesn't implement that interface.

        /// <summary>Gets the location of the source of this diagnostic</summary>
        public Location? Location { get; }

        /// <summary>Factory to create a <see cref="Diagnostic"/> from the information contained in this holder</summary>
        /// <returns><see cref="Diagnostic"/> that represents this information</returns>
        public Diagnostic CreateDiagnostic()
        {
            return Diagnostic.Create(Descriptor, Location, Params.ToArray());
        }
    }
}
