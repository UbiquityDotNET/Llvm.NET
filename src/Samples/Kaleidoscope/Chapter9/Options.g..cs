// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// TODO: Generate this file from attributes using a Roslyn source generator.

using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Ubiquity.NET.CommandLine;

namespace Kaleidoscope.Chapter9
{
    internal partial class Options
        : ICommandLineOptions<Options>
    {
        public static Options Bind( ParseResult parseResult )
        {
            return new()
            {
                // NOTE: an Argument descriptor does NOT have a "Required" property
                // binding to a required value that is NOT present AND has no Default value provider
                // will generate an exception. There is no "try" semantics. One could argue that
                // anything with a default value provider isn't really required but it depends on perspective.
                // Any value that MUST be provided by the user (no default is possible) vs. any value that must
                // be used by the consumer (default possible if not provided) are different scenarios and each
                // can consider the value as "required" as things can't work without it. (That is, evaluation of
                // the term "required" must include a subject [that is, an answer to the "to whom/what?"].
                // "required" is a concept that is a dependent term and evaluation without a subject is not possible.)
                //
                // Source generator should make this CRYSTAL CLEAR.
                SourcePath = parseResult.GetRequiredValue( Descriptors.SourceFilePath )
            };
        }

        public static AppControlledDefaultsRootCommand BuildRootCommand( CmdLineSettings settings )
        {
            // description is hard coded and should come from generation attribute on the "Options" type
            return new AppControlledDefaultsRootCommand( settings, "Kaleidoscope sample app (Chapter 9)" )
            {
                Descriptors.SourceFilePath,

                // Additional Options, Arguments, Sub-commands, and directives go here...
            };
        }
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "[Sigh...] It's FILE scoped!" )]
    file static class Descriptors
    {
        internal static readonly Argument<FileInfo> SourceFilePath
            = new Argument<FileInfo>("SourceFilePath")
            {
                Description = "Path of the input source file to compile",
            }.AcceptExistingFileOnly();
    }
}
