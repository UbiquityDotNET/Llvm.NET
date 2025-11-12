// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.SourceGenerator.Test.Utils
{
    /// <summary>Utility class for <see cref="GeneratorDriver"/> to add extensions for testing</summary>
    public static class GeneratorDriverExtensions
    {
        /// <summary>Runs a source generator twice and validates the results</summary>
        /// <param name="driver">Driver to use for the run</param>
        /// <param name="compilation">Compilation to use for the run</param>
        /// <param name="trackingNames">Array of names to filter all of the internal tracking names</param>
        /// <returns>Results of first run</returns>
        /// <remarks>
        /// This will run the generator twice. The value of results are tested for
        /// any banned types and are additionally tested to ensure the expected
        /// tracking names are found and only use cached results on the second run.
        /// </remarks>
        public static GeneratorDriverRunResult RunGeneratorAndAssertResults(
            this GeneratorDriver driver,
            CSharpCompilation compilation,
            ImmutableArray<string> trackingNames
            )
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(trackingNames.Length, 1, nameof(trackingNames));

            var compilationClone = compilation.Clone();

            // save the resulting immutable driver for use in second run.
            driver = driver.RunGenerators(compilation);
            GeneratorDriverRunResult runResult1 = driver.GetRunResult();
            GeneratorDriverRunResult runResult2 = driver.RunGenerators(compilationClone)
                                                        .GetRunResult();
            Assert.That.AreEqual(runResult1, runResult2, trackingNames);
            Assert.That.Cached(runResult2);
            return runResult1;
        }
    }
}
