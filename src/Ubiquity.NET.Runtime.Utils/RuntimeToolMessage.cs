using System;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Tool Message category</summary>
    public enum MsgCategory
    {
        /// <summary>Message is an error</summary>
        Error,

        /// <summary>Message is a warning</summary>
        Warning,

        /// <summary>Message is informational</summary>
        Information,
    }

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
            SourceLocation? location,
            string? subcategory,
            MsgCategory category,
            string? code,
            string msgText,
            IFormatProvider? formatProvider = null
        )
        {
            if (OperatingSystem.IsWindows())
            {
                return FormatMSBuild(origin, location, subcategory, category, code, msgText, null);
            }
            else
            {
                // TODO: Determine common/standard format(s) for other runtimes...
                return msgText;
            }
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
            SourceLocation? location,
            string? subcategory,
            MsgCategory category,
            string? code,
            string msgText,
            IFormatProvider? formatProvider = null
        )
        {
            var msgInfo = new MsBuildMessageInfo(origin, location, subcategory, category, code, msgText);
            return msgInfo.ToString();
        }
    }
}
