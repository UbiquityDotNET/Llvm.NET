// -----------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [DebuggerDisplay( "GlobalHandle({HandleName},{Disposer}, Alias:{Alias})" )]
    internal class YamlGlobalHandle
        : IHandleInfo
    {
        public string HandleName { get; set; }

        public string Disposer { get; set; }

        public bool Alias { get; set; }
    }
}
