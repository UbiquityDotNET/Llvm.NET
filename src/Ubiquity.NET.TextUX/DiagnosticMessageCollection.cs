// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Reporter that collects diagnostic information without reporting it in any UI/UX</summary>
    public class DiagnosticMessageCollection
        : IDiagnosticReporter
        , IReadOnlyCollection<DiagnosticMessage>
    {
        /// <summary>Initializes a new instance of the <see cref="DiagnosticMessageCollection"/> class.</summary>
        /// <param name="level">Minimal reporting level for this collector [Default is <see cref="MsgLevel.Error"/> to collect all messaged</param>
        public DiagnosticMessageCollection( MsgLevel level = MsgLevel.Error )
        {
            Level = level;
        }

        /// <inheritdoc/>
        public MsgLevel Level { get; }

        /// <inheritdoc/>
        public Encoding Encoding => Encoding.Unicode;

        /// <inheritdoc/>
        public int Count => Messages.Count;

        /// <inheritdoc/>
        public void Report( DiagnosticMessage diagnostic )
        {
            if(this.IsEnabled( Level ))
            {
                Messages = Messages.Add( diagnostic );
            }
        }

        /// <inheritdoc/>
        public IEnumerator<DiagnosticMessage> GetEnumerator( )
        {
            return ((IEnumerable<DiagnosticMessage>)Messages).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( )
        {
            return ((IEnumerable)Messages).GetEnumerator();
        }

        /// <summary>Gets the current list of messages received by this collector</summary>
        public ImmutableList<DiagnosticMessage> Messages { get; private set; } = [];
    }
}
