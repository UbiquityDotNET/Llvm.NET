// -----------------------------------------------------------------------
// <copyright file="IGeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator.Configuration
{
    internal interface IGeneratorConfig
    {
        IReadOnlyDictionary<string, YamlFunctionBinding> FunctionBindings { get; }

        IReadOnlyCollection<string> IgnoredHeaders { get; }

        IEnumerable<IHandleInfo> HandleMap { get; }

        IReadOnlyDictionary<string, string> AnonymousEnums { get; }

        HandleTemplateMap BuildTemplateMap( );
    }
}
