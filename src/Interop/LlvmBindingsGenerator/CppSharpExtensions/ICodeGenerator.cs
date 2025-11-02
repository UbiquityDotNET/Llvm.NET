// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace LlvmBindingsGenerator
{
    internal interface ICodeGenerator
    {
        bool IsValid { get; }

        string? FileNameWithoutExtension { get; }

        string? FileRelativeDirectory { get; }

        string? FileAbsolutePath { get; }

        ICodeGenTemplate Template { get; }
    }
}
