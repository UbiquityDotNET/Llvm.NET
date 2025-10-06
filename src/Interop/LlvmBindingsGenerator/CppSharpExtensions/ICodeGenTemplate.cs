// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Until https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3100 is resolved
// silence the warning about undocumented internal interfaces.
//
#pragma warning disable SA1600 // Elements should be documented

namespace LlvmBindingsGenerator.CppSharpExtensions
{
    internal interface ICodeGenTemplate
    {
        string ToolVersion { get; }

        string? FileExtension { get; }

        string? SubFolder { get; }

        string Generate( );
    }

    internal interface IHandleCodeTemplate
        : ICodeGenTemplate
    {
        string HandleName { get; }
    }
}
