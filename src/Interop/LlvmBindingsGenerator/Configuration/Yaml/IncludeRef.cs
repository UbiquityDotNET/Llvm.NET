// -----------------------------------------------------------------------
// <copyright file="LocationAnnotatedNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using YamlDotNet.Core;

namespace LlvmBindingsGenerator.Configuration
{
    internal class IncludeRef
        : IYamlConfigLocation
    {
        public string Path { get; set; }

        public Mark Start { get; set; }
    }
}
