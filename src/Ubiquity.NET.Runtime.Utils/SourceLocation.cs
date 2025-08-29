// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Abstraction to hold a source location</summary>
    /// <param name="StartLine">Line number of the starting line [1..n][0 = uninitialized/unknown]</param>
    /// <param name="StartColumn">Starting column position of the location [0..n-1]</param>
    /// <param name="EndLine">Ending line number of the location [1..n]</param>
    /// <param name="EndColumn">Ending column position of the location [0..n-1]</param>
    public readonly record struct SourceLocation( int StartLine, int StartColumn, int EndLine, int EndColumn )
        : IFormattable
    {
        /// <inheritdoc/>
        public override string ToString( )
        {
            return StartLine == EndLine && StartColumn == EndColumn
                ? $"({StartLine},{StartColumn})"
                : $"({StartLine},{StartColumn},{EndLine},{EndColumn})";
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Accepted format strings are:
        /// "B" for MSBuild format used for Windows build tools.
        /// "R" for runtime specific default ("B" for Windows)
        /// [Format strings for other runtimes TBD]
        /// </remarks>
        public string ToString( string? format, IFormatProvider? formatProvider )
        {
            formatProvider ??= CultureInfo.CurrentCulture;
            return format switch
            {
                "B" => FormatMsBuild( formatProvider ),
                "R" => FormatMsBuild( formatProvider ), // TODO: Adjust this to select format based on current runtime
                _ => ToString()
            };
        }

        // SEE: https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-diagnostic-format-for-tasks?view=vs-2022
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Result is NOT simpler" )]
        private string FormatMsBuild( IFormatProvider formatProvider )
        {
            if(StartLine == 0)
            {
                return string.Empty;
            }

            if(EndLine == 0)
            {
                return StartColumn == 0
                     ? $"({StartLine}"
                     : $"({StartLine}, {StartColumn})";
            }
            else if(EndLine == StartLine)
            {
                return $"({StartLine}, {StartColumn}-{EndColumn})";
            }
            else
            {
                return StartColumn == 0 && EndColumn == 0
                     ? $"({StartLine}-{EndLine})"
                     : $"({StartLine}, {StartColumn}, {EndLine}, {EndColumn})";
            }
        }
    }
}
