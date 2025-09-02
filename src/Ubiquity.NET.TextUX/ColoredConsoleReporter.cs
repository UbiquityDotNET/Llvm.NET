// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using AnsiCodes;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Implementation of <see cref="IDiagnosticReporter"/> that uses colorized console output</summary>
    public sealed class ColoredConsoleReporter
        : ConsoleReporter
    {
        /// <summary>Initializes a new instance of the <see cref="ColoredConsoleReporter"/> class</summary>
        /// <inheritdoc cref="ConsoleReporter(MsgLevel)"/>
        public ColoredConsoleReporter( MsgLevel level = MsgLevel.Information)
            : base(level)
        {
        }

        /// <remarks>
        /// This implementation will apply ANSI color code sequences to each message based on <paramref name="level"/>.
        /// The colors are:
        /// <list type="table">
        /// <listheader><term>Level</term><description>Description</description></listheader>
        /// <item><term><see cref="MsgLevel.Verbose"/></term><description> Default console color</description></item>
        /// <item><term><see cref="MsgLevel.Information"/></term><description> Blue</description></item>
        /// <item><term><see cref="MsgLevel.Warning"/></term><description> Yellow</description></item>
        /// <item><term><see cref="MsgLevel.Error"/></term><description> Red</description></item>
        /// </list>
        /// </remarks>
        /// <inheritdoc/>
        protected override void ReportMessage( MsgLevel level, string msg )
        {
            AnsiCode color = level switch
            {
                MsgLevel.Verbose => Color.Default,
                MsgLevel.Information => Color.Blue,
                MsgLevel.Warning => Color.Yellow,
                MsgLevel.Error => Color.Red,
                _ => throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(MsgLevel))
            };

            // use base to write to the correct stream
            base.ReportMessage( level, $"{color}{msg}{Reset.All}" );
        }
    }
}
