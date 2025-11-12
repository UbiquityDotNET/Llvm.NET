// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Ubiquity.NET.SourceGenerator.Test.Utils.CSharp
{
    /// <summary>Source generator tests for C# that allows specification of the language</summary>
    /// <typeparam name="TSourceGenerator">Source generator type</typeparam>
    /// <typeparam name="TVerifier">Verifier type</typeparam>
    public class SourceGeneratorTest<TSourceGenerator, TVerifier>
        : Microsoft.CodeAnalysis.CSharp.Testing.CSharpSourceGeneratorTest<TSourceGenerator, TVerifier>
        where TSourceGenerator : new()
        where TVerifier : IVerifier, new()
    {
        /// <summary>Initializes a new instance of the <see cref="SourceGeneratorTest{TSourceGenerator, TVerifier}"/> class.</summary>
        /// <param name="ver">Version of the language for this test</param>
        public SourceGeneratorTest(LanguageVersion ver)
        {
            LanguageVersion = ver;
        }

        /// <inheritdoc/>
        /// <remarks>Creates parse options</remarks>
        protected override ParseOptions CreateParseOptions( )
        {
            // TODO: until C# 14 is formally released, this is "preview"
            return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
        }

        private readonly LanguageVersion LanguageVersion;
    }
}
