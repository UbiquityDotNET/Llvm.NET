// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

#pragma warning disable IDE0005 // Using directive is unnecessary.
#pragma warning disable SA1649 // File name should match first type name

using System;

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

#if USE_RUNTIME_TOOL_MESSAGE
    /// <summary>Static utility class to provide formatting for various runtime tool messaging</summary>
    /// <remarks>
    /// Each runtime environment uses different formats for message strings. This abstracts the differences
    /// and allows callers to remain blissfully ignorant of the distinctions.
    /// </remarks>
    public static class RuntimeToolMessage
    {
        /// <summary>Format a message in a runtime specific manner</summary>
        /// <param name="origin">Origin of the message (May be a file or app name)</param>
        /// <param name="location">Location in origin of the message source (If <paramref name="origin"/> is a file (do not use if origin is not a path)</param>
        /// <param name="subcategory">Sub category of the message normally omitted but may include additional information about the message</param>
        /// <param name="category">Category of the message; Most tooling only deals with Warning or Error</param>
        /// <param name="code">Code for the message - must not contain spaces</param>
        /// <param name="msgText">Text of the message [Localized]</param>
        /// <param name="formatProvider">Format provider for formatting the values</param>
        /// <returns>string formatted to the patterns defined by the current runtime</returns>
        public static string Format(
            string origin,
            SourceRange? location,
            string? subcategory,
            MsgLevel category,
            string? code,
            string msgText,
            IFormatProvider? formatProvider = null
        )
        {
            var msgInfo = new DiagnosticMessage(origin, location, subcategory, category, code, msgText);
            return msgInfo.ToString("G", formatProvider);
        }

        /// <summary>Format a message in the MSBUILD style</summary>
        /// <param name="origin">Origin of the message (May be a file or app name)</param>
        /// <param name="location">Location in origin of the message source (If <paramref name="origin"/> is a file (do not use if origin is not a path)</param>
        /// <param name="subcategory">Sub category of the message normally omitted but may include additional information about the message</param>
        /// <param name="category">Category of the message; Most tooling only deals with Warning or Error</param>
        /// <param name="code">Code for the message - must not contain spaces</param>
        /// <param name="msgText">Text of the message [Localized]</param>
        /// <param name="formatProvider">Format provider for formatting the values</param>
        /// <returns>string formatted for MSBUILD consumption</returns>
        /// <seealso href="https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-diagnostic-format-for-tasks?view=vs-2022"/>
        public static string FormatMSBuild(
            string origin,
            SourceRange? location,
            string? subcategory,
            MsgLevel category,
            string? code,
            string msgText,
            IFormatProvider? formatProvider = null
        )
        {
            var msgInfo = new DiagnosticMessage(origin, location, subcategory, category, code, msgText);
            return msgInfo.ToString("B", formatProvider);
        }
    }
#endif
}
