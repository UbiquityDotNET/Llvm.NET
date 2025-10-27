// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Text;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Implementation of <see cref="IDiagnosticReporter"/> that throws <see cref="ArgumentException"/> for any errors reported.</summary>
    public class ArgumentExceptionReporter
        : IDiagnosticReporter
    {
        /// <summary>Initializes a new instance of the <see cref="ArgumentExceptionReporter"/> class.</summary>
        /// <param name="exp">Expression for the argument to throw exceptions for</param>
        public ArgumentExceptionReporter( string exp )
        {
            ArgumentExpression = exp;
        }

        /// <summary>Gets the expression for the argument to throw exceptions for</summary>
        public string ArgumentExpression { get; }

        /// <inheritdoc/>
        public MsgLevel Level => MsgLevel.Error;

        /// <inheritdoc/>
        public Encoding Encoding => Encoding.Unicode;

        /// <remarks>Any diagnostics with <see cref="MsgLevel.Error"/> will throw an argument exception</remarks>
        /// <inheritdoc/>
        public void Report( DiagnosticMessage diagnostic )
        {
            if(this.IsEnabled(diagnostic.Level))
            {
                throw new ArgumentException(diagnostic.ToString(), ArgumentExpression);
            }
        }
    }
}
