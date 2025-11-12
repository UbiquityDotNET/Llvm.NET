// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Utility class to provide extensions for a <see cref="SourceProductionContext"/></summary>
    public static class SourceProductionContextExtensions
    {
        /// <summary>Reports deferred diagnostics for a <see cref="Result{T}"/></summary>
        /// <typeparam name="T">Type of the result values</typeparam>
        /// <param name="self">The context to report any diagnostics to</param>
        /// <param name="result">The result</param>
        /// <remarks>
        /// if <paramref name="result"/> has no diagnostics associated with it this is a NOP.
        /// </remarks>
        public static void ReportDiagnostic<T>(this SourceProductionContext self, Result<T> result)
            where T : IEquatable<T>
        {
            if(result.HasDiagnostics)
            {
                for(int i = 0; i < result.Diagnostics.Length; ++i)
                {
                    self.ReportDiagnostic(result.Diagnostics[i]);
                }
            }
        }

        /// <summary>Reports a diagnostic to <paramref name="self"/></summary>
        /// <param name="self">The context to report the diagnostic to</param>
        /// <param name="info">Cached info to report</param>
        public static void ReportDiagnostic( this SourceProductionContext self, DiagnosticInfo info)
        {
            self.ReportDiagnostic(info.CreateDiagnostic());
        }

        /// <summary>Reports a diagnostic to <paramref name="self"/></summary>
        /// <param name="self">The context to report the diagnostic to</param>
        /// <param name="descriptor">Descriptor of the diagnostic</param>
        /// <param name="messageArgs">Message arguments</param>
        public static void ReportDiagnostic( this SourceProductionContext self, DiagnosticDescriptor descriptor, params object[] messageArgs)
        {
            self.ReportDiagnostic(Diagnostic.Create(descriptor, null, messageArgs));
        }

        /// <summary>Reports a diagnostic to <paramref name="self"/></summary>
        /// <param name="self">The context to report the diagnostic to</param>
        /// <param name="location">Location of the source of this diagnostic</param>
        /// <param name="descriptor">Descriptor for the diagnostic</param>
        /// <param name="messageArgs">Argumnets, if any, for the diagnostic message</param>
        public static void ReportDiagnostic(
            this SourceProductionContext self,
            Location location,
            DiagnosticDescriptor descriptor,
            params object[] messageArgs
            )
        {
            self.ReportDiagnostic(Diagnostic.Create(descriptor, location, messageArgs));
        }

        /// <summary>Reports a diagnostic to <paramref name="self"/></summary>
        /// <param name="self">The context to report the diagnostic to</param>
        /// <param name="node">Node as the source of the diagnostic</param>
        /// <param name="descriptor">Descriptor for the diagnostic</param>
        /// <param name="messageArgs">Argumnets, if any, for the diagnostic message</param>
        public static void ReportDiagnostic(
            this SourceProductionContext self,
            CSharpSyntaxNode node,
            DiagnosticDescriptor descriptor,
            params object[] messageArgs
            )
        {
            self.ReportDiagnostic(node.GetLocation(), descriptor, messageArgs);
        }

        /// <summary>Report diagnostics for results to <paramref name="self"/></summary>
        /// <typeparam name="T">Type of the result</typeparam>
        /// <param name="self">The context to report the diagnostic to</param>
        /// <param name="results">Array of <see cref="Result{T}"/> for the results</param>
        public static void ReportDiagnostics<T>( this SourceProductionContext self, ImmutableArray<Result<T>> results)
            where T : IEquatable<T>
        {
            for(int i = 0; i < results.Length; ++i)
            {
                self.ReportDiagnostic(results[i]);
            }
        }
    }
}
