// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;

using AnsiCodes;

using Ubiquity.NET.Extensions;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Implementation of <see cref="IDiagnosticReporter"/> that uses colorized console output</summary>
    public sealed class ColoredConsoleReporter
        : ConsoleReporter
    {
        /// <summary>Initializes a new instance of the <see cref="ColoredConsoleReporter"/> class</summary>
        /// <param name="level">Level of messages to enable for this reporter [Default: <see cref="MsgLevel.Information"/></param>
        /// <param name="mapping">Provides the mapping for the message levels this reporter will use (see remarks)</param>
        /// <remarks>
        /// <paramref name="mapping"/> (or a default if <see langword="null"/>) is set as the <see cref="CodeMap"/> property.
        /// <para>
        /// The default colors, if not provided via <paramref name="mapping"/>, are:
        /// <list type="table">
        /// <listheader><term>Level</term><description>Description</description></listheader>
        /// <item><term><see cref="MsgLevel.Verbose"/></term><description> <see cref="Color.LtBlue"/></description></item>
        /// <item><term><see cref="MsgLevel.Information"/></term><description> <see cref="Color.Default"/></description></item>
        /// <item><term><see cref="MsgLevel.Warning"/></term><description> <see cref="Color.LtYellow"/></description></item>
        /// <item><term><see cref="MsgLevel.Error"/></term><description> <see cref="Color.LtRed"/></description></item>
        /// </list></para>
        /// <para>
        /// Any level not in <see cref="CodeMap"/> is reported using <see cref="Color.Default"/>.
        /// </para>
        /// </remarks>
        public ColoredConsoleReporter( MsgLevel level = MsgLevel.Information, ImmutableDictionary<MsgLevel, AnsiCode>? mapping = null)
            : base(level)
        {
            CodeMap = mapping ?? new DictionaryBuilder<MsgLevel, AnsiCode>()
                                 {
                                    [MsgLevel.Verbose] = Color.LtBlue,
                                    [MsgLevel.Information] = Color.Default,
                                    [MsgLevel.Warning] = Color.LtYellow,
                                    [MsgLevel.Error] = Color.LtRed,
                                 }.ToImmutable();
        }

        /// <summary>Gets the <see cref="MsgLevel"/> to <see cref="AnsiCode"/> mapping used for coloring</summary>
        public ImmutableDictionary<MsgLevel, AnsiCode> CodeMap { get; }

        /// <remarks>
        /// This implementation will apply ANSI color code sequences to each message based on <paramref name="level"/>.
        /// </remarks>
        /// <inheritdoc/>
        protected override void ReportMessage( MsgLevel level, string msg )
        {
            if(!CodeMap.TryGetValue(level, out AnsiCode? color))
            {
                color = Color.Default;
            }

            // use base to write to the correct stream
            base.ReportMessage( level, $"{color}{msg}{Reset.All}" );
        }
    }
}
