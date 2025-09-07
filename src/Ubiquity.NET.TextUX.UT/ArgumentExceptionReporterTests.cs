// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.TextUX.UT
{
    [TestClass]
    public class ArgumentExceptionReporterTests
    {
        private const string ParamExpressionName = "expression";
        private const string VerboseMessage = "This is a verbose";
        private const string InformationMessage = "This is an Information level message";
        private const string WarningMessage = "This is a warning";
        private const string ErrorMessage = "This is an error";

        [TestMethod]
        public void ArgumentExceptionReporterTest( )
        {
            var reporter = new ArgumentExceptionReporter(ParamExpressionName);
            Assert.IsNotNull( reporter );
            Assert.AreEqual( ParamExpressionName, reporter.ArgumentExpression );
            Assert.AreEqual( MsgLevel.Error, reporter.Level );
            Assert.AreEqual( Encoding.Unicode, reporter.Encoding );
        }

        [TestMethod]
        public void ReportTest( )
        {
            var reporter = new ArgumentExceptionReporter(ParamExpressionName);

            // should not throw for a Verbose level message
            var verboseMsg = new DiagnosticMessage()
            {
                Level = MsgLevel.Verbose,
                Text = VerboseMessage,
            };

            reporter.Report( verboseMsg );

            // should not throw for an Information level message
            var informationMsg = new DiagnosticMessage()
            {
                Level = MsgLevel.Information,
                Text = InformationMessage,
            };

            reporter.Report( verboseMsg );

            // should not throw for a warning
            var warningMsg = new DiagnosticMessage()
            {
                Level = MsgLevel.Warning,
                Text = WarningMessage,
            };

            reporter.Report( warningMsg );

            // should only throw for an error
            var errorMsg = new DiagnosticMessage()
            {
                Level = MsgLevel.Error,
                Text = ErrorMessage,
            };

            var ex = Assert.ThrowsExactly<ArgumentException>(()=>reporter.Report(errorMsg));
            Assert.AreSame( ParamExpressionName, ex.ParamName );
            Assert.StartsWith( ErrorMessage, ex.Message );
        }
    }
}
