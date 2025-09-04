using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubiquity.NET.CommandLine.UT
{
    // FUTURE: Generate this with source generator from attributes in other partial declaration
    internal partial class TestOptions
        : ICommandLineOptions<TestOptions>
    {
        public static TestOptions Bind( ParseResult parseResult )
        {
            return new()
            {
                Option1 = parseResult.GetValue(Descriptors.Option1),
            };
        }

        public static AppControlledDefaultsRootCommand BuildRootCommand( CmdLineSettings settings )
        {
            return new( settings, "Test option root command")
            {
                Descriptors.Option1,
            };
        }

        internal static class Descriptors
        {
            internal static Option<string> Option1
                = new("--option1")
                {
                    Description = "Test Option",
                    Required = true, // Should have no impact on Help/Version
                };
        }
    }
}
