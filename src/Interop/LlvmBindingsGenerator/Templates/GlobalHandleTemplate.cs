// -----------------------------------------------------------------------
// <copyright file="GlobalHandleTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class GlobalHandleTemplate
        : IHandleCodeTemplate
    {
        public GlobalHandleTemplate( string name, string disposerFunctionName, bool needsAlias = false )
        {
            HandleName = name;
            HandleDisposeFunction = disposerFunctionName;
            NeedsAlias = needsAlias;
        }

        public Version ToolVersion => GetType( ).Assembly.GetName( ).Version;

        public string HandleName { get; }

        public string HandleDisposeFunction { get; }

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public bool NeedsAlias { get; }

        public string Generate( ) => TransformText( );
    }
}
