// -----------------------------------------------------------------------
// <copyright file="IGeneratorCodeTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    internal interface ICodeGenerator
    {
        bool IsValid { get; }

        string FileNameWithoutExtension { get; }

        string FileRelativeDirectory { get; }

        ICodeGenTemplate Template { get; }
    }
}
