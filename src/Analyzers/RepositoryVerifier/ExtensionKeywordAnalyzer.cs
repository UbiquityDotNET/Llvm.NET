// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
            context.RegisterSyntaxNodeAction( OnExtensionKeyword, SyntaxKind.ExtensionKeyword, SyntaxKind.ExtensionDeclaration );
        }

        private void OnExtensionKeyword( SyntaxNodeAnalysisContext context )
        {
            context.ReportDiagnostic( Diagnostic.Create( Diagnostics.ExtensionKeywordUsed, context.Node.GetLocation() ) );
        }
    }
}
