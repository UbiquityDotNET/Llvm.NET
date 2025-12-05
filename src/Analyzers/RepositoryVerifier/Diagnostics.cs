// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace Ubiquity.NET.Llvm.Analyzer
{
    // The prefix "UNL" used here reflects the constrained usage of this analyzer. It is
    // limited to analyzing source in the "Ubiquity.NET.Llvm" namespace only. It is intentionally
    // NOT general purpose.
    internal static class Diagnostics
    {
        internal static class IDs
        {
            internal const string InternalError = "UNL000";
            internal const string RefEqualityWhenEquatable = "UNL001";
        }

        private static LocalizableResourceString Localized( string resName )
        {
            return new LocalizableResourceString( resName, Resources.ResourceManager, typeof( Resources ) );
        }

        internal static ImmutableArray<DiagnosticDescriptor> ReferenceEqualityAnalyzer
            => [InternalError, RefEqualityWhenEquatable ];

        internal static readonly DiagnosticDescriptor InternalError = new(
            id: IDs.InternalError,
            title: Localized(nameof(Resources.InternalError_Title)),
            messageFormat: Localized(nameof(Resources.InternalError_MessageFormat)),
            category: "Internal",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Localized(nameof(Resources.InternalError_Description)),
            WellKnownDiagnosticTags.NotConfigurable
            );

        internal static readonly DiagnosticDescriptor RefEqualityWhenEquatable = new(
            id: IDs.RefEqualityWhenEquatable,
            title: Localized(nameof(Resources.RefEqualityWhenEquatable_Title)),
            messageFormat: Localized(nameof(Resources.RefEqualityWhenEquatable_MessageFormat)),
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Localized(nameof(Resources.RefEqualityWhenEquatable_Description)),
            WellKnownDiagnosticTags.NotConfigurable
            );
    }
}
