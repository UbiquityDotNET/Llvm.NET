// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// due to bug in Nuget publication, this can't be tested yet...
// see: https://developercommunity.visualstudio.com/t/VS2026--NET-10-Cant-build-analyzier/10989212
#if NET10_0_OR_GREATER
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Microsoft.CodeAnalysis.Testing.ReferenceAssemblies;

namespace RepositoryVerifier.UT
{
    [TestClass]
    public class ExtensionsKeywordAnalyzerTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task EmptySourceAnalyzesClean( )
        {
            var analyzerTest = CreateTestRunner(string.Empty);
            await analyzerTest.RunAsync( TestContext.CancellationTokenSource.Token );
        }

        [TestMethod]
        public async Task UsingKeywordReportsDiagnostic( )
        {
            var analyzerTest = CreateTestRunner(ExtensionKeywordUsed);
            analyzerTest.ExpectedDiagnostics.AddRange(
                [
                    new DiagnosticResult("UNL002", DiagnosticSeverity.Error).WithLocation(5, 5)
                ]
            );

            await analyzerTest.RunAsync( TestContext.CancellationTokenSource.Token );
        }

        [TestMethod]
        public async Task StructEquatableIsNotReferenceEquality( )
        {
            var analyzerTest = CreateTestRunner(NoExtensionKeywordUsed);

            // no diagnostics expected
            await analyzerTest.RunAsync( TestContext.CancellationTokenSource.Token );
        }

        private static AnalyzerTest<DefaultVerifier> CreateTestRunner( string source )
        {
            return new ExtensionKeywordAnalyzerTest()
            {
                TestState =
                {
                    Sources = { source },
                    ReferenceAssemblies = Net.Net80
                }
            };
        }

       private class ExtensionKeywordAnalyzerTest
            : CSharpAnalyzerTest<ExtensionKeywordAnalyzer, DefaultVerifier>
        {
            protected override ParseOptions CreateParseOptions( )
            {
                // Until C# 14 and .NET SDK 10 is formally released, it is considered "Preview"
                return new CSharpParseOptions(LanguageVersion.Preview, DocumentationMode.Diagnose);
            }
        }

       private const string ExtensionKeywordUsed = """
        using System;

        public static class TestExtension
        {
            extension(string self)
            {
                public string MyExtension()
                {
                    return self;
                }
            }
        }
        """;

       private const string NoExtensionKeywordUsed = """
        using System;

        public static class TestExtension
        {
            public static string MyExtension(this string self)
            {
                return self;
            }
        }
        """;
    }
}
#endif
