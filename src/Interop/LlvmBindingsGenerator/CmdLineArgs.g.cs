// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppSharp;

using Ubiquity.NET.CommandLine;

namespace LlvmBindingsGenerator
{
    internal partial class CmdLineArgs
        : ICommandLineOptions<CmdLineArgs>
    {
        public static CmdLineArgs Bind( ParseResult parseResult )
        {
            return new()
            {
                // NOTE: an Argument descriptor does NOT have a "Required" property
                // binding to a required value that is NOT present AND has no Default value provider
                // will generate an exception. There is no "try" semantics. One could argue that
                // anything with a default value provider isn't really required but it depends on perspective.
                // Any value that MUST be provided by the user (no default is possible) vs. any value that must
                // be used by the consumer (default possible if not provided) are different scenarios and each
                // can consider the value as "required" as things can't work without it. (In other words,
                // evaluation of the term "required" must include a subject [that is, an answer to the "to whom/what?"].
                // The idea of "required" is a concept that is a dependent term and evaluation without a subject
                // is not possible.)
                //
                // Source generator should make this CRYSTAL CLEAR.

                LlvmRoot = parseResult.GetRequiredValue( Descriptors.LlvmRoot ),
                ExtensionsRoot = parseResult.GetRequiredValue( Descriptors.ExtensionsRoot ),
                HandleOutputPath = parseResult.GetRequiredValue( Descriptors.HandleOutputPath ),
                Diagnostics = parseResult.GetValue( Descriptors.Diagnostics ),
            };
        }

        public static AppControlledDefaultsRootCommand BuildRootCommand( CmdLineSettings settings )
        {
            // description is hard coded and should come from generation attribute on the "CmdLineArgs" type
            // TODO: Check for duplicate options.
            //       (-?, -h, --help, and --version) are used by default options so nothing should use those values.
            //       Generator/analyzer should validate that so it isn't a runtime hit.
            return new AppControlledDefaultsRootCommand( settings, "LLVM handle wrapper source generator" )
            {
                Descriptors.LlvmRoot,
                Descriptors.ExtensionsRoot,
                Descriptors.HandleOutputPath,
                Descriptors.Diagnostics,

                // Additional Options, Arguments, Sub-commands, and directives go here...
            };
        }
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "[Sigh...] It's FILE scoped!" )]
    file static class Descriptors
    {
        internal static readonly Option<DirectoryInfo> LlvmRoot
            = new Option<DirectoryInfo>("-l")
            {
                Required = true,
                Description = "Root of source with the LLVM headers to parse (Assumes and validates a sub-folder 'include')",
            }.AcceptExistingFolderOnly()
             .AddValidator(CmdLineArgs.ValidateIncludeFolder);

        internal static readonly Option<DirectoryInfo> ExtensionsRoot
            = new Option<DirectoryInfo>("-e")
            {
                Required = true,
                Description = "Root of source with the LibLLVM extension headers to parse (Assumes and validates a sub-folder 'include')",
            }.AcceptExistingFolderOnly()
             .AddValidator(CmdLineArgs.ValidateIncludeFolder);

        internal static readonly Option<DirectoryInfo> HandleOutputPath
            = new Option<DirectoryInfo>("-o")
            {
                Required = true,
                Description = "Output to place the generated code for handles. No handle source is generated if this is not provided",
            }.EnsureFolder();

        internal static readonly Option<DiagnosticKind> Diagnostics
            = new("--diagnostics")
            {
                Description = "Diagnostics output level",
                DefaultValueFactory = (_)=>DiagnosticKind.Message,
            };
    }
}
