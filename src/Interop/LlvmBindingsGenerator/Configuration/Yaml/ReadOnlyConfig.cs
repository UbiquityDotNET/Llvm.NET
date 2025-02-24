// -----------------------------------------------------------------------
// <copyright file="ReadOnlyConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

namespace LlvmBindingsGenerator.Configuration.Yaml
{
    internal class ReadOnlyConfig
        : IGeneratorConfig
    {
        public ReadOnlyConfig( YamlConfiguration config )
        {
            YamlConfig = config;
        }

        public ImmutableArray<IncludeRef> IgnoredHeaders
            => [ .. YamlConfig.IgnoredHeaders ];

        public IEnumerable<IHandleInfo> HandleMap => YamlConfig.HandleMap;

        public HandleTemplateMap BuildTemplateMap( ) => YamlConfig.BuildTemplateMap( );

        private readonly YamlConfiguration YamlConfig;
    }
}
