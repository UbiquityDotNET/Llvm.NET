// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.CommandLine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.CommandLine.UT
{
    /// <summary>This is mostly for validation.understanding of the underlying RAW API as well as a good place to put "samples" for bug reports</summary>
    [TestClass]
    public class RawApiTests
    {
        [TestMethod]
        [Ignore( "https://github.com/dotnet/command-line-api/issues/2664" )]
        public void RawApi_Version_Error_tests( )
        {
            var rootCommand = new RootCommand("Test Root")
            {
                new Option<string>("--option1")
                {
                    Description = "Test option `",
                    Required = true,
                },
            };

            var result = rootCommand.Parse(["--FooBar", "--version"]);
            Assert.HasCount( 3, result.Errors, "Errors should account for, bogus arg (`--FooBar`), missing required arg (`--option1`), AND that `--version` should be solo" );
        }

        [TestMethod]
        [Ignore( "https://github.com/dotnet/command-line-api/issues/2664" )]
        public void RawApi_Help_Error_tests( )
        {
            var rootCommand = new RootCommand("Test Root")
            {
                new Option<string>("--option1")
                {
                    Description = "Test option",
                    Required = true,
                },
            };

            var result = rootCommand.Parse(["--FooBar", "--help"]);
            var helpOption = result.GetHelpOption();
            Assert.IsNotNull(helpOption);
            Assert.AreEqual(helpOption.Action, result.Action);
            Assert.HasCount( 3, result.Errors, "Errors should account for bogus arg (`--FooBar`), missing required arg (`--option1`), AND that `--version` should be solo" );
        }

        [TestMethod]
        public void RawApi_Version_Only_with_required_has_no_errors( )
        {
            var rootCommand = new RootCommand("Test Root")
            {
                new Option<string>("--option1")
                {
                    Description = "Test option `",
                    Required = true,
                },
            };

            var result = rootCommand.Parse(["--version"]);
            Assert.HasCount( 0, result.Errors, "Should not be any errors" );
        }

        [TestMethod]
        [Ignore("https://github.com/dotnet/command-line-api/issues/2664")]
        public void RawApi_Version_with_required_option_has_errors( )
        {
            var rootCommand = new RootCommand("Test Root")
            {
                new Option<string>("--option1")
                {
                    Description = "Test option `",
                    Required = true,
                },
            };

            ParseResult result = rootCommand.Parse(["--version", "--option1"]);

            // Known bug in runtime lib. This assert will fail - result.Errors.Count == 1!
            // result.Errors:
            //    [0]{--version option cannot be combined with other arguments.}
            //    [1]{Required argument missing for option: '--option1'.}
            Assert.HasCount( 2, result.Errors, "Should be two errors (version not used solo, missing arg)" );

            // try with arguments in reversed order (--version is later)
            result = rootCommand.Parse(["--option1", "--version"]);

            // result.Action == null! [BUG]
            // result.Errors.Count == 0! [BUG]
            Assert.HasCount( 2, result.Errors, "Should be two errors (version not used solo, missing arg)" );
            result = rootCommand.Parse("--option1 --version");

            // Known bug in runtime lib. This assert will fail - result.Errors.Count == 0!
            // result.Action == null! [BUG]
            // result.Errors.Count == 0! [BUG]
            Assert.HasCount( 2, result.Errors, "Should be two errors (version not used solo, missing arg)" );
        }
    }
}
