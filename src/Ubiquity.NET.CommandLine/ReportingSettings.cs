// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Ubiquity.NET.CommandLine
{
    // internal extension for IDiagnosticReporter
    internal static class ReportingSettings
    {
        // Create an InvocationConiguration that wraps an IDiagnosticReporter
        public static InvocationConfiguration CreateConfig( this IDiagnosticReporter self )
        {
            ArgumentNullException.ThrowIfNull( self );

            return new()
            {
                EnableDefaultExceptionHandler = false,
                Error = new ReportingTextWriter( self, MsgLevel.Error ),
                Output = new ReportingTextWriter( self, MsgLevel.Information ),
            };
        }
    }

    // TextWriter that wraps an IDiagnosticReporter for a given level
    // This is an implementation of the GoF "Adapter Pattern"
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "File scoped..." )]
    file class ReportingTextWriter
        : TextWriter
    {
        public ReportingTextWriter( IDiagnosticReporter diagnosticReporter, MsgLevel level )
        {
            Reporter = diagnosticReporter;
            MsgLevel = level;
            Builder = new();
        }

        public override Encoding Encoding => Reporter.Encoding;

        public MsgLevel MsgLevel { get; }

        public override void Write(string? value)
        {
            if (value == Environment.NewLine)
            {
                WriteLine();
            }
            else
            {
                base.Write(value);
            }
        }

        public override void WriteLine( )
        {
            Reporter.Report( MsgLevel, Builder.ToString() );
            Builder.Clear();
        }

        public override void Write( char value )
        {
            Builder.Append( value );
        }

        private readonly IDiagnosticReporter Reporter;
        private readonly StringBuilder Builder;
    }
}
