// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Utility class to provide extensions for consumers</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "BS, extension" )]
    public static class AssemblyExtensions
    {
// Sadly support for the 'extension' keyword outside of the compiler is
// spotty at best. Third party tools and analyzers don't know what to do
// with it. First party analyzers and tools don't yet handle it properly.
// (Looking at you VS 2026 Insider's preview!) So don't use it yet...
#if ALL_TOOLS_SUPPORT_EXTENSION_KEYWORD
        /// <summary>Extensions for <see cref="Assembly"/></summary>
        extension(Assembly asm)
        {
            /// <summary>Gets the value of the <see cref="AssemblyInformationalVersionAttribute"/> from an assembly</summary>
            [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "BS, extension" )]
            public string InformationalVersion
            {
                get
                {
                    var assemblyVersionAttribute = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

                    return assemblyVersionAttribute is not null
                        ? assemblyVersionAttribute.InformationalVersion
                        : asm.GetName().Version?.ToString() ?? string.Empty;
                }
            }
        }
#else
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
#endif
    }
}
