// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// TODO: Generate this file from attributes using a Roslyn source generator.

using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Ubiquity.NET.CommandLine;

namespace Kaleidoscope
{
    internal partial class Options
        : IRootCommand<Options>
    {
        public static Options Bind( ParseResult parseResult )
        {
            return new()
            {
                // NOTE: an Argument descriptor does NOT have a "Required" property
                SourcePath = parseResult.GetRequiredValue( Descriptors.SourceFilePath )
            };
        }

        public static AppControlledDefaultsRootCommand BuildRootCommand( CmdLineSettings? settings )
        {
            return new AppControlledDefaultsRootCommand( "Kaleidoscope sample app", settings )
            {
                Descriptors.SourceFilePath
            };
        }
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "[Sigh...] It's FILE scoped!" )]
    file static class Descriptors
    {
        internal static readonly Argument<FileInfo> SourceFilePath
            = new Argument<FileInfo>("SourceFilePath")
            {
                Description = "Path of the input source file [Used as ref for debug information]",
                HelpName = "Source file path",
            }.AcceptExistingFileOnly();
    }
}
