// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>MSBUILD/VS message information holder</summary>
    /// <remarks>
    /// <para>This record holds the information necessary for a valid message for MSBUILD/VS processing.</para>
    /// <para>This is one of many possible formats of error messages parsed and understood by various build utilities.
    /// It is the most common (almost a `defacto` standard) format on Windows. But recognized on other platforms as well.
    /// Each such format may require different data and therefore is not easily generalized/abstracted. The
    /// <see cref="RuntimeToolMessage"/> class is used to attempt to generalize this concept and allow for independence
    /// of format. This includes the plausibility of a command line option to determine the output format. Though it is
    /// constrained by reality and can only include support for what it knows about.</para>
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-diagnostic-format-for-tasks?view=vs-2022"/>
    public readonly record struct MsBuildMessageInfo
    {
        /// <summary>Initializes a new instance of the <see cref="MsBuildMessageInfo"/> struct</summary>
        /// <param name="origin">Origin of the message</param>
        /// <param name="location">Location in source for the origin of this message</param>
        /// <param name="subcategory">Subcategory of the message</param>
        /// <param name="category">Category of the message</param>
        /// <param name="code">Code for the message (No spaces)</param>
        /// <param name="msgText">Text of the message</param>
        /// <remarks>
        /// <paramref name="code"/> must NOT be localized. It is required to universally identify a particular message.
        /// <paramref name="msgText"/>should be localized if the source tool supports localization.
        /// </remarks>
        /// <exception cref="ArgumentException">An argument is not valid</exception>
        /// <exception cref="ArgumentException">An argument is null</exception>
        public MsBuildMessageInfo(
            string origin,
            SourceRange? location,
            string? subcategory,
            MsgCategory category,
            string? code,
            string msgText )
        {
            ArgumentNullException.ThrowIfNull( origin );
            ArgumentException.ThrowIfNullOrWhiteSpace( msgText );
            if(code is not null && code.Any( ( c ) => char.IsWhiteSpace( c ) ))
            {
                throw new ArgumentException( "code must not contain whitespace", nameof( code ) );
            }

            Origin = origin;
            Subcategory = subcategory;
            Category = category;
            Code = code;
            Text = msgText;
            Location = location;
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            string locString = string.Empty;
            if(Location is not null)
            {
                locString = Location.Value.ToString( "B", null );
            }

            // account for optional values with leading space.
            string subCat = Subcategory is not null ? $" {Subcategory}" : string.Empty;
            string code = Code is not null ? $" {Code}" : string.Empty;

            return $"{Origin}{locString} :{subCat} {Category}{code} : {Text}";
        }

        private readonly string Origin;
        private readonly string? Subcategory;
        private readonly MsgCategory Category;
        private readonly string? Code;
        private readonly string Text;
        private readonly SourceRange? Location;
    }
}
