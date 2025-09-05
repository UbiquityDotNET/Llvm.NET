// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Tool Message category</summary>
    public enum MsgLevel
    {
        /// <summary>All channels off</summary>
        None = 0,

        /// <summary>Verbose messages (or higher) are enabled</summary>
        Verbose = 1,

        /// <summary>Informational messages (or higher) are enabled.</summary>
        Information = 2,

        /// <summary>Warning messages (or higher) are enabled. [This is the default value]</summary>
        Warning = 3, // Default level is warning & error only

        /// <summary>Error messages (or higher) are enabled.</summary>
        Error = 4,
    }

    /// <summary>Diagnostic message for reporting diagnostics as part of end-user facing experience (UX)</summary>
    /// <remarks>
    /// <see cref="Code"/> must NOT be localized. It is required to universally identify a
    /// particular message. <see cref="Text"/> should be localized if the source tool supports
    /// localization.
    /// <para>Diagnostic codes (<see cref="Code"/>) are of the form &lt;prefix&gt;&lt;number&gt;
    /// (example: FOO01234). This is a unique identifier for the message that allows a user to reference
    /// it for support or other diagnostic analysis. The &lt;prefix&gt; portion of the code indicates
    /// the application source of the message.</para>
    /// </remarks>
    public readonly record struct DiagnosticMessage
        : IFormattable
    {
        /// <summary>Gets the origin of the message</summary>
        public Uri? Origin { get; init; }

        /// <summary>Gets the location in source for the origin of this message</summary>
        public SourceRange? Location { get; init; }

        /// <summary>Gets the subcategory of the message</summary>
        public string? Subcategory { get; init; }

        /// <summary>Gets the Level/Category of the message</summary>
        public MsgLevel Level { get; init; }

        /// <summary>Gets the code for the message (No spaces)</summary>
        public string? Code
        {
            get;
            init
            {
                if(value is not null && value.Any( ( c ) => char.IsWhiteSpace( c ) ))
                {
                    throw new ArgumentException( "If provided, code must not contain whitespace", nameof( value ) );
                }

                field = value;
            }
        }

        /// <summary>Gets the text of the message</summary>
        public string Text
        {
            get;
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                field = value;
            }
        }

        /// <summary>Formats this instance using the general runtime specific format</summary>
        /// <returns>Formatted string for the message</returns>
        public override string ToString( )
        {
            // use runtime default formatting
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Accepted format strings are:
        /// "M" for MSBuild format used for Windows build tools.
        /// "G" for runtime specific (For Windows, this is the MSBuild format)
        /// [Format strings for other runtimes TBD (L:Linux, A:Apple ... ????)]
        /// </remarks>
        public string ToString( string? format, IFormatProvider? formatProvider )
        {
            formatProvider ??= CultureInfo.CurrentCulture;
            return format switch
            {
                "M" => FormatMsBuild( formatProvider ),
                "G" => FormatRuntime( formatProvider ),
                _ => throw new FormatException($"{format} is not a valid format specifier for {nameof(DiagnosticMessage)}")
            };
        }

        private string FormatMsBuild(IFormatProvider formatProvider)
        {
            if(Origin is null || string.IsNullOrWhiteSpace(Origin.AbsoluteUri))
            {
                return Text;
            }

            string locString = string.Empty;
            if(Location is not null)
            {
                locString = Location.Value.ToString( "M", formatProvider );
            }

            // account for optional values with leading space.
            string subCat = Subcategory is not null ? $" {Subcategory}" : string.Empty;
            string code = Code is not null ? $" {Code}" : string.Empty;

            return $"{Origin}{locString} :{subCat} {Level}{code} : {Text}";
        }

        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Place holder for future work" )]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1108:BlockStatementsMustNotContainEmbeddedComments", Justification = "Reviewed.")]
        private string FormatRuntime(IFormatProvider formatProvider)
        {
            if(OperatingSystem.IsWindows())
            {
                return FormatMsBuild(formatProvider);
            }
            else // TODO: Adjust this to format based on styles of additional runtimes
            {
                // for now - always use MSBUILD format
                return FormatMsBuild(formatProvider);
            }
        }
    }
}
