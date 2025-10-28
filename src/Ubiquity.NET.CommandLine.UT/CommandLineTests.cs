// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.CommandLine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.CommandLine.UT
{
    [TestClass]
    public class CommandLineTests
    {
        [TestMethod]
        public void CommandLine_parse_with_version_option_only_succeeds( )
        {
            var settings = CreateTestSettings();
            var result = ArgsParsing.Parse<TestOptions>(["--version"], settings);

            Assert.HasCount( 0, result.Errors, "Version alone should not procue errors" );

            var versionOption = result.GetVersionOption();
            Assert.IsNotNull(versionOption);
            Assert.AreEqual( versionOption.Action, result.Action);
        }

        [TestMethod]
        public void CommandLine_with_help_option_only_succeeds( )
        {
            var settings = CreateTestSettings();

            var result = ArgsParsing.Parse<TestOptions>(["--help"], settings);
            Assert.HasCount( 0, result.Errors );
        }

        [TestMethod]
        public void CommandLine_with_unknown_option_has_errors( )
        {
            var settings = CreateTestSettings();
            ParseResult result = ArgsParsing.Parse<TestOptions>(["--FooBar"], settings );
            Assert.HasCount( 2, result.Errors, "Errors should include missing Required, and invalid param" );
        }

        [TestMethod]
        public void CommandLine_with_known_option_and_version_has_errors( )
        {
            var settings = CreateTestSettings();
            ParseResult result = ArgsParsing.Parse<TestOptions>(["--version", "--option1"], settings );

            Assert.HasCount( 1, result.Errors, "Should be one error (--version must be set alone) [Other errors ignored for --version]" );
        }

        [TestMethod]
        [Ignore("https://github.com/dotnet/command-line-api/issues/2664")]
        public void CommandLine_with_known_option_requiring_arg_and_version_has_errors( )
        {
            var settings = CreateTestSettings();
            ParseResult result = ArgsParsing.Parse<TestOptions>(["--option1", "--version"], settings );

            // until https://github.com/dotnet/command-line-api/issues/2664 is resolved this will fail
            Assert.HasCount( 2, result.Errors, "Should be one error (--version must be set alone, missing arg for --option1)" );
        }

        internal static CmdLineSettings CreateTestSettings( DefaultOption defaultOptions = DefaultOption.Help | DefaultOption.Version )
        {
            return new CmdLineSettings()
            {
                DefaultOptions = defaultOptions,
            };
        }
    }
}
