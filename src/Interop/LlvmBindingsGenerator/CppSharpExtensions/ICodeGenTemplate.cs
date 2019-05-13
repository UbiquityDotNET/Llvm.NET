// -----------------------------------------------------------------------
// <copyright file="ICodeGenTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace LlvmBindingsGenerator.Templates
{
    internal interface ICodeGenTemplate
    {
        Version ToolVersion { get; }

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
