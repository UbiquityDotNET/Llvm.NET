// -----------------------------------------------------------------------
// <copyright file="ColoredConsoleParseErrorReporter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Implementation of <see cref="IParseErrorReporter"/> that uses colorized console output</summary>
    public class ColoredConsoleParseErrorReporter
        : IParseErrorReporter
    {
        /// <summary>Initializes a new instance of <see cref="ColoredConsoleParseErrorReporter"/></summary>
        /// <param name="origin">Origin of messages to report (Default is none)</param>
        /// <param name="formatProvider">Format provider to use (Default is <see cref="CultureInfo.CurrentCulture"/>)</param>
        /// <remarks>
        /// If <paramref name="origin"/> is <see langword="null"/> or empty, then no special message formatting is applied.
        /// This is normally used with an interpreter loop. However if <paramref name="origin"/> is provided a value then
        /// an OS runtime specific standard format is used to generate a message. This form is normally used for tools
        /// operating as a stand alone/AOT parser or code generation. In such a case errors generally refer to the input
        /// source file and conform to a platform defined standard for recognition by subsequent stages of the build tooling.
        /// </remarks>
        public ColoredConsoleParseErrorReporter(string? origin = null, IFormatProvider? formatProvider = null)
        {
            origin ??= string.Empty;
            Origin = origin;
            formatProvider ??= CultureInfo.CurrentCulture;
            FormatProvider = formatProvider;
        }

        /// <summary>Origin of the messages</summary>
        /// <remarks>This is either a single file or a an app name</remarks>
        public string Origin {get; init;}

        /// <summary>Format provider to use when formatting values to string</summary>
        public IFormatProvider FormatProvider {get; init;}

        /// <inheritdoc/>
        public void ReportError( ErrorNode node )
        {
            ArgumentNullException.ThrowIfNull( node );

            if(string.IsNullOrWhiteSpace(Origin))
            {
                ReportError(node.ToString());
            }
            else
            {
                // Formats the error message as a string according to current runtime environment common standard
                string msg = RuntimeToolMessage.Format(Origin, node.Location, null, MsgCategory.Error, null, node.ToString(), FormatProvider);
                ReportError( msg );
            }
        }

        /// <inheritdoc/>
        public void ReportError( string msg )
        {
            if( !string.IsNullOrWhiteSpace( msg ) )
            {
                var color = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine( msg );
                }
                finally
                {
                    Console.ForegroundColor = color;
                }
            }
        }
    }
}
