﻿// based on blog code (but heavily modified to use built-in MSTest and other enhancements/updates):
// https://andrewlock.net/creating-a-source-generator-part-10-testing-your-incremental-generator-pipeline-outputs-are-cacheable/

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace SourceGenerator.Test.Utils
{
    /// <summary>Static class with extensions/helpers for testing incremental source generators</summary>
    public static class GeneratorDriverRunResultExtensions
    {
        /// <summary>Extension method for a <see cref="GeneratorDriverRunResult"/> to Get all of the tracked steps matching a name contained in the input</summary>
        /// <param name="runResult">Results to get steps from</param>
        /// <param name="trackingNames">Set of names to get from <paramref name="runResult"/></param>
        /// <returns>Dictionary to map the name to any run steps for that name</returns>
        [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Significantly less readable if applied")]
        public static ImmutableDictionary<string, ImmutableArray<IncrementalGeneratorRunStep>> GetTrackedSteps(
            this GeneratorDriverRunResult runResult,
            ImmutableArray<string> trackingNames
            )
        {
            if(runResult is null)
            {
                throw new ArgumentNullException( nameof( runResult ) );
            }

            if (trackingNames.Length == 0)
            {
                return ImmutableDictionary<string, ImmutableArray<IncrementalGeneratorRunStep>>.Empty;
            }

            return runResult.Results[0]
                            .TrackedSteps
                            .Where(step => trackingNames.Contains(step.Key))
                            .ToImmutableDictionary(v => v.Key, v => v.Value);
        }
    }
}
