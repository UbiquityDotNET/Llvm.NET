// -----------------------------------------------------------------------
// <copyright file="ICodeGenTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LlvmBindingsGenerator.Templates
{
    internal interface ICodeGenTemplate
    {
        string ToolVersion { get; }

        string FileExtension { get; }

        string SubFolder { get; }

        string Generate( );
    }

    internal interface IHandleCodeTemplate
        : ICodeGenTemplate
    {
        string HandleName { get; }
    }
}
