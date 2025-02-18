using System;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenerator.Utils
{
    public static class SourceProductionContextExtensions
    {
        public static void ReportDiagnostic(this SourceProductionContext ctx, DiagnosticDescriptor descriptor, params object[] messageArgs)
        {
            ctx.ReportDiagnostic(Diagnostic.Create(descriptor, null, messageArgs));
        }

        public static void ReportDiagnostic(this SourceProductionContext ctx, Location location, DiagnosticDescriptor descriptor, params object[] messageArgs)
        {
            ctx.ReportDiagnostic(Diagnostic.Create(descriptor, location, messageArgs));
        }

        public static void ReportDiagnostic(this SourceProductionContext ctx, CSharpSyntaxNode node, DiagnosticDescriptor descriptor, params object[] messageArgs)
        {
            ctx.ReportDiagnostic(node.GetLocation(), descriptor, messageArgs);
        }

        [Obsolete("Report diagnostics from an analyzer; Generators should use node predicates to eliminate erroneous nodes [silently ignoring them]")]
        public static bool ReportDiagnostics<T>(this SourceProductionContext ctx, ImmutableArray<Result<T>> results)
            where T : IEquatable<T>
        {
            var resultsWithDiagnostics = from r in results
                                         where r.HasDiagnostics
                                         select r;

            bool hasErrors = false;
            foreach (var r in resultsWithDiagnostics)
            {
                r.ReportDiagnostics(ctx);
                hasErrors = true;
            }

            return hasErrors;
        }
    }
}