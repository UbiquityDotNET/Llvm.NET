// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Mostly from https://github.com/Sergio0694/PolySharp/blob/main/src/PolySharp.SourceGenerators/Extensions/AnalyzerConfigOptionsProviderExtensions.cs
// Reformatted and made to conform to repo guides

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Extension methods for the <see cref="AnalyzerConfigOptionsProvider"/> type.</summary>
    public static class AnalyzerConfigOptionsProviderExtensions
    {
        /// <summary>Checks whether the input property has a valid <see cref="bool"/> value.</summary>
        /// <param name="options">The input <see cref="AnalyzerConfigOptionsProvider"/> instance.</param>
        /// <param name="propertyName">The Build property name.</param>
        /// <param name="propertyValue">The resulting property value, if invalid.</param>
        /// <returns>Whether the target property is a valid <see cref="bool"/> value.</returns>
        public static bool IsValidBoolBuildProperty(
            this AnalyzerConfigOptionsProvider options,
            string propertyName,
            [NotNullWhen( false )] out string? propertyValue
            )
        {
            return !options.GlobalOptions.TryGetValue( $"{BuildProperty}.{propertyName}", out propertyValue )
               || string.IsNullOrEmpty( propertyValue )
               || string.Equals( propertyValue, bool.TrueString, StringComparison.OrdinalIgnoreCase )
               || string.Equals( propertyValue, bool.FalseString, StringComparison.OrdinalIgnoreCase );
        }

        /// <summary>Gets the value of a <see cref="bool"/> build property.</summary>
        /// <param name="options">The input <see cref="AnalyzerConfigOptionsProvider"/> instance.</param>
        /// <param name="propertyName">The build property name.</param>
        /// <returns>The value of the specified build property.</returns>
        /// <remarks>
        /// The return value is equivalent to a (case insensitive) <c>'$(PropertyName)' == 'true'</c> check.
        /// That is, any other value, including empty/not present, is considered <see langword="true"/>.
        /// </remarks>
        public static bool GetBoolBuildProperty( this AnalyzerConfigOptionsProvider options, string propertyName )
        {
            return options.GlobalOptions.TryGetValue( $"{BuildProperty}.{propertyName}", out string? propertyValue )
                && string.Equals( propertyValue, bool.TrueString, StringComparison.OrdinalIgnoreCase );
        }

        /// <summary>Gets the value of a Build property representing a semicolon-separated list of strings.</summary>
        /// <param name="options">The input <see cref="AnalyzerConfigOptionsProvider"/> instance.</param>
        /// <param name="propertyName">The build property name.</param>
        /// <returns>The value of the specified build property.</returns>
        public static ImmutableArray<string> GetStringArrayBuildProperty( this AnalyzerConfigOptionsProvider options, string propertyName )
        {
            return options.GlobalOptions.TryGetValue( $"{BuildProperty}.{propertyName}", out string? propertyValue )
                ? [ .. propertyValue.Split( ',', ';' ) ]
                : [];
        }

        // MSBuild properties that are visible to the compiler are available with the "build_property." prefix
        // See: https://andrewlock.net/creating-a-source-generator-part-13-providing-and-accessing-msbuild-settings-in-source-generators/
        private const string BuildProperty = "build_property";
    }
}
