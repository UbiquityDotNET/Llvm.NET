// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Ubiquity.NET.CommandLine;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Adapter to redirect calls to <see cref="IParseErrorReporter"/> to a given <see cref="IDiagnosticReporter"/></summary>
    public class ParseErrorDiagnosticAdapter
        : IParseErrorReporter
    {
        /// <summary>Initializes a new instance of the <see cref="ParseErrorDiagnosticAdapter"/> class.</summary>
        /// <param name="targetReporter">reporter that this instance adapts errors to</param>
        /// <param name="diagnosticCodePrefix">Prefix used for all codes</param>
        /// <param name="origin">Origin of the diagnostics reported by this adapter (default is an in memory string [string:memory])</param>
        public ParseErrorDiagnosticAdapter( IDiagnosticReporter targetReporter, string diagnosticCodePrefix, Uri? origin = null )
        {
            TargetReporter = targetReporter;
            Origin = origin ?? new( "string:memory" );
            Prefix = diagnosticCodePrefix;
        }

        /// <summary>Gets the origin reported by this adapter</summary>
        public Uri Origin { get; }

        /// <summary>Gets the prefix used for each diagnostic</summary>
        public string Prefix { get; }

        /// <summary>Gets the target reporter this adapter redirects to</summary>
        public IDiagnosticReporter TargetReporter { get; }

        /// <inheritdoc/>
        public void ReportError( ErrorNode node )
        {
            var diagnostic = new DiagnosticMessage()
            {
                Origin = Origin,
                Code = $"{Prefix}{Convert.ToInt32(node.Code, CultureInfo.InvariantCulture)}",
                Level = node.Level,
                Location = node.Location,
                Subcategory = default,
                Text = node.Message
            };
            TargetReporter.Report( diagnostic );
        }

        /// <inheritdoc/>
        public void ReportError( string msg )
        {
            var diagnostic = new DiagnosticMessage()
            {
                Origin = Origin,
                Code = default,
                Level = MsgLevel.Error,
                Location = default,
                Subcategory = default,
                Text = msg
            };
            TargetReporter.Report( diagnostic );
        }
    }
}
