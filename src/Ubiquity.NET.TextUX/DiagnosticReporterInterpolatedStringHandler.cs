// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Interpolated string handler for an <see cref="IDiagnosticReporter"/></summary>
    /// <remarks>
    /// <para>This handler will use the state of the <see cref="IDiagnosticReporter.Level"/> to filter messages
    /// as interpolated strings. If the channel for the message is not enabled, then the handler filters
    /// the entire message and can skip even constructing the parameterized elements (unless it is the first
    /// one [Limit of how Handlers work in .NET]). The limitation of the first is further restricted to the
    /// first "thing" interpolated including a string literal. Thus, the parameterized elements are not generated
    /// if the channel isn't enabled unless the element is the first thing in the interpolated string. In that
    /// case only the first entry is evaluated.</para>
    /// <note type="important">
    /// Apps should NOT depend on the subtleties of interpolated parameter evaluation to guarantee invocation
    /// (or not) of side effects. This is a "best-effort" optimization. In particular, apps should assume that
    /// a parameterized value may or may not be executed (i.e., non-deterministic). Therefore, it cannot assume
    /// (one way or another) that the side-effects of evaluation have, or have not, occurred.
    /// </note>
    /// <note type="note">Despite lots of samples (for preview variants) that use a 'ref struct', this is NOT
    /// a by ref like type. This is to allow use interpolating async parameters.</note>
    /// </remarks>
    [InterpolatedStringHandler]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not relevant for an interpolated string handler" )]
    public readonly struct DiagnosticReporterInterpolatedStringHandler
    {
        /// <summary>Initializes a new instance of the <see cref="DiagnosticReporterInterpolatedStringHandler"/> struct.</summary>
        /// <param name="literalLength">Length of the literal</param>
        /// <param name="formattedCount">Sadly .NET doesn't document this, or much else in relation to interpolated string handlers</param>
        /// <param name="reporter">"this" reference for the reporter. (Mapped via InterpolatedStringHandlerArgument applied to method)</param>
        /// <param name="level">Reporting level parameter to report for. (Mapped via InterpolatedStringHandlerArgument applied to method)</param>
        public DiagnosticReporterInterpolatedStringHandler( int literalLength, int formattedCount, IDiagnosticReporter reporter, MsgLevel level )
        {
            Builder = reporter.IsEnabled( level ) ? new( literalLength ) : null;
        }

        /// <summary>Gets a value indicating whether this handler is enabled</summary>
        public bool IsEnabled => Builder is not null;

        /// <summary>Appends a literal value to the results of interpolating a string</summary>
        /// <param name="s">literal value to append</param>
        /// <returns><see langword="true"/> if the interpolation should continue with other conversions or <see langword="false"/> if not.</returns>
        /// <remarks>
        /// The return is used to short circuit all other calls to interpolation, thus, this implementation returns if the
        /// reporting level is enabled for a given reporter.
        /// </remarks>
        public bool AppendLiteral( string s )
        {
            if(!IsEnabled)
            {
                return false;
            }

            Builder?.Append( s );
            return true;
        }

        /// <summary>Appends an interpolated value to the result of interpolation</summary>
        /// <typeparam name="T">Type of the interpolated value</typeparam>
        /// <param name="t">Value to format</param>
        /// <returns><see langword="true"/> if the interpolation should continue with other conversions or <see langword="false"/> if not.</returns>
        /// <remarks>
        /// The return is used to short circuit all other calls to interpolation, thus, this implementation returns if the
        /// reporting level is enabled for a given reporter.
        /// </remarks>
        public readonly bool AppendFormatted<T>( T t )
        {
            if(!IsEnabled)
            {
                return false;
            }

            Builder?.Append( t?.ToString() );
            return true;
        }

        /// <summary>Appends an interpolated value to the result of interpolation</summary>
        /// <typeparam name="T">Type of the interpolated value</typeparam>
        /// <param name="t">Value to format</param>
        /// <param name="format">format string for formatting the value</param>
        /// <returns><see langword="true"/> if the interpolation should continue with other conversions or <see langword="false"/> if not.</returns>
        /// <remarks>
        /// The return is used to short circuit all other calls to interpolation, thus, this implementation returns if the
        /// reporting level is enabled for a given reporter.
        /// </remarks>
        public readonly bool AppendFormatted<T>( T t, string format )
            where T : IFormattable
        {
            if(!IsEnabled)
            {
                return false;
            }

            Builder?.Append( t?.ToString( format, null ) );
            return true;
        }

        /// <summary>Gets the full results of interpolation</summary>
        /// <returns>Results of the interpolation (thus far)</returns>
        public string GetFormattedText( )
        {
            return Builder?.ToString() ?? string.Empty;
        }

        private readonly StringBuilder? Builder;
    }
}
