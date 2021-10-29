// -----------------------------------------------------------------------
// <copyright file="Options.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using CommandLine;

using CppSharp;

namespace LlvmBindingsGenerator
{
    [SuppressMessage("Build", "CA1812", Justification = "Instantiated via reflection from commandline parser" )]
    [SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "It is necessary, tooling can't agree on the point. (removing it generates a warning)" )]
    internal class Options
    {
        public Options( string llvmRoot, string extensionsRoot, string outputPath, DiagnosticKind diagnostics )
        {
            LlvmRoot = llvmRoot;
            ExtensionsRoot = extensionsRoot;
            OutputPath = outputPath;
            Diagnostics = diagnostics;
        }

        [Value( 0, MetaName = "LLVM Root", HelpText = "Root of source with the LLVM headers to parse", Required = true )]
        public string LlvmRoot { get; }

        [Value( 1, MetaName = "Extensions Root", HelpText = "Root of source with the LibLLVM extension headers to parse", Required = true )]
        public string ExtensionsRoot { get; }

        [Value( 2, MetaName = "Output Path", HelpText = "Root of the output to place the generated code", Required = true )]
        public string OutputPath { get; } = Environment.CurrentDirectory;

        [Option( HelpText = "Diagnostics output level", Required = false, Default = DiagnosticKind.Message )]
        public DiagnosticKind Diagnostics { get; }
    }
}
