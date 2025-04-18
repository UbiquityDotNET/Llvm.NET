using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace ReferenceEqualityVerifier
{
    internal static class Diagnostics
    {
        internal static ImmutableArray<DiagnosticDescriptor> AllDiagnostics
            => ImmutableArray.Create( RefEqualityWhenEquatable );

        internal static readonly DiagnosticDescriptor RefEqualityWhenEquatable = new DiagnosticDescriptor(
            id: "REFQ001",
            title: new LocalizableResourceString(nameof(Resources.REFQ001_Title), Resources.ResourceManager, typeof(Resources)),
            messageFormat: new LocalizableResourceString(nameof(Resources.REFQ001_MessageFormat), Resources.ResourceManager, typeof(Resources)),
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: new LocalizableResourceString(nameof(Resources.REFQ001_Description), Resources.ResourceManager, typeof(Resources)),
            WellKnownDiagnosticTags.Compiler
            );
    }
}
