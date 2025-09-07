// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Implementation of <see cref="IDiagnosticReporter"/> that reports messages to a <see cref="Console"/></summary>
    /// <remarks>
    /// Messages with a <see cref="DiagnosticMessage.Level"/> of <see cref="MsgLevel.Error"/> are reported to the console's <see cref="Console.Error"/>
    /// writer, while other levels, if enabled, are reported to the console's <see cref="Console.Out"/> writer.
    /// </remarks>
    public class ConsoleReporter
        : TextWriterReporter
    {
        /// <summary>Initializes a new instance of the <see cref="ConsoleReporter"/> class.</summary>
        /// <param name="level">Level of messages to enable for this reporter [Default: <see cref="MsgLevel.Information"/></param>
        /// <remarks>
        /// Any message reported with a level that is greater than or equal to <paramref name="level"/>
        /// is enabled, and thus reported.
        /// </remarks>
        [SetsRequiredMembers]
        public ConsoleReporter( MsgLevel level = MsgLevel.Information)
            : base(level, Console.Error, Console.Out, Console.Out, Console.Out)
        {
        }
    }
}
