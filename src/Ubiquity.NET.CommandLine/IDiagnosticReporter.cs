// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Text;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Interface for UX message reporting</summary>
    /// <remarks>
    /// Unlike logging, which is meant for developers/administrator diagnostics,
    /// this interface is focused solely on text based (typically command line)
    /// <b><em>end user</em></b> facing experiences. The idea is that UX is distinct
    /// from any logging and should not be mixed or confused as they have very different
    /// uses/intents.
    /// </remarks>
    public interface IDiagnosticReporter
    {
        /// <summary>Gets the current reporting level for this reporter</summary>
        MsgLevel Level { get; }

        /// <summary>Gets the encoding used for this reporter</summary>
        Encoding Encoding { get; }

        /// <summary>Report a message as defined by the implementation</summary>
        /// <param name="diagnostic">Message to report</param>
        void Report( DiagnosticMessage diagnostic );
    }
}
