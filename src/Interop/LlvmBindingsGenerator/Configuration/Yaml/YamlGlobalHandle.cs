// -----------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Tooling is not acapable of seeing the need for this" )]
    [DebuggerDisplay( "GlobalHandle({HandleName},{Disposer}, Alias:{Alias})" )]
    internal class YamlGlobalHandle
        : IYamlNodeLocation
        , IHandleInfo
    {
        public string HandleName { get; set; } = string.Empty;

        public string Disposer { get; set; } = string.Empty;

        public bool Alias { get; set; }

        [YamlIgnore]
        public Mark Start { get; set; }
    }
}
