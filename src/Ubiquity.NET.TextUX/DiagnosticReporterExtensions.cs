// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Ubiquity.NET.TextUX.InterpolatedStringHandlers;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Extensions to <see cref="IDiagnosticReporter"/> for specific message levels</summary>
    public static class DiagnosticReporterExtensions
    {
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
        /// <paramref name="handler"/> is NOT applied to format the final message the level for the message is
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
