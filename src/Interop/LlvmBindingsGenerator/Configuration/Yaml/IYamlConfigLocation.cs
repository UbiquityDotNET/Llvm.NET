// -----------------------------------------------------------------------
// <copyright file="YamlConfigNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using YamlDotNet.Core;

namespace LlvmBindingsGenerator.Configuration
{
    internal interface IYamlConfigLocation
    {
        Mark Start { get; set; }
    }
}
