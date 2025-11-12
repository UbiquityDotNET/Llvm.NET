// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Container for a result or an error descriptor (<see cref="DiagnosticInfo"/>)</summary>
    /// <typeparam name="T">Value contained in the result [Constrained to <see cref="IEquatable{T}"/>]</typeparam>
    /// <remarks>
    /// <note type="warning">
    /// <para>It is debatable if an incremental generator should produce diagnostics. The official
    /// <see href="https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md">cookbook</see>
    /// recommends against it [Section: "Issue diagnostics"]. Instead it recommends use of an analyzer. While it
    /// doesn't say what a generator is supposed to do in the event of an input error explicitly, it is implied
    /// that it should silently ignore them.</para>
    ///
    /// <para>This type allows generating diagnostics in the final source production stage of the pipeline by plumbing
    /// through all of the available data AND diagnostics via this generic record. This acts as a sort of
    /// discriminated union of results or diagnostics (or possibly a combination of both). All while maintaining
    /// support for caching with <see cref="IEquatable{T}"/>.</para>
    /// </note>
    /// </remarks>
    public readonly record struct Result<T>
        where T : IEquatable<T>
    {
        /// <summary>Initializes a new instance of the <see cref="Result{T}"/> struct from a value [No diagnostics]</summary>
        /// <param name="value">Value of the result</param>
        public Result(T value)
            : this(value, [])
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Result{T}"/> struct from diagnostics</summary>
        /// <param name="diagnostics">Information describing the diagnostics for this result</param>
        public Result(params IEnumerable<DiagnosticInfo> diagnostics)
            : this(default, [.. diagnostics])
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Result{T}"/> struct from a nullable value and set of potentially empty diagnostics</summary>
        /// <param name="value">Value of the result (may be null to indicate no results)</param>
        /// <param name="diagnostics">Array of <see cref="DiagnosticInfo"/> to describe any diagnostics/warnings encountered while producing <paramref name="value"/></param>
        /// <remarks>
        /// This is the most generalized from of constructor. It supports BOTH a value and diagnostics as it is possible that
        /// a value is producible, but there are warnings or other informative diagnostics to include with it. Attempts to construct
        /// a result with no value and no diagnostics throws an exception.
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

        /// <summary>Gets a value indicating whether this result contains any diagnostics</summary>
        /// <remarks>This is a shorthand for testing the length of the <see cref="Diagnostics"/> property</remarks>
        public bool HasDiagnostics => !Diagnostics.IsEmpty;

        /// <summary>Report all diagnostics to the provided <paramref name="ctx"/></summary>
        /// <param name="ctx"><see cref="SourceProductionContext"/> to report the diagnostics to</param>
        /// <remarks>
        /// This supports the deferral of reporting with a collection of cahceable <see cref="Result{T}"/>. This allows
        /// for a generatr to report critical internal problems.
        /// </remarks>
        public void ReportDiagnostics(SourceProductionContext ctx)
        {
            foreach (var di in Diagnostics)
            {
                ctx.ReportDiagnostic(di.CreateDiagnostic());
            }
        }
    }
}
