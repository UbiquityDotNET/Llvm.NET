// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Text;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Implementation of <see cref="IDiagnosticReporter"/> that reports messages to <see cref="Console"/></summary>
    /// <remarks>
    /// <see cref="MsgLevel.Error"/> is reported to <see cref="Console.Error"/> while other levels, if
    /// enabled, are reported to <see cref="Console.Out"/>
    /// </remarks>
    public class ConsoleReporter
        : IDiagnosticReporter
    {
        /// <summary>Initializes a new instance of the <see cref="ConsoleReporter"/> class.</summary>
        /// <param name="level">Level of messages to enable for this reporter [Default: <see cref="MsgLevel.Information"/></param>
        /// <remarks>
        /// Any message reported with a level that is greater than or equal to <paramref name="level"/>
        /// is enabled, and thus reported.
        /// </remarks>
        public ConsoleReporter( MsgLevel level = MsgLevel.Information)
        {
            Level = level;
        }

        /// <inheritdoc/>
        public MsgLevel Level { get; }

        /// <inheritdoc/>
        public Encoding Encoding => Console.OutputEncoding;

        /// <inheritdoc/>
        /// <remarks>
        /// This implementation currently ignores the source of any diagnostics
        /// </remarks>
        public void Report( DiagnosticMessage diagnostic )
        {
            if(!this.IsEnabled( diagnostic.Level ))
            {
                return;
            }

            ReportMessage(diagnostic.Level, diagnostic.ToString("G", CultureInfo.CurrentCulture));
        }

        /// <summary>Virtual method to report a message formatted as a string</summary>
        /// <param name="level">Level of the message</param>
        /// <param name="msg">Message formatted as a string</param>
        /// <remarks>
        /// The default base implementation will simply redirect messages based on <paramref name="level"/> to
        /// <see cref="Console.Error"/> if <paramref name="level"/> == <see cref="MsgLevel.Error"/> or <see cref="Console.Out"/>
        /// for any other levels.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Invalid/Unknown level - should never hit this, internal error if it does.</exception>
        protected virtual void ReportMessage(MsgLevel level, string msg)
        {
            var writer = level switch
            {
                MsgLevel.Error => Console.Error,
                _ => Console.Out
            };

            writer.WriteLine(msg.ToString());
        }
    }
}
