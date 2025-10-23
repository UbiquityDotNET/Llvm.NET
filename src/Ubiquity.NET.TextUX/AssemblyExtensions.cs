// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Ubiquity.NET.TextUX
{
    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be supressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx don't support the new syntax yet)
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

    /// <summary>Utility class to provide extensions for consumers</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "BS, extension" )]
    public static class AssemblyExtensions
    {
        /// <summary>Gets the value of the <see cref="AssemblyInformationalVersionAttribute"/> from an assembly</summary>
        /// <param name="self">Assembly to get informational version from</param>
        /// <returns>Information version of the assembly or an empty string if not available</returns>
        [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "BS, extension" )]
        public static string GetInformationalVersion(this Assembly self)
        {
            var assemblyVersionAttribute = self.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            return assemblyVersionAttribute is not null
                ? assemblyVersionAttribute.InformationalVersion
                : self.GetName().Version?.ToString() ?? string.Empty;
        }
    }
}
