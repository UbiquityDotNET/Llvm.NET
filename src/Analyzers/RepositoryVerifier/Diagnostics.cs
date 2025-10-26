// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace RepositoryVerifier
{
    // The prefix "UNL" used here reflects the constrained usage of this analyzer. It is
    // limited to anylizing source in the "Ubiquity.NET.Llvm" namespace only. It is intentionally
    // NOT general purpose.
    internal static class Diagnostics
    {
        private static LocalizableResourceString Localized( string resName )
        {
            return new LocalizableResourceString( resName, Resources.ResourceManager, typeof( Resources ) );
        }

        internal static ImmutableArray<DiagnosticDescriptor> ReferenceEqualityAnalyzer
            => [InternalError, RefEqualityWhenEquatable ];

        internal static ImmutableArray<DiagnosticDescriptor> ExtensionKeywordAnalyzer
            => [InternalError, ExtensionKeywordUsed];

        internal static readonly DiagnosticDescriptor InternalError = new(
            id: "UNL000",
            title: Localized(nameof(Resources.UNL000_Title)),
            messageFormat: Localized(nameof(Resources.UNL000_MessageFormat)),
            category: "Internal",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Localized(nameof(Resources.UNL000_Description)),
            WellKnownDiagnosticTags.AnalyzerException
            );

        internal static readonly DiagnosticDescriptor RefEqualityWhenEquatable = new(
            id: "UNL001",
            title: Localized(nameof(Resources.UNL001_Title)),
            messageFormat: Localized(nameof(Resources.UNL001_MessageFormat)),
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Localized(nameof(Resources.UNL001_Description)),
            WellKnownDiagnosticTags.Compiler
            );

        internal static readonly DiagnosticDescriptor ExtensionKeywordUsed = new(
            id: "UNL002",
            title: Localized(nameof(Resources.UNL002_Title)),
            messageFormat: Localized(nameof(Resources.UNL002_MessageFormat)),
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Localized(nameof(Resources.UNL002_Description)),
            WellKnownDiagnosticTags.Compiler
            );
    }
}
