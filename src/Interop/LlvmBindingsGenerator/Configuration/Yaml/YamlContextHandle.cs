// -----------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [DebuggerDisplay( "ContextHandle({HandleName})" )]
    internal class YamlContextHandle
        : IYamlConfigLocation
        , IHandleInfo
    {
        public string HandleName { get; set; }

        [YamlIgnore]
        public Mark Start { get; set; }
    }
}
