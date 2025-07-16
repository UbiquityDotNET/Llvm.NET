// -----------------------------------------------------------------------
// <copyright file="Diagnostics.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace ReferenceEqualityVerifier
{
    // The prefix "UNL" used here reflects the constrained usage of this analyzer. It is
    // limited to testing types in the "Ubiquity.NET.Llvm" namespace only. It is intentionally
    // NOT general purpose.
    internal static class Diagnostics
    {
        private static LocalizableResourceString Localized( string resName )
        {
            return new LocalizableResourceString( resName, Resources.ResourceManager, typeof( Resources ) );
        }

        internal static ImmutableArray<DiagnosticDescriptor> AllDiagnostics
            => ImmutableArray.Create( RefEqualityWhenEquatable );

        internal static readonly DiagnosticDescriptor RefEqualityInternalError = new DiagnosticDescriptor(
            id: "UNL000",
            title: Localized(nameof(Resources.UNL000_Title)),
            messageFormat: Localized(nameof(Resources.UNL000_MessageFormat)),
            category: "Internal",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Localized(nameof(Resources.UNL000_Description)),
            WellKnownDiagnosticTags.AnalyzerException
            );

        internal static readonly DiagnosticDescriptor RefEqualityWhenEquatable = new DiagnosticDescriptor(
            id: "UNL001",
            title: Localized(nameof(Resources.UNL001_Title)),
            messageFormat: Localized(nameof(Resources.UNL001_MessageFormat)),
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Localized(nameof(Resources.UNL001_Description)),
            WellKnownDiagnosticTags.Compiler
            );
    }
}
