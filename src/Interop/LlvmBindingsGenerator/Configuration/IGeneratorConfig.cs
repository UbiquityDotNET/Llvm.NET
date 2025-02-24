// -----------------------------------------------------------------------
// <copyright file="IGeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

namespace LlvmBindingsGenerator.Configuration
{
    internal interface IGeneratorConfig
    {
        ImmutableArray<IncludeRef> IgnoredHeaders { get; }

        IEnumerable<IHandleInfo> HandleMap { get; }

        HandleTemplateMap BuildTemplateMap( );
    }
}
