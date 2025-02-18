using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace SourceGenerator.Utils
{
    /// <summary>Container for a result or an error descriptor (<see cref="DiagnosticInfo"/>)</summary>
    /// <typeparam name="T">Value contained in the result [Constrained to <see cref="IEquatable{T}"/>]</typeparam>
    /// <remarks>
    /// <note type="warning">
    /// It is debatable if an incremental generator should produce diagnostics. The official
    /// <see href="https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md">cookbook</see>
    /// recommends against it [Section: "Issue diagnostics"]. Instead it recommends use of an analyzer. While it
    /// doesn't say what a generator is supposed to do in the event of an input error explicitly, it is implied
    /// that it should silently ignore them. For the moment this is generating diagnostics in the final source
    /// production stage of the pipeline by plumbing through all of the available data AND diagnostics via this
    /// generic record. This acts as a sort of discriminated union of results or diagnostics (or possibly a combination
    /// of both). All while maintaining support for caching.
    /// </note>
    /// </remarks>
    [Obsolete("Use T directly and a predicate that filters out erroneous nodes [silently ignoring them]; generate diagnostics from an analyzer instead")]
    public readonly record struct Result<T>
        where T : IEquatable<T>
    {
        /// <summary>Initializes an instance of <see cref="Result{T}"/> from a value [No diagnostics]</summary>
        /// <param name="value"></param>
        public Result(T value)
            : this(value, [])
        {
        }

        public Result(params IEnumerable<DiagnosticInfo> diagnostics)
            : this(default, diagnostics.ToImmutableArray())
        {
        }

        /// <summary>Initializes an instance of <see cref="Result{T}"/> from an array of error descriptions</summary>
        /// <param name="diagnostics">Diagnostics that prevent production of a value</param>
        public Result(ImmutableArray<DiagnosticInfo> diagnostics)
            : this(default, diagnostics)
        {
        }

        /// <summary>Initializes an instance of <see cref="Result{T}"/> from a nullable value and set of potentially empty diagnostics</summary>
        /// <param name="value">Value of the result (may be null to indicate no results)</param>
        /// <param name="diagnostics">Array of <see cref="DiagnosticInfo"/> to describe any diagnostics/warnings encountered while producing <paramref name="value"/></param>
        /// <remarks>
        /// This is the most generalized from of constructor. It supports BOTH a value and diagnostics as it is possible that
        /// a value is producible, but there are warnings or other informative diagnostics to include with it. Attempts to construct
        /// a result with no value and no diagnostics generate an exception.
        /// </remarks>
        /// <exception cref="ArgumentException">Both <paramref name="value"/> and <see cref="diagnostics"/> are <see langword="null"> or empty</exception>
        public Result(T? value, ImmutableArray<DiagnosticInfo> diagnostics)
        {
            if (value is null && diagnostics.IsDefaultOrEmpty)
            {
                throw new ArgumentException($"Either {nameof(Value)} or {nameof(diagnostics)} must contain a value");
            }

            Value = value;
            Diagnostics = diagnostics;
        }

        /// <summary>Gets the value produced for this result or <see langword="null"/> if no value produced</summary>
        public T? Value { get; init; } = default;

        /// <summary>Gets a value indicating whether a value was produced for this result</summary>
        public bool HasValue => Value is not null;

        /// <summary>Gets the diagnostics produced for this result (if any)</summary>
        /// <remarks>This may provide an empty array but is never <see langword="null"/></remarks>
        public EquatableArray<DiagnosticInfo> Diagnostics { get; init; } = ImmutableArray<DiagnosticInfo>.Empty;

        /// <summary>Gets a value indicating if this result contains any diagnostics</summary>
        /// <remarks>This is a shorthand for testing the length of the <see cref="Diagnostics"/> property</remarks>
        public bool HasDiagnostics => !Diagnostics.IsEmpty;

        /// <summary>Report all diagnostics to the provided <paramref name="ctx"/></summary>
        /// <param name="ctx"><see cref="SourceProductionContext"/> to report the diagnostics to</param>
        public void ReportDiagnostics(SourceProductionContext ctx)
        {
            foreach (var di in Diagnostics)
            {
                ctx.ReportDiagnostic(di.CreateDiagnostic());
            }
        }
    }
}
