// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

using CommandLine;

using CppSharp;

namespace LlvmBindingsGenerator
{
    [SuppressMessage("Build", "CA1812", Justification = "Instantiated via reflection from Command line parser" )]
    [SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "It is necessary, tooling can't agree on the point. (removing it generates a warning)" )]
    internal class Options
    {
        public Options(
            string? llvmRoot,
            string? extensionsRoot,
            string? handleOutputPath,
            DiagnosticKind diagnostics
            )
        {
            // NOTE: This MUST NOT throw ANY exceptions.
            // [see](https://github.com/commandlineparser/commandline/issues/734)
            // The parser that calls this will consider any exceptions as a reflection error finding
            // the constructor for an immutable type. Actual exception is wrapped in a new exception
            // with a message indicating it can't find the correct overload. (This is really a compile
            // time bug but the use of reflection makes it a runtime bug). Thus, any exceptions generated
            // by this constructor are buried/lost.
            LlvmRoot = llvmRoot is null ? string.Empty : Path.GetFullPath(llvmRoot);
            ExtensionsRoot = extensionsRoot is null ? string.Empty : Path.GetFullPath(extensionsRoot);
            HandleOutputPath = handleOutputPath is null ? string.Empty : Path.GetFullPath(handleOutputPath);
            Diagnostics = diagnostics;
        }

        [Option('l', HelpText = "Root of source with the LLVM headers to parse (Assumes a subfolder 'include')", Required = true )]
        public string LlvmRoot { get; } = string.Empty;

        [Option('e', HelpText = "Root of source with the LibLLVM extension headers to parse", Required = true )]
        public string ExtensionsRoot { get; } = string.Empty;

        [Option( 'h', HelpText = "Output to place the generated code for handles. No handle source is generated if this is not provided" )]
        public string HandleOutputPath { get; } = string.Empty;

        [Option( HelpText = "Diagnostics output level", Required = false, Default = DiagnosticKind.Message )]
        public DiagnosticKind Diagnostics { get; }

        public bool GenerateHandles => !string.IsNullOrWhiteSpace(HandleOutputPath);

        public bool Validate(TextWriter helpWriter)
        {
            // start by assuming all options validate OK, set to false if any checks fail
            bool retVal = true;

            if(!Directory.Exists(LlvmRoot))
            {
                helpWriter.WriteLine($"LlvmRoot path does not exist '{LlvmRoot}'.");
                retVal = false;
            }

            if(!Directory.Exists(Path.Combine(LlvmRoot, "include")))
            {
                helpWriter.WriteLine($"LlvmRoot path does not contain a sub folder named 'include'; LlvmRoot: '{LlvmRoot}'.");
                retVal = false;
            }

            if(!Directory.Exists(ExtensionsRoot))
            {
                helpWriter.WriteLine($"ExtensionsRoot path does not exist '{ExtensionsRoot}'.");
                retVal = false;
            }

            if(!Directory.Exists(Path.Combine(ExtensionsRoot, "include")))
            {
                helpWriter.WriteLine($"ExtensionsRoot path does not contain a sub folder named 'include'; ExtensionsRoot: '{ExtensionsRoot}'.");
                retVal = false;
            }

            // if HandleOutputPath is specified but does not exist, it is created so no need to test for that here

            return retVal;
        }

        public override string ToString( )
        {
            var bldr = new StringBuilder("Options:");
            bldr.AppendLine()
                .AppendLine(CultureInfo.InvariantCulture, $"          LlvmRoot: {LlvmRoot}" )
                .AppendLine(CultureInfo.InvariantCulture, $"    ExtensionsRoot: {ExtensionsRoot}")
                .AppendLine(CultureInfo.InvariantCulture, $"  HandleOutputPath: {HandleOutputPath}")
                .AppendLine(CultureInfo.InvariantCulture, $"       Diagnostics: {Diagnostics}");
            return bldr.ToString();
        }
    }
}
