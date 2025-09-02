// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

using Ubiquity.NET.TextUX.InterpolatedStringHandlers;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Interface for UX message reporting</summary>
    /// <remarks>
    /// Unlike logging, which is meant for developers/administrator diagnostics,
    /// this interface is focused solely on text based (typically command line)
    /// <b><em>end user</em></b> facing experiences. The idea is that UX is distinct
    /// from any logging and should not be mixed or confused as they have very different
    /// uses/intents.
    /// </remarks>
    public interface IDiagnosticReporter
    {
        /// <summary>Gets the current reporting level for this reporter</summary>
        MsgLevel Level { get; }

        /// <summary>Gets the encoding used for this reporter</summary>
        Encoding Encoding { get; }

        /// <summary>Report a message as defined by the implementation</summary>
        /// <param name="diagnostic">Message to report</param>
        void Report( DiagnosticMessage diagnostic );
    }

    /// <summary>Extension for <see cref="IDiagnosticReporter"/></summary>
    public static class IReporterExtensions
    {
        /// <summary>Reports a message</summary>
        /// <param name="self">Reporter to use in reporting the message</param>
        /// <param name="level">Message level</param>
        /// <param name="msg">Message text</param>
        public static void Report( this IDiagnosticReporter self, MsgLevel level, string msg )
        {
            ArgumentNullException.ThrowIfNull( self );
            var diagnostic = new DiagnosticMessage()
            {
                Origin = null,
                Location = default,
                Subcategory = default,
                Level = level,
                Code = default,
                Text = msg
            };

            self.Report( diagnostic );
        }

        /// <summary>Reports a message using an interpolated string</summary>
        /// <param name="self">Reporter to use in reporting the message</param>
        /// <param name="level">Message level</param>
        /// <param name="location">Location in source relating to the diagnostic</param>
        /// <param name="handler">Interpolated string for the message (processed via <see cref="DiagnosticReporterInterpolatedStringHandler"/>)</param>
        /// <remarks>
        /// The <see cref="DiagnosticReporterInterpolatedStringHandler"/> will optimize (short circuit) the interpolation based on the value of
        /// <see cref="MsgLevel"/>. If a level is not enabled, then the handler will short circuit the rest of the
        /// interpolation. Thus, any methods with side effects may not be called and callers should not depend on them happening
        /// in all cases.
        /// </remarks>
        public static void Report(
            this IDiagnosticReporter self,
            MsgLevel level,
            SourceRange location,
            [InterpolatedStringHandlerArgument( "self", "level" )] DiagnosticReporterInterpolatedStringHandler handler
            )
        {
            ArgumentNullException.ThrowIfNull( self );
            if(handler.IsEnabled)
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Origin = null,
                    Location = location,
                    Subcategory = default,
                    Level = level,
                    Code = default,
                    Text = handler.GetFormattedText()
                };

                self.Report( diagnostic );
            }
        }

        /// <summary>Reports a message using an interpolated string</summary>
        /// <param name="self">Reporter to use in reporting the message</param>
        /// <param name="level">Message level</param>
        /// <param name="handler">Interpolated string for the message (processed via <see cref="DiagnosticReporterInterpolatedStringHandler"/>)</param>
        /// <remarks>
        /// The <see cref="DiagnosticReporterInterpolatedStringHandler"/> will optimize (short circuit) the interpolation based on the value of
        /// <see cref="MsgLevel"/>. If a level is not enabled, then the handler will short circuit the rest of the
        /// interpolation. Thus, any methods with side effects may not be called and callers should not depend on them happening
        /// in all cases.
        /// </remarks>
        public static void Report(
            this IDiagnosticReporter self,
            MsgLevel level,
            [InterpolatedStringHandlerArgument( "self", "level" )] DiagnosticReporterInterpolatedStringHandler handler
            )
        {
            ArgumentNullException.ThrowIfNull( self );
            if(handler.IsEnabled)
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Origin = null,
                    Location = default,
                    Subcategory = default,
                    Level = level,
                    Code = default,
                    Text = handler.GetFormattedText()
                };

                self.Report( diagnostic );
            }
        }

        /// <summary>Reports a message using classic string formatting</summary>
        /// <param name="self">Reporter to use in reporting the message</param>
        /// <param name="level">Message level</param>
        /// <param name="location">Location in source relating to the diagnostic</param>
        /// <param name="fmt">Format string for the message</param>
        /// <param name="args">Arguments for the message</param>
        public static void Report(
            this IDiagnosticReporter self,
            MsgLevel level,
            SourceRange location,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            var diagnostic = new DiagnosticMessage()
            {
                Origin = null,
                Location = location,
                Subcategory = default,
                Level = level,
                Code = default,
                Text = args.Length == 0 ? fmt : string.Format( CultureInfo.CurrentCulture, fmt, args )
            };

            self.Report( diagnostic );
        }

        /// <summary>Reports multiple diagnostics to the reporter</summary>
        /// <param name="self">Reporter to report the diagnostics to</param>
        /// <param name="diagnostics">DIagnostics to report. This is a 'params' value so it is variadic in languages that support such a thing</param>
        public static void Report( this IDiagnosticReporter self, params IEnumerable<DiagnosticMessage> diagnostics )
        {
            foreach(var dm in diagnostics)
            {
                self.Report( dm );
            }
        }

        /// <summary>Gets a value indicating whether the specified level (and higher) messages are enabled</summary>
        /// <param name="self">Reporter to test</param>
        /// <param name="level">Level to test if enabled</param>
        /// <returns><see langword="true"/> if the level or higher messages are enabled or <see langword="false"/> if not</returns>
        public static bool IsEnabled( this IDiagnosticReporter self, MsgLevel level )
        {
            return self.Level <= level;
        }
    }
}
