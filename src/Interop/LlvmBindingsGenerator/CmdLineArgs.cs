// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.CommandLine.Parsing;
using System.IO;

using CppSharp;

namespace LlvmBindingsGenerator
{
    internal partial class CmdLineArgs
    {
        // [Option("-l", Required=true, Description="Root of source with the LLVM headers to parse (Assumes and validates a subfolder 'include')"]
        // [AcceptExistingFolderOnly]
        // [Validator( 'CmdLineArgs.ValidateIncludeFolder' )]
        public required DirectoryInfo LlvmRoot { get; init; }

        // [Option("-e", Required=true, Description="Root of source with the LibLLVM extension headers to parse (Assumes and validates a subfolder 'include')"]
        // [AcceptExistingFolderOnly]
        // [Validator( "CmdLineArgs.ValidateIncludeFolder" )]
        public required DirectoryInfo ExtensionsRoot { get; init; }

        // [Option("-e", Required=true, Description="Output to place the generated code for handles. No handle source is generated if this is not provided")]
        // [EnsureFolder]
        public required DirectoryInfo HandleOutputPath { get; init; }

        // [Option("-o", Required=true, DefaultValue=DiagnosticKind.Message, Description="Diagnostics output level")]
        public DiagnosticKind Diagnostics { get; init; }

        /// <summary>Custom validation that a folder has a sub-folder named 'include'</summary>
        /// <param name="result">results of the command line parse of the option to validate</param>
        internal static void ValidateIncludeFolder( OptionResult result )
        {
            string optionName = result.Option.HelpName ?? result.Option.Name;
            foreach(var token in result.Tokens)
            {
                if(!Directory.Exists( Path.Combine(token.Value, "include" )))
                {
                    result.AddError( $"Folder '{token.Value}' specified for '{optionName}' does not contain a sub-folder named 'include'" );
                    return;
                }
            }
        }
    }
}
