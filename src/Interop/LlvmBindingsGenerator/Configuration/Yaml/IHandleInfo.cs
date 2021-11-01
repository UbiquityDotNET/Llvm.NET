// -----------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LlvmBindingsGenerator.Configuration
{
    internal interface IHandleInfo
        : IYamlNodeLocation
    {
        string HandleName { get; }
    }
}
