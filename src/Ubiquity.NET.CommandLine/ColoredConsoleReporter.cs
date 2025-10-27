// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using AnsiCodes;

using Ubiquity.NET.Extensions;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Implementation of <see cref="IDiagnosticReporter"/> that uses colorized console output</summary>
    public sealed class ColoredConsoleReporter
        : ConsoleReporter
    {
        /// <summary>Initializes a new instance of the <see cref="ColoredConsoleReporter"/> class</summary>
        /// <param name="level">Level of messages to enable for this reporter [Default: <see cref="MsgLevel.Information"/></param>
        /// <param name="colorMapping">Provides the color mapping for the message levels this reporter will use (see remarks)</param>
        /// <remarks>
        /// <paramref name="colorMapping"/> (or a default if <see langword="null"/>) is set as the <see cref="ColorMap"/> property.
        /// <para>
        /// The default colors, if not provided via <paramref name="colorMapping"/>, are:
        /// <list type="table">
        /// <listheader><term>Level</term><description>Description</description></listheader>
        /// <item><term><see cref="MsgLevel.Verbose"/></term><description> <see cref="Color.LtBlue"/></description></item>
        /// <item><term><see cref="MsgLevel.Information"/></term><description> <see cref="Color.Default"/></description></item>
        /// <item><term><see cref="MsgLevel.Warning"/></term><description> <see cref="Color.LtYellow"/></description></item>
        /// <item><term><see cref="MsgLevel.Error"/></term><description> <see cref="Color.LtRed"/></description></item>
        /// </list></para>
        /// <para>
        /// Any level not in <see cref="ColorMap"/> is reported using <see cref="Color.Default"/>.
        /// </para>
        /// </remarks>
        [SetsRequiredMembers]
        public ColoredConsoleReporter( MsgLevel level = MsgLevel.Information, ImmutableDictionary<MsgLevel, AnsiCode>? colorMapping = null)
            : base(level)
        {
            ColorMap = colorMapping ?? ImmutableDictionary<MsgLevel, AnsiCode>.Empty;
        }

        /// <summary>Gets the <see cref="MsgLevel"/> to <see cref="AnsiCode"/> colorMapping used for coloring</summary>
        public ImmutableDictionary<MsgLevel, AnsiCode> ColorMap
        {
            get => field.IsEmpty ? DefaultColorMap : field;
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                field = value;
            }
        }

        /// <summary>Gets the default color map for this type</summary>
        public static ImmutableDictionary<MsgLevel, AnsiCode> DefaultColorMap { get; }
            = new DictionaryBuilder<MsgLevel, AnsiCode>
            {
                [MsgLevel.Verbose] = Color.LtBlue,
                [MsgLevel.Information] = Color.Default,
                [MsgLevel.Warning] = Color.LtYellow,
                [MsgLevel.Error] = Color.LtRed,
            }.ToImmutable();

        /// <remarks>
        /// This implementation will apply ANSI color code sequences to each message based on <paramref name="level"/>.
        /// </remarks>
        /// <inheritdoc/>
        protected override void ReportMessage( MsgLevel level, string msg )
        {
            if(!ColorMap.TryGetValue(level, out AnsiCode? color))
            {
                color = Color.Default;
            }

            // use base to write to the correct stream
            base.ReportMessage( level, $"{color}{msg}{Reset.All}" );
        }
    }
}
