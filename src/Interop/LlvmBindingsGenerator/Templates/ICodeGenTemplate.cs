// -----------------------------------------------------------------------
// <copyright file="ICodeGenTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace LlvmBindingsGenerator.Templates
{
    internal interface ICodeGenTemplate
    {
        Version ToolVersion { get; }

        string FileExtension { get; }

        string Generate( );
    }

    internal interface IHandleCodeTemplate
        : ICodeGenTemplate
    {
        string HandleName { get; }
    }
}
