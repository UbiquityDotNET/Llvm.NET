// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// This isn't a normal built-in define; but follows the pattern for built-in defines and expresses the intent.
// There is no Known way to "light up" at runtime for functionality available in newer versions
#if COMPILER_5_OR_GREATER
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RepositoryVerifier
{
    /// <summary>Analyzer to detect use of `extension` keyword.</summary>
    [DiagnosticAnalyzer( LanguageNames.CSharp )]
    public class ExtensionKeywordAnalyzer
        : DiagnosticAnalyzer
    {
        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Diagnostics.ExtensionKeywordAnalyzer;

        /// <inheritdoc/>
        public override void Initialize( AnalysisContext context )
        {
            // ignore generated code
            context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction( OnExtensionKeyword, SyntaxKind.ExtensionKeyword, SyntaxKind.ExtensionBlockDeclaration );
        }

        private void OnExtensionKeyword( SyntaxNodeAnalysisContext context )
        {
            context.ReportDiagnostic( Diagnostic.Create( Diagnostics.ExtensionKeywordUsed, context.Node.GetLocation() ) );
        }
    }
}
#endif
