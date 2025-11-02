// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Base class for reporting diagnostics via an instance of <see cref="TextWriter"/></summary>
    public class TextWriterReporter
        : IDiagnosticReporter
    {
        /// <summary>Initializes a new instance of the <see cref="TextWriterReporter"/> class.</summary>
        /// <param name="level">Reporting level for this reporter (any message with an equal or greater level is reported)</param>
        /// <param name="error">Error writer</param>
        /// <param name="warning">Warning writer</param>
        /// <param name="information">Information writer</param>
        /// <param name="verbose">Verbose writer</param>
        [SetsRequiredMembers]
        public TextWriterReporter(
            MsgLevel level,
            TextWriter? error = null,
            TextWriter? warning = null,
            TextWriter? information = null,
            TextWriter? verbose = null
            )
        {
            ArgumentNullException.ThrowIfNull(error);

            Level = level;
            Error = error ?? TextWriter.Null;
            Warning = warning ?? TextWriter.Null;
            Information = information ?? TextWriter.Null;
            Verbose = verbose ?? TextWriter.Null;
        }

        /// <summary>Gets the Error writer for this reporter</summary>
        public required TextWriter Error
        {
            get;
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                field = value;
            }
        }

        /// <summary>Gets the warning writer for this reporter</summary>
        public TextWriter Warning
        {
            get;
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                field = value;
            }
        }

        /// <summary>Gets the information writer for this reporter</summary>
        public TextWriter Information
        {
            get;
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                field = value;
            }
        }

        /// <summary>Gets the verbose writer for this reporter</summary>
        public TextWriter Verbose
        {
            get;
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                field = value;
            }
        }

        /// <inheritdoc/>
        public MsgLevel Level { get; init; }

        /// <inheritdoc/>
        public Encoding Encoding => Error.Encoding;

        /// <inheritdoc/>
        /// <remarks>
        /// This implementation will test if the <see cref="DiagnosticMessage.Level"/> of the
        /// message is enabled. If so, then a call is made to the virtual <see cref="ReportMessage(MsgLevel, string)"/>
        /// with the results of <see cref="DiagnosticMessage.ToString()"/> as the message text.
        /// </remarks>
        public void Report( DiagnosticMessage diagnostic )
        {
            if(!this.IsEnabled( diagnostic.Level ))
            {
                return;
            }

            ReportMessage(diagnostic.Level, diagnostic.ToString());
        }

        /// <summary>Virtual method to report a message formatted as a string</summary>
        /// <param name="level">Level of the message</param>
        /// <param name="msg">Message formatted as a string</param>
        /// <remarks>
        /// The default base implementation will simply redirect messages based on <paramref name="level"/> to
        /// the <see cref="TextWriter"/> writer for <paramref name="level"/>. It is possible that such a writer is <see cref="TextWriter.Null"/>
        /// and the messages go nowhere.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Invalid/Unknown level - should never hit this, internal or derived type error if it does.</exception>
        protected virtual void ReportMessage(MsgLevel level, string msg)
        {
            var writer = level switch
            {
                MsgLevel.Error => Error,
                MsgLevel.Warning => Warning,
                MsgLevel.Information => Information,
                MsgLevel.Verbose => Verbose,
                MsgLevel.None => null,
                _ => throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(MsgLevel))
            };

            // A message level of None is always ignored, this will not occur normally as that level is
            // NEVER enabled. But, if a derived type ever calls this directly it might not check for enabled.
            // Additionally, not all streams are guaranteed non-null, so this will ignore any that are.
            writer?.WriteLine(msg);
        }
    }
}
