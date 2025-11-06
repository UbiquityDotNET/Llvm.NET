// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Extensions
{
    /// <summary>Utility class to provide extensions for consumers</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Extension" )]
    public static class AssemblyExtensions
    {
        // VS2026 builds of this are OK, however command line/PR/CI builds will generate an error
        // No idea why there's a difference the $(NETCoreSdkVersion) is the same in both so it's
        // unclear why the two things behave differently. This is just another example of why the
        // `extension` keyword is "not yet ready for prime time". Too many things don't support it
        // properly yet. [Hopefully, that works itself out in short order as it's useless unless
        // fully supported]
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
