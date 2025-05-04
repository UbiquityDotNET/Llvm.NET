// -----------------------------------------------------------------------
// <copyright file="LocationAnnotatedNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using YamlDotNet.Core;

namespace LlvmBindingsGenerator.Configuration
{
    internal class IncludeRef
        : IYamlNodeLocation
    {
        public string Path { get; set; } = string.Empty;

        public Mark Start { get; set; }
    }
}
