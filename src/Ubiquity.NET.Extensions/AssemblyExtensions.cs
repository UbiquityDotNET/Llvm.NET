// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Extensions
{
    /// <summary>Utility class to provide extensions for consumers</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Extension" )]
    public static class AssemblyExtensions
    {
        // VS2026 builds of this are OK, however VS2019, command line/PR/CI builds will generate an error.
        // The VS builds use the VS provided MSBuild, while the command line uses the .NET Core build.
        // This is just another example of why the `extension` keyword is "not yet ready for prime time".
        // Too many things don't support it properly yet so use needs justification as the ONLY option and
        // HEAVY testing to ensure all the issues are accounted for.
        // [Hopefully, that works itself out in short order as it's a mostly useless feature unless fully supported]
#if COMPILER_SUPPORTS_CALLER_ATTRIBUES_ON_EXTENSION
        extension(Assembly self)
        {
            /// <summary>Gets the informational version from an assembly</summary>
            /// <param name="exp">Expresssion for the assembly to retrieve the attribute data from; normally provided by compiler</param>
            /// <returns>String contents from the <see cref="AssemblyInformationalVersionAttribute"/> in the assembly or <see cref="string.Empty"/></returns>
            [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "Instance extension" )]
            public string GetInformationalVersion( [CallerArgumentExpression( nameof( self ) )] string? exp = null )
            {
                ArgumentNullException.ThrowIfNull( self, exp );

                var assemblyVersionAttribute = self.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

                return assemblyVersionAttribute is not null
                    ? assemblyVersionAttribute.InformationalVersion
                    : self.GetName().Version?.ToString() ?? string.Empty;
            }
        }
#else
        /// <summary>Gets the informational version from an assembly</summary>
        /// <param name="self">Assembly to extract the version from</param>
        /// <param name="exp">Expresssion for the assembly to retrieve the attribute data from; normally provided by compiler</param>
        /// <returns>String contents from the <see cref="AssemblyInformationalVersionAttribute"/> in the assembly or <see cref="string.Empty"/></returns>
        public static string GetInformationalVersion(this Assembly self, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
        {
            ArgumentNullException.ThrowIfNull( self, exp );

            var assemblyVersionAttribute = self.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            return assemblyVersionAttribute is not null
                ? assemblyVersionAttribute.InformationalVersion
                : self.GetName().Version?.ToString() ?? string.Empty;
        }
#endif

    }
}
