// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Utility extension methods for command line parsing</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "BS, extension" )]
    [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "BS, Extension" )]
    public static class ParseResultExtensions
    {
#if DOCFX_BUILD_SUPPORTS_EXTENSION_KEYWORD
        extension(ParseResult self)
        {
            /// <summary>Gets a value indicating whether <paramref name="self"/> has any errors</summary>
            public bool HasErrors => self.Errors.Count > 0;

            public HelpOption? HelpOption
            {
                get
                {
                    var helpOptions = from r in self.CommandResult.RecurseWhileNotNull(r => r.Parent as CommandResult)
                                      from o in r.Command.Options.OfType<HelpOption>()
                                      select o;

                    return helpOptions.FirstOrDefault();
                }
            }

            public VersionOption? VersionOption
            {
                get
                {
                    var versionOptions = from r in self.CommandResult.RecurseWhileNotNull(r => r.Parent as CommandResult)
                                         from o in r.Command.Options.OfType<VersionOption>()
                                         select o;

                    return versionOptions.FirstOrDefault();
                }
            }
        }
#else
        /// <summary>Gets a value indicating whether <paramref name="self"/> has any errors</summary>
        /// <param name="self">Result to test for errors</param>
        /// <returns>value indicating whether <paramref name="self"/> has any errors</returns>
        public static bool HasErrors(this ParseResult self) => self.Errors.Count > 0;

        /// <summary>Gets the optional <see cref="HelpOption"/> from the result of a parse</summary>
        /// <param name="self">Result of parse to get the option from</param>
        /// <returns><see cref="HelpOption"/> if found or <see langword="null"/></returns>
        public static HelpOption? GetHelpOption(this ParseResult self)
        {
            var helpOptions = from r in self.CommandResult.RecurseWhileNotNull(r => r.Parent as CommandResult)
                              from o in r.Command.Options.OfType<HelpOption>()
                              select o;

            return helpOptions.FirstOrDefault();
        }

        /// <summary>Gets the optional <see cref="VersionOption"/> from the result of a parse</summary>
        /// <param name="self">Result of parse to get the option from</param>
        /// <returns><see cref="VersionOption"/> if found or <see langword="null"/></returns>
        public static VersionOption? GetVersionOption(this ParseResult self)
        {
            var versionOptions = from r in self.CommandResult.RecurseWhileNotNull(r => r.Parent as CommandResult)
                                 from o in r.Command.Options.OfType<VersionOption>()
                                 select o;

            return versionOptions.FirstOrDefault();
        }
#endif

        // shamelessly "borrowed" from: https://github.com/dotnet/dotnet/blob/8c7b3dcd2bd657c11b12973f1214e7c3c616b174/src/command-line-api/src/System.CommandLine/Help/HelpBuilderExtensions.cs#L42
        internal static IEnumerable<T> RecurseWhileNotNull<T>( this T? source, Func<T, T?> next )
            where T : class
        {
            while(source is not null)
            {
                yield return source;

                source = next( source );
            }
        }
    }
}
