// -----------------------------------------------------------------------
// <copyright file="ReadOnlyConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator.Configuration.Yaml
{
    internal class ReadOnlyConfig
        : IGeneratorConfig
    {
        public ReadOnlyConfig(YamlConfiguration config)
        {
            YamlConfig = config;
        }

        public IReadOnlyDictionary<string, YamlFunctionBinding> FunctionBindings
            => YamlConfig.FunctionBindings;

        public IReadOnlyCollection<string> IgnoredHeaders
            => YamlConfig.IgnoredHeaders;

        public IEnumerable<IHandleInfo> HandleMap => YamlConfig.HandleMap;

        public IReadOnlyDictionary<string, string> AnonymousEnums => YamlConfig.AnonymousEnums;

        public HandleTemplateMap BuildTemplateMap( ) => YamlConfig.BuildTemplateMap( );

        private readonly YamlConfiguration YamlConfig;
    }
}
