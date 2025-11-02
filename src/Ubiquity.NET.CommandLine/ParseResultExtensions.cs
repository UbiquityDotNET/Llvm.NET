// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CommandLine
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

    /// <summary>Utility extension methods for command line parsing</summary>
    public static class ParseResultExtensions
    {
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
