// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

using Ubiquity.NET.CommandLine.InterpolatedStringHandlers;
using Ubiquity.NET.Extensions;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Extensions to <see cref="IDiagnosticReporter"/> for specific message levels</summary>
    public static class DiagnosticReporterExtensions
    {
        /// <summary>Gets a value indicating whether the specified level (and higher) messages are enabled</summary>
        /// <param name="self">Reporter to test</param>
        /// <param name="level">Level to test if enabled</param>
        /// <returns><see langword="true"/> if the level or higher messages are enabled or <see langword="false"/> if not</returns>
        public static bool IsEnabled( this IDiagnosticReporter self, MsgLevel level )
        {
            return self.Level <= level;
        }

        #region Report

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
        /// <param name="origin">Origin for the source of this diagnostic</param>
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
            Uri? origin,
            SourceRange location,
            [InterpolatedStringHandlerArgument( "self", "level" )] DiagnosticReporterInterpolatedStringHandler handler
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(handler.IsEnabled)
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Origin = origin,
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
            Report(self, level, null, location, handler);
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
        /// <param name="origin">Origin for the source of this diagnostic</param>
        /// <param name="location">Location in source relating to the diagnostic</param>
        /// <param name="fmt">Format string for the message</param>
        /// <param name="args">Arguments for the message</param>
        public static void Report(
            this IDiagnosticReporter self,
            MsgLevel level,
            Uri? origin,
            SourceRange location,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            var diagnostic = new DiagnosticMessage()
            {
                Origin = origin,
                Location = location,
                Subcategory = default,
                Level = level,
                Code = default,
                Text = args.Length == 0 ? fmt : string.Format( CultureInfo.CurrentCulture, fmt, args )
            };

            self.Report( diagnostic );
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
            Report(self, level, null, location, fmt, args);
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
        #endregion

        #region MsgLevel.Error

        // maintainer's note: Doc comments are inherited from the "Error" implementation (except the summary)
        //                    and therefore the verbiage of the full comments should remain neutral.

        /// <summary>Reports a <see cref="MsgLevel.Error"/> level message to <paramref name="self"/></summary>
        /// <param name="self">Reporter to report message to</param>
        /// <param name="code">Identifier code for this message</param>
        /// <param name="location">Location in the origin that this message refers to</param>
        /// <param name="origin">Origin of the diagnostic (Usually the origin is a File, but may be anything or nothing)</param>
        /// <param name="subCategory">Subcategory for this message</param>
        /// <param name="formatProvider">Format provider to use when formatting the string</param>
        /// <param name="fmt">Format string for the message</param>
        /// <param name="args">Arguments for the message</param>
        /// <remarks>
        /// The reporter (<paramref name="self"/>) may filter out any messages reported for this level. The
        /// <paramref name="fmt"/> is NOT applied to <paramref name="args"/> unless the level for the message is
        /// enabled. This helps reduce the overhead of producing the final formatted string if it is ignored anyway.
        /// </remarks>
        public static void Error(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(self.IsEnabled(MsgLevel.Error))
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Error,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = string.Format(formatProvider, fmt, args),
                };

                self.Report( diagnostic );
            }
        }

        /// <summary>Reports a a <see cref="MsgLevel.Error"/> level message to <paramref name="self"/></summary>
        /// <param name="self">Reporter to report message to</param>
        /// <param name="code">Identifier code for this message</param>
        /// <param name="location">Location in the origin that this message refers to</param>
        /// <param name="origin">Origin of the diagnostic (Usually the origin is a File, but may be anything or nothing)</param>
        /// <param name="subCategory">Subcategory for this message</param>
        /// <param name="handler">Interpolated string for the message (processed via <see cref="VerboseReportingInterpolatedStringHandler"/>)</param>
        /// <remarks>
        /// The reporter (<paramref name="self"/>) may filter out any messages reported for this level. The
        /// <paramref name="handler"/> is NOT applied to format the final message unless the level for the message is
        /// enabled. This helps reduce the overhead of producing the final formatted string if it is ignored anyway.
        /// </remarks>
        public static void Error(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            [InterpolatedStringHandlerArgument( "self" )] ErrorReportingInterpolatedStringHandler handler
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(handler.IsEnabled)
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Error,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = handler.GetFormattedText()
                };

                self.Report( diagnostic );
            }
        }

        /// <summary>Reports a <see cref="MsgLevel.Error"/> level message to <paramref name="self"/></summary>
        /// <param name="self">Reporter to report message to</param>
        /// <param name="ex">Exception to report</param>
        /// <param name="code">Identifier code for this message</param>
        /// <param name="location">Location in the origin that this message refers to</param>
        /// <param name="origin">Origin of the diagnostic (Usually the origin is a File, but may be anything or nothing)</param>
        /// <param name="subCategory">Subcategory for this message</param>
        /// <remarks>
        /// The reporter (<paramref name="self"/>) may filter out any messages reported for <see cref="MsgLevel.Error"/> level.
        /// </remarks>
        public static void Error(
            this IDiagnosticReporter self,
            Exception ex,
            string? code = null,
            SourceRange location = default,
            Uri? origin = null,
            string? subCategory = null
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(self.IsEnabled(MsgLevel.Error))
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Error,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = ex.Message,
                };

                self.Report( diagnostic );
            }
        }

        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Error(
            this IDiagnosticReporter self,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Error(self, code: null, location: default, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Error(
            this IDiagnosticReporter self,
            SourceRange location,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Error(self, code: null, location, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, ErrorReportingInterpolatedStringHandler)"/>
        public static void Error(
            this IDiagnosticReporter self,
            [InterpolatedStringHandlerArgument( "self" )] ErrorReportingInterpolatedStringHandler handler
            )
        {
            Error(self, code: null, location: default, origin: null, subCategory: null, handler);
        }

        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, ErrorReportingInterpolatedStringHandler)"/>
        public static void Error(
            this IDiagnosticReporter self,
            SourceRange location,
            [InterpolatedStringHandlerArgument( "self" )] ErrorReportingInterpolatedStringHandler handler
            )
        {
            Error(self, code: null, location, origin: null, subCategory: null, handler);
        }
        #endregion

        #region MsgLevel.Warning

        /// <summary>Reports a <see cref="MsgLevel.Warning"/> level message to <paramref name="self"/></summary>
        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Warning(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(self.IsEnabled(MsgLevel.Warning))
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Warning,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = string.Format(formatProvider, fmt, args),
                };

                self.Report( diagnostic );
            }
        }

        /// <summary>Reports a <see cref="MsgLevel.Warning"/> level message to <paramref name="self"/></summary>
        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, ErrorReportingInterpolatedStringHandler)"/>
        public static void Warning(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            [InterpolatedStringHandlerArgument( "self" )] WarningReportingInterpolatedStringHandler handler
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(handler.IsEnabled)
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Warning,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = handler.GetFormattedText()
                };

                self.Report( diagnostic );
            }
        }

        /// <inheritdoc cref="Warning(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Warning(
            this IDiagnosticReporter self,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Warning(self, code: null, location: default, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Warning(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Warning(
            this IDiagnosticReporter self,
            SourceRange location,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Warning(self, code: null, location, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Warning(IDiagnosticReporter, string?, SourceRange, Uri?, string?, WarningReportingInterpolatedStringHandler)"/>
        public static void Warning(
            this IDiagnosticReporter self,
            [InterpolatedStringHandlerArgument( "self" )] WarningReportingInterpolatedStringHandler handler
            )
        {
            Warning(self, code: null, location: default, origin: null, subCategory: null, handler);
        }

        /// <inheritdoc cref="Warning(IDiagnosticReporter, string?, SourceRange, Uri?, string?, WarningReportingInterpolatedStringHandler)"/>
        public static void Warning(
            this IDiagnosticReporter self,
            SourceRange location,
            [InterpolatedStringHandlerArgument( "self" )] WarningReportingInterpolatedStringHandler handler
            )
        {
            Warning(self, code: null, location, origin: null, subCategory: null, handler);
        }
        #endregion

        #region MsgLevel.Information

        /// <summary>Reports a <see cref="MsgLevel.Information"/> level message to <paramref name="self"/></summary>
        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Information(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(self.IsEnabled(MsgLevel.Information))
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Information,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = string.Format(formatProvider, fmt, args),
                };

                self.Report( diagnostic );
            }
        }

        /// <summary>Reports a <see cref="MsgLevel.Information"/> level message to <paramref name="self"/></summary>
        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Information(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            [InterpolatedStringHandlerArgument( "self" )] InformationReportingInterpolatedStringHandler handler
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(handler.IsEnabled)
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Information,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = handler.GetFormattedText()
                };

                self.Report( diagnostic );
            }
        }

        /// <inheritdoc cref="Information(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Information(
            this IDiagnosticReporter self,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Information(self, code: null, location: default, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Information(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Information(
            this IDiagnosticReporter self,
            SourceRange location,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Information(self, code: null, location, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Information(IDiagnosticReporter, string?, SourceRange, Uri?, string?, InformationReportingInterpolatedStringHandler)"/>
        public static void Information(
            this IDiagnosticReporter self,
            [InterpolatedStringHandlerArgument( "self" )] InformationReportingInterpolatedStringHandler handler
            )
        {
            Information(self, code: null, location: default, origin: null, subCategory: null, handler);
        }

        /// <inheritdoc cref="Information(IDiagnosticReporter, string?, SourceRange, Uri?, string?, InformationReportingInterpolatedStringHandler)"/>
        public static void Information(
            this IDiagnosticReporter self,
            SourceRange location,
            [InterpolatedStringHandlerArgument( "self" )] InformationReportingInterpolatedStringHandler handler
            )
        {
            Information(self, code: null, location, origin: null, subCategory: null, handler);
        }
        #endregion

        #region MsgLevel.Verbose

        /// <summary>Reports a <see cref="MsgLevel.Verbose"/> level message to <paramref name="self"/></summary>
        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Verbose(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(self.IsEnabled(MsgLevel.Verbose))
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Verbose,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = string.Format(formatProvider, fmt, args),
                };

                self.Report( diagnostic );
            }
        }

        /// <summary>Reports a <see cref="MsgLevel.Verbose"/> level message to <paramref name="self"/></summary>
        /// <inheritdoc cref="Error(IDiagnosticReporter, string?, SourceRange, Uri?, string?, ErrorReportingInterpolatedStringHandler)"/>
        public static void Verbose(
            this IDiagnosticReporter self,
            string? code,
            SourceRange location,
            Uri? origin,
            string? subCategory,
            [InterpolatedStringHandlerArgument( "self" )] VerboseReportingInterpolatedStringHandler handler
            )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(handler.IsEnabled)
            {
                var diagnostic = new DiagnosticMessage()
                {
                    Code = code,
                    Level = MsgLevel.Verbose,
                    Location = location,
                    Origin = origin,
                    Subcategory = subCategory,
                    Text = handler.GetFormattedText()
                };

                self.Report( diagnostic );
            }
        }

        /// <inheritdoc cref="Verbose(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Verbose(
            this IDiagnosticReporter self,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Verbose(self, code: null, location: default, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Verbose(IDiagnosticReporter, string?, SourceRange, Uri?, string?, IFormatProvider?, string, object[])"/>
        public static void Verbose(
            this IDiagnosticReporter self,
            SourceRange location,
            IFormatProvider? formatProvider,
            [StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt,
            params object[] args
            )
        {
            Verbose(self, code: null, location, origin: null, subCategory: null, formatProvider, fmt, args);
        }

        /// <inheritdoc cref="Verbose(IDiagnosticReporter, string?, SourceRange, Uri?, string?, VerboseReportingInterpolatedStringHandler)"/>
        public static void Verbose(
            this IDiagnosticReporter self,
            [InterpolatedStringHandlerArgument( "self" )] VerboseReportingInterpolatedStringHandler handler
            )
        {
            Verbose(self, code: null, location: default, origin: null, subCategory: null, handler);
        }

        /// <inheritdoc cref="Verbose(IDiagnosticReporter, string?, SourceRange, Uri?, string?, VerboseReportingInterpolatedStringHandler)"/>
        public static void Verbose(
            this IDiagnosticReporter self,
            SourceRange location,
            [InterpolatedStringHandlerArgument( "self" )] VerboseReportingInterpolatedStringHandler handler
            )
        {
            Verbose(self, code: null, location, origin: null, subCategory: null, handler);
        }
        #endregion
    }
}
